namespace mlblazorserver.ArticleEngine;

public class Article
{
    public int Id { get; set; }
    public DateTime Posted { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Post { get; set; }

    public string PostSummary
    {
        get
        {
            if (Post.Length > 50)
            {
                return Post.Substring(0, 50);
            }

            return Post;
        }
    }
    public string[] Tags { get; set; }

    public Article(int id, DateTime posted, DateTime lastupdated, string author, string title, string post, string[] tags)
    {
        Id = id;
        Posted = posted;
        LastUpdated = lastupdated;
        Author = author;
        Title = title;
        Post = post;
        Tags = tags;
    }

    public Article()
    {
    }
}