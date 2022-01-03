namespace mlblazorserver.ArticleEngine;
using Microsoft.AspNetCore.Components;

public class ArticlePreviewComponent : ComponentBase
{
    [Parameter] public Article articlePost { get; set; }
}