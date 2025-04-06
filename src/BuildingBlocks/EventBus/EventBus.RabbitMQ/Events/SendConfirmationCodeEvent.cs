﻿namespace EventBus.RabbitMQ.Events;

using Infrastructures;
using Infrastructures.Interfaces;

public class SendConfirmationCodeEvent(string email, string fullname, string confirmationCode, TimeSpan expiryTime) : Event, IEvent
{
    public string Email { get; private set; } = email;
    public string Fullname { get; private set; } = fullname;
    public string ConfirmationCode { get; private set; } = confirmationCode;
    public TimeSpan ExpiryTime { get; private set; } = expiryTime;
}
