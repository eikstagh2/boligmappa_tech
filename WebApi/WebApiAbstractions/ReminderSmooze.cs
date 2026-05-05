using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApiAbstractions
{
    public class ReminderSnooze: DbContext
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid DocumentId { get; set; }
        [Required]
        public DateOnly SnoozedUntil { get; set; }
        [Required]
        public DateOnly SnoozedAt { get; set; }

    }
}
