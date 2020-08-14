using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Final.Models.DataAccess;
using Final.Models;

namespace Final.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly RegistrationDbContext _context;
        private const int Max_Weekly_Hours = 20;

        public RegistrationsController(RegistrationDbContext context)
        {
            _context = context;
        }

        //Complete this method and use it to get a student.
        private Student GetStudent()
        {
            Student student = null;

            //Get student number from the session and retrieve the student with the student number from the database.

            //student = 
            string studentNum = HttpContext.Session.GetString("StudentNum");
            student = _context.Student.FirstOrDefault(m => m.StudentNum == studentNum);
            return student;
        }

        // GET: Registrations
        public IActionResult Index()
        {
            Student student = GetStudent();
            if (student == null )
            {
                return RedirectToAction("Index", "Students");
            }
            var courses = student.GetRegisteredCourses(_context);

            //Pass all necessary data to the view to display the required information.
            //You need look at the view to determine the ways you pass the required data to the view.
            ViewBag.Student = student;


            //You have to replace this empty result with an appropriate view and parameter
            return View(courses);  
        }

        // GET: Registrations/Delete/5
        public IActionResult Delete(string id)
        {
            //Retrieve the registration from the database and remove it. 
            //A registration is determined by the student number and course id.
            string studentNum = HttpContext.Session.GetString("StudentNum");
            Registration registration = (_context.Registration.Where(m => m.CourseCourseId == id && m.StudentStudentNum == studentNum)).FirstOrDefault();
            _context.Registration.Remove(registration);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        public IActionResult AddRegistration()
        {
            Student student = GetStudent();
            if (student == null)
            {
                return RedirectToAction("Index", "Students");
            }
            List<Course> registeredCourses = student.GetRegisteredCourses(_context);

            //Create an available courses which are the courses not contained in the student's registered courses
            //Make sure the display texts in the selection box are in the required format 
            //You need look at the view to see to determine the ways you pass the data to the view.

            List<Course> courses = new List<Course>();
            foreach(Course course in _context.Course.ToList())
            {
                if(!registeredCourses.Contains(course))
                {

                    courses.Add(course);
                }
            }

            List<SelectListItem> unRegisteredCourses = new List<SelectListItem>();
            foreach(Course course in courses)
            {
                unRegisteredCourses.Add(new SelectListItem() { Text = course.CourseId + " - " + course.CourseTitle, Value = course.CourseId });
            }

            this.ViewBag.Courses = new SelectList(unRegisteredCourses, "Value", "Text");

            return View();

        }

        [HttpPost]
        public IActionResult AddRegistration(SelectCourseIDsViewModel selectedCourseIDs)
        {
            Student student = GetStudent();
            if (student == null)
            {
                return RedirectToAction("Index", "Students");
            }
            int hours = student.GetHoursOfCurrentRegisteredCourses(_context);
            string studentNum = HttpContext.Session.GetString("StudentNum");

            //Retrieve selected courses from database
            //if the total hours exceed the allowed max hours, show the same view with an error message
            if(ModelState.IsValid)
            { 
            for (int i=0;i<selectedCourseIDs.CourseIds.Count;i++)
            {
                Course course = (_context.Course.Where(m => m.CourseId == selectedCourseIDs.CourseIds[i])).FirstOrDefault();
                hours += course.HoursPerWeek.Value;
            }
            if(hours > Max_Weekly_Hours)
            {
                ModelState.AddModelError("CourseIds", "Exceed the max weekly hours after adding the selected course(s).");

                AddRegistration();
                return View(selectedCourseIDs);

            }
            else
            {
                for (int i = 0; i < selectedCourseIDs.CourseIds.Count; i++)
                {
                    Registration registration = new Registration
                    { CourseCourseId = selectedCourseIDs.CourseIds[i], StudentStudentNum = studentNum };
                    _context.Registration.Add(registration);
                }
                _context.SaveChanges();
            }
            }

            //Otherwise, for each selected course, create a new registration and save it to the database.
            //Once complete, redirect the user to the Index action to show the updated registration

            return RedirectToAction("Index");
        }


    }
}
