namespace Crowd50.Controllers
{
    using System.Web.Mvc;

    public class CampaignTierController : Controller
    {
        // GET: CampaignTier
        public ActionResult Index()
        {
            return View();
        }

        // GET: CampaignTier/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CampaignTier/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CampaignTier/Create
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

        // GET: CampaignTier/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CampaignTier/Edit/5
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

        // GET: CampaignTier/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CampaignTier/Delete/5
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
