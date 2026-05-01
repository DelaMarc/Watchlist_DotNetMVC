namespace Watchlist.Data
{
    public class FilmUser
    {
        public int UserID {  get; set; }
        public int FilmID { get; set; }
        public bool Watched { get; set; }
        public int Note { get; set; }
        public virtual AccountUser User { get; set; }
        public virtual Film Film { get; set; }
    }
}
