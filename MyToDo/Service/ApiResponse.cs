namespace MyToDo.Services;

public struct ApiResponse
{
    public ApiResponse(string message, bool status = false)
    {
        this.Message = message;
        this.Status = status;
    }

    public ApiResponse(bool status, object result)
    {
        this.Status = status;
        this.Result = result;
    }

    public string Message { get; set; } = default;

    public bool Status { get; set; }

    public object Result { get; set; } = default;
}

public struct ApiResponse<T>
{
    public ApiResponse(string message, bool status = false)
    {
        this.Message = message;
        this.Status = status;
    }

    public ApiResponse(bool status, T result)
    {
        this.Status = status;
        this.Result = result;
    }

    public string Message { get; set; } = default;

    public bool Status { get; set; }

    public T Result { get; set; } = default(T);
}