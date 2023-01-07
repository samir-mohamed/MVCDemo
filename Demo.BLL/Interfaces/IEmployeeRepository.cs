using Demo.DAL.Entities;
using System.Linq;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IQueryable<Employee> GetEmployeesByDepartmentName(string departmentName);
        IQueryable<Employee> SearchEmployeesByName(string employeeName);
    }
}
