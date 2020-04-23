using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace leave_management.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public async Task<bool> CheckAllocation(int leavetypeid, string employeeid)
        {
            var period = DateTime.Now.Year;
            var findall = await FindAll();
            return findall.Where(q => q.EmployeeId == employeeid && q.LeaveTypeId == leavetypeid && q.Period == period).Any();
        }

        public async Task<bool> Create(LeaveAllocation entity)
        {
            await _db.LeaveAllocations.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveAllocation>> FindAll()
        {
            var res = await _db.LeaveAllocations.Include(q => q.LeaveType).Include(q => q.Employee).ToListAsync();
            return res;
        }

        public async Task<LeaveAllocation> FindById(int id)
        {
            var res = await  _db.LeaveAllocations.Include(q => q.LeaveType).Include(q => q.Employee).FirstOrDefaultAsync(q => q.Id == id);
            return res;
        }

        public async Task<ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string id)
        {
            var period = DateTime.Now.Year;
            var findall = await FindAll();
            return findall.Where(q => q.EmployeeId == id && q.Period == period).ToList();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationsByEmployeeAndType(string id, int leavetypeid)
        {
            var period = DateTime.Now.Year;
            var res = await FindAll();
            var ret = res.FirstOrDefault(q => q.EmployeeId == id && q.Period == period && q.LeaveTypeId == leavetypeid);
            return ret;
        }

        public async Task<bool> isExists(int id)
        {
            var res = await _db.LeaveAllocations.AnyAsync(q => q.Id == id);
            return res;
        }

        public async Task<bool> Save()
        {
            var res = await _db.SaveChangesAsync();
            return res >= 1 ? true : false;
        }

        public async Task<bool> Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return await Save();
        }
    }
}
