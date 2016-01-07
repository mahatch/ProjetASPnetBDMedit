using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetASPnet.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace ProjetASPnet.Controllers
{
    public class RisquesController : Controller
    {
        private MeditRisquesEntities db = new MeditRisquesEntities();

        // GET: Risques
        public ActionResult Index()
        {
            ContratTravailSoumis contratsTrav = Session["contratTravSelectionne"] as ContratTravailSoumis;
            if(contratsTrav != null) { 
                ContratTravailSoumis contratsTravAvecRisques =
                    db.ContratTravailSoumis
                        .Include("Risque")
                        .Where(r => r.contrat_id == contratsTrav.contrat_id)
                        .Single();

                if (contratsTravAvecRisques != null)
                {
                    List<Risque> tousLesRisques = db.Risque.ToList();
                    List<Risque> lesRisquesDuTravailleur = contratsTravAvecRisques.Risque.ToList();

                    tousLesRisques.RemoveAll(x => lesRisquesDuTravailleur.Exists(y => y.code == x.code));

                    var modelRisque = new RisqueViewModel()
                    {
                        ListRisquesSelected = tousLesRisques
                    };

                    return View(modelRisque);
                }
            }

            return HttpNotFound();


        }

        [HttpPost]
        public ActionResult SelectedRisques(RisqueViewModel risquesSelected)
        {
            if (risquesSelected.ListRisquesSelected.Count > 0)
            {
                ContratTravailSoumis contratsTrav = Session["contratTravSelectionne"] as ContratTravailSoumis;
                var contratTravChoisiBd = db.ContratTravailSoumis.Find(contratsTrav.contrat_id);

                foreach (Risque risque in risquesSelected.ListRisquesSelected)
                {
                    if (risque.isSelected)
                    {
                        var risqueChoisiBd = db.Risque.Single(x => x.code == risque.code);
                        db.Entry(contratTravChoisiBd).Collection(x => x.Risque).Load();
                        contratTravChoisiBd.Risque.Add(risqueChoisiBd);
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
