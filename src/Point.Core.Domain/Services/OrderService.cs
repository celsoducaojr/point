using System.Text.RegularExpressions;
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
            var base64 = Convert.ToBase64String(BitConverter.GetBytes(DateTime.UtcNow.Ticks));

            return Regex.Replace(base64, "[^a-zA-Z0-9]", string.Empty).ToUpper();
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
