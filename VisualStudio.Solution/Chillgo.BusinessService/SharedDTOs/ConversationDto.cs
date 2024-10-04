using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.BusinessService.SharedDTOs
{
    public class ConversationDto
    {
        public Guid Id { get; set; }

        public bool IsHuman { get; set; }

        public string? FirstName { get; set; }

        public Guid? FirstAccountId { get; set; }

        public string? SecondName { get; set; }

        public Guid? SecondAccountId { get; set; }

        public Guid? AibotId { get; set; }

        public DateTime LastUpdated { get; set; }

        public string Status { get; set; } = null!;
    }
}
