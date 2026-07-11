using FluentAssertions;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Recruitment.Jobs.PublishJob;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.ValueObjects;
using Xunit;

namespace RecruitPro.Application.Tests.Recruitment.Jobs;

public sealed class PublishJobCommandHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    [Fact]
    public async Task Handle_JobWithSalaryRange_PublishesSuccessfully()
    {
        var job = Job.Create("Senior .NET Developer");
        job.SetSalaryRange(new SalaryRange(1_500_000, 2_200_000));
        _db.Jobs.Add(job);
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await new PublishJobCommandHandler(_db).Handle(new PublishJobCommand(job.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Status.Should().Be(JobStatus.Published);
    }

    [Fact]
    public async Task Handle_JobWithoutSalaryRange_ThrowsJobMissingSalaryRangeException()
    {
        var job = Job.Create("Senior .NET Developer");
        _db.Jobs.Add(job);
        await _db.SaveChangesAsync(CancellationToken.None);

        var act = async () => await new PublishJobCommandHandler(_db).Handle(new PublishJobCommand(job.Id), CancellationToken.None);

        await act.Should().ThrowAsync<JobMissingSalaryRangeException>();
    }

    [Fact]
    public async Task Handle_UnknownJob_ReturnsNotFound()
    {
        var result = await new PublishJobCommandHandler(_db).Handle(new PublishJobCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Status.Should().Be(ResultStatus.NotFound);
    }
}
