using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentService.Models;

public class Attempt
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ChallengeId { get; set; }
    public Challenge Challenge { get; set; }
    public int UserId { get; set; }
    public bool Checked { get; set; }
    public bool? Successful { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; }
    public DateTime CheckedAt { get; set; }
}