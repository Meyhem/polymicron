namespace Pm.Data.Entity
{
    public class Rating
    {
        public int Id { get; set; }

        public bool Direction { get; set; }

        public string Token { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}
