namespace KulinarikaApp.Models;

public class Comment
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public int? RecipeId { get; set; }

    public string? CommentText { get; set; }

    public ApplicationUser User { get; set; }
    public Recipe Recipe { get; set; }

}