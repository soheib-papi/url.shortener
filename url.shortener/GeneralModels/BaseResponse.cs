namespace url.shortener.GeneralModels;

public class BaseResponse<T>
{
    public bool Succeeded { get; set; }
    public string? Error { get; set; }
    public T? Result { get; set; } = default;

    private BaseResponse(bool succeeded, string? error, T result)
    {
        Succeeded = succeeded;
        Error = error;
        Result = result;
    }

    private BaseResponse(bool succeeded, string error)
    {
        Succeeded = succeeded;
        Error = error;
    }

    /// <summary>
    /// It's just exists because of deserialization purpose
    /// </summary>
    public BaseResponse()
    {

    }

    public static BaseResponse<T> Success(T result)
    {
        return new BaseResponse<T>(true, null, result);
    }

    public static BaseResponse<T> Failure(string? error = null)
    {
        return new BaseResponse<T>(false, error ?? string.Empty);
    }
}