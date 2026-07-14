using FluentAssertions;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Candidates.UpdateCandidate;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Recruitment.Entities;
using Xunit;

namespace RecruitPro.Application.Tests.Recruitment.Candidates;

public sealed class UpdateCandidateCommandHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private UpdateCandidateCommandHandler CreateHandler() => new(_db);

    [Fact]
    public async Task Handle_ExistingCandidate_UpdatesPersonalDetails()
    {
        var candidate = Candidate.Create("Ada", "Lovelace", "ada@example.com", "1234567890");
        _db.Candidates.Add(candidate);
        await _db.SaveChangesAsync(CancellationToken.None);

        var command = new UpdateCandidateCommand(candidate.Id, Gender.Female, new DateOnly(1990, 1, 1),
            "ABCDE1234F", 8.5m, 6m, "221B Baker Street", "London", "Greater London", "NW1", "London");
        var result = await CreateHandler().Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Pan.Should().Be("ABCDE1234F");
        result.Value.Gender.Should().Be(Gender.Female);
    }

    [Fact]
    public async Task Handle_UnknownCandidate_ReturnsNotFound()
    {
        var command = new UpdateCandidateCommand(Guid.NewGuid(), null, null, null, null, null, null, null, null, null, null);
        var result = await CreateHandler().Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.NotFound);
    }
}
