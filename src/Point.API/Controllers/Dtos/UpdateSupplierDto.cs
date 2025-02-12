namespace Point.API.Controllers.Models
{
    public sealed record UpdateSupplierDto(
        string Name,
        string? Remarks);
}
