using Microsoft.AspNetCore.Mvc;
using Sklep.PortalWWW.Models;
using System.Diagnostics;
using Sklep.Data.Models;

namespace Sklep.PortalWWW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SklepDbContext _context; //baza danych
        private const string CartSessionKey = "CartData";

        public HomeController(SklepDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id) //to jest id strony ktora kliknieto
        {
            //viewbag to taki listonosz ktory przenosi dane miedzy controllerem a widokiem
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
            //przy pierwszym kliknieciu nic nie kliknieto wiec wyswietlona bedzie pierwsza strona
            if (id == null)
            {
                id = _context.Strona.First().IdStrony;
            }
            //wyszukujemy w bazie danych strone o danym kliknietym id lub pierwsza strone
            var item = _context.Strona.Find(id);
            //znaleziona strone przekazujemy do widoku
            return View(item);
        }

        //public async Task<IActionResult> Index(int id) //to jest id strony ktora kliknieto
        //{
        //    var ModelStrony =
        //       (
        //           from produkt in _context.Products

        //           select produkt
        //       ).ToList();

        //    var item = await _context.Products.FindAsync(id);
        //    //znaleziona strone przekazujemy do widoku
        //    return View(ModelStrony);

        //}

        public IActionResult Privacy()
        {
            return View();
        }
     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}