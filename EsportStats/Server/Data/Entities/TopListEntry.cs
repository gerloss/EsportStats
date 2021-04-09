using EsportStats.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Data.Entities
{
    public class TopListEntry
    {
        public int Id { get; set; }
        
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ulong? ExternalUserId { get; set; }
        public ExternalUser ExternalUser { get; set; }

        public Hero? Hero { get; set; }

        [Required]
        public Metric Metric { get; set; }

        [Required]
        public double Value { get; set; }

        public DateTime? Timestamp { get; set; }
    }
}
