using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NGODP.Models;

namespace NGODP.Controllers {
    public class AccountController : Controller {
        internal ngodpContext _ctx;

        public AccountController (ngodpContext Context) {
            _ctx = Context;
        }
        
        public IActionResult Register () {
            return View ();
        }
        [HttpPost]
        public IActionResult Register (string fname, string mname, string lname,string suffix,string age,string gender, string civilstat, string addr, string bdate, string bplace, string mobile, string email, string uname, string pwd, string confirm, string isgrad) {

            if (confirm == pwd)
            {
                Student x = new Student(){

                    Fname = fname,
                    Mname = mname,
                    Lname = lname,
                    Suffix = suffix,
                    Age = long.Parse(age),
                    Gender = gender,
                    CivilStatus = civilstat,
                    Address = addr,

                    Mobileno = mobile,
                    Email = email,

                    Birthdate = DateTime.Parse(bdate).ToString("MM/dd/yyyy"),
                    Birthplace = bplace,
                    Isgraduate = isgrad,
                    
                    Uname = uname,
                    Pwd = pwd,
                };

                _ctx.Students.Add(x);

                _ctx.SaveChanges();

                ViewData["yesmsg"] = "You have been successfully registered to the site. You can now login as student.";                
                ViewData["yesmsg2"] = "Complete your academic information in your Profile page.";

                return View();
            } 
            else
            {
                ViewData["fname"] = fname;
                ViewData["mname"] = mname;
                ViewData["lname"] = lname;
                ViewData["suffix"] = suffix;
                ViewData["age"] = age;
                ViewData["gender"] = gender;
                ViewData["civilstat"] = civilstat;
                ViewData["bdate"] = bdate;
                ViewData["bplace"] = bplace;
                ViewData["addr"] = addr;
                ViewData["mobileno"] = mobile;
                ViewData["email"] = email;

                ViewData["uname"] = uname;
                ViewData["isgrad"] = isgrad;


                ViewData["errmsg"] = "Confirm password doesn't match. Please try again.";

                return View();
            }
        }

        public RedirectToActionResult Logout () {
            string pos = HttpContext.User.FindFirst ("position").Value;

            HttpContext.SignOutAsync (scheme: CookieAuthenticationDefaults.AuthenticationScheme);

            if (pos == "Student") {
                // return View ("Login");
                return RedirectToAction("Login");
            } else {
                // return View ("AdminLogin");
                return RedirectToAction("AdminLogin");
            }

        }
        public IActionResult Login () {
            return View ();
        }
        [HttpPost]
        public IActionResult Login (string Uname, string Pwd) {
            var user = _ctx.Students.Find (Uname);

            if (user != null && user.Pwd == Pwd) {
                List<Claim> claims = new List<Claim> () {
                new Claim ("username", Uname),
                new Claim ("position", "Student"),
                };

                ClaimsIdentity identity = new ClaimsIdentity (claims, "cookie");
                ClaimsPrincipal principal = new ClaimsPrincipal (identity);

                HttpContext.SignInAsync (scheme: CookieAuthenticationDefaults.AuthenticationScheme, principal: principal);

                return RedirectToAction ("Home", "Student");
            }

            ViewData["Message"] = "No account found.";

            return View ();
        }
        public IActionResult AdminLogin () {
            return View ();
        }
        [HttpPost]
        public IActionResult AdminLogin (string Uname, string Pwd) {
            var user = _ctx.Users.Find (Uname);

            if (user != null && user.Pwd == Pwd) {
                List<Claim> claims = new List<Claim> () {
                new Claim ("username", Uname),
                new Claim ("position", "Administrator"),
                };

                ClaimsIdentity identity = new ClaimsIdentity (claims, "cookie");
                ClaimsPrincipal principal = new ClaimsPrincipal (identity);

                HttpContext.SignInAsync (scheme: CookieAuthenticationDefaults.AuthenticationScheme, principal: principal);

                // return View("Admin/Index");
                return RedirectToAction ("Home", "Admin");
            }

            ViewData["Message"] = "No account found.";

            return View ();
        }
        public IActionResult Requests()
        {
            var requests = _ctx.Requests.ToList();
            var list = new List<RequestViewModel>();

            foreach (Request r in requests)
            {
                list.Add(
                    
                    new RequestViewModel(){

                        Refno = r.Refno,
                        Student = _ctx.Students.Find(r.Student).Fname + " " + _ctx.Students.Find(r.Student).Lname,
                        StudentId = r.Student,
                        Filedate = DateTime.Parse(r.Filedate.ToString()).ToShortDateString(),
                        Type = r.Type,
                        Purpose = r.Purpose,
                        Releasedate = DateTime.Parse(r.Refno.ToString()).ToShortDateString(),
                        Lacking = r.Lacking,
                        Status = r.Status
                    }
                );
            }

            return View(list);
        }
    }
}