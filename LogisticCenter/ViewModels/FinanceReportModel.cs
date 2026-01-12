using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticCenter.ViewModels
{
    public class FinanceReportModel
    {
        public string Total_Sales { get; set; }
        public string Completed_Orders { get; set; }
        public decimal Avg_Check { get; set; }
        public List<TopProduct> Top_Products { get; set; }
    }

    public class TopProduct
    {
        public string Name { get; set; }
        public string Qty { get; set; }
    }

}
