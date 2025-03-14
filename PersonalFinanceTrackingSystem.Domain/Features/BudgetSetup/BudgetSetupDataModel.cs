using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTrackingSystem.Domain.Features.BudgetSetup
{
    public class BudgetSetupDataModel
    {
        public string BudgetId {  get; set; }
        public string UserName {  get; set; }
        public string CategoryName { get; set; }
        public string CategoryCode{ get; set; }
        public string BudgetName { get; set; }
        public decimal? LimitAmount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; } 
        public string FinanceType { get; set; }
        
        public decimal NewUsedAmount { get; set; }
    }
}
