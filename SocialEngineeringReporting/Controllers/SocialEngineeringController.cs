using SocialEngineeringReporting.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace SocialEngineeringReporting.Controllers
{
    public class SocialEngineeringController : Controller
    {


        [HttpGet]
        public ActionResult Index()
        {

            try
            {
                Tuple<string, string, string> Info;
                string ssokey = Request.QueryString["ssokey"].ToString();
                Info = SingleSignOn.SSO(ssokey);
                if (Info.Item1 != "LoginFailed")
                {
                    List<Tuple<string, DateTime, string>> recentReporting = ReadFromDatabaseModel.ReadFromDatabase();
                    Session["Auth"] = SingleSignOn.GetAuth(Info.Item1);
                    Session["Account"] = Info.Item1;
                    Session["Name"] = Info.Item2;
                    Session["Dept"] = Info.Item3;
                    ViewBag.RecentReporting = recentReporting;
                    Session["RecentReporting"] = recentReporting;

                    return View();
                }
            }
            catch
            {
                return View("LoginFailed");
            }



            return View("LoginFailed");
        }

        [HttpPost]
        public ActionResult Index(ReportingModel reporting)
        {

            reporting.Account = (string)Session["Account"];
            reporting.Department = (string)Session["Dept"];
            reporting.Name = (string)Session["Name"];
            reporting.Time = DateTime.Now;
            ViewData["Account"] = (string)Session["Account"];
            if (ModelState.IsValid)
            {
                Session["PresearchPost"] = true;
                WriteToDatabaseModel.WriteToDatabase(reporting);
                SendEmailModel email = new SendEmailModel
                {
                    From = new MailAddress("cdcsereporting@cdc.gov.tw", "釣魚郵件通報系統"),
                    Subject = "【社交通報系統】審核通知",
                    To = ReadFromDatabaseModel.GetMailAddress()
                };
                string mailbody = $"通報主旨：{reporting.Subject}\r\n請盡速檢查該信件是否為釣魚郵件信。";
                email.Send(mailbody);
                return View("Result");
            }
            else
            {
                List<Tuple<string, DateTime, string>> recentReporting = ReadFromDatabaseModel.ReadFromDatabase();

                ViewBag.RecentReporting = recentReporting;
                return View();
            }

        }

        [HttpGet]
        public ActionResult List()
        {

            if (Session["Auth"] != null && (bool)Session["Auth"])
            {
                string sqlcmd = "SELECT id,Subject,Time, Name, Result, ReplyTime, Account FROM SOCIALENGINEERINGREPORTING ORDER BY Result, Time DESC";
                List<List<string>> recentReporting = ReadFromDatabaseModel.ReadFromDatabaseAll(sqlcmd);
                ViewBag.RecentReporting = recentReporting;


                return View();
            }
            else
            {
                return View("LoginFailed");
            }

        }

        [HttpPost]
        public ActionResult List(string button, UpdateReportingModels reporting)
        {

            int i = int.Parse(button.Substring(5));
            WriteToDatabaseModel.UpdateReporting(reporting.Result[i], reporting.Subject[i], (string)Session["Name"],reporting.Id[i]);
            if (Session["Auth"] != null && (bool)Session["Auth"])
            {
                string sqlcmd = "SELECT id,Subject,Time, Name, Result, ReplyTime, Account FROM SOCIALENGINEERINGREPORTING ORDER BY Result, Time DESC";
                List<List<string>> recentReporting = ReadFromDatabaseModel.ReadFromDatabaseAll(sqlcmd);
                ViewBag.RecentReporting = recentReporting;
                SendEmailModel email = new SendEmailModel
                {
                    From = new MailAddress("cdcsereporting@cdc.gov.tw", "釣魚郵件通報系統"),
                    Subject = "【社交通報系統】審核結果通知",
                    To = reporting.Account[i].Split(',').ToList()
                };
                Debug.WriteLine("test2");
                Debug.WriteLine(reporting.Account);
                string mailbody = $"通報主旨：{reporting.Subject[i]}\r\n目前審核狀態：{reporting.Result[i]}\r\n感謝您的回報";
                email.Send(mailbody);
                Debug.WriteLine("test3");
                return View();
            }
            else
            {
                return View("LoginFailed");
            }


        }

        [HttpGet]
        public ActionResult GetRecentReport()
        {
            string partitialTitle = Request.QueryString["partitialTitle"];

            List<Tuple<string, DateTime, string>> recentReporting = (List<Tuple<string, DateTime, string>>)Session["RecentReporting"];


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[");
            Debug.WriteLine(recentReporting.Count);
            for (int i = 0; i < recentReporting.Count; i++)
            {

                if (recentReporting[i].Item1.ToString().IndexOf(partitialTitle) >= 0)
                {
                    sb.AppendLine("{");
                    sb.AppendLine($"\"Subject\":\"{recentReporting[i].Item1}\",");
                    sb.AppendLine($"\"Time\":\"{recentReporting[i].Item2}\",");
                    sb.AppendLine($"\"Result\":\"{recentReporting[i].Item3}\"");
                    sb.AppendLine("},");
                }

            }

            sb.AppendLine("]");
            sb.Replace("},\r\n]\r\n", "}\r\n]");

            return Content(sb.ToString());
        }

        [HttpGet]
        public ActionResult Reporting()
        {
            if (Session["Auth"] != null && (bool)Session["Auth"])
            {
                return View();
            }
            else
            {
                return View("LoginFailed");
            }
        }

        [HttpPost]
        public ActionResult Reporting(ReportingServicesModel query)
        {
            //TODO 未來可修改sqlcmd 來改變單位呈現方式
            if (Session["Auth"] != null && (bool)Session["Auth"])
            {
                string startTime = query.StartTime.ToString("yyyy-MM-dd");
                string endTime = query.EndTime.AddDays(+1).ToString("yyyy-MM-dd");
                string sqlcmd = $@"SELECT Subject, MIN(Time) as Time, Name, Department, Result FROM SOCIALENGINEERINGREPORTING
                            WHERE Time BETWEEN '{startTime}' AND '{endTime}'
                            GROUP BY Subject, Name, Result,Department ORDER BY  Min(Time)";
                Debug.WriteLine(sqlcmd);
                List<List<string>> reportingData = ReadFromDatabaseModel.ReadFromDatabaseAll(sqlcmd);
                ViewBag.reportingData = reportingData;
                return View();
            }
            else
            {
                return View("LoginFailed");
            }

        }




    }
}