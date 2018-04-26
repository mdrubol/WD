using WD.DataAccess.Abstract;
using WD.Domain.Entity;
using WD.Domain.Managers;
using System;
using System.Linq;
using System.Web.Mvc;
using WD.WebApi.Models;
using WD.DataAccess.Enums;
using WD.DataAccess.Logger;
using WD.DataAccess.Context;
using System.Data;
using System.Collections.Generic;
using WD.DataAccess.Helpers;
using WD.DataAccess.Parameters;
using System.Reflection;
namespace WD.WebApi.Controllers
{
   

    public class HomeController : Controller
    {
        [WD.DataAccess.Attributes.Custom(Name = "Employee")]
        public class tbEmployee
        {
            [WD.DataAccess.Attributes.Custom(IsPrimary = true)]
            public int ID { get; set; }
            public string Name { get; set; }
            public string Country { get; set; }
            public string Location { get; set; }
        }
        private  ICommands ICommands { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ILogger.Info("Start Time:" + DateTime.UtcNow);
            base.OnActionExecuting(filterContext);
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            ILogger.Info("End Time:" + DateTime.UtcNow);
        }
        [HttpGet]
        public  ActionResult Index()
        {
           
            ViewBag.Title = "WD";
            return View();  
        }
        public async System.Threading.Tasks.Task<ActionResult> SendEmail() {

            await System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient("smtp.wdc.com");
                    mail.From = new System.Net.Mail.MailAddress("shahid.kochak@wdc.com");
                    mail.To.Add("shahid.kochak@wdc.com");
                    mail.Subject = "Test Mail";
                    mail.Body = "This is for testing SMTP mail from GMAIL";
                    SmtpServer.Port = 25;
                    //SmtpServer.Credentials = new System.Net.NetworkCredential("shahidkochak@gmail.com", "XXXXXXX");
                    //SmtpServer.EnableSsl = false;
                    SmtpServer.Send(mail);
                }

            });
            return null;
        
        }
        [HttpGet]
        public ActionResult Connect(int dbType)
        {
            GeneralModel<Employee> input = new GeneralModel<Employee>();
            input.DbType = dbType;
            ICommands = new DbContext(dbType).ICommands;
            input.IList = ICommands.GetList<Employee>();
            return View(input);
        }
        [HttpGet]
        public ActionResult Insert(int dbType)
        {
            GeneralModel<Employee> input = new GeneralModel<Employee>();
            input.DbType = dbType;
            input.Entity = new Employee() { EmployeeId = new Random().Next(900), Provider = dbType.ToString() };
            return View(input);
        }
        [HttpPost]
        public ActionResult Insert(GeneralModel<Employee> input)
        {
            ICommands = CustomerManager.Instance(input.DbType).ICommands;
            input.Entity.EmployeeId = new Random().Next(1000);
            ICommands.Insert<Employee>(input.Entity);

            return RedirectToAction("Connect", new { dbType = input.DbType });
        }
        [HttpGet]
        public ActionResult Update(dynamic id,int dbType)
        {
            ICommands = CustomerManager.Instance(dbType).ICommands;
            switch (dbType)
            {
                case DBProvider.Sql:
                    break;
                default:
                    break;
            }
            GeneralModel<Employee> input = new GeneralModel<Employee>();
            input.DbType = dbType;
            string theSql = "SELECT * FROM TempEmployee WHERE EMPLOYEEID=" + id;
            input.Entity = ICommands.GetEntity<Employee>(theSql);
            return View(input);
        }
        [HttpPost]
        public ActionResult Update(GeneralModel<Employee> input)
        {
            try
            {
                ICommands = CustomerManager.Instance(input.DbType).ICommands;
               // input.Entity.EmployeeId =Convert.ToInt32(((string[])input.Entity.EmployeeId)[0]);
                ICommands.Update(input.Entity);
            }
            catch (Exception exc) {

                ILogger.Error(exc);
            }
            return RedirectToAction("Connect", new { dbType = input.DbType });
        }
        [HttpGet]
        public ActionResult Delete(int id,int dbType)
        {
            ICommands = CustomerManager.Instance(dbType).ICommands;
            Employee input = ICommands.GetEntity<Employee>(x => x.EmployeeId == id);
            ICommands.Delete<Employee>(input);
            return RedirectToAction("Connect", new { dbType = dbType });
        }

     


    }
}
