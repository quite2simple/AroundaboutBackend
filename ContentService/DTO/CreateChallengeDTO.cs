using System.ComponentModel.DataAnnotations;

namespace ContentService.DTO;

public class CreateChallengeDTO
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Url]
    [Required]
    public string ModelURL { get; set; }
    
    [EmailAddress]
    [Required]
    public string OwnerEmail { get; set; }
}