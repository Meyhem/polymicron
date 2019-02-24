namespace Pm.Data.Entity
{
    public class PostTag
    {
        public int Id { get; set; }

        public string Tag { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

    }
}
