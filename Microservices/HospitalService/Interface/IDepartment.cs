using HospitalService.Entity;
using HospitalService.Model;

namespace HospitalService.Interface
{
    public interface IDepartment
    {
        object? CreateDept(DepartmentRequest deptRequest);
        DepartmentEntity? getByDeptId(int id);
        DepartmentEntity? getByDeptName(string name);

        public Task<DepartmentRequest> UpdateDepartment(int DeptId, DepartmentRequest deptRequest);

        public Task<int> Deletedepartment(int DeptId);
    }
}
