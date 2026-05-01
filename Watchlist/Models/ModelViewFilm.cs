using Microsoft.EntityFrameworkCore;

namespace Watchlist.Models
{
    [Keyless]
    public class ModelViewFilm
    {
        public int IdFilm { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public bool PresentInList { get; set; }
        public bool Watched {  get; set; }
        public int? Note {  get; set; }
    }
}
