namespace RecruitPro.Domain.Common.Exceptions;

public sealed class ApplicationStageTransitionException(string fromStage, string toStage, string reason)
    : DomainException($"Cannot move application from '{fromStage}' to '{toStage}': {reason}");
