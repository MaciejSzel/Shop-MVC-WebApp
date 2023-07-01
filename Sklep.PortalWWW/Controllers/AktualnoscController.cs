using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Sklep.Data.Models;
using System.Data.Entity;

namespace Sklep.PortalWWW.Controllers
{
    public class AktualnoscController : Controller
    {
        private readonly SklepDbContext _context;
        public AktualnoscController(SklepDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int id)//to jest id kliknietej aktualnosci
        {
            ViewBag.ModelStrony =
                (
                    from strona in _context.Strona
                    orderby strona.Pozycja
                    select strona
                 ).ToList();
            ViewBag.ModelAktualnosci =
             (
                 from aktualnosc in _context.Aktualnosc
                 orderby aktualnosc.Pozycja
                 select aktualnosc
              ).ToList();

            //odnajdujemy aktualnosc o danym klknietym id
            var item =  _context.Aktualnosc.ToList();
            //przekazujemy ją do widoku
            return View(item);
        }
    }
}
