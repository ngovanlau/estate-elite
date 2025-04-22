using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PropertyService.Application.Interfaces;
using PropertyService.Application.Requests.Properties;
using PropertyService.Domain.Entities;
using SharedKernel.Commons;
using SharedKernel.Extensions;
using SharedKernel.Interfaces;
using static SharedKernel.Constants.ErrorCode;
using PropertyService.Application.Extensions;

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

            // Map and prepare property
            var property = mapper.Map<Property>(request);

            // Upload images
            logger.LogInformation("Uploading {ImageCount} images for property", request.Images.Count);
            await property.UploadImagesAsync(request.Images, fileStorageService, logger, cancellationToken);
            await imageRepository.AddImagesAsync(property.Images.ToList(), cancellationToken);

            // Verify and set property type
            var propertyType = await propertyTypeRepository.GetPropertyTypeByIdAsync(request.PropertyTypeId, cancellationToken);
            if (propertyType is null)
            {
                logger.LogWarning("Property type not found. Id: {PropertyTypeId}", request.PropertyTypeId);
                return response.SetError(nameof(E008), string.Format(E008, nameof(request.PropertyTypeId)));
            }
            property.Type = propertyType;

            // Process address
            var address = mapper.Map<Address>(request.Address);
            if (!await addressRepository.AddAddressAsync(address, cancellationToken))
            {
                logger.LogError("Failed to add address for property");
                return response.SetError(nameof(E011), string.Format(E011, nameof(address)));
            }
            property.Address = address;

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

            // Save property
            logger.LogInformation("Saving new property to database");
            var data = await propertyRepository.AddProperty(property, cancellationToken);

            logger.LogInformation("Property created successfully. ID: {PropertyId}", property.Id);
            return response.SetSuccess(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating property: {ErrorMessage}", ex.Message);
            return response.SetError(nameof(E000), E000, ex);
        }
    }
}