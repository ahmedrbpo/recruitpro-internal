namespace RecruitPro.Domain.Recruitment.ValueObjects;

/// <summary>
/// The fixed application pipeline state machine: applied -> screening -> interview -> offer ->
/// hired/rejected. Rejected is reachable from every non-terminal stage; Hired is reachable only
/// from Offer. Hired and Rejected are terminal — no further transitions are valid from either.
/// </summary>
public static class ApplicationStage
{
    public const string Applied = "applied";
    public const string Screening = "screening";
    public const string Interview = "interview";
    public const string Offer = "offer";
    public const string Hired = "hired";
    public const string Rejected = "rejected";

    private static readonly Dictionary<string, string[]> ValidTransitions = new()
    {
        [Applied] = [Screening, Rejected],
        [Screening] = [Interview, Rejected],
        [Interview] = [Offer, Rejected],
        [Offer] = [Hired, Rejected],
        [Hired] = [],
        [Rejected] = [],
    };

    public static bool IsValidTransition(string from, string to) =>
        ValidTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);

    public static bool IsTerminal(string stage) => stage is Hired or Rejected;
}
