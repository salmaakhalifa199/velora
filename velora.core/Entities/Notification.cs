using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace velora.core.Entities
{
    public class Notification : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string? UserId { get; set; } 
        public bool IsGuestOnly { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
