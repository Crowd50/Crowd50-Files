namespace Crowd50.Controllers
{
    using AutoMapping;
    using Crowd50.Custom;
    using Crowd50.Models;
    using Crowd50_DataAccess;
    using Crowd50_DataAccess.Models;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Web.Mvc;

    public class CampaignController : Controller
    {
        public CampaignController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Datasource"].ConnectionString;
            string logFile = ConfigurationManager.AppSettings["LogFile"];

            _CampaignDataAccess = new CampaignDataAccess(connectionString, logFile);
        }

        private CampaignDataAccess _CampaignDataAccess;

        // GET: Campaign
        public ActionResult Index()
        {
            ActionResult oResult = null;
            try
            {
                List<CampaignPO> AllCampaigns = (List<CampaignPO>)AutoMap<Campaign>.To<CampaignPO>
                                                        (_CampaignDataAccess.ViewAllCampaigns());
                oResult = View(AllCampaigns);
            }
            catch(SqlException exception)
            {
                string message = "";
            }
            return oResult;
        }

        // GET: Campaign/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Campaign/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Campaign/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Campaign/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Campaign/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Campaign/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Campaign/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
