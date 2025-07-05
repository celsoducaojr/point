using Point.Core.Domain.Contracts.Services;
using Point.Core.Domain.Enums;
using Point.Core.Domain.Services.Constants;

namespace Point.Core.Domain.Services
{
    public interface IOrderService : IServices 
    {
        string GenerateOrderNumber();
        bool IsStatusChangeAllowed(OrderStatus from, OrderStatus to);
    }

    public class OrderService : IOrderService
    {
        public string GenerateOrderNumber()
        {
            DateTime now = DateTime.UtcNow;
            long ticks = now.Ticks;
            string base64 = Convert.ToBase64String(BitConverter.GetBytes(ticks));

            // Make it URL/file-safe
            return base64.Replace("+", "-").Replace("/", "_").TrimEnd('=').ToUpper(); 
        }

        public bool IsStatusChangeAllowed(OrderStatus from, OrderStatus to)
        {
            if (OrderConstants.AllowedStatusUpdateDictionary.TryGetValue(from, out var allowedStatuses)
                && allowedStatuses.Contains(to))
            {
                return true;
            }

            return false;
        }
    }
}
