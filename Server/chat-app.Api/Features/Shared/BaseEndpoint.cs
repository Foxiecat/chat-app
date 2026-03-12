namespace chat_app.Api.Features.Shared;

public abstract class BaseEndpoint<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor)
{
    private ILogger? _logger;

    protected HttpContext HttpContext =>
        httpContextAccessor.HttpContext
        ?? throw new InvalidOperationException("HttpContext is not available.");
    protected IServiceProvider Services => HttpContext.RequestServices;

    protected T GetRequired<T>()
        where T : notnull => Services.GetRequiredService<T>();

    protected T? GetService<T>() => Services.GetService<T>();

    // Logger helper on-demand
    protected ILogger Logger => _logger ??= GetRequired<ILoggerFactory>().CreateLogger(GetType());

    // Result Helpers
    protected static IResult Ok(object? value = null) => Results.Ok(value);

    protected static IResult Created(string uri, object? value = null) =>
        Results.Created(uri, value);

    protected static IResult CreatedAt(string routeName, object? routeValues, object? value) =>
        Results.CreatedAtRoute(routeName, routeValues, value);

    protected static IResult NoContent() => Results.NoContent();

    protected static IResult NotFound(object? value = null) => Results.NotFound(value);

    protected static IResult BadRequest(object? error = null) => Results.BadRequest(error);

    protected static IResult Unauthorized() => Results.Unauthorized();

    protected static IResult Forbid(params string[] authenticationSchemes) =>
        Results.Forbid(authenticationSchemes: authenticationSchemes);

    protected static IResult Problem(
        string title,
        int status = 500,
        string? detail = null,
        string? instance = null
    ) => Results.Problem(title: title, statusCode: status, detail: detail, instance: instance);

    protected static IResult ValidationProblem(
        IDictionary<string, string[]> errors,
        int status = 400
    ) => Results.ValidationProblem(errors, statusCode: status);

    ///<summary>
    /// Validation hook (override if needed)
    /// </summary>
    protected virtual (bool IsValid, IDictionary<string, string[]> Errors) ValidateRequest(
        TRequest request
    ) => (true, new Dictionary<string, string[]>());

    ///<summary>
    ///Safe executor to unify try/catch, logging and validation
    ///</summary>
    protected async Task<IResult> ExecuteAsync(
        TRequest request,
        Func<CancellationToken, Task<IResult>> action,
        Func<Exception, IResult>? onError = null,
        bool validate = true,
        CancellationToken ct = default
    )
    {
        if (validate)
        {
            (bool isValid, IDictionary<string, string[]> errors) = ValidateRequest(request);
            if (!isValid)
                return ValidationProblem(errors);
        }

        try
        {
            return await action(ct).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return Results.StatusCode(StatusCodes.Status499ClientClosedRequest);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Unhandled exception in {Endpoint}", GetType().Name);
            return onError?.Invoke(e) ?? Problem("An unexpected error occured.");
        }
    }
}
