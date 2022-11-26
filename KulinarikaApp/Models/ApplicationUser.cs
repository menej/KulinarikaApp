using Microsoft.AspNetCore.Identity;

namespace KulinarikaApp.Models;

public class ApplicationUser : IdentityUser
{
    public string FristName { get; set; }
    public string LastName { get; set; }

    public ICollection<Recipe> Recipes { get; set; } = null!;
    public ICollection<Bookmark> Bookmarks { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = null!;
}