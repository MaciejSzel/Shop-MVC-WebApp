using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sklep.Data.Models;
using System.Data.Entity;

namespace Sklep.Intranet.Controllers
{
    public class PodkategoriaController : BaseController<Podkategoria>
    {
        public PodkategoriaController(SklepDbContext context) : base(context) { }

        public override async Task<List<Podkategoria>> GetEntityList()
        {
            var entities = _context.Podkategoria.Include(p => p.Rodzaj.Nazwa).ToList();
            return await Task.FromResult(entities);
        }


        public override async Task SetSelectList()
        {
            var rodzaje =  _context.Rodzaj.ToList();
            ViewBag.Rodzaje = new SelectList(rodzaje, "IdRodzaju", "Nazwa");
        }



        public async Task<IActionResult> Edit(int? id)
        {
            await SetSelectList();
            if (id == null || _context.Podkategoria == null)
            {
                return NotFound();
            }

            var strona = await _context.Podkategoria.FindAsync(id);
            if (strona == null)
            {
                return NotFound();
            }
            return View(strona);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Podkategoria podkategoria)
        {
            if (id != podkategoria.IdPodkategorii)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(podkategoria);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Podkategoria");  
            }
            return View(podkategoria);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var podkategoria = await _context.Podkategoria.FindAsync(id);
            if (podkategoria == null)
            {
                return NotFound();
            }

            _context.Podkategoria.Remove(podkategoria);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }

}
