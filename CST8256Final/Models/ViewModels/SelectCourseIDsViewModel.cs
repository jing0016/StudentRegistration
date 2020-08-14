using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    public class SelectCourseIDsViewModel
    {
        [Required(ErrorMessage = "Please select at least one course.")]
        [Display(Name = "Course IDs")]
        public List<string> CourseIds { get; set; }
    }
}
