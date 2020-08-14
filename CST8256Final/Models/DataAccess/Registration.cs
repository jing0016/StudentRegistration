using System;
using System.Collections.Generic;

namespace Final.Models.DataAccess
{
    public partial class Registration
    {
        public string CourseCourseId { get; set; }
        public string StudentStudentNum { get; set; }

        public Course CourseCourse { get; set; }
        public Student StudentStudentNumNavigation { get; set; }
    }
}
