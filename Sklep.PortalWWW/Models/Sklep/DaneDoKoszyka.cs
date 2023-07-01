using System.Collections.Generic;
using Sklep.Data.Models;

namespace Sklep.PortalWWW.Models.Sklep
{
    public class DaneDoKoszyka
    {
        public List<ElementKoszyka> ElementyKoszyka { get; set; }
        public string nazwa { get; set; }
        public decimal cena { get; set; }
        public decimal Razem { get; set; }
    }
}
