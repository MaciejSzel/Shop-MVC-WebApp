using Microsoft.AspNetCore.Mvc;
using Sklep.Data.Models.CMS;
using Sklep.Data.Models;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;


namespace Sklep.Intranet.Controllers
{
    public class RodzajController : Controller
    {
        private readonly SklepDbContext _context;

        public RodzajController(SklepDbContext context)
        {
            _context = context;
        }

        // GET: Aktualnosc
        public IActionResult Index()
        {
            
            //var rodzaje = await _context.Rodzaj.Include(r => r.RodzajImagePath).ToListAsync();

            return View(_context.Rodzaj.ToList());
        }

        // GET: Aktualnosc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rodzaj == null)
            {
                return NotFound();
            }

            var rodzaj = await _context.Rodzaj
                .FirstOrDefaultAsync(m => m.IdRodzaju == id);
            if (rodzaj == null)
            {
                return NotFound();
            }

            return View(rodzaj);
        }

        // GET: Aktualnosc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aktualnosc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nazwa,Opis,RodzajImagePath")] Rodzaj rodzaj, IFormFile rodzajImageFile)
        {
                if (rodzajImageFile != null && rodzajImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(rodzajImageFile.FileName); // Pobierz nazwę pliku
                    var imagePath = Path.Combine("C:\\Users\\48505\\Desktop\\OstatniSemestr\\apkiInternetowe\\Sklep\\Sklep.Intranet\\wwwroot\\images\\", fileName); // Ścieżka, gdzie chcesz zapisać plik

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await rodzajImageFile.CopyToAsync(stream);
                    }

                    rodzaj.RodzajImagePath = fileName; // Przypisz nazwę pliku do właściwości
                }

                _context.Add(rodzaj);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            

           
        }



        // GET: Aktualnosc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Aktualnosc == null)
            {
                return NotFound();
            }

            var aktualnosc = await _context.Aktualnosc.FindAsync(id);
            if (aktualnosc == null)
            {
                return NotFound();
            }
            return View(aktualnosc);
        }

        // POST: Aktualnosc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAktualnosci,LinkTytul,Tytul,Tresc,Pozycja")] Aktualnosc aktualnosc)
        {
            if (id != aktualnosc.IdAktualnosci)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aktualnosc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RodzajExists(aktualnosc.IdAktualnosci))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aktualnosc);
        }

        // GET: Aktualnosc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Aktualnosc == null)
            {
                return NotFound();
            }

            var aktualnosc = await _context.Aktualnosc
                .FirstOrDefaultAsync(m => m.IdAktualnosci == id);
            if (aktualnosc == null)
            {
                return NotFound();
            }

            return View(aktualnosc);
        }

        // POST: Aktualnosc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Aktualnosc == null)
            {
                return Problem("Entity set 'SklepDbContext.Aktualnosc'  is null.");
            }
            var aktualnosc = await _context.Aktualnosc.FindAsync(id);
            if (aktualnosc != null)
            {
                _context.Aktualnosc.Remove(aktualnosc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RodzajExists(int id)
        {
            return (_context.Rodzaj?.Any(e => e.IdRodzaju == id)).GetValueOrDefault();
        }
    }
}
