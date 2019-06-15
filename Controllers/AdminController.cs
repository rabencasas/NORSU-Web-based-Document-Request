using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NGODP.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NGODP.Services;

namespace NGODP.Controllers

{
    [Authorize]
    public class AdminController : Controller
    {
        internal ngodpContext _ctx;
        internal IGenerator _generate;

        internal ISmsSender _smssender;

        public AdminController(ngodpContext Context,IGenerator Generator, ISmsSender Sender)
        {
            _ctx = Context;
            _generate = Generator;
            _smssender = Sender;
        }
        public IActionResult Home()
        {
            List<Request> rs = _ctx.Requests.ToList();
            List<RequestViewModel> m = new List<RequestViewModel>();

            List<Request> rs_tor = rs.Where(x => x.Status == "Released" && x.Type == "Transcript of Record").ToList();
            List<Request> rs_dpl = rs.Where(x => x.Status == "Released" && x.Type == "Diploma").ToList();

            float tor = 0;
            float dpl = 0;

            foreach (Request a in rs_tor)
            {
                tor += float.Parse(a.Amount);
            }
            foreach (Request b in rs_dpl)
            {
                dpl += float.Parse(b.Amount);
            }

            ViewData["releases"] = rs_tor.Count() + rs_dpl.Count();
            ViewData["ttl_tor"] = tor;
            ViewData["ttl_dpl"] = dpl;
            ViewData["total"] = tor + dpl;

            foreach (Request r in rs)
            {

                    Student s = _ctx.Students.Find(r.Student);
                    string name = s.Fname + " " + s.Mname + " " + s.Lname;

                RequestViewModel x = new RequestViewModel(){

                        Refno = r.Refno,
                        Student = name,
                        StudentId = r.Student,
                        Filedate = r.Filedate,
                        Type = r.Type,
                        Purpose = r.Purpose,
                        Releasedate = DateTime.Parse(r.Releasedate).ToShortDateString(),
                        Lacking = r.Lacking,
                        Status = r.Status,
                        Course = s.Course,
                        MobileNo = s.Mobileno,
                        Email = s.Email,
                        Amount = r.Amount
                };

                m.Add(x);
            }

            return View(m);
        }

