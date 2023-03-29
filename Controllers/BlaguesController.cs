using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlagueWebApp.Data;
using BlagueWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlagueWebApp.Controllers
{
    //[Route("api/[controller]/[action]")]
    //[ApiController]


    public class BlaguesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlaguesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Blagues
        public async Task<IActionResult> Index()
        {
              return _context.Blague != null ? 
                          View(await _context.Blague.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Blague'  is null.");
        }


        // GET: Blagues/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }


        // POST: Blagues/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            return View("Index", await _context.Blague.Where(blague => blague.BlagueQuestion.Contains(SearchPhrase)).ToListAsync());
        }


        // GET: Blagues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Blague == null)
            {
                return NotFound();
            }

            var blague = await _context.Blague
                .FirstOrDefaultAsync(m => m.id == id);
            if (blague == null)
            {
                return NotFound();
            }

            return View(blague);
        }

        // GET: Blagues/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blagues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,BlagueQuestion,BlagueAnswer")] Blague blague)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blague);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blague);
        }

        // GET: Blagues/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Blague == null)
            {
                return NotFound();
            }

            var blague = await _context.Blague.FindAsync(id);
            if (blague == null)
            {
                return NotFound();
            }
            return View(blague);
        }

        // POST: Blagues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,BlagueQuestion,BlagueAnswer")] Blague blague)
        {
            if (id != blague.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blague);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlagueExists(blague.id))
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
            return View(blague);
        }

        // GET: Blagues/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Blague == null)
            {
                return NotFound();
            }

            var blague = await _context.Blague
                .FirstOrDefaultAsync(m => m.id == id);
            if (blague == null)
            {
                return NotFound();
            }

            return View(blague);
        }

        // POST: Blagues/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Blague == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Blague'  is null.");
            }
            var blague = await _context.Blague.FindAsync(id);
            if (blague != null)
            {
                _context.Blague.Remove(blague);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlagueExists(int id)
        {
          return (_context.Blague?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
