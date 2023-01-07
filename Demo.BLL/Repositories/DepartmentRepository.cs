using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context) 
            : base(context)
        {
            _context = context;
        }

        public IQueryable<Department> SearchByDepartmentName(string departmentName)
            => _context.Departments.Where(d => d.Name.ToLower().Contains(departmentName.ToLower()));
    }
}
