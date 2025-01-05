namespace Point.Order.Core.Domain.Contracts.Entities
{
    public interface IAuditable
    {
        DateTime Created { get; set; }
        DateTime LastModified { get; set; }
    }
}
