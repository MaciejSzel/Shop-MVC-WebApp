using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Sklep.Data.Models
{
    public class Rodzaj
    {
        [Key]
        public int IdRodzaju { get; set; }

        [Required(ErrorMessage = "Podaj nazwę rodzaju")]
        [MaxLength(30, ErrorMessage = "Nazwa kategorii powinna mieć maksymalnie 30 znaków")]
        public string Nazwa { get; set; }

        public string Opis { get; set; }

        public string RodzajImagePath { get; set; }

        public List<Podkategoria> Podkategorie { get; set; }

        public List<Product> Produkty { get; set; }
    }


}
