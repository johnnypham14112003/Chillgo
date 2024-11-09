namespace Chillgo.BusinessService.BusinessModels
{
    public class BM_FinanceStatistics
    {
        public int TotalTransactions { get; set; }
        public decimal TotalAmount { get; set; }
        public Dictionary<string, int> PaymentMethodStats { get; set; }
    }
}