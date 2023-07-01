using Microsoft.AspNetCore.Mvc;
using Sklep.Data.Models;

namespace Sklep.PortalWWW.Controllers
{
    public abstract class BaseController<TEntity> : Controller
    {
        SklepDbContext _context;
        public BaseController(SklepDbContext context)
        {
            _context = context;
        }
    }
}
