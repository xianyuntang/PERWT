using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SocialEngineeringReporting.Models
{
    public class ReportingModel
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Result { get; set; }
        public bool? SandBox { get; set; }
        public string SandBoxResult { get; set; }
        public bool? Spam { get; set; }
        public DateTime ReplyTime { get; set; }
        public int? Score { get; set; }



        
        [Display(Name ="您的帳號")]
        public string Account { get; set; }
        
        [Required(ErrorMessage = "請輸入信件主旨關鍵字")]
        [Display(Name = "通報主旨")]
        public string Subject { get; set; }


       
    }
}