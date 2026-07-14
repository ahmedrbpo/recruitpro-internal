using FluentAssertions;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Applications.SetSubmissionDetails;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Recruitment.Entities;
using Xunit;

namespace RecruitPro.Application.Tests.Recruitment.Applications;

public sealed class SetSubmissionDetailsCommandHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private SetSubmissionDetailsCommandHandler CreateHandler() => new(_db);

    [Fact]
    public async Task Handle_ExistingApplication_SetsSubmissionDetails()
    {
        var application = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid());
        _db.Applications.Add(application);
        await _db.SaveChangesAsync(CancellationToken.None);

        var command = new SetSubmissionDetailsCommand(application.Id, ApplicationWorkType.Remote,
            ApplicationInterviewType.Telephonic, 1_000_000m, 1_300_000m, "123456789012");
        var result = await CreateHandler().Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.WorkType.Should().Be(ApplicationWorkType.Remote);
        result.Value.CurrentCTC.Should().Be(1_000_000m);
    }

    [Fact]
    public async Task Handle_UnknownApplication_ReturnsNotFound()
    {
        var command = new SetSubmissionDetailsCommand(Guid.NewGuid(), ApplicationWorkType.Remote,
            ApplicationInterviewType.Telephonic, 1_000_000m, 1_300_000m, null);
        var result = await CreateHandler().Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.NotFound);
    }
}
