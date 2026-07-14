namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>The interview format expected at submission time — distinct from
/// <see cref="InterviewMode"/>, which is set per scheduled <see cref="Interview"/> round and
/// uses different vocabulary (Phone, Video, Onsite).</summary>
public enum ApplicationInterviewType
{
    Virtual,
    FaceToFace,
    Telephonic,
}
