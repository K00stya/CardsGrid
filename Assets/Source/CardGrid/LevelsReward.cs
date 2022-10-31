namespace CardGrid
{
    public class LevelReward
    {
        public int InLevels;
        public (CT, int)[] Rewards;
    }

    public static class LevelsReward //max 5 rewards
    {
        public static LevelReward[] Rewards = new[]
        {
            new LevelReward() {InLevels = 1, Rewards = new[] {(CT.Ha, 1), (CT.Ha, 1), (CT.Ha, 1)}},
            new LevelReward() {InLevels = 3, Rewards = new[] {(CT.Ha, 1), (CT.SV, 1), (CT.SH, 1)}},
            new LevelReward() {InLevels = 5, Rewards = new[] {(CT.SV, 1), (CT.SH, 1), (CT.Bo, 1)}},
        };
    }
}