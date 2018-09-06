using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialEngineeringReporting.Models
{
    public class UpdateReportingModels
    {
        public List<string> Id { get; set; }
        public List<string> Account { get; set; }
        public List<string> Subject { get; set; }
        public List<DateTime> Time { get; set; }
        public List<string> Name { get; set; }
        public List<string> Result { get; set; }
        public List<DateTime> UpdateTime { get; set; }
    }

}