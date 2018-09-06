using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace SocialEngineeringReporting.Models
{
    public class ReportingServicesModel
    {
        [Display(Name ="起：")]
        [Required]
        public DateTime StartTime { get; set; }
        [Display(Name = "迄：")]
        [Required]
        public DateTime EndTime { get; set; }

       

    }
}