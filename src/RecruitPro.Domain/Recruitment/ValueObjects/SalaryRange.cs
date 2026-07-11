namespace RecruitPro.Domain.Recruitment.ValueObjects;

public sealed record SalaryRange
{
    public decimal Min { get; }
    public decimal Max { get; }

    public SalaryRange(decimal min, decimal max)
    {
        if (min < 0)
            throw new ArgumentOutOfRangeException(nameof(min), "Salary minimum cannot be negative.");
        if (max < min)
            throw new ArgumentOutOfRangeException(nameof(max), "Salary maximum must be greater than or equal to the minimum.");

        Min = min;
        Max = max;
    }
}
