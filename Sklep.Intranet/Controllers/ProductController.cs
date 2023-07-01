using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sklep.Data.Models;

namespace Sklep.Intranet.Controllers
{
    public class ProductController : Controller  //  BaseController<Product>
    {
        private readonly SklepDbContext _context;
        public ProductController(SklepDbContext context)
        {
            _context = context;
        }


        // GET: Product
        public async Task<IActionResult> Index()
        {
            //z bazy danych pobieramy asynchronicznie listę wszystkich produktów
            var list = await _context.Products.Include(p => p.Rodzaj).Include(p => p.Podkategoria).Where(t => t.IsActive == true).ToListAsync();
            //I przekazujemy ją do widoku 
            return View(list);


            //var sklepDbContext = _context.Products.Include(p => p.Rodzaj);
            //return View(await sklepDbContext.ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Rodzaje = _context.Rodzaj.ToList();
            ViewBag.Podkategorie = _context.Podkategoria.ToList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Quantity,IsActive,RodzajId,PodkategoriaId")] Product product, IFormFile imageFile)
        {
            ViewBag.Rodzaje = _context.Rodzaj.ToList();
            ViewBag.Podkategorie = _context.Podkategoria.ToList();

            _context.Add(product);
            await _context.SaveChangesAsync();

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var imagePath1 = Path.Combine("C:\\Users\\48505\\Desktop\\OstatniSemestr\\apkiInternetowe\\Sklep\\Sklep.PortalWWW\\wwwroot\\images\\", fileName); // Zmień "YourImagePath" na odpowiednią ścieżkę, gdzie chcesz zapisać obrazy
                var imagePath2 = Path.Combine("C:\\Users\\48505\\Desktop\\OstatniSemestr\\apkiInternetowe\\Sklep\\Sklep.Intranet\\wwwroot\\images\\", fileName);

                using (var stream = new FileStream(imagePath1, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                using (var stream = new FileStream(imagePath2, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }


                var image = new Image
                {
                    ImagePath = fileName,
                    ProductId = product.Id
                };

                _context.Add(image);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Rodzaje = _context.Rodzaj.ToList();
            ViewBag.Podkategorie = _context.Podkategoria.ToList();

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Quantity,IsActive,RodzajId,PodkategoriaId")] Product product, IFormFile imageFile)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var imagePath1 = Path.Combine("C:\\Users\\48505\\Desktop\\OstatniSemestr\\apkiInternetowe\\Sklep\\Sklep.PortalWWW\\wwwroot\\images\\", fileName);
                        var imagePath2 = Path.Combine("C:\\Users\\48505\\Desktop\\OstatniSemestr\\apkiInternetowe\\Sklep\\Sklep.Intranet\\wwwroot\\images\\", fileName);

                        using (var stream = new FileStream(imagePath1, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        using (var stream = new FileStream(imagePath2, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        var existingImage = await _context.Images.FirstOrDefaultAsync(i => i.ProductId == product.Id);
                        if (existingImage != null)
                        {
                            // Update existing image
                            existingImage.ImagePath = fileName;
                            _context.Update(existingImage);
                        }
                        else
                        {
                            // Create new image
                            var image = new Image
                            {
                                ImagePath = fileName,
                                ProductId = product.Id
                            };
                            _context.Add(image);
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            ViewBag.Rodzaje = _context.Rodzaj.ToList();
            ViewBag.Podkategorie = _context.Podkategoria.ToList();

            return View(product);
        }


        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Rodzaj)
                .Include(p => p.Podkategoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        //public override async Task<List<Product>> GetEntityList()
        //{
        //    var list = await _context.Products.Include(p => p.Rodzaj).Include(p => p.Podkategoria).Where(t => t.IsActive == true).ToListAsync();
        //    return list;
        //}





        //// GET: Product/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Products == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Rodzaj)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// GET: Product/Create
        //public IActionResult Create()
        //{
        //    ViewData["RodzajId"] = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa");
        //    return View();
        //}

        //// POST: Product/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Quantity,IsActive,Image,RodzajId")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["RodzajId"] = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa", product.RodzajId);
        //    return View(product);
        //}

        //// GET: Product/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Products == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["RodzajId"] = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa", product.RodzajId);
        //    return View(product);
        //}

        //// POST: Product/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Quantity,IsActive,Image,RodzajId")] Product product)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductExists(product.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["RodzajId"] = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa", product.RodzajId);
        //    return View(product);
        //}

        //// GET: Product/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Products == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Rodzaj)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// POST: Product/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Products == null)
        //    {
        //        return Problem("Entity set 'SklepDbContext.Products'  is null.");
        //    }
        //    var product = await _context.Products.FindAsync(id);
        //    if (product != null)
        //    {
        //        _context.Products.Remove(product);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ProductExists(int id)
        //{
        //  return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
