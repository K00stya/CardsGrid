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
            new LevelReward() {InLevels = 1, Rewards = new[] {(Ha: CT.Hammer, 1), (Ha: CT.Hammer, 1), (Ha: CT.Hammer, 1)}},
            new LevelReward() {InLevels = 3, Rewards = new[] {(Ha: CT.Hammer, 1), (SV: CT.SwordVer, 1), (SH: CT.SwordHor, 1)}},
            new LevelReward() {InLevels = 5, Rewards = new[] {(SV: CT.SwordVer, 1), (SH: CT.SwordHor, 1), (Bo: CT.Bomb, 1)}},
        };
    }
}