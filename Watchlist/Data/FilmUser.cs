namespace Watchlist.Data
{
    public class FilmUser
    {
        public string UserId { get; set; }
        public int FilmID { get; set; }
        public bool Watched { get; set; }
        public int Note { get; set; }
        public virtual AccountUser AccountUser { get; set; }
        public virtual Film Film { get; set; }
    }
}
