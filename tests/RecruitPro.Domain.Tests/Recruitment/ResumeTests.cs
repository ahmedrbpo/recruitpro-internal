using FluentAssertions;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;
using Xunit;

namespace RecruitPro.Domain.Tests.Recruitment;

public sealed class ResumeTests
{
    private static readonly Guid CandidateId = Guid.NewGuid();

    [Fact]
    public void Create_UnsupportedContentType_ThrowsUnsupportedResumeFileTypeException()
    {
        var act = () => Resume.Create(CandidateId, "resumes/x/y.exe", "resume.exe", "application/x-msdownload", 1024);

        act.Should().Throw<UnsupportedResumeFileTypeException>();
    }

    [Fact]
    public void Create_FileTooLarge_ThrowsResumeFileTooLargeException()
    {
        var act = () => Resume.Create(CandidateId, "resumes/x/y.pdf", "resume.pdf", "application/pdf", Resume.MaxSizeBytes + 1);

        act.Should().Throw<ResumeFileTooLargeException>();
    }

    [Fact]
    public void Create_ValidPdf_Succeeds()
    {
        var resume = Resume.Create(CandidateId, "resumes/x/y.pdf", "resume.pdf", "application/pdf", 1024);

        resume.IsConfirmed.Should().BeFalse();
    }

    [Fact]
    public void Confirm_SetsConfirmedState()
    {
        var resume = Resume.Create(CandidateId, "resumes/x/y.pdf", "resume.pdf", "application/pdf", 1024);
        var now = DateTimeOffset.UtcNow;

        resume.Confirm(now);

        resume.IsConfirmed.Should().BeTrue();
        resume.ConfirmedAt.Should().Be(now);
    }
}
