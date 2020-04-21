using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public bool CheckAllocation(int leavetypeid, string employeeid)
        {
            var period = DateTime.Now.Year;
            return FindAll().Where(q => q.EmployeeId == employeeid && q.LeaveTypeId == leavetypeid && q.Period == period).Any();
        }

        public bool Create(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Add(entity);
            return Save();
        }

        public bool Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return Save();
        }

        public ICollection<LeaveAllocation> FindAll()
        {
            var res = _db.LeaveAllocations.Include(q => q.LeaveType).Include(q => q.Employee).ToList();
            return res;
        }

        public LeaveAllocation FindById(int id)
        {
            return _db.LeaveAllocations.Include(q => q.LeaveType).Include(q => q.Employee).FirstOrDefault(q => q.Id == id);
        }

        public ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string id)
        {
            var period = DateTime.Now.Year;
            return FindAll().Where(q => q.EmployeeId == id && q.Period == period).ToList();
        }

        public LeaveAllocation GetLeaveAllocationsByEmployeeAndType(string id, int leavetypeid)
        {
            var period = DateTime.Now.Year;
            var res = FindAll();
            var ret = res.FirstOrDefault(q => q.EmployeeId == id && q.Period == period && q.LeaveTypeId == leavetypeid);
            return ret;
        }

        public bool isExists(int id)
        {
            return _db.LeaveAllocations.Any(q => q.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 1 ? true : false;
        }

        public bool Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return Save();
        }
    }
}
