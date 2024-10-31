using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AuditLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AuditLogId { get; set; }

    [Required]
    public string Action { get; set; }

    public DateTime ActionDate { get; set; } = DateTime.Now;

    [Required, MaxLength(100)]
    public string UserId { get; set; }

    [MaxLength(500)]
    public string Details { get; set; }
}
