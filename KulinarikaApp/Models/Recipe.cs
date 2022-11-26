using System.ComponentModel.DataAnnotations;

namespace KulinarikaApp.Models;

public class Recipe
{
    public int Id { get; set; }
    public string? UserId { get; set; }

    [Required(ErrorMessage = "The Title field is required.")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Naslov recepta mora vsebovati med 5 in 100 znakov")]
    public string Title { get; set; }

    [Required(ErrorMessage = "The RecipeText field is required.")]
    public string RecipeText { get; set; }
    
    public ApplicationUser? User { get; set; }
    public ICollection<Comment>? Comments { get; set; } = null!;
}