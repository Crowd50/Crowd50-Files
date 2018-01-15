namespace Crowd50.Controllers
{
    using System.Web.Mvc;

    public class ApplicationLogController : Controller
    {
        // GET: ApplicationLog
        public ActionResult Index()
        {
            return View();
        }

        // GET: ApplicationLog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ApplicationLog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationLog/Create
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

        // GET: ApplicationLog/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ApplicationLog/Edit/5
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

        // GET: ApplicationLog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ApplicationLog/Delete/5
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
