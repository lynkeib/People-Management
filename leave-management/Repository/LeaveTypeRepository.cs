using leave_management.Contracts;
using leave_management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace leave_management.Repository
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveTypeRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public async Task<bool> Create(LeaveType entity)
        {
            await _db.LeaveTypes.AddAsync(entity);
            // Save
            return await Save();
        }

        public async Task<bool> Delete(LeaveType entity)
        {
            _db.LeaveTypes.Remove(entity);
            // Save
            return await Save();
        }

        public async Task<ICollection<LeaveType>> FindAll()
        {
            var leavetypes = await _db.LeaveTypes.ToListAsync();
            return leavetypes;
        }

        public async Task<LeaveType> FindById(int id)
        {
            //_db.LeaveTypes.FirstOrDefault();
            var leavetype = await _db.LeaveTypes.FindAsync(id);
            return leavetype;

        }

        public ICollection<LeaveType> GetEmployeesByLeaveType(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> isExists(int id)
        {
            var exists = await _db.LeaveTypes.AnyAsync(q => q.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var isSuccess = await _db.SaveChangesAsync();
            return isSuccess >= 1 ? true : false;
        }

        public async Task<bool> Update(LeaveType entity)
        {
            _db.LeaveTypes.Update(entity);
            // Save
            return await Save();
        }
    }
}
