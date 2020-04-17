using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace leave_management.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveAllocationController : Controller
    {
        private readonly ILeaveTypeRepository _leaverepo;
        private readonly ILeaveAllocationRepository _leaveallocationrepo;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public LeaveAllocationController(ILeaveTypeRepository leaverepo, ILeaveAllocationRepository leaveallocationrepo, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _leaverepo = leaverepo;
            _leaveallocationrepo = leaveallocationrepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: LeaveAllocation
        public ActionResult Index()
        {
            var leavetypes = _leaverepo.FindAll().ToList();
            var mappedLeaveTypes = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes);
            var model = new CreateLeaveAllocationVM { LeaveTypes = mappedLeaveTypes, NumberUpdated = 0 };
            return View(model);
        }
        public ActionResult SetLeave(int id)
        {
            var leavetype = _leaverepo.FindById(id);
            var employee = _userManager.GetUsersInRoleAsync("Employee").Result;
            foreach (var emp in employee)
            {
                if (!_leaveallocationrepo.CheckAllocation(id, emp.Id))
                {
                    var allocation = new LeaveAllocationVM
                    {
                        DataCreated = DateTime.Now,
                        EmployeeId = emp.Id,
                        LeaveTypeId = id,
                        NumberOfDays = leavetype.DefaultDays,
                        Period = DateTime.Now.Year
                    };
                    var leaveallocation = _mapper.Map<LeaveAllocationVM, LeaveAllocation>(allocation);
                    _leaveallocationrepo.Create(leaveallocation);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: LeaveAllocation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LeaveAllocation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveAllocation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocation/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}