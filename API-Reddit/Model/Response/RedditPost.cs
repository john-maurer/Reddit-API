namespace API_Reddit.Model.Response
{
    public class RedditPost
    {
        public RedditPost() 
        {
            Id = string.Empty;
            Author = string.Empty;
            Upvotes = 0;
        }

        public RedditPost(RedditPost post)
        {
            Id = post.Id;
            Author = post.Author;
            Upvotes = post.Upvotes;
        }

        public string Id { get; set; }
        public string Author { get; set; }
        public int Upvotes { get; set; }
    }
}
