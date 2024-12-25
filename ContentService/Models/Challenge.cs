using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentService.Models;

public class Challenge
{
    public Challenge() => Attempts = new List<Attempt>();
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ModelUrl { get; set; }
    public string OwnerEmail { get; set; }
    
    public IList<Attempt> Attempts { get; set; }
}