using Point.Core.Domain.Contracts.Services;

namespace Point.Core.Domain.Services
{
    public interface IOrderService : IServices 
    {
        string GenerateOrderNumber();
    }

    public class OrderService : IOrderService
    {
        public string GenerateOrderNumber()
        {
            return DateTime.Now.ToString("yyMMdd-HHmm");
        }
    }
}
