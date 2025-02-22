using MediatR;

namespace Point.Core.Application.Handlers.Request.Children
{
    public sealed record CreateDiscountVariationRequest(
        decimal? Amount,
        decimal? Percentage,
        string? Remarks)
        : IRequest;
}
