using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace leave_management.Controllers
{
    public class LeaveTypesController : Controller
    {
        private readonly ILeaveTypeRepository _repo;
        private readonly IMapper _mapper;

        public LeaveTypesController(ILeaveTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET: LeaveTypes
        public ActionResult Index()
        {
            var leavetypes = _repo.FindAll().ToList();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetypes);
            return View(model);
        }

        // GET: LeaveTypes/Details/5
        public ActionResult Details(int id)
        {
            if (!_repo.isExists(id))
            {
                return NotFound();
            }
            var leavetype = _repo.FindById(id);
            var model = _mapper.Map<LeaveType, LeaveTypeVM>(leavetype);
            return View(model);
        }

        // GET: LeaveTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LeaveTypeVM model)
        {
            try
            {
                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var leavetype = _mapper.Map<LeaveTypeVM, LeaveType>(model);
                leavetype.DateCreated = DateTime.Now;
                var isSuccess = _repo.Create(leavetype);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something Went Wrong ...");
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong ...");
                return View(model);
            }
        }

        // GET: LeaveTypes/Edit/5
        public ActionResult Edit(int id)
        {
            if(!_repo.isExists(id))
            {
                return NotFound();
            }
            var leavetype = _repo.FindById(id);
            var model = _mapper.Map<LeaveType, LeaveTypeVM>(leavetype);
            return View(model);
        }

        // POST: LeaveTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LeaveTypeVM model)
        {
            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var leavetype = _mapper.Map<LeaveTypeVM, LeaveType>(model);
                var isSuccess = _repo.Update(leavetype);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "Something Went Wrong ...");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something Went Wrong ...");
                return View();
            }
        }

        // GET: LeaveTypes/Delete/5
        public ActionResult Delete(int id)
        {
            if (!_repo.isExists(id))
            {
                return NotFound();
            }
            var leavetype = _repo.FindById(id);
            var isSuccess = _repo.Delete(leavetype);
            if (!isSuccess)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                if (!_repo.isExists(id))
                {
                    return NotFound();
                }
                var leavetype = _repo.FindById(id);
                var isSuccess = _repo.Delete(leavetype);
                if (!isSuccess)
                {
                    return View(collection);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(collection);
            }
        }
    }
}