namespace Common.Mediator;

public sealed record MediatRPipelineOptions
{
    public bool UseAuthorization { get; init; } = true;
    public bool UseLogging { get; init; } = true;
    public bool UsePerformance { get; init; } = true;
    public bool UseExceptions { get; init; } = true;
    public bool UseValidation { get; init; } = true;
}