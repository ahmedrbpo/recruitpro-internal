using FluentAssertions;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Candidates.ManageEducation;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Recruitment.Entities;
using Xunit;

namespace RecruitPro.Application.Tests.Recruitment.Candidates;

public sealed class RemoveCandidateEducationCommandHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private RemoveCandidateEducationCommandHandler CreateHandler() => new(_db);

    [Fact]
    public async Task Handle_ExistingEducation_RemovesRow()
    {
        var candidate = Candidate.Create("Ada", "Lovelace", "ada@example.com", null);
        var education = CandidateEducation.Create(candidate.Id, "MIT", "B.Sc.", null,
            EducationType.FullTime, new DateOnly(2016, 6, 1), null, null);
        _db.Candidates.Add(candidate);
        _db.CandidateEducations.Add(education);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await CreateHandler().Handle(new RemoveCandidateEducationCommand(candidate.Id, education.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _db.CandidateEducations.Should().NotContain(e => e.Id == education.Id);
    }

    [Fact]
    public async Task Handle_UnknownEducation_ReturnsNotFound()
    {
        var result = await CreateHandler().Handle(
            new RemoveCandidateEducationCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.NotFound);
    }
}
