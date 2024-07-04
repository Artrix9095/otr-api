using Database.Entities;
using Database.Enums;
using Database.Enums.Verification;
using DataWorkerService.AutomationChecks.Scores;
using TestingUtils.SeededData;

namespace DataWorkerService.Tests.AutomationChecks.Scores;

public class ScoreModCheckTests : AutomationChecksTestBase<ScoreModCheck>
{
    [Theory]
    [ClassData(typeof(SharedTestData.ModTestData))]
    public void Check_PassesWhenExpected(Mods mods, bool expectedPass)
    {
        // Arrange
        GameScore score = SeededScore.Generate(mods: mods, rejectionReason: ScoreRejectionReason.None);

        ScoreRejectionReason expectedRejectionReason = expectedPass
            ? ScoreRejectionReason.None
            : ScoreRejectionReason.InvalidMods;

        // Act
        var actualPass = AutomationCheck.Check(score);

        // Assert
        Assert.Equal(expectedPass, actualPass);
        Assert.Equal(expectedRejectionReason, score.RejectionReason);
    }
}
