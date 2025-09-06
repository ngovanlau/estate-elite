namespace Common.Application.Responses;

public class ApiResponse<T> where T : ApiResponse<T>
{
    public bool Succeeded { get; protected set; }
    public string? Message { get; protected set; }
    public string? Code { get; protected set; }
    public object? Data { get; protected set; }
    public object? Errors { get; protected set; }

    public ApiResponse()
    {
        Succeeded = true;
    }

    public T SetSuccess(object? data)
    {
        Succeeded = true;
        Data = data;

        return (T)this;
    }

    public T SetError(string code)
    {
        Succeeded = false;
        Code = code;

        return (T)this;
    }

    public T SetError(string code, string? message)
    {
        SetError(code);
        Message = message;

        return (T)this;
    }

    public T SetError(string code, string? message, object? data)
    {
        SetError(code, message);
        Data = data;

        return (T)this;
    }

    public T SetError(string code, string? message, object? data, object? errors)
    {
        SetError(code, message, data);
        Errors = errors;

        return (T)this;
    }
}
public class ApiResponse : ApiResponse<ApiResponse>
{
}
