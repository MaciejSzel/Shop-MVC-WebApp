using Sklep.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sklep.Data.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Klient jest wymagany")]
        [Display(Name = "Klient")]
        public string Customer { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Data zamówienia jest wymagana")]
        [Display(Name = "Data zamówienia")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Aktywny")]
        public bool IsActive { get; set; }

        
    }
}
