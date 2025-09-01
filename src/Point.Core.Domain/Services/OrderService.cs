using Point.Core.Domain.Contracts.Services;
using Point.Core.Domain.Entities.Orders;
using Point.Core.Domain.Enums;
using System.Text.RegularExpressions;

namespace Point.Core.Domain.Services
{
    public interface IOrderService : IServices 
    {
        bool IsStatusChangeAllowed(OrderStatus from, OrderStatus to);
        bool IsStatusChangeAllowed(OrderStatus orderStatus, OrderItemStatus orderItemStatusTo);
        bool IsValidCalculations(Order order);
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

        public bool IsValidCalculations(Order order)
        {
            var valid = true;

            foreach (var item in order.Items)
            {
                if (item.Total != (item.Price * item.Quantity) - item.Discount)
                {
                    valid = false;
                    break;
                }
            }

            if (order.SubTotal != GenerateTotal(order.Items))
            {
                valid = false;
            }

            if (order.Total != order.SubTotal - order.Discount)
            {
                valid = false;
            }

            if (order.Payments != null && order.Payments[0].Amount != order.Total)
            {
                valid = false;
            }

            return valid;
        }

        public string GenerateOrderNumber()
        {
            var base64 = Convert.ToBase64String(BitConverter.GetBytes(DateTime.UtcNow.Ticks));

            return Regex.Replace(base64, "[^a-zA-Z0-9]", string.Empty).ToUpper();
        }

        public decimal GenerateBalance(Order order)
        {
            return order.Total - (order.Payments?.Sum(payment => payment.Amount) ?? 0);
        }
        
        public decimal GenerateTotal(List<OrderItem> items)
        {
            return items.Where(item => item.Status == OrderItemStatus.Active).Sum(item => item.Total);
        }
    }
}
