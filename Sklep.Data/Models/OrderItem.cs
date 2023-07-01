using System.ComponentModel.DataAnnotations;

namespace Sklep.Data.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Towar jest wymagany")]
        [Display(Name = "Towar")]
        public virtual Product Product { get; set; }

        [Required(ErrorMessage = "Ilość jest wymagana")]
        [Range(1, int.MaxValue, ErrorMessage = "Ilość musi być większa niż 0")]
        [Display(Name = "Ilość")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Cena jest wymagana")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa niż 0")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        public virtual Order Order { get; set; }
    }
}
