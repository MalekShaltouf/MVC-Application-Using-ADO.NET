using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADO.NET_APPLICATION.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult Index()
        {
            DataTable table = Employee.GetEmployeesData();
            return View(Employee.ConvertTableToList(table));
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            try
            {
                Employee.Insert(employee);
                return RedirectToAction("Index", "Employees");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("validation-error", ex.Message);
            }
            return View();
        }
        public ActionResult Edit(int Id)
        {
            try
            {
                DataTable table = Employee.GetEmployeeDataBasedId(Id);
                Employee employee = Employee.ConvertTableSingleObject(table);
                return View("Create", employee);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("validation-error", ex.Message);
            }
            return View("Create");
        }
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            try
            {
                Employee.Update(employee);
                return RedirectToAction("Index", "Employees");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("validation-error", ex.Message);
            }
            return View("Create");
        }
        [HttpPost]
        public ActionResult Delete(int Id) 
        {
            try
            {
                Employee.Delete(Id);
                return Json("SUCCESS");
            }
            catch (Exception ex) 
            {
                return Json(ex.Message);
            }
        }
    }
}