using leave_management.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Contracts
{
    public interface ILeaveAllocationRepository : IRepositoryBase<LeaveAllocation>
    {
        public Task<bool> CheckAllocation(int leavetypeid, string employeeid);
        Task<ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string id);
        Task<LeaveAllocation> GetLeaveAllocationsByEmployeeAndType(string id, int leavetypeid);
    }
}
