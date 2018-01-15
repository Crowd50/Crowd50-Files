namespace Crowd50.Controllers
{
    using System.Web.Mvc;

    public class DatabaseLogController : Controller
    {
        // GET: DatabaseLog
        public ActionResult Index()
        {
            return View();
        }

        // GET: DatabaseLog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DatabaseLog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DatabaseLog/Create
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

        // GET: DatabaseLog/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DatabaseLog/Edit/5
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

        // GET: DatabaseLog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DatabaseLog/Delete/5
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
