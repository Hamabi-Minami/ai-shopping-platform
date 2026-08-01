namespace MyShippingPlatform.Application.Common.Models;

public class Result
{
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }

    // Required by Reflection and System.Text.Json Serializer
    public Result()
    {
    }

    public Result(bool isSuccess, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string errorMessage) => new(false, errorMessage);

    // Generic static factory methods
    public static Result<T> Success<T>(T data) => Result<T>.Success(data);
    public static Result<T> Failure<T>(string errorMessage) => Result<T>.Failure(errorMessage);
}

public class Result<T> : Result
{
    public T? Data { get; private set; }

    public Result()
    {
    }

    private Result(bool isSuccess, T? data, string? errorMessage)
        : base(isSuccess, errorMessage)
    {
        Data = data;
    }

    public static Result<T> Success(T data) => new(true, data, null);
    public new static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
}