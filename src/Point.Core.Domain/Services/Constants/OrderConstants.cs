using Point.Core.Domain.Enums;

namespace Point.Core.Domain.Services.Constants
{
    internal static class OrderConstants
    {
        internal static Dictionary<OrderStatus, List<OrderStatus>> AllowedStatusUpdateDictionary = new()
        {
            { OrderStatus.New, new()
            {
                OrderStatus.Released,
                OrderStatus.Cancelled
            } }
        };
    }
}
