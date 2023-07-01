using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Sklep.Data.Models
{
    // Klasa reprezentująca Produkt
    public class Product 
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Opis jest wymagany")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cena jest wymagana")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa niż 0")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Ilość jest wymagana")]
        [Range(0, int.MaxValue, ErrorMessage = "Ilość musi być większa lub równa 0")]
        [Display(Name = "Ilość")]
        public int Quantity { get; set; }

        [Display(Name = "Aktywny")]
        public bool IsActive { get; set; }

        

        public int RodzajId { get; set; }
        public Rodzaj Rodzaj { get; set; }


        //Wywal baze danych i dodaj ją na nowo z FK poniżej!!!!!!!!!!!!!!!!! wtedy podkategorie should work! 
       //[ForeignKey("Podkategoria")]
        public int PodkategoriaId { get; set; }
        public Podkategoria Podkategoria { get; set; }

        public ICollection<Image> Images { get; set; }

    }
}
