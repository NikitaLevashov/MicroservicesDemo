using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.PurchaseWorkflow.Models
{
    public record PurchaseCreatedMessage(int PurchaseId, int ClientId, decimal Total);
}
