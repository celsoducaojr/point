using Point.Core.Domain.Contracts.Services;
using Point.Core.Domain.Enums;

namespace Point.Core.Domain.Services
{
    public interface IOrderItemService : IServices
    {
        bool IsStatusChangeAllowed(OrderItemStatus from, OrderItemStatus to);
    }

    public class OrderItemService : IOrderItemService
    {
        #region Constant

        private static Dictionary<OrderItemStatus, List<OrderItemStatus>> AllowedStatusUpdateDictionary = new()
        {
            { OrderItemStatus.Active, new()
            {
                OrderItemStatus.Voided,
                OrderItemStatus.Refunded
            } }
        };

        #endregion

        public bool IsStatusChangeAllowed(OrderItemStatus from, OrderItemStatus to)
        {
            if (AllowedStatusUpdateDictionary.TryGetValue(from, out var allowedStatuses)
                && allowedStatuses.Contains(to))
            {
                return true;
            }

            return false;
        }
    }
}
