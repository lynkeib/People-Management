using leave_management.Contracts;
using leave_management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Repository
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveTypeRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public bool Create(LeaveType entity)
        {
            _db.LeaveTypes.Add(entity);
            // Save
            return Save();
        }

        public bool Delete(LeaveType entity)
        {
            _db.LeaveTypes.Remove(entity);
            // Save
            return Save();
        }

        public ICollection<LeaveType> FindAll()
        {
            return _db.LeaveTypes.ToList();
        }

        public LeaveType FindById(int id)
        {
            //_db.LeaveTypes.FirstOrDefault();
            return _db.LeaveTypes.Find(id);

        }

        public ICollection<LeaveType> GetEmployeesByLeaveType(int id)
        {
            throw new NotImplementedException();
        }

        public bool isExists(int id)
        {
            return _db.LeaveTypes.Any(q => q.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 1 ? true : false;
        }

        public bool Update(LeaveType entity)
        {
            _db.LeaveTypes.Update(entity);
            // Save
            return Save();
        }
    }
}
