using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sklep.Data.Models;

namespace Sklep.PortalWWW.Models.BusinessLogic
{
    public class KoszykB
    {
        private readonly SklepDbContext _context;
        private string IdSesjiKoszyka;

        public KoszykB(SklepDbContext context, HttpContext httpContext)
        {
            _context = context;
            IdSesjiKoszyka = GetIdSesjiKoszyka(httpContext);
        }

        private string GetIdSesjiKoszyka(HttpContext httpContext)
        {
            if (httpContext.Session.GetString("IdSesjiKoszyka") == null)
            {
                if (!string.IsNullOrWhiteSpace(httpContext.User.Identity.Name))
                {
                    httpContext.Session.SetString("IdSesjiKoszyka", httpContext.User.Identity.Name);
                }
                else
                {
                    Guid tempIdSesjiKoszyka = Guid.NewGuid();
                    httpContext.Session.SetString("IdSesjiKoszyka", tempIdSesjiKoszyka.ToString());
                }
            }
            return httpContext.Session.GetString("IdSesjiKoszyka");
        }

        public void DodajDoKoszyka(Product towar)
        {
            var elementKoszyka = _context.ElementKoszyka.FirstOrDefault(x => x.IDTowaru == towar.Id && x.IdSesjiKoszyka == this.IdSesjiKoszyka);
            if (elementKoszyka == null)
            {
                elementKoszyka = new ElementKoszyka
                {
                    IdSesjiKoszyka = this.IdSesjiKoszyka,
                    IDTowaru = towar.Id,
                    Ilosc = 1,
                    DataUtworzenia = DateTime.Now
                };
                _context.ElementKoszyka.Add(elementKoszyka);
            }
            else
            {
                elementKoszyka.Ilosc++;
            }
            _context.SaveChanges();
        }
        public void UsunZKoszyka(Product towar)
        {
            var elementKoszyka = _context.ElementKoszyka.FirstOrDefault(x => x.IDTowaru == towar.Id && x.IdSesjiKoszyka == this.IdSesjiKoszyka);

            if (elementKoszyka != null)
            {
                _context.ElementKoszyka.Remove(elementKoszyka);
                _context.SaveChanges();
            }
        }

        public async Task<List<ElementKoszyka>> GetElementyKoszyka()
        {
            return await _context.ElementKoszyka
                .Include(x => x.Towar).Include(x=>x.Towar.Images)
                .Where(x => x.IdSesjiKoszyka == this.IdSesjiKoszyka)
                .ToListAsync();
        }
        public async Task<string> GetNazwa()
        {
            var elementyKoszyka = await GetElementyKoszyka();
            string nazwa = "";
            foreach (var element in elementyKoszyka)
            {
                nazwa = element.Towar.Name;
            }
            return nazwa;
        }
        public async Task<decimal> GetCenaProduktu()
        {
            var elementyKoszyka = await GetElementyKoszyka();
            decimal cena = 0;
            foreach (var element in elementyKoszyka)
            {
                cena = element.Towar.Price;
            }
            return cena;
        }
        public async Task<decimal> GetRazem()
        {
            var elementyKoszyka = await GetElementyKoszyka();
            decimal razem = 0;
            foreach (var element in elementyKoszyka)
            {
                razem += element.Ilosc * element.Towar.Price+10;
            }
            return razem;
        }
    }
}