        [HttpPost]
        public IActionResult Home(string Id)
        {
            var z = _ctx.Requests.Find(Id);

            if (z == null)
            {
                ViewData["resultmsg"] = "No reference number found in the database.";
                ViewData["resultstat"] = "0";
            }
            else
            {
                ViewData["resultmsg"] = "A result was found in the database.";
                ViewData["resultstat"] = "1";

                ViewData["refno"] = z.Refno;
            }

            List<Request> rs = _ctx.Requests.ToList();
            List<RequestViewModel> m = new List<RequestViewModel>();

            List<Request> rs_tor = rs.Where(x => x.Status == "Released" && x.Type == "Transcript of Record").ToList();
            List<Request> rs_dpl = rs.Where(x => x.Status == "Released" && x.Type == "Diploma").ToList();

            float tor = 0;
            float dpl = 0;

            foreach (Request a in rs_tor)
            {
                tor += float.Parse(a.Amount);
            }
            foreach (Request b in rs_dpl)
            {
                dpl += float.Parse(b.Amount);
            }

            ViewData["releases"] = rs_tor.Count() + rs_dpl.Count();
            ViewData["ttl_tor"] = tor;
            ViewData["ttl_dpl"] = dpl;
            ViewData["total"] = tor + dpl;

            foreach (Request r in rs)
            {

                    Student s = _ctx.Students.Find(r.Student);
                    string name = s.Fname + " " + s.Mname + " " + s.Lname;

                RequestViewModel x = new RequestViewModel(){

                        Refno = r.Refno,
                        Student = name,
                        StudentId = r.Student,
                        Filedate = r.Filedate,
                        Type = r.Type,
                        Purpose = r.Purpose,
                        Releasedate = DateTime.Parse(r.Releasedate).ToShortDateString(),
                        Lacking = r.Lacking,
                        Status = r.Status,
                        Course = s.Course,
                        MobileNo = s.Mobileno,
                        Email = s.Email,
                        Amount = r.Amount
                };

                m.Add(x);
            }

            return View(m);
        }
        public IActionResult ManageRequest(string Id)
        {
            Request r = _ctx.Requests.Find(Id);
            string x = r.Student;

            Student s = _ctx.Students.Find(x);

            RequestViewModel a = new RequestViewModel();
            a.Refno = r.Refno;
            a.Student = string.Concat(s.Fname," ",s.Mname," ",s.Lname);
            a.StudentId = r.Student;
            a.Filedate = DateTime.Parse(r.Filedate).ToLongDateString();
            a.Type = r.Type;
            a.Purpose = r.Purpose;
            a.Releasedate = DateTime.Parse(r.Releasedate).ToLongDateString();
            a.Lacking = r.Lacking;
            a.Status = r.Status;
            a.Comments = r.Comments;

            a.Course = s.Course;
            a.MobileNo = s.Mobileno;
            a.Email = s.Email;

            a.Amount = r.Amount;

            return View(a);
        }
        public IActionResult Requests()
        {
            List<Request> rs = _ctx.Requests.ToList();
            List<RequestViewModel> m = new List<RequestViewModel>();

            foreach (Request r in rs)
            {

                    Student s = _ctx.Students.Find(r.Student);
                    string name = s.Fname + " " + s.Mname + " " + s.Lname;

                RequestViewModel x = new RequestViewModel(){

                        Refno = r.Refno,
                        Student = name,
                        StudentId = r.Student,
                        Filedate = r.Filedate,
                        Type = r.Type,
                        Purpose = r.Purpose,
                        Releasedate = DateTime.Parse(r.Releasedate).ToShortDateString(),
                        Lacking = r.Lacking,
                        Status = r.Status,
                        Course = s.Course,
                        MobileNo = s.Mobileno,
                        Email = s.Email,
                        Amount = r.Amount
                };

                m.Add(x);
            }

            return View(m);
        }
        public IActionResult Students()
        {
            var list = _ctx.Students.ToList();

            return View(list);
        }
        public IActionResult Users()
        {
            var list = _ctx.Users.ToList();

            return View(list);
        }
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddUser(string Uname, string Fname, string Lname, string Gender, string Pwd, string Confirm)
        {
            if (Confirm == Pwd)
            {
                User x = new User(){

                    Uname = Uname,
                    Fname = Fname,
                    Lname = Lname,
                    Gender = Gender,
                    Pwd = Pwd
                };

                _ctx.Users.Add(x);

                _ctx.SaveChanges();

                return RedirectToAction("Users");
            }
            else
            {
                ViewData["err"] = "Incorrect confirmation password.";

                return View();
            }
        }
        public IActionResult Student(string Id)
        {
            Student s = _ctx.Students.Find(Id);

            return View(s);
        }

        public IActionResult RequestHistory()
        {
            List<HistoryViewModel> hs = new List<HistoryViewModel>();
            HistoryViewModel h;

            var list = _ctx.History.ToList();

            foreach (History r in list)
            {
                h = new HistoryViewModel(){
                    
                    Timestamp = r.Timestamp,
                    Timedate = string.Concat(DateTime.Parse(r.Timedate).ToString("dddd - MMMM dd, yyyy")," ",r.Timestamp),
                    Requestref = r.Requestref,
                    Type = _ctx.Requests.Find(r.Requestref).Type,

                };

                hs.Add(h);
            }

            hs.Reverse();

            return View(hs);
        }

        [HttpPost]
        public IActionResult RelesaeRequest()
        {
            return View();
        }
        
        public IActionResult ApproveRequest(string Id)
        {
                Request x = _ctx.Requests.Find(Id);

                Student y = _ctx.Students.Find(x.Student);

                string no = y.Mobileno;

                x.Status = "Approved";

                _ctx.Requests.Update(x);

                _ctx.SaveChanges();

                _smssender.SendMessage(no,"Your request " + x.Type + " has been approved today.","TR-NGODP507563_43D1K");

                TempData["a_msg"] = "A text message is being sent to the student for notification.";

                return RedirectToAction("Requests");

                TempData["a_msg"] = "An sms notification during the approval but was unable to send due to a system error. Please check internet connectivity.";

                return RedirectToAction("Requests");
        }

        public IActionResult PendingRequest(string Id)
        {
            Request x = _ctx.Requests.Find(Id);

            x.Status = "Pending";

            _ctx.Requests.Update(x);

            _ctx.SaveChanges();

            return RedirectToAction("Requests");
        }

        public IActionResult SetProcessRequest(string Id)
        {
            Request x = _ctx.Requests.Find(Id);

            x.Status = "On Process";

            _ctx.Requests.Update(x);

            _ctx.SaveChanges();

            return RedirectToAction("Requests");
        }

        public IActionResult SetReleaseDate(string Id)
        {
            ViewData["id"] = Id;

            return View();
        }
        [HttpPost]
        public IActionResult SetReleaseDate(string Id, string Releasedate)
        {
            Request x = _ctx.Requests.Find(Id);

            x.Releasedate = Releasedate;
            x.Status = "For Release";

            _ctx.Requests.Update(x);

            _ctx.SaveChanges();

            return RedirectToAction("Requests","Admin");
        }

        [HttpPost]
        public IActionResult UpdateComment(string Id, string Comment)
        {
            Request x = _ctx.Requests.Find(Id);

            x.Comments = Comment;

            _ctx.Requests.Update(x);

            _ctx.SaveChanges();

            return RedirectToAction("Requests");
        }

        public IActionResult ReleaseDocument(string Id)
        {
            string stud = _ctx.Requests.Find(Id).Student;
            string no = _ctx.Students.Find(stud).Mobileno;

            Request r = _ctx.Requests.Find(Id);

            History x = new History(){
                
                Timestamp = _generate.TimeStamp(),
                Timedate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"),
                Requestref = Id,
                Student = stud,
            };

            _ctx.History.Add(x);

            // update status
            r.Status = "Released";

            _ctx.Requests.Update(r);

            _ctx.SaveChanges();

            TempData["id"] = r.Refno;
            TempData["type"] = r.Type;
            TempData["filedate"] = r.Filedate;
            TempData["amt"] = r.Amount;

            try
            {
                _smssender.SendMessage(no,"Your requeset " + r.Type + " has been released today.","TR-NGODP507563_43D1K");

                ViewData["r"] = "1";
                ViewData["result"] = "Successfully sent a release notification sms.";
            }
            catch (System.Exception)
            {
                ViewData["r"] = "0";
                ViewData["result"] = "Unable to send the student a release notification. Please check internet connectivity.";
            }

            return View("ReleaseSuccessful");
        }

        
        [HttpPost]
        public IActionResult SearchStudent(string Keyword)
        {
            var studs = _ctx.Students.Where(x => x.Fname.Contains(Keyword) || x.Mname.Contains(Keyword) || x.Lname.Contains(Keyword));

            ViewData["total"] = studs.Count();

            return View(studs);
        }
        
        public IActionResult SearchStudent()
        {
            List<Student> list = new List<Student>();

            return View(list);
        }

        [HttpPost]
        public IActionResult SearchRequest(string Id)
        {
            var x = _ctx.Requests.Find(Id);

            if (x == null)
            {
                x = new Request(){

                    Refno = "x"
                };                
            }

            return View(x);
        }
        
        public IActionResult SearchRequest()
        {
            Request x = new Request(){

                Refno = "z"
            };

            return View(x);
        }

        public IActionResult Feedbacks()
        {
            var list = _ctx.Feedbacks.ToList();

            list.Reverse();

            return View(list);
        }

        public IActionResult Messaging()
        {
            string uname = HttpContext.User.FindFirst("username").Value;

            User s = _ctx.Users.Find(uname);

            uname = string.Concat(s.Fname," ",s.Lname);

            ViewData["stud"] = uname;

            return View();
        }
    }
}