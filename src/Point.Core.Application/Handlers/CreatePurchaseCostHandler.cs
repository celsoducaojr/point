using MediatR;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Request.Children;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers
{
    public sealed record CreatePurchaseCostRequest(
        int Id,
        decimal InitialAmount,
        decimal FinalAmount)
        : IRequest;
    
    public class CreatePurchaseCostHandler(IPointDbContext pointDbContext) : IRequestHandler<CreatePurchaseCostRequest>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task Handle(CreatePurchaseCostRequest request, CancellationToken cancellationToken)
        {
            if (await _pointDbContext.ItemUnit.FindAsync(request.Id, cancellationToken) == null)
            {
                throw new NotFoundException("Item Unit not found.");
            }

            var cost = new PurchaseCost 
            {
                Id = request.Id,
                InitialAmount = request.InitialAmount,
                FinalAmount = request.FinalAmount
                //Variations = request.Variations?
                //    .Select(v => new DiscountVariation 
                //    { 
                //        Amount = v.Amount, 
                //        Percentage = v.Percentage, 
                //        Remarks = v.Remarks 
                //    }).ToList()
            };

            _pointDbContext.PurchaseCost.Add(cost);
            await _pointDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
