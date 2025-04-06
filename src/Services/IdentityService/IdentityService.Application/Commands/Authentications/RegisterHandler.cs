using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace IdentityService.Application.Commands.Authentications;

using DistributedCache.Redis;
using EventBus.Infrastructures.Interfaces;
using EventBus.RabbitMQ.Events;
using Domain.Entities;
using Interfaces;
using SharedKernel.Commons;
using SharedKernel.Extensions;
using Dtos.Authentications;
using static SharedKernel.Constants.ErrorCode;
using IdentityService.Application.Requests.Authentications;
using IdentityService.Application.Validates.Authentications;

public class RegisterHandler(
    IUserRepository repository,
    IPasswordHasher hasher,
    IDistributedCache cache,
    IConfirmationCodeGenerator generator,
    IEventBus eventBus
    ) : IRequestHandler<RegisterRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        // Validate
        var validate = new RegisterValidate().Validate(request);
        if (!validate.IsValid)
        {
            var errors = validate.Errors.ToDic();
            return res.SetError(nameof(E000), E000, errors);
        }

        var username = request.Username + "";
        var email = request.Email + "";
        var fullname = request.Fullname + "";
        var password = hasher.Hash(request.Password + "");

        if (await repository.IsUsernameExist(username))
        {
            return res.SetError(nameof(E101), E101);
        }

        if (await repository.IsEmailExist(email))
        {
            return res.SetError(nameof(E102), E102);
        }

        var confirmationCode = generator.GenerateCode();
        var user = User.Create(username, email, fullname, password);

        await cache.SetAsync(CacheKeys.ForEntity<User>(user.Id), user);

        var expiryTime = TimeSpan.FromMinutes(5); //TODD: updates
        var UserConfirmationDto = new UserConfirmationDto(user.Id, confirmationCode, expiryTime);

        await cache.SetAsync(CacheKeys.ForDto<UserConfirmationDto>(UserConfirmationDto.UserId), UserConfirmationDto);

        var integrationEvent = new SendConfirmationCodeEvent(email, fullname, confirmationCode, expiryTime);
        await eventBus.PublishAsync(integrationEvent);

        return res.SetSuccess(user.Id);
    }
}
