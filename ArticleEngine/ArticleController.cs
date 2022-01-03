using Microsoft.AspNetCore.Mvc;

namespace mlblazorserver.ArticleEngine;


public class ArticleController : Controller
{
    public List<Article> _articles { get; set; } = new List<Article>
    {
        new Article
        {
            Id = 1,
            Posted = DateTime.Now,
            LastUpdated = DateTime.Today,
            Author = "Max",
            Title = "Test Post One",
            Post = "This is test text to display the post bla bla bla",
            Tags = new[] {"asdf", "asdfd", "asdawdad"},

        },
        new Article
        {
            Id = 2,
            Posted = DateTime.Now,
            LastUpdated = DateTime.Today,
            Author = "Max",
            Title = "Test Post two",
            Post = "This is test text to display displaydisplaydisplay the post bla bla bla",
            Tags = new[] {"asdf", "display", "asdawdad"},

        },
        new Article
        {
            Id = 3,
            Posted = DateTime.Now,
            LastUpdated = DateTime.Today,
            Author = "Max",
            Title = "Test Post three",
            Post = "This is test text texttexttexttexttexttexttexttextto display the post bla bla bla",
            Tags = new[] {"asdf", "asdfd", "text", "asdawdad"},

        },
        new Article
        {
            Id = 4,
            Posted = DateTime.Now,
            LastUpdated = DateTime.Today,
            Author = "Max",
            Title = "Test Post Max",
            Post = "This is MaxMaxMaxMaxMaxMaxMaxMaxMaxMaxMaxMaxtest text to display the post bla bla bla",
            Tags = new[] {"asdf", "Max", "asdawdad"},

        }
    };

}