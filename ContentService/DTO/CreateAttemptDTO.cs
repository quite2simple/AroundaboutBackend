using System.ComponentModel.DataAnnotations;


namespace ContentService.DTO;

public class CreateAttemptDTO
{
    [Required]
    public int ChallengeId { get; set; }
    [Required]
    public string Body { get; set; }
}