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
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;

namespace leave_management.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveAllocationController : Controller
    {
        private readonly ILeaveTypeRepository _leaverepo;
        private readonly ILeaveAllocationRepository _leaveallocationrepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveAllocationController(ILeaveTypeRepository leaverepo, ILeaveAllocationRepository leaveallocationrepo, IMapper mapper, UserManager<Employee> userManager)
        {
            _leaverepo = leaverepo;
            _leaveallocationrepo = leaveallocationrepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: LeaveAllocation
        public async Task<ActionResult> Index()
        {
            var leavetypes = await _leaverepo.FindAll();
            var mappedLeaveTypes = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes.ToList());
            var model = new CreateLeaveAllocationVM { LeaveTypes = mappedLeaveTypes, NumberUpdated = 0 };
            return View(model);
        }
        public async Task<ActionResult> SetLeave(int id)
        {
            var leavetype = await _leaverepo.FindById(id);
            var employee = await _userManager.GetUsersInRoleAsync("Employee");
            foreach (var emp in employee)
            {
                if (!await _leaveallocationrepo.CheckAllocation(id, emp.Id))
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
                    await _leaveallocationrepo.Create(leaveallocation);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> ListEmployees()
        {
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            var model = _mapper.Map<IList<Employee>, List<EmployeeVM>>(employees);
            return View(model);
        }

        // GET: LeaveAllocation/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var employee = _mapper.Map<Employee, EmployeeVM>(await _userManager.FindByIdAsync(id));
            var allocations = _mapper.Map<ICollection<LeaveAllocation>, List<LeaveAllocationVM>>(await _leaveallocationrepo.GetLeaveAllocationsByEmployee(employee.Id));
            var model = new ViewLeaveAllocationVM
            {
                Employee = employee,
                LeaveAllocation = allocations
            };
            return View(model);
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
        public async Task<ActionResult> Edit(int id)
        {
            var leaveallocation = await _leaveallocationrepo.FindById(id);
            var model = _mapper.Map<LeaveAllocation, EditLeaveAllocationVM>(leaveallocation);
            return View(model);
        }

        // POST: LeaveAllocation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditLeaveAllocationVM model)
        {
            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var record = await _leaveallocationrepo.FindById(model.Id);
                //var alocation = _mapper.Map<EditLeaveAllocationVM, LeaveAllocation>(model);
                record.NumberOfDays = model.NumberOfDays;
                var isSuccess = await _leaveallocationrepo.Update(record);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Error while saving");
                    return View(model);
                }
                return RedirectToAction(nameof(Details), new {id = model.EmployeeId });
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