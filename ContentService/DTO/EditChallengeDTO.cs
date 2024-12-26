using System.ComponentModel.DataAnnotations;

namespace ContentService.DTO;

public class EditChallengeDTO
{
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    [Url]
    public string ModelURL { get; set; }
    
    [EmailAddress]
    public string OwnerEmail { get; set; }
}