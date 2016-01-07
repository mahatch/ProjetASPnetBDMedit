using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using ProjetASPnet.Models;

namespace ProjetASPnet.Controllers
{
    public class ContratTravailSoumisController : Controller
    {
        private MeditRisquesEntities db = new MeditRisquesEntities();

        // GET: ContratTravailSoumis
        public ActionResult Index()
        {
            List<ContratTravailSoumis> listContratTravailSoumises = TempData["listTravailleursEntrep"] as List<ContratTravailSoumis>;
            return View(listContratTravailSoumises);
        }

        private void SetTravailleur(decimal id)
        {
            ContratTravailSoumis contratSelectionne = db.ContratTravailSoumis.Include("Travailleur").Include("Entreprise").Where(ct => ct.contrat_id == id).Single();
            Session["contratTravSelectionne"] = contratSelectionne;
        }

        public ActionResult ChoixTravailleurNewRisque(decimal id)
        {
            SetTravailleur(id);
            return RedirectToAction("Index", "Risques");
        }

        public ActionResult ChoixTravailleurNewExamen(decimal id)
        {
            SetTravailleur(id);
            return RedirectToAction("Index", "Examen");
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
