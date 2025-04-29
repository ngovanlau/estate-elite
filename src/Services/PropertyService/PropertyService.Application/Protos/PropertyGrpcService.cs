using Grpc.Core;
using Microsoft.Extensions.Logging;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.Protos;

namespace PropertyService.Application.Protos;

public class PropertyGrpcService : SharedKernel.Protos.PropertyService.PropertyServiceBase
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IPropertyRentalRepository _propertyRentalRepository;
    private readonly ILogger<PropertyGrpcService> _logger;

    public PropertyGrpcService(
        IPropertyRepository propertyRepository,
        IPropertyRentalRepository propertyRentalRepository,
        ILogger<PropertyGrpcService> logger)
    {
        _propertyRepository = propertyRepository ?? throw new ArgumentNullException(nameof(propertyRepository));
        _propertyRentalRepository = propertyRentalRepository ?? throw new ArgumentNullException(nameof(propertyRentalRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<GetPropertyResponse?> GetProperty(GetPropertyRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Attempting to get property with ID: {PropertyId}", request.Id);

        try
        {
            if (!Guid.TryParse(request.Id, out var propertyId))
            {
                _logger.LogWarning("Invalid property ID format: {PropertyId}", request.Id);
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid property ID format"));
            }

            var property = await _propertyRepository.GetDtoByIdAsync<GetPropertyResponse>(propertyId, context.CancellationToken);

            if (property == null)
            {
                _logger.LogWarning("Property not found with ID: {PropertyId}", request.Id);
            }
            else
            {
                _logger.LogInformation("Successfully retrieved property with ID: {PropertyId}", request.Id);
            }

            return property;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting property with ID: {PropertyId}", request.Id);
            return null;
        }
    }

    public override async Task<CreatePropertyRentalResponse> CreatePropertyRental(CreatePropertyRentalRequest request, ServerCallContext context)
    {
        var res = new CreatePropertyRentalResponse { Success = false };
        _logger.LogInformation("Attempting to create property rental for property ID: {PropertyId} and user ID: {UserId}",
            request.PropertyId, request.UserId);

        try
        {
            if (!Guid.TryParse(request.PropertyId, out var propertyId))
            {
                _logger.LogWarning("Invalid property ID format: {PropertyId}", request.PropertyId);
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid property ID format"));
            }

            if (!Guid.TryParse(request.UserId, out var userId))
            {
                _logger.LogWarning("Invalid property ID format: {UserId}", request.UserId);
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid property ID format"));
            }

            var property = await _propertyRepository.GetByIdAsync(propertyId, context.CancellationToken);
            if (property == null)
            {
                _logger.LogWarning("Property not found with ID: {PropertyId}", request.PropertyId);
                throw new RpcException(new Status(StatusCode.NotFound, "Property not found"));
            }

            var startDate = DateTime.UtcNow;
            var endDate = CalculateEndDate(startDate, property.RentPeriod, request.RentalPeriod);

            if (endDate == startDate)
            {
                _logger.LogWarning("Invalid rent period type: {RentPeriod}", property.RentPeriod);
                return res;
            }

            var propertyRental = new PropertyRental
            {
                PropertyId = propertyId,
                StartDate = startDate,
                EndDate = endDate,
                UserId = userId,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow,
            };

            property.Status = PropertyStatus.Completed;

            res.Success = await _propertyRentalRepository.AddEntityAsync(propertyRental, context.CancellationToken);

            _logger.LogInformation("Property rental creation {(Status)} for property ID: {PropertyId}",
                res.Success ? "succeeded" : "failed", request.PropertyId);

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating property rental for property ID: {PropertyId}", request.PropertyId);
            return res;
        }
    }

    private static DateTime CalculateEndDate(DateTime startDate, RentPeriod? rentPeriod, int rentalPeriod)
    {
        return rentPeriod switch
        {
            RentPeriod.Day => startDate.AddDays(rentalPeriod),
            RentPeriod.Month => startDate.AddMonths(rentalPeriod),
            RentPeriod.Year => startDate.AddYears(rentalPeriod),
            _ => throw new RpcException(new Status(StatusCode.InvalidArgument, "Property not have rent period"))
        };
    }
}