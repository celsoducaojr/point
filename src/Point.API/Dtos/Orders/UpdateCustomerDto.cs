namespace Point.API.Dtos.Orders
{
    public sealed record UpdateCustomerDto(
        string Name, 
        string? MobileNumber,
        string? Email,
        string? Address,
        string? Remarks);
}
