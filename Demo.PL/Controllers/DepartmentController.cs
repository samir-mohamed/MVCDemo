using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
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
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchValue)
        {
            var departments = Enumerable.Empty<Department>();
            if (string.IsNullOrWhiteSpace(searchValue))
                departments = await _unitOfWork.DepartmentRepository.GetAll();
            else
                departments = _unitOfWork.DepartmentRepository.SearchByDepartmentName(searchValue);

            var departmentsVM = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(departmentsVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                var department = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                await _unitOfWork.DepartmentRepository.Add(department);
                TempData["Message"] = "Department is Created Successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(departmentVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id == null)
                return NotFound();

            var department = await _unitOfWork.DepartmentRepository.Get(id.Value);
            if (department == null)
                return NotFound();

            var departmentVM = _mapper.Map<Department, DepartmentViewModel>(department);

            return View(viewName, departmentVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");

            //if (id == null)
            //    return NotFound();

            //var department = _departmentRepository.Get(id.Value);
            //if (department == null)
            //    return NotFound();

            //return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var department = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    await _unitOfWork.DepartmentRepository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log Exception

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(departmentVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();

            try
            {
                var department = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                await _unitOfWork.DepartmentRepository.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log Exception
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(departmentVM);
            }
        }
    }
}
