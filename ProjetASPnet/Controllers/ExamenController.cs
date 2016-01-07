using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjetASPnet.Models;

namespace ProjetASPnet.Controllers
{
    public class ExamenController : Controller
    {
        private MeditRisquesEntities db = new MeditRisquesEntities();

        // GET: Examen
        public ActionResult Index()
        {
            ContratTravailSoumis contratsTrav = Session["contratTravSelectionne"] as ContratTravailSoumis;
            if(contratsTrav != null) {
                ContratTravailSoumis contratsTravAvecExamen =
                    db.ContratTravailSoumis
                        .Include("Examen")
                        .Where(r => r.contrat_id == contratsTrav.contrat_id)
                        .Single();
                if(contratsTravAvecExamen != null) { 
                    List<Examen> tousLesExamens = db.Examen.ToList();
                    List<Examen> lesExamensDuTravailleur = contratsTravAvecExamen.Examen.ToList();
                    tousLesExamens.RemoveAll(x => lesExamensDuTravailleur.Exists(y => y.code == x.code));

                    var modelExamen = new ExamenViewModels()
                    {
                        ListExamensSelected = tousLesExamens.ToList()
                    };
                
                    return View(modelExamen);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult SelectedExamens(ExamenViewModels examensSelected)
        {
            if(examensSelected.ListExamensSelected.Count > 0) { 
                ContratTravailSoumis contratsTrav = Session["contratTravSelectionne"] as ContratTravailSoumis;
                var contratTravChoisiBd = db.ContratTravailSoumis.Find(contratsTrav.contrat_id);

                foreach (Examen examen in examensSelected.ListExamensSelected)
                {
                    if (examen.isSelected)
                    {
                        var examensChoisisBd = db.Examen.Single(x => x.code == examen.code);
                        db.Entry(contratTravChoisiBd).Collection(x => x.Examen).Load();
                        contratTravChoisiBd.Examen.Add(examensChoisisBd);
                    }
                }
                db.SaveChanges();

                return RedirectToAction("Index", "Recapitulatif");
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
