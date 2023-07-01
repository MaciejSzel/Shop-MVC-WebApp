using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklep.Data.Models
{
    public class ElementKoszyka
    {
        [Key]
        public int IdElementuKoszyka { get; set; }
        public string IdSesjiKoszyka { get; set; } //tu jest przechowywany identyfikator przegladarki klienta

        public int IDTowaru { get; set; }
        [ForeignKey("IDTowaru")]
        public Product Towar { get; set; }

        public int Ilosc { get; set; }
        public DateTime DataUtworzenia { get; set; }
    }
}

