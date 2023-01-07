using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchValue)
        {
            var employees = Enumerable.Empty<Employee>();
            if(string.IsNullOrWhiteSpace(searchValue))
                employees = await _unitOfWork.EmployeeRepository.GetAll();
            else
                employees = _unitOfWork.EmployeeRepository.SearchEmployeesByName(searchValue);

            var employeesVM = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(employeesVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                //var employee = new Employee
                //{
                //    Name = employeeVM.Name,
                //    Age = employeeVM.Age,
                //    Address = employeeVM.Address,
                //    Salary = employeeVM.Salary,
                //    IsActive = employeeVM.IsActive,
                //    DepartmentId = employeeVM.DepartmentId,
                //    Email = employeeVM.Email,
                //    PhoneNumber = employeeVM.PhoneNumber
                //};

                employeeVM.ImageName = await DocumentSettings.UploadFile(employeeVM.Image, "images");

                var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                await _unitOfWork.EmployeeRepository.Add(employee);
                return RedirectToAction(nameof(Index));
            }

            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id == null)
                return NotFound();

            var employee = await _unitOfWork.EmployeeRepository.Get(id.Value);
            if (employee == null)
                return NotFound();

            var employeeVM = _mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(viewName, employeeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");

            //if (id == null)
            //    return NotFound();

            //var employee = _employeeRepository.Get(id.Value);
            //if (employee == null)
            //    return NotFound();

            //return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    if(employeeVM.Image != null)
                    {
                        DocumentSettings.DeleteFile(employeeVM.ImageName, "images");
                        employeeVM.ImageName = await DocumentSettings.UploadFile(employeeVM.Image, "images");
                    }

                    var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    await _unitOfWork.EmployeeRepository.Update(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log Exception

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(employeeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            try
            {
                var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                int count = await _unitOfWork.EmployeeRepository.Delete(employee);
                if(count > 0 && employeeVM.ImageName != null)
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "images");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log Exception
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }
        }
    }
}
