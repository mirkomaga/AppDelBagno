#nullable disable
using AppDelBagno.Data;
using AppDelBagno.Hubs;
using AppDelBagno.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace AppDelBagno.Controllers
{
    public class BagnoesController : Controller
    {
        //private readonly IHubContext<ChatHub> _hubContext;

        private readonly AppDelBagnoContext _context;

        private readonly INotyfService _notyf;

        public BagnoesController(AppDelBagnoContext context, INotyfService notyf)
        {
            _notyf = notyf;
            _context = context;
        }

        //_notyf.Success("Bagno liberato");

        // GET: Bagnoes
        public async Task<IActionResult> Index()
        {
            ViewBag.bagno = is_Bloccato();

            ViewBag.Coda = await _context.Coda.ToListAsync();

            ViewBag.SonoInCoda = IAmInQueue();

            ViewBag.FirstQueue = FirstInQueue();

            return View(await _context.Bagno.ToListAsync());
        }

        /// <summary>
        /// Sono in coda?
        /// </summary>
        /// <returns></returns>
        public bool IAmInQueue()
        {
            // Controllo che non sia già in coda
            if (_context.Coda.Where(cod => Environment.UserName == cod.Utente).Count() > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// primo in coda
        /// </summary>
        /// <returns></returns>
        public Coda FirstInQueue()
        {
            if (_context.Coda.Count() == 0) return null;

            return _context.Coda.First();
        }


        /// <summary>
        /// Sblocco lo stato del bagno
        /// </summary>
        public async Task<IActionResult> UnlockBagno()
        {
            // Sono io
            Bagno b = is_Bloccato();

            if (b == null) return NoContent();

            _context.Bagno.Remove(b);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// blocca lo stato del bagno
        /// </summary>
        public async Task<IActionResult> LockBagno()
        {
            // Sono io
            Bagno b = new Bagno();
            b.Entrata = DateTime.Now;
            b.Utente = Environment.UserName;

            _context.Bagno.Add(b);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> DeleteMeToQueue()
        {
            // se sono in coda
            if (!IAmInQueue()) return NoContent();


            // Elimino dalla coda l'utente
            _context.Coda.RemoveRange(_context.Coda.Where(cod => Environment.UserName == cod.Utente));
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> AddMeToQueue()
        {
            // se sono in coda
            if (IAmInQueue()) return NoContent();


            // Aggiungo in coda
            Coda c = new Coda();
            c.Utente = Environment.UserName;
            c.datetime = DateTime.Now;






            _context.Coda.Add(c);
            await _context.SaveChangesAsync();




            return RedirectToAction(nameof(Index));
        }

        // GET: Bagnoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var bagno = await _context.Bagno.FirstOrDefaultAsync(m => m.Id == id);


            if (bagno == null)
            {
                return NotFound();
            }

            return View(bagno);
        }

        // GET: Bagnoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bagnoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Utente,Entrata,Uscita")] Bagno bagno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bagno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bagno);
        }

        // GET: Bagnoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bagno = await _context.Bagno.FindAsync(id);
            if (bagno == null)
            {
                return NotFound();
            }
            return View(bagno);
        }

        // POST: Bagnoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Utente,Entrata,Uscita")] Bagno bagno)
        {
            if (id != bagno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bagno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BagnoExists(bagno.Id))
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
            return View(bagno);
        }

        // GET: Bagnoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bagno = await _context.Bagno.FirstOrDefaultAsync(m => m.Id == id);

            if (bagno == null)
            {
                return NotFound();
            }

            return View(bagno);
        }

        // POST: Bagnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bagno = await _context.Bagno.FindAsync(id);
            _context.Bagno.Remove(bagno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BagnoExists(int id)
        {
            return _context.Bagno.Any(e => e.Id == id);
        }


        private Bagno is_Bloccato()
        {
            IQueryable<Bagno> iqBloccato = _context.Bagno.Where(e => string.IsNullOrEmpty(e.Uscita.Value.ToString()));


            if (iqBloccato.Count() == 0) return null;
            else 
                return iqBloccato.First();
        }
    }
}
