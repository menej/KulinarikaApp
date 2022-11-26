using System.ComponentModel.DataAnnotations.Schema;

namespace KulinarikaApp.Models;

public class Bookmark
{
    public int Id { get; set; }
    public int? RecipeId { get; set; }
    public string? UserId { get; set; }
    
    public string BookmarkName { get; set; }
    
    public Recipe? Recipe { get; set; }
    public ApplicationUser? User { get; set; }
}
