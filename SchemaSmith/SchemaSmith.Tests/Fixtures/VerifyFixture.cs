using VerifyTests;

namespace SchemaSmith.Tests.Fixtures;

public static class VerifyFixture
{
    public static readonly VerifySettings VerifySettings;

    static VerifyFixture()
    {
        VerifySettings = new VerifySettings();
        VerifySettings.ScrubInlineGuids();
    }
}