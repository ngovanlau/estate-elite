namespace SharedKernel.Responses;

public class ApiResponse
{
    public bool Succeeded { get; private set; }
    public string? Message { get; private set; }
    public string? Code { get; private set; }
    public object? Data { get; private set; }
    public object? Errors { get; private set; }

    public ApiResponse()
    {
        Succeeded = true;
    }

    public ApiResponse SetSuccess(object? data)
    {
        Succeeded = true;
        Data = data;

        return this;
    }

    public ApiResponse SetError(string code)
    {
        Succeeded = false;
        Code = code;

        return this;
    }

    public ApiResponse SetError(string code, string? message)
    {
        SetError(code);
        Message = message;

        return this;
    }

    public ApiResponse SetError(string code, string? message, object? data)
    {
        SetError(code, message);
        Data = data;

        return this;
    }

    public ApiResponse SetError(string code, string? message, object? data, object? errors)
    {
        SetError(code, message, data);
        Errors = errors;

        return this;
    }
}
