using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; private set; }
        public string Type { get; private set; } = null!;
        public string Content { get; private set; } = null!;
        public DateTime OccurredOn { get; private set; }
        public DateTime? ProcessedOn { get; private set; }
        public string? Error { get; private set; }

        private OutboxMessage(){}

        public OutboxMessage(Guid id,string type,string content, DateTime occurredOn)
        {
            Id = id;
            Type = type;
            Content = content;
            OccurredOn = occurredOn;
        }

        public void MarkAsProcessed()
        {
            ProcessedOn = DateTime.UtcNow;
            Error = null;
        }

        public void MarkAsFailed(string error)
        {
            Error = error;
        }
    }
}
