using FluentAssertions;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Candidates.ManageEducation;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;
using Xunit;

namespace RecruitPro.Application.Tests.Recruitment.Candidates;

public sealed class AddCandidateEducationCommandHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private AddCandidateEducationCommandHandler CreateHandler() => new(_db);

    private async Task<Guid> SeedCandidateAsync()
    {
        var candidate = Candidate.Create("Ada", "Lovelace", "ada@example.com", null);
        _db.Candidates.Add(candidate);
        await _db.SaveChangesAsync(CancellationToken.None);
        return candidate.Id;
    }

    [Fact]
    public async Task Handle_ValidEducation_AddsRowAndReturnsDto()
    {
        var candidateId = await SeedCandidateAsync();

        var command = new AddCandidateEducationCommand(candidateId, "MIT", "B.Sc.", "Computer Science",
            EducationType.FullTime, new DateOnly(2016, 6, 1), new DateOnly(2020, 6, 1), "Boston");
        var result = await CreateHandler().Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.College.Should().Be("MIT");
        _db.CandidateEducations.Should().ContainSingle(e => e.CandidateId == candidateId);
    }

    [Fact]
    public async Task Handle_UnknownCandidate_ReturnsNotFound()
    {
        var command = new AddCandidateEducationCommand(Guid.NewGuid(), "MIT", "B.Sc.", null,
            EducationType.FullTime, new DateOnly(2016, 6, 1), null, null);
        var result = await CreateHandler().Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.NotFound);
    }

    [Fact]
    public async Task Handle_EndDateBeforeStartDate_ThrowsInvalidDateRangeException()
    {
        var candidateId = await SeedCandidateAsync();

        var command = new AddCandidateEducationCommand(candidateId, "MIT", "B.Sc.", null,
            EducationType.FullTime, new DateOnly(2020, 6, 1), new DateOnly(2019, 6, 1), null);
        var act = async () => await CreateHandler().Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidDateRangeException>();
    }
}
