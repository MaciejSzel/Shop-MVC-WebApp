using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sklep.Data.Models;
using System.Drawing.Printing;

namespace Sklep.PortalWWW.Controllers
{
    public class SklepController : Controller
    {
        private readonly SklepDbContext _context;

        public SklepController(SklepDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id, int? podkategoriaId, string searchQuery, int? pageSize)
        {
            // Przykładowa wartość domyślna dla liczby produktów na stronie
            int defaultPageSize = 5;

            // Sprawdź, czy użytkownik wybrał wartość dla liczby produktów
            int selectedPageSize = pageSize.HasValue ? pageSize.Value : defaultPageSize;

            ViewBag.Rodzaj = await _context.Rodzaj.ToListAsync();
            ViewBag.RodzajeImage = await _context.Rodzaj.Where(r => r.IdRodzaju == id).ToListAsync();
            ViewBag.Podkategoria = await _context.Podkategoria.Where(p => p.RodzajId == id).ToListAsync();


            if (id == null)
            {
                var pierwszy = await _context.Rodzaj.Include(r => r.Podkategorie).FirstOrDefaultAsync();
                if (pierwszy != null)
                {
                    podkategoriaId = pierwszy.Podkategorie.FirstOrDefault()?.IdPodkategorii;
                    id = pierwszy.IdRodzaju;
                }
            }

            IQueryable<Product> productsQuery = _context.Products
                .Include(p => p.Images)
                .Where(p => p.RodzajId == id);

            if (podkategoriaId != null)
            {
                productsQuery = productsQuery.Where(p => p.PodkategoriaId == podkategoriaId);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchQuery));
            }

            var products = await productsQuery.Take(selectedPageSize).ToListAsync();

            ViewBag.SearchQuery = searchQuery; // Przekazanie wartości wyszukiwania do widoku
            ViewBag.PageSize = selectedPageSize;

            return View(products);
        }






        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Rodzaj = await _context.Rodzaj.ToListAsync();

            var product = await _context.Products
                .Include(p => p.Images) // Załaduj powiązane obiekty Images
                .FirstOrDefaultAsync(t => t.Id == id);

            var recommendedProducts = await _context.Products.Include(p => p.Images)
           .Where(p => p.RodzajId == product.RodzajId && p.Id != product.Id).ToListAsync();

            ViewBag.RecommendedProducts = recommendedProducts;
            ViewBag.Recommended = await _context.Products
                .Include(p => p.Images) // Załaduj powiązane obiekty Images
                .Where(t => t.RodzajId == id)
                .ToListAsync();


            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                // Jeśli pasek wyszukiwania jest pusty, przekieruj do akcji Index
                return RedirectToAction("Index");
            }

            // Przetwarzanie zapytania i pobieranie pasujących produktów
            var products = await _context.Products
                .Include(p => p.Images)
                .Where(p => p.Name.Contains(searchQuery))
                .ToListAsync();

            // Przekazanie wyników wyszukiwania do widoku
            return View("Index", products);
        }



    }
}
