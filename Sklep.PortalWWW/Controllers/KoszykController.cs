
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Sklep.Data.Models;
using Sklep.PortalWWW.Models.BusinessLogic;
using Sklep.PortalWWW.Models.Sklep;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;


namespace Sklep.PortalWWW.Controllers
{
    public class KoszykController : Controller
	{
		private readonly SklepDbContext _context;

		public KoszykController(SklepDbContext context)
		{
			_context = context;
		}

		// Akcja wyświetlająca zawartość koszyka
		public async Task<IActionResult> Index()
		{
			KoszykB koszykB = new KoszykB(_context, HttpContext);
            var daneDoKoszyka = new DaneDoKoszyka
            {
                ElementyKoszyka = await koszykB.GetElementyKoszyka(),
                cena = await koszykB.GetCenaProduktu(),
				Razem = await koszykB.GetRazem()
			};
			return View(daneDoKoszyka);
		}

		// Akcja dodająca produkt do koszyka
		public async Task<IActionResult> DodajDoKoszyka(int id)
		{
			KoszykB koszykB = new KoszykB(_context, HttpContext);
			koszykB.DodajDoKoszyka(await _context.Products.FindAsync(id));
			return RedirectToAction("Index");
		}
        // Akcja usuwająca produkt z koszyka
        public async Task<IActionResult> UsunZKoszyka(int id)
        {
            KoszykB koszykB = new KoszykB(_context, HttpContext);
             koszykB.UsunZKoszyka(await _context.Products.FindAsync(id));
            return RedirectToAction("Index");
        }


        public IActionResult Zamowienie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Zamowienie(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                // Dodaj zamówienie do bazy danych
                 _context.Orders.Add(order);
                 await _context.SaveChangesAsync();

                await SendOrderConfirmationEmail(order);

                return RedirectToAction("Index", "Sklep"); 
            }

            return View(order);
        }




        private async Task SendOrderConfirmationEmail(Order order)
        {
            KoszykB koszykB = new KoszykB(_context, HttpContext);
            var daneDoKoszyka = new DaneDoKoszyka
            {
                ElementyKoszyka = await koszykB.GetElementyKoszyka(),
                Razem = await koszykB.GetRazem(),
                nazwa = await koszykB.GetNazwa(),
                cena = await koszykB.GetCenaProduktu()
            };
           
            var smtpHost = "smtp.elasticemail.com";
            var smtpPort = 2525;
            var smtpUsername = "ProjektAplikacjeInternetowe@elasticemail.com";
            var smtpPassword = "5BBC164870944C547B936275B2B58383B94F";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sender Name", "dark.bk27@gmail.com"));
            message.To.Add(new MailboxAddress("", order.Customer));
            message.Subject = "Potwierdzenie zamówienia";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = $"Dziękujemy za złożenie zamówienia nr {order.Id}.\n\nSzczegóły zamówienia:\n\nKlient: {order.Customer}\nAdres: {order.Address}\nData zamówienia: {order.OrderDate}\n Produkt: {daneDoKoszyka.nazwa} Cena: {daneDoKoszyka.cena} \n Razem: {daneDoKoszyka.Razem}\n\nDziękujemy za zakupy!";
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }




        // Akcja wyświetlająca stronę potwierdzenia zamówienia
        public async Task<IActionResult> PotwierdzenieZamowienia(int id)
        {
            // Pobierz zamówienie z bazy danych na podstawie identyfikatora
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound(); // Można dodać obsługę braku znalezionego zamówienia
            }

            return View("PotwierdzenieZamowienia", order);
        }

    }

}
