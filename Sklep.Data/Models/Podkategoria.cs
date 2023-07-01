using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sklep.Data.Models
{
    public class Podkategoria
    {
        [Key]
        public int IdPodkategorii { get; set; }

        [Required(ErrorMessage = "Podaj nazwę podkategorii")]
        [MaxLength(30, ErrorMessage = "Nazwa podkategorii powinna mieć maksymalnie 30 znaków")]
        public string Nazwa { get; set; }

        public string Opis { get; set; }

        public int RodzajId { get; set; }
        public Rodzaj Rodzaj { get; set; }

        public List<Product> Produkty { get; set; }
    }
}
