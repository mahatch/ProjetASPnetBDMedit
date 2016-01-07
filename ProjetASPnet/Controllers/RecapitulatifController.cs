using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetASPnet.Models;

namespace ProjetASPnet.Controllers
{
    public class RecapitulatifController : Controller
    {
        private MeditRisquesEntities db = new MeditRisquesEntities();
        // GET: Recapitulatif
        public ActionResult Index()
        {
            ContratTravailSoumis contratTravSelectionne = Session["contratTravSelectionne"] as ContratTravailSoumis;

            if(contratTravSelectionne != null) {
                ContratTravailSoumis contratTravBd = db.ContratTravailSoumis.Include("Examen").Include("Risque").Where(r => r.contrat_id == contratTravSelectionne.contrat_id).Single();

                if(contratTravBd != null) { 
                    RecapitulatifViewModel recapitulatif = new RecapitulatifViewModel();
                    recapitulatif.NomEntreprise = contratTravSelectionne.Entreprise.denomation;
                    recapitulatif.Risque = contratTravBd.Risque.ToList();
                    recapitulatif.NomTravailleur = contratTravSelectionne.Travailleur.nom;
                    recapitulatif.Examens = contratTravBd.Examen.ToList();
                    Session["contratTravSelectionne"] = null;
                    return View(recapitulatif);
                }
            }

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