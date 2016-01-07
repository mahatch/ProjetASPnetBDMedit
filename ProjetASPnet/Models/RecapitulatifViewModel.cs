using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetASPnet.Models
{
    public class RecapitulatifViewModel
    {
        public RecapitulatifViewModel()
        {
           
        }

        public string NomEntreprise { get; set; }
        public string NomTravailleur { get; set; }
        public virtual ICollection<Risque> Risque { get; set; }
        public virtual List<Examen> Examens { get; set; }
        
    }
}