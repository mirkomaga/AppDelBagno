#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppDelBagno.Data;
using AppDelBagno.Models;
using MySqlConnector;

namespace AppDelBagno.Controllers
{
    public class BagnoesController : Controller
    {
        private readonly AppDelBagnoContext _context;

        public BagnoesController(AppDelBagnoContext context)
        {
            _context = context;
        }

        // GET: Bagnoes
        public async Task<IActionResult> Index()
        {
            ViewBag.m_BagnoLibero = BagnoLibero();



            return View(await _context.Bagno.ToListAsync());
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

            var bagno = await _context.Bagno
                .FirstOrDefaultAsync(m => m.Id == id);
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


        private bool BagnoLibero()
        {
            return _context.Bagno.Any(e => e.Uscita == null);
        }
    }
}
