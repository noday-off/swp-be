namespace SWP391_PreCookingPackage.ModelsDTO
{
    public class OrderModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? TotalPrice { get; set; }

        public string? Status { get; set; }

        public DateTime? ShipDate { get; set; }

        public string? PaymentMethod { get; set; }
        public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel> { };
    }
    public class OrderCreateModel
    {
        public int? Id { get; set; }
        public int UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? TotalPrice { get; set; }

        public string? Status { get; set; }

        public DateTime? ShipDate { get; set; }

        public string? PaymentMethod { get; set; }
    }

    public class OrderItemModel
    {
        public int? Id { get; set; }

        public int OrderId { get; set; }

        public int PackageId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
        public PackageModel? Package { get; set; }
    }
}
