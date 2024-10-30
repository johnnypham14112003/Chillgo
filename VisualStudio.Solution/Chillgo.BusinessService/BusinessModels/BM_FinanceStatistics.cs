namespace Chillgo.BusinessService.BusinessModels
{
    public class BM_FinanceStatistics
    {
        public int TotalPackagesSold { get; set; }
        public int TotalPackagesSoldByDay { get; set; }
        public int Commission { get; set; }
        public decimal RevenueCash { get; set; }
        public decimal RevenueByDay { get; set; }
        public string DayTime { get; set; } = "Today";
    }
}