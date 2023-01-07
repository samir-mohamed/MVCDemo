using Demo.DAL.Entities;
using System.Linq;

namespace Demo.BLL.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        IQueryable<Department> SearchByDepartmentName(string departmentName);
    }
}
