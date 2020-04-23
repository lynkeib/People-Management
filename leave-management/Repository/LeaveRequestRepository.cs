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
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRequestRepository(ApplicationDbContext db)
        {
            this._db = db;
        }
        public async Task<bool> Create(LeaveRequest entity)
        {
            await _db.LeaveRequests.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveRequest>> FindAll()
        {
            var res = await _db.LeaveRequests.Include(q => q.RequestingEmployee).Include(q => q.ApprovedBy).Include(q => q.LeaveType).ToListAsync();
            return res;
        }

        public async Task<LeaveRequest> FindById(int id)
        {
            var res = await _db.LeaveRequests.Include(q => q.RequestingEmployee).Include(q => q.ApprovedBy).Include(q => q.LeaveType).FirstOrDefaultAsync(q => q.Id == id);
            return res;
        }

        public async Task<bool> isExists(int id)
        {
            var res = await _db.LeaveRequests.AnyAsync(q => q.Id == id);
            return res;
        }

        public async Task<bool> Save()
        {
            var res = await  _db.SaveChangesAsync();
            return res >= 1 ? true : false;
        }

        public async Task<bool> Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveRequest>> GetLeaveRequestsByEmployee(string employeeid)
        {
            var findall = await FindAll();
            var res = findall.Where(q => q.RequestingEmployeeId == employeeid).ToList();
            return res;
        }
    }
}
