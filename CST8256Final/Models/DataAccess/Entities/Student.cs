using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Final.Models.DataAccess
{
    public partial class Student
    {
        private List<Course> registeredCourses = null;

        public List<Course> GetRegisteredCourses(RegistrationDbContext _context)
        {
            if (registeredCourses != null) return registeredCourses;

            var registrations = (from r in _context.Registration where r.StudentStudentNum == StudentNum select r).Include(r => r.CourseCourse);

            registeredCourses = new List<Course>();

            foreach (Registration registration in registrations)
            {
                Course co = registration.CourseCourse;
                registeredCourses.Add(co);
            }
            return registeredCourses;
        }

        public int GetHoursOfCurrentRegisteredCourses(RegistrationDbContext _context)
        {
            if (registeredCourses == null)
                GetRegisteredCourses(_context);

            int hours = 0;
            foreach (Course c in registeredCourses)
            {
                hours += c.HoursPerWeek.Value;
            }

            return hours;
        }
    }
}
