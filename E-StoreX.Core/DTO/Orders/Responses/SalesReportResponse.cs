namespace EStoreX.Core.DTO.Orders.Responses
{
    public class SalesReportResponse
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public List<TopProductResponse> TopProducts { get; set; } = new();
    }
}
