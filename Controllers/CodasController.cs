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
using AppDelBagno.Hubs;

namespace AppDelBagno.Controllers
{
    public class CodasController : Controller
    {
        private readonly AppDelBagnoContext _context;

        public CodasController(AppDelBagnoContext context)
        {
            _context = context;
        }

        // GET: Codas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Coda.ToListAsync());
        }

        // GET: Codas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coda = await _context.Coda
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coda == null)
            {
                return NotFound();
            }

            return View(coda);
        }

        // GET: Codas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Codas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Utente,datetime")] Coda coda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coda);
        }

        // GET: Codas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coda = await _context.Coda.FindAsync(id);
            if (coda == null)
            {
                return NotFound();
            }
            return View(coda);
        }

        // POST: Codas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Utente,datetime")] Coda coda)
        {
            if (id != coda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CodaExists(coda.Id))
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
            return View(coda);
        }

        // GET: Codas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coda = await _context.Coda
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coda == null)
            {
                return NotFound();
            }

            return View(coda);
        }

        // POST: Codas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coda = await _context.Coda.FindAsync(id);
            _context.Coda.Remove(coda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CodaExists(int id)
        {
            return _context.Coda.Any(e => e.Id == id);
        }
    }
}
