namespace ShibaPatrol.Tests;

public class UnitTest1
{
    [Fact]
    public void ShibaStats_InitializeCorrectly()
    {
        var stats = new ShibaStats
        {
            Bravery = 10,
            Energy = 10
        };

        Assert.Equal(10, stats.Bravery);
        Assert.Equal(10, stats.Energy);
    }

    [Fact]
    public void Encounter_ShouldReduceThreat()
    {
        var stats = new ShibaStats
        {
            Bravery = 10,
            Energy = 10
        };

        int startingThreat = 10;

        var result = Patrol.Encounter(stats, "TestEnemy", startingThreat);

        Assert.True(result.threat <= startingThreat);
    }
}
