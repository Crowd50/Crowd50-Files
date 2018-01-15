namespace Crowd50.Controllers
{
    using System.Web.Mvc;

    public class CampaignDiscountController : Controller
    {
        // GET: CampaignDiscount
        public ActionResult Index()
        {
            return View();
        }

        // GET: CampaignDiscount/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CampaignDiscount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CampaignDiscount/Create
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

        // GET: CampaignDiscount/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CampaignDiscount/Edit/5
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

        // GET: CampaignDiscount/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CampaignDiscount/Delete/5
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
