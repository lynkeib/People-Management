using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using AspNetCore;
using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace leave_management.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;

        public LeaveRequestController(
            ILeaveRequestRepository leaveRequestRepo,
            ILeaveTypeRepository leaveTypeRepo,
            ILeaveAllocationRepository leaveAllocationRepo,
            IMapper mapper, 
            UserManager<Employee> userManager
            )
        {
            _leaveRequestRepo = leaveRequestRepo;
            _mapper = mapper;
            _userManager = userManager;
            _leaveTypeRepo = leaveTypeRepo;
            _leaveAllocationRepo = leaveAllocationRepo;
        }

        [Authorize(Roles = "Administrator")]
        // GET: LeaveRequest
        public ActionResult Index()
        {
            var leaveRequests = _leaveRequestRepo.FindAll();
            var leaveRequestsModel = _mapper.Map<ICollection<LeaveRequest>, List<LeaveRequestVM>>(leaveRequests);
            var model = new AdminLeaveRequestsViewVM
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = leaveRequests.Count(q => q.Approved == true),
                PendingRequests = leaveRequests.Count(q => q.Approved == null),
                RejectedRequests = leaveRequests.Count(q => q.Approved == false),
                LeaveReuqests = leaveRequestsModel
            };
            return View(model);
        }

        // GET: LeaveRequest/Details/5
        public ActionResult Details(int id)
        {
            var leaverequest = _leaveRequestRepo.FindById(id);
            var model = _mapper.Map<LeaveRequest, LeaveRequestVM>(leaverequest);
            return View(model);
        }

        public ActionResult ApproveRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaverequest = _leaveRequestRepo.FindById(id);

                var employeeid = leaverequest.RequestingEmployeeId;
                var leaveTypeId = leaverequest.LeaveTypeId;
                var allocation = _leaveAllocationRepo.GetLeaveAllocationsByEmployeeAndType(employeeid, leaveTypeId);
                int daysRequested = (int)(leaverequest.EndDate - leaverequest.StartDate).TotalDays;
                allocation.NumberOfDays -= daysRequested;

                _leaveAllocationRepo.Update(allocation);

                leaverequest.Approved = true;
                leaverequest.ApprovedById = user.Id;
                leaverequest.DateActioned = DateTime.Now;
                _leaveRequestRepo.Update(leaverequest);
                return RedirectToAction(nameof(Index), "Home");
            }
            catch(Exception ex)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
        }

        public ActionResult RejectRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaverequest = _leaveRequestRepo.FindById(id);
                leaverequest.Approved = false;
                leaverequest.ApprovedBy = user;
                leaverequest.ApprovedById = user.Id;
                leaverequest.DateActioned = DateTime.Now;
                var isSuccess = _leaveRequestRepo.Update(leaverequest);
                return RedirectToAction(nameof(Index), "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
        }

        // GET: LeaveRequest/Create
        public ActionResult Create()
        {
            var leavetypes = _leaveTypeRepo.FindAll();
            var leaveTypeItem = leavetypes.Select(q => new SelectListItem {
                Text = q.Name,
                Value = q.Id.ToString()
            });
            var model = new CreateLeaveRequestVM
            {
                LeaveTypes = leaveTypeItem,

            };
            return View(model);
        }

        // POST: LeaveRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateLeaveRequestVM model)
        {
            try
            {
                var StartDate = Convert.ToDateTime(model.StartDate);
                var EndDate = Convert.ToDateTime(model.EndDate);
                var leaveTypes = _leaveTypeRepo.FindAll();
                var leaveTypeItems = leaveTypes.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                });
                model.LeaveTypes = leaveTypeItems;
                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (DateTime.Compare(StartDate, EndDate) > 1)
                {
                    ModelState.AddModelError("", "Start Date cannot greater than End Date");
                    return View(model);
                }

                var employee = _userManager.GetUserAsync(User).Result;
                var allocation = _leaveAllocationRepo.GetLeaveAllocationsByEmployeeAndType(employee.Id, model.LeaveTypeId);

                int daysRequested = (int)(EndDate.Date - StartDate.Date).TotalDays;
                if (daysRequested > allocation.NumberOfDays)
                {
                    ModelState.AddModelError("", "Do not have enough Number of Days");
                    return View(model);
                }
                var leaveRequestModel = new LeaveRequestVM
                {
                    RequestingEmployeeId = employee.Id,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Approved = null,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now,
                    LeaveTypeId = model.LeaveTypeId
                };
                var leaveRequest = _mapper.Map<LeaveRequestVM, LeaveRequest>(leaveRequestModel);
                var isSuccess = _leaveRequestRepo.Create(leaveRequest);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Database Error");
                    return View(model);
                }
                return RedirectToAction(nameof(Index), "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }
        }

        // GET: LeaveRequest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveRequest/Edit/5
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

        // GET: LeaveRequest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveRequest/Delete/5
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