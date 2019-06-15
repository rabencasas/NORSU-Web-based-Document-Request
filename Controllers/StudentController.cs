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
using DinkToPdf;
using DinkToPdf.Contracts;

namespace NGODP.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        internal ngodpContext _ctx;
        internal IGenerator _generate;

        public StudentController(ngodpContext Context, IGenerator Generator)
        {
            _ctx = Context;
            _generate = Generator;
        }

        // redirects students to home page & display the pending and for  release document requests
        public IActionResult Home() // done
        {
            string unme = HttpContext.User.FindFirst("username").Value;
            List<RequestViewModel> final= new List<RequestViewModel>();

            // get total: all
            int tor = _ctx.Requests.Where(x => x.Student == unme && x.Type == "Transcript of Record").ToList().Count();
            int dpl = _ctx.Requests.Where(x => x.Student == unme  && x.Type == "Diploma").ToList().Count();

            ViewData["tor"] = tor;
            ViewData["dpl"] = dpl;

            var list = _ctx.Requests.Where(x => x.Student == unme);

            foreach (Request r in list)
            {
                r.Filedate = DateTime.Parse(r.Filedate).ToString("MM/dd/yyyy");
                r.Releasedate = DateTime.Parse(r.Releasedate).ToString("MM/dd/yyyy");

                RequestViewModel x = new RequestViewModel(){

                    Refno = r.Refno,
                    Filedate = r.Filedate,
                    Type = r.Type,
                    Releasedate = r.Releasedate,
                    Status = r.Status,
                    Amount = r.Amount,
                    Feedback = r.Feedback,
                };

                final.Add(x);
            }

            // get notification
            int result = _ctx.Requests.Where(z => z.Student == unme && z.Status == "Released" && z.Notification == "0" && z.Feedback == "0").ToList().Count();

            ViewData["notification"] = result;

            return View(final);
        }

        // redirects students to profile page & display current student information    
        public IActionResult Profile() // done
        {
            Student s = _ctx.Students.Find(HttpContext.User.FindFirst("username").Value);

            bool isfilled = s.Elemname == "" || s.Elemname == string.Empty ? false : true;

            if (!isfilled)
            {
                @ViewData["msg"] = "Please complete your registration information by supplying academic information ";
            }

            return View(s);
        }

        public IActionResult UpdateProfile()  // not implemented
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateProfile(string mobile, string email, string isgrad, string gradyr, string course, string major, string elemname, string elemaddr, string elemyr, string secname, string secaddr, string secyr, string tername, string teraddr, string teryr)  // not implemented
        {
            string u = HttpContext.User.FindFirst("username").Value;

            Student x = _ctx.Students.Find(u);

            x.Mobileno = mobile;
            x.Email = email;
            x.Isgraduate = isgrad;
            
           if (isgrad == "Yes")
           {
                if (gradyr != "N/A" || gradyr != "NA" || gradyr != "")
                {
                    x.Graduateyr = int.Parse(gradyr);
                }
           }

            x.Course = course;
            x.Major = major;
            
            x.Elemname = elemname;
            x.Elemaddr = elemaddr;
            x.Elemyr = int.Parse(elemyr);

            x.Secname = secname;
            x.Secaddr = secaddr;
            x.Secyr = int.Parse(secyr);

            x.Tername = tername;
            x.Teraddr = teraddr;
            
            int yr = 0;
            if (teryr != "N/A" || teryr != "NA" || teryr != "")
            {
                // x.Teryr = int.Parse(teryr);
                int.TryParse(teryr, out yr);
            }

            x.Teryr = yr;

            _ctx.Students.Update(x);

            _ctx.SaveChanges();

            ViewData["yesmsg"] = "Your profile has been successfully updated.";

            return View();
        }

        // redirects student to create request page
        public IActionResult CreateRequest() // done
        {
            return View("Requests/CreateRequest");
        }

        [HttpPost]
        public IActionResult ConfirmRequest(string Id) // not yet
        {
            return View("Requests/ConfirmRequest");
        }

        [HttpPost]
        public IActionResult CreateRequest(string Type,string Purpose,string OtherPurpose) // almost done
        {
            Request r;
            string s = HttpContext.User.FindFirst("username").Value;


            // no chosen
            if (Type == "0")
            {
                ViewData["result"] = "Please choose a valid type of request from selection.";  

                return View("Requests/CreateRequest");                  
            }
            // has chosen
            else
            {
                // CHOSEN TOR
                if (Type == "1")
                {
                    if (Purpose != "0")
                    {
                        r = new Request(){

                            Refno = _generate.RequestID(),
                            Student = s,
                            Filedate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"),
                            Releasedate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"),
                            Type = "Transcript of Record",
                            Purpose = Purpose,
                            Status = "Pending",
                            Notification = "0",
                            Feedback = "0"
                        };

                        if (Purpose == "Other")
                        {
                            r.Purpose = OtherPurpose;
                        }
                        else
                        {
                            r.Purpose = Purpose;
                        }

                        if (r.Purpose == "Transfer" || r.Purpose == "Further Studies")
                        {
                            r.Amount = "500.00";
                        }
                        else
                        {
                            r.Amount = "200.00";
                        }

                        _ctx.Requests.Add(r);

                        _ctx.SaveChanges();

                        ViewData["result"] = "1";
                        ViewData["amt"] = r.Amount;

                        return View("Requests/CreateRequest");

                    }
                    else
                    {
                        ViewData["result"] = "Please state the purpose from selection or specify in the textbox.";  

                        return View("Requests/CreateRequest");                                                        
                    }
                }

                // CHOSEN DIPLOMA: HAS NO PURPOSE
                if (Type == "2")
                {
                    r = new Request(){

                        Refno = _generate.RequestID(),
                        Student = s,
                        Filedate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"),
                        Releasedate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"),
                        Type = "Diploma",
                        Status = "Pending"
                    };

                    // Diploma charging
                    // get request count
                    int ctr = _ctx.Requests.Where(x => x.Student == s && x.Type == "Diploma").ToList().Count();

                    r.Amount = ctr >= 1 ? "300.00" : "50.00";

                    _ctx.Requests.Add(r);

                    _ctx.SaveChanges();

                    ViewData["result"] = "1";
                    ViewData["amt"] = r.Amount;

                    return View("Requests/CreateRequest");
                }
            }

        return View("Requests/CreateRequest");                  

        }

        public IActionResult Request(string Id) // done
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

            a.Course = s.Course;
            a.MobileNo = s.Mobileno;
            a.Email = s.Email;

            a.Amount = r.Amount;

            return View(a);
        }

        public IActionResult ConfirmCancel(string Id) // done
        {
            Request r = _ctx.Requests.Find(Id);

            ViewData["id"] = Id;
            ViewData["type"] = r.Type;
            ViewData["filedate"] = r.Filedate.ToString();

            return View("Requests/ConfirmCancel");
        }

        public IActionResult DeleteRequest(string Id) // done
        {
            Request x = _ctx.Requests.Find(Id);
            _ctx.Requests.Remove(x);

            _ctx.SaveChanges();

            return RedirectToAction("Home");
        }

        public IActionResult RequestHistory() // done
        {
            string unme = HttpContext.User.FindFirst("username").Value;

            List<HistoryViewModel> hs = new List<HistoryViewModel>();
            HistoryViewModel h;

            var list = _ctx.History.Where(x => x.Student == unme);

            foreach (History r in list)
            {
                h = new HistoryViewModel(){
                    
                    Timestamp = r.Timestamp,
                    Timedate = DateTime.Parse(r.Timedate).ToString("MM/dd/yyyy"),
                    Requestref = r.Requestref,
                    Type = _ctx.Requests.Find(r.Requestref).Type,

                };

                hs.Add(h);
            }

            hs.Reverse();

            return View(hs);
        }

        public IActionResult ApplicationForm(string Id)
        {
            var r = _ctx.Requests.Find(Id);
            var s = _ctx.Students.Find(r.Student);

            string uname = HttpContext.User.FindFirst("username").Value;

            ApplicationForm x = new ApplicationForm(){

                RefNo = Id,
                FilingDate = DateTime.Parse(r.Filedate).ToShortDateString(),
                
                Name = string.Concat(s.Fname," ",s.Mname," ",s.Lname).ToUpper(),
                Gender = s.Gender.ToUpper(),
                CivilStat = s.CivilStatus.ToUpper(),

                BDate = DateTime.Parse(r.Filedate).ToShortDateString(),
                BPlace = s.Birthplace.ToUpper(),
                Address = s.Address.ToUpper(),
                Mobile = s.Mobileno,
                Email = s.Email,
                Course = s.Course.ToUpper(),
                Major = s.Major.ToUpper(),

                Elementary = string.Concat(s.Elemname," - ",s.Elemaddr," - ",s.Elemyr.ToString()).ToUpper(),
                Secondary = string.Concat(s.Secname," - ",s.Secaddr," - ",s.Secyr.ToString()).ToUpper(),
                College = string.Concat(s.Tername," - ",s.Teraddr," - ",s.Teryr.ToString()).ToUpper(),

                Type = r.Type.ToUpper(),
                Purpose = r.Purpose.ToUpper()
            };

            _generate.Report("Requests/ApplicationForm.cshtml");

            return View(x);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] 
        public IActionResult Error() // unnecessary
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AddFeedback(string Id)
        {
            ViewData["id"] = Id;

            return View();
        }

        [HttpPost]
        public IActionResult AddFeedback(string Id, string Message)
        {
            Feedback x = new Feedback(){

                Id = _generate.FeedbackID(),
                TimeAndDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"),
                Message = Message,
            };

            Request y = _ctx.Requests.Find(Id);

            y.Feedback = "1";

            _ctx.Requests.Update(y);

            _ctx.Feedbacks.Add(x);

            _ctx.SaveChanges();

            ViewData["msg"] = "Your feedback was successfully sent to the Office of the Registrar.";

            return View();
        }

        public IActionResult Notifications()
        {
            List<Request> l = _ctx.Requests.ToList().Where(x => x.Status == "Released").ToList();

            return View(l);
        }

        public IActionResult UpdateNotifications(string Id)
        {
            Request x = _ctx.Requests.Find(Id);

            x.Notification = "1";

            _ctx.Requests.Update(x);

            _ctx.SaveChanges();

            return RedirectToAction("Notifications","Student");
        }

        public IActionResult NoFeedback(string Id)
        {
            Request x = _ctx.Requests.Find(Id);

            x.Feedback = "2";

            _ctx.Requests.Update(x);

            _ctx.SaveChanges();

            return RedirectToAction("Notifications","Student");
        }

        public IActionResult Messaging()
        {
            string uname = HttpContext.User.FindFirst("username").Value;

            Student s = _ctx.Students.Find(uname);

            uname = string.Concat(s.Fname," ",s.Lname);

            ViewData["stud"] = uname;

            return View();
        }
     }
}
