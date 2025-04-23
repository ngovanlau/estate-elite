using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PropertyService.Application.Extensions;
using PropertyService.Application.Interfaces;
using PropertyService.Application.Requests.Properties;
using PropertyService.Domain.Entities;
using SharedKernel.Extensions;
using SharedKernel.Interfaces;
using SharedKernel.Responses;
using static SharedKernel.Constants.ErrorCode;

namespace PropertyService.Application.Commands.Properties;

public class CreatePropertyHandler(
    IValidator<CreatePropertyRequest> validator,
    IMapper mapper,
    IFileStorageService fileStorageService,
    IPropertyRepository propertyRepository,
    IPropertyTypeRepository propertyTypeRepository,
    IAddressRepository addressRepository,
    IRoomRepository roomRepository,
    IUtilityRepository utilityRepository,
    IImageRepository imageRepository,
    ILogger<CreatePropertyHandler> logger) : IRequestHandler<CreatePropertyRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(CreatePropertyRequest request, CancellationToken cancellationToken)
    {
        var response = new ApiResponse();

        using var transaction = await propertyRepository.BeginTransactionAsync(cancellationToken);

        try
        {
            logger.LogInformation("Processing create property request with title: {PropertyTitle}", request.Title);

            // Validate request
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                logger.LogWarning("Property creation validation failed");
                var errors = validationResult.Errors.ToDic();
                return response.SetError(nameof(E000), E000, errors);
            }

            // Verify property type exists first
            var propertyType = await propertyTypeRepository.GetByIdAsync(request.PropertyTypeId, cancellationToken);
            if (propertyType is null)
            {
                logger.LogWarning("Property type not found. Id: {PropertyTypeId}", request.PropertyTypeId);
                return response.SetError(nameof(E008), string.Format(E008, nameof(request.PropertyTypeId)));
            }

            // Map and prepare property with relationships
            var property = mapper.Map<Property>(request);
            property.PropertyTypeId = propertyType.Id;
            property.Address = mapper.Map<Address>(request.Address);

            // Upload images last after all other operations succeeded
            if (request.Images is not null && request.Images.Any())
            {
                logger.LogInformation("Uploading {ImageCount} images for property", request.Images.Count);
                await property.UploadImagesAsync(request.Images, fileStorageService, logger, cancellationToken);
                //await imageRepository.AddImagesAsync(property.Images.ToList(), cancellationToken);
            }

            // Save property to generate ID
            var data = await propertyRepository.AddProperty(property, cancellationToken);
            logger.LogInformation("Property created successfully. ID: {PropertyId}", property.Id);

            // Process utilities if present
            if (request.UtilityIds is not null && request.UtilityIds.Any())
            {
                logger.LogInformation("Processing {UtilityCount} utilities for property", request.UtilityIds.Count);
                var utilities = await utilityRepository.GetUtilityByIdsAsync(request.UtilityIds, cancellationToken);
                property.Utilities = utilities;
            }

            // Process rooms if present
            if (request.Rooms is not null && request.Rooms.Any())
            {
                logger.LogInformation("Processing {RoomCount} rooms for property", request.Rooms.Count);
                var ids = request.Rooms.Select(p => p.Id).ToList();
                var rooms = await roomRepository.GetRoomsByIdsAsync(ids, cancellationToken);

                foreach (var roomDto in request.Rooms)
                {
                    var room = rooms.GetValueOrDefault(roomDto.Id);
                    if (room is not null)
                    {
                        property.PropertyRooms.Add(new PropertyRoom
                        {
                            Property = property,
                            Room = room,
                            Quantity = roomDto.Quantity
                        });
                    }
                    else
                    {
                        logger.LogWarning("Room with ID {RoomId} not found when creating property", roomDto.Id);
                    }
                }
            }

            await transaction.CommitAsync(cancellationToken);
            return response.SetSuccess(data);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(ex, "Unexpected error while creating property: {ErrorMessage}", ex.Message);
            return response.SetError(nameof(E000), E000, ex);
        }
    }
}