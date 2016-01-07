using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ProjetASPnet.Models;

namespace ProjetASPnet.Controllers
{
    public class EntreprisesController : Controller
    {
        private MeditRisquesEntities db = new MeditRisquesEntities();

        // GET: Entreprises
        public ActionResult Index()
        {
            return View(db.Entreprise.ToList());
        }

        public ActionResult ChoixEntreprise(decimal id)
        {
            if (ModelState.IsValid)
            {
                List<ContratTravailSoumis> contratsDesTravailleursDeLentreprise = db.ContratTravailSoumis.Include("Travailleur").Include("Entreprise").Where(ct => ct.numero == id).ToList();

                if (contratsDesTravailleursDeLentreprise.Count > 0) { 
                    TempData["listTravailleursEntrep"] = contratsDesTravailleursDeLentreprise.ToList();
                    return RedirectToAction("Index", "ContratTravailSoumis");
                }
                return HttpNotFound();
            }
            else
                return HttpNotFound();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
