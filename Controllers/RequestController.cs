using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NGODP.Models;
using NGODP.Services;

namespace NGODP.Controllers
{
    public class RequestController : Controller
    {
        internal ngodpContext _ctx;
        Student s = new Student();
        Request e = new Request();

        public RequestController(ngodpContext Context)
        {
             this._ctx = Context;
        }

        public IActionResult Index()
        {
            var list = _ctx.Requests;

            foreach (Request r in list)
            {
                r.Filedate = DateTime.Parse(r.Filedate).ToString("MM/dd/yyyy");
                r.Releasedate = DateTime.Parse(r.Releasedate).ToString("MM/dd/yyyy");
            }

            return View(list);
        }

        public IActionResult Info(string Refno)
        {
            return View();
        }

        public IActionResult History()
        {
            var log = _ctx.History.ToList();

            foreach (History r in log)
            {
                s = _ctx.Students.Find(r.Student);
                r.Student = s.Fname + " " + s.Lname;

                string dt = DateTime.Parse(r.Timedate).ToString("MM/dd/yyyy");
                r.Timedate = dt;

                e = _ctx.Requests.Find(r.Requestref);

                r.Requestref = e.Type;
            }

            return View(log);
        }

        public IActionResult Manage(string ReferenceNumber)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update(string ReferenceNumber)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string ReferenceNumber)
        {
            return View();
        }
    }
}
