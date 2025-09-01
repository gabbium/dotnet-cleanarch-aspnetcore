namespace CleanArch.AspNetCore;

public static class ResultExtensions
{
    public static IResult Ok<TValue>(this Result<TValue> result)
    {
        return result.IsSuccess ? Results.Ok(result.Value) : CustomResults.Problem(result);
    }

    public static IResult NoContent(this Result result)
    {
        return result.IsSuccess ? Results.NoContent() : CustomResults.Problem(result);
    }

    public static IResult Created<TValue>(this Result<TValue> result, Func<TValue, string> location)
    {
        return result.IsSuccess ? Results.Created(location(result.Value), result.Value) : CustomResults.Problem(result);
    }
}
