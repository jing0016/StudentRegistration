using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; 
using Final.Models;
using Final.Models.DataAccess;

namespace Final.Controllers
{
    public class StudentsController : Controller
    {
        private readonly RegistrationDbContext _context;

        public StudentsController(RegistrationDbContext context)
        {
            _context = context;
        }

        // GET: Student Number
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(StudentNumberViewModel studentNumber)
        {
            //Retrieve the student with the user entered student number.
            Student student = _context.Student.FirstOrDefault(m => m.StudentNum == studentNumber.StudentNum);

            //If not found, show the same view with the error message: "The entered student number is not valid!"
            if(student == null)
            {
                ModelState.AddModelError("StudentNum", "The entered student number is not valid!");
                return View(studentNumber);
            }
            else
            {
                HttpContext.Session.SetString("StudentNum",student.StudentNum);
            }

            //If found, save the student number is the session. 
            //From now on, Whenever you need the student data, get the student's number from the session 
            //and retrieve the student from the database with the student number. 
            //Redirect to the Index action of the Registrations controller


            return RedirectToAction("Index", "Registrations");
        }
        
    }
}
