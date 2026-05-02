using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Watchlist.Data;
using Watchlist.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Watchlist.Controllers
{
    public class FilmsController : Controller
    {
        private readonly ApplicationDbContext m_context;
        private readonly UserManager<AccountUser> m_manager;

        public FilmsController(ApplicationDbContext context, UserManager<AccountUser> manager)
        {
            m_context = context;
            m_manager = manager;
        }

        private Task<AccountUser> GetCurrentuserAsync() => m_manager.GetUserAsync(HttpContext.User);


        [HttpGet]
        public async Task<string> GetCurrentuserId()
        {
            AccountUser user = await GetCurrentuserAsync();
            if (user == null)
                System.Diagnostics.Debug.WriteLine("CURRENT USER IS NULL");
            return user?.Id;
        }

        [HttpGet]
        public async Task<AccountUser> GetCurrentuser()
        {
            AccountUser user = await GetCurrentuserAsync();
            return user;
        }

        // GET: Films
        public async Task<IActionResult> Index()
        {
            var userId = await GetCurrentuserId();
            var model = await m_context.Films.Select(x => new ModelViewFilm
            {
                IdFilm = x.Id,
                Title = x.Title,
                Year = x.Year,
            }).ToListAsync();

            foreach (var item in model)
            {
                var m = await m_context.FilmUsers.FirstOrDefaultAsync(x => x.UserId == userId && x.FilmID == item.IdFilm);
                if (m != null)
                {
                    item.PresentInList = true;
                    item.Note = m.Note;
                    item.Watched = m.Watched;
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> AddRemove(int id, int val)
        {
            int retVal = -1;
            //System.Diagnostics.Debug.WriteLine($"");
            //var userId = await GetCurrentuserId();
            var user = await GetCurrentuser();

            if (val == 1)
            {
                // film must be removed from user list
                var film = m_context.FilmUsers.FirstOrDefault(x => x.FilmID == id && x.UserId == user.Id);
                if (film != null)
                {
                    m_context.FilmUsers.Remove(film);
                    retVal = 0;
                }
            }
            else
            {
                // film is not in user list, so we must add it
                m_context.FilmUsers.Add(new FilmUser
                {
                    UserId = user.Id,
                    FilmID = id,
                    Watched = false,
                    Note = 0,
                    AccountUser = user,
                });
                retVal = 1;
            }
            // save the changes to the database
            await m_context.SaveChangesAsync();
            return Json(retVal);
        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await m_context.Films
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Year")] Film film)
        {
            if (ModelState.IsValid)
            {
                m_context.Add(film);
                await m_context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await m_context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Year")] Film film)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    m_context.Update(film);
                    await m_context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
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
            return View(film);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await m_context.Films
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await m_context.Films.FindAsync(id);
            if (film != null)
            {
                m_context.Films.Remove(film);
            }

            await m_context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return m_context.Films.Any(e => e.Id == id);
        }
    }
}
