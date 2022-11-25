using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models;

public class Tag
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string BlogUserId { get; set; }

    [Required]
    [StringLength(25, ErrorMessage = "The {0} must be at least {2} and no more that {1} characters long")]
    public string Text { get; set; }

    public virtual Post Post { get; set; } //nav property
    public virtual BlogUser BlogUser { get; set; }//nav property
}
