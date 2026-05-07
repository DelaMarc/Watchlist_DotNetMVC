using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Watchlist.Data;
using Watchlist.Models;


namespace Watchlist.Controllers
{
    [Authorize]
    public class FilmListController : Controller
    {
        private readonly ApplicationDbContext m_context;
        private readonly UserManager<AccountUser> m_userManager;

        public FilmListController(ApplicationDbContext context, UserManager<AccountUser> userManager)
        {
            m_context = context;
            m_userManager = userManager;
        }

        private Task<AccountUser> GetCurrentuserAsync() => m_userManager.GetUserAsync(HttpContext.User);

        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            AccountUser user = await GetCurrentuserAsync();
            return user?.Id;
        }

        public async Task<IActionResult> Index()
        {
            var id = await GetCurrentUserId();
            var filmsUser = m_context.FilmUsers.Where(x => x.AccountUser.Id == id);
            var model = filmsUser.Select(x => new ModelViewFilm
            {
                IdFilm = x.Film.Id,
                Title = x.Film.Title,
                Year = x.Film.Year,
                Watched = x.Watched,
                PresentInList = true,
                Note = x.Note,
            }).ToList();

            return View(model);
        }

        // GET: FilmList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userId = await GetCurrentUserId();

            if (id == null)
            {
                return NotFound();
            }

            var film = await m_context.FilmUsers.FirstOrDefaultAsync(x => x.UserId == userId && x.FilmID == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: FilmList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = await GetCurrentUserId();

            if (id == null)
            {
                return NotFound();
            }

            var film = await m_context.FilmUsers.FirstOrDefaultAsync(x => x.UserId == userId && x.FilmID == id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: FilmList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,AccountUser,FilmID,Watched,Note")] FilmUser filmUser)
        {
            var userId = await GetCurrentUserId();

            if (!FilmUserExists(filmUser.FilmID, filmUser.UserId))
            {
                return NotFound();
            }
            if (filmUser.UserId != userId)
            {
                return NotFound();
            }
            ModelState.Remove("AccountUser");
            ModelState.Remove("Film");
            if (ModelState.IsValid)
            {
                try
                {
                    m_context.Update(filmUser);
                    await m_context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmUserExists(filmUser.FilmID, filmUser.UserId))
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
            return View(filmUser);
        }

        private bool FilmUserExists(int filmID, string userId)
        {
            return m_context.FilmUsers.Any(e => e.FilmID == filmID && e.UserId == userId);
        }
    }
}
