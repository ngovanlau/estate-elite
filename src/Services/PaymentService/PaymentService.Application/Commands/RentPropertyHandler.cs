using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Requests;
using SharedKernel.Extensions;
using SharedKernel.Responses;
using static SharedKernel.Constants.ErrorCode;

namespace PaymentService.Application.Commands;

public class RentPropertyHandler(
    IValidator<RentPropertyRequest> validator,
    ITransactionRepository repository,
    IPaypalService paypalService,
    ILogger<RentPropertyHandler> logger) : IRequestHandler<RentPropertyRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(RentPropertyRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        try
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDic();
                return res.SetError(nameof(E000), E000, errors);
            }



            return res.SetSuccess(true);
        }
        catch (Exception ex)
        {
            return res.SetError(nameof(E000), E000, ex);
        }
    }
}