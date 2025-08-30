using System.Text.RegularExpressions;
using Point.Core.Domain.Contracts.Services;
using Point.Core.Domain.Entities.Orders;
using Point.Core.Domain.Enums;

namespace Point.Core.Domain.Services
{
    public interface IOrderService : IServices 
    {
        bool IsStatusChangeAllowed(OrderStatus from, OrderStatus to);
        bool IsStatusChangeAllowed(OrderStatus orderStatus, OrderItemStatus orderItemStatusTo);
        string GenerateOrderNumber();
        decimal GenerateBalance(Order order);
        decimal GenerateTotal(List<OrderItem> items);
    }

    public class OrderService : IOrderService
    {
        #region Constant

        private static Dictionary<OrderStatus, List<OrderStatus>> AllowedStatusUpdateDictionary = new()
        {
            { OrderStatus.New, new()
            {
                OrderStatus.Released,
                OrderStatus.Cancelled
            } }
        };

        private static Dictionary<OrderStatus, List<OrderItemStatus>> AllowedOrderItemStatusUpdateDictionary = new()
        {
            { OrderStatus.Released, new() {OrderItemStatus.Voided }},
            { OrderStatus.Paid, new() {OrderItemStatus.Refunded }},
            { OrderStatus.PartiallyPaid, new() {OrderItemStatus.Refunded }}
        };

        #endregion

        public bool IsStatusChangeAllowed(OrderStatus from, OrderStatus to)
        {
            if (AllowedStatusUpdateDictionary.TryGetValue(from, out var allowedStatuses)
                && allowedStatuses.Contains(to))
            {
                return true;
            }

            return false;
        }

        public bool IsStatusChangeAllowed(OrderStatus orderStatus, OrderItemStatus orderItemStatusTo)
        {
            if (AllowedOrderItemStatusUpdateDictionary.TryGetValue(orderStatus, out var allowedStatuses)
                && allowedStatuses.Contains(orderItemStatusTo))
            {
                return true;
            }

            return false;
        }

        public string GenerateOrderNumber()
        {
            var base64 = Convert.ToBase64String(BitConverter.GetBytes(DateTime.UtcNow.Ticks));

            return Regex.Replace(base64, "[^a-zA-Z0-9]", string.Empty).ToUpper();
        }

        public decimal GenerateBalance(Order order)
        {
            var payments = order.Payments?.Sum(payment => payment.Amount) ?? 0;

            return order.Total - payments;
        }
        
        public decimal GenerateTotal(List<OrderItem> items)
        {
            return items.Where(item => item.Status == OrderItemStatus.Active).Sum(item => item.Total);
        }
    }
}
