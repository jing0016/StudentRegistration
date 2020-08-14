using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Final.Models
{
    public class StudentNumberViewModel
    {
        [Required]
        [Display(Name = "Student Number")]
        public string StudentNum { get; set; }

    }
}
