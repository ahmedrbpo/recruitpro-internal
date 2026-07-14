using FluentAssertions;
using RecruitPro.Domain.Recruitment.Entities;
using Xunit;

namespace RecruitPro.Domain.Tests.Recruitment;

public sealed class CandidateTests
{
    private static Candidate CreateCandidate() =>
        Candidate.Create("Ada", "Lovelace", "ada@example.com", "1234567890");

    [Fact]
    public void UpdatePersonalDetails_SetsAllFields()
    {
        var candidate = CreateCandidate();
        var dob = new DateOnly(1990, 1, 1);

        candidate.UpdatePersonalDetails(Gender.Female, dob, "ABCDE1234F", 8.5m, 6m,
            "221B Baker Street", "London", "Greater London", "NW1", "London");

        candidate.Gender.Should().Be(Gender.Female);
        candidate.DateOfBirth.Should().Be(dob);
        candidate.Pan.Should().Be("ABCDE1234F");
        candidate.TotalExperienceYears.Should().Be(8.5m);
        candidate.RelevantExperienceYears.Should().Be(6m);
        candidate.StreetAddress.Should().Be("221B Baker Street");
        candidate.City.Should().Be("London");
        candidate.State.Should().Be("Greater London");
        candidate.PostalCode.Should().Be("NW1");
        candidate.WorkLocation.Should().Be("London");
    }
}
