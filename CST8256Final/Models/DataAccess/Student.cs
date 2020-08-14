using System;
using System.Collections.Generic;

namespace Final.Models.DataAccess
{
    public partial class Student
    {
        public Student()
        {
            Registration = new HashSet<Registration>();
        }

        public string StudentNum { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public ICollection<Registration> Registration { get; set; }
    }
}
