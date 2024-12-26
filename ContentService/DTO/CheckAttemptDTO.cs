using System.ComponentModel.DataAnnotations;

namespace ContentService.DTO;

public class CheckAttemptDTO
{
    [Required]
    public int AttemptId { get; set; }
    [Required]
    public bool Checked  { get; set; }
    public bool? Successful { get; set; }
}