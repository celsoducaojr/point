using Point.Core.Domain.Entities.Orders;
using Point.Core.Domain.Enums;

namespace Point.API.Dtos.Listing
{
    public sealed class SearchOrderResponseDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Released { get; set; }
        public string Number { get; set; } // Order Number
        public Customer? Customer { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
        public List<SearchOrderItemResponseDto> Items { get; set; }
        public PaymentTerm? PaymentTerm { get; set; }
        public List<SearchPaymentResponseDto>? Payments { get; set; }
        public List<SearchRefundResponseDto>? Refunds { get; set; }
    }

    public sealed class SearchOrderItemResponseDto
    {
        public int Id { get; set; }
        public int ItemUnitId { get; set; }
        public string ItemName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public OrderItemStatus Status { get; set; }
    }

    public sealed class SearchPaymentResponseDto 
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public decimal Amount { get; set; }
        public PaymentMode Mode { get; set; }
        public string? Reference { get; set; }
        public string? Remarks { get; set; }
    }

    public sealed class SearchRefundResponseDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public decimal Amount { get; set; }
        public PaymentMode Mode { get; set; }
        public string? Reference { get; set; }
        public string? Remarks { get; set; }
        public int OrderItemId { get; set; }
    }
}
