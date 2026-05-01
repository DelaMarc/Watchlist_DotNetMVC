using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Watchlist.Data;
using Watchlist.Models;

namespace Watchlist.Controllers
{
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
            var filmsUser = m_context.FilmUsers.Where(x => x.UserID.ToString() == id);
            var model = filmsUser.Select(x => new ModelViewFilm
            {
                IdFilm = x.FilmID,
                Title = x.Film.Title,
                Year = x.Film.Year,
                Watched = x.Watched,
                PresentInList = true,
                Note = x.Note,
            }).ToList();

            return View(model);
        }
    }
}
