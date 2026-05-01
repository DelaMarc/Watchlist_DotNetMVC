using Microsoft.AspNetCore.Identity;

namespace Watchlist.Data
{
    public class AccountUser : IdentityUser
    {
        public string Name {  get; set; }
        public virtual ICollection<FilmUser> FilmList { get; set; }


        public AccountUser(): base()
        {
            FilmList = new HashSet<FilmUser>();
        }
    }
}
