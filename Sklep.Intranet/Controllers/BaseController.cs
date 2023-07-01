using Microsoft.AspNetCore.Mvc;
using Sklep.Data.Models;

namespace Sklep.Intranet.Controllers
{
     
    public abstract class BaseController<TEntity> : Controller
    {
        //protected bo bedziemy uzywac w klasach dziedziczacych
        protected readonly SklepDbContext _context;

        public BaseController(SklepDbContext context)
        {
            _context = context;
        }
      
        public abstract Task<List<TEntity>> GetEntityList();

        public async Task<IActionResult> Index()
        {
            return View(await GetEntityList());
        }

		public virtual Task SetSelectList()//
		{
			return null;
		}
		//
		public async Task<IActionResult> Create()
		{
			await SetSelectList();  //
			return View();  //funkcja Create w kontr.. wlacza widok o tej samej nazwie czy Create
		}
		//
		[HttpPost]
		public async Task<IActionResult> Create(TEntity entity)
		{
			await _context.AddAsync(entity);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
	}
}
