using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Infrastructure.Queues
{
    public class QueueOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string PurchasesQueueName { get; set; } = "purchases";

        /// <summary>
        /// When the message becomes visible for the first time.
        /// Example: "00:30:00" (30 minutes). Optional.
        /// </summary>
        public TimeSpan? InitialVisibility { get; set; }

        /// <summary>
        /// Message time-to-live. Example: "7.00:00:00" (7 days). Optional.
        /// </summary>
        public TimeSpan? TimeToLive { get; set; }
    }

}
