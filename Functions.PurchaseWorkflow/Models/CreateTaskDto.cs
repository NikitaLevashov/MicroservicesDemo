using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.PurchaseWorkflow.Models
{
    public record CreateTaskDto(string Title, string Description, int PurchaseId, string Status);
}
