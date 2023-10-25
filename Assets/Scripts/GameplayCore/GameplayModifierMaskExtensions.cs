using System;

// Token: 0x0200000C RID: 12
public static class GameplayModifierMaskExtensions
{
    // Token: 0x0600005E RID: 94 RVA: 0x00003045 File Offset: 0x00001245
    public static bool Contains(this GameplayModifierMask mask, GameplayModifierMask other)
    {
        return (mask & other) == other;
    }

    // Token: 0x0600005F RID: 95 RVA: 0x0000304D File Offset: 0x0000124D
    public static int DifferenceFrom(this GameplayModifierMask mask, GameplayModifierMask other)
    {
        return (int)BitMaskUtil.NumberOfSetBits((uint)(mask & other));
    }

    // Token: 0x06000060 RID: 96 RVA: 0x00003058 File Offset: 0x00001258
    public static string ToHexString(this GameplayModifierMask mask)
    {
        ushort num = (ushort)mask;
        return num.ToString("x4");
    }

    // Token: 0x06000061 RID: 97 RVA: 0x00003074 File Offset: 0x00001274
    public static GameplayModifiers ToModifiers(this GameplayModifierMask gameplayModifierMask)
    {
        return new GameplayModifiers(((gameplayModifierMask & GameplayModifierMask.BatteryEnergy) != GameplayModifierMask.None) ? GameplayModifiers.EnergyType.Battery : GameplayModifiers.EnergyType.Bar, (gameplayModifierMask & GameplayModifierMask.NoFail) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.InstaFail) > GameplayModifierMask.None, false, ((gameplayModifierMask & GameplayModifierMask.NoObstacles) != GameplayModifierMask.None) ? GameplayModifiers.EnabledObstacleType.NoObstacles : GameplayModifiers.EnabledObstacleType.All, (gameplayModifierMask & GameplayModifierMask.NoBombs) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.FastNotes) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.StrictAngles) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.DisappearingArrows) > GameplayModifierMask.None, ((gameplayModifierMask & GameplayModifierMask.FasterSong) != GameplayModifierMask.None) ? GameplayModifiers.SongSpeed.Faster : (((gameplayModifierMask & GameplayModifierMask.SlowerSong) != GameplayModifierMask.None) ? GameplayModifiers.SongSpeed.Slower : (((gameplayModifierMask & GameplayModifierMask.SuperFastSong) != GameplayModifierMask.None) ? GameplayModifiers.SongSpeed.SuperFast : GameplayModifiers.SongSpeed.Normal)), (gameplayModifierMask & GameplayModifierMask.NoArrows) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.GhostNotes) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.ProMode) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.ZenMode) > GameplayModifierMask.None, (gameplayModifierMask & GameplayModifierMask.SmallCubes) > GameplayModifierMask.None);
    }

    // Token: 0x06000062 RID: 98 RVA: 0x0000311C File Offset: 0x0000131C
    public static GameplayModifierMask ToMask(this GameplayModifiers gameplayModifiers)
    {
        return ((gameplayModifiers.energyType == GameplayModifiers.EnergyType.Battery) ? GameplayModifierMask.BatteryEnergy : GameplayModifierMask.None) | (gameplayModifiers.noFailOn0Energy ? GameplayModifierMask.NoFail : GameplayModifierMask.None) | (gameplayModifiers.instaFail ? GameplayModifierMask.InstaFail : GameplayModifierMask.None) | ((gameplayModifiers.enabledObstacleType == GameplayModifiers.EnabledObstacleType.NoObstacles) ? GameplayModifierMask.NoObstacles : GameplayModifierMask.None) | (gameplayModifiers.noBombs ? GameplayModifierMask.NoBombs : GameplayModifierMask.None) | (gameplayModifiers.fastNotes ? GameplayModifierMask.FastNotes : GameplayModifierMask.None) | (gameplayModifiers.strictAngles ? GameplayModifierMask.StrictAngles : GameplayModifierMask.None) | (gameplayModifiers.disappearingArrows ? GameplayModifierMask.DisappearingArrows : GameplayModifierMask.None) | ((gameplayModifiers.songSpeed == GameplayModifiers.SongSpeed.Faster) ? GameplayModifierMask.FasterSong : GameplayModifierMask.None) | ((gameplayModifiers.songSpeed == GameplayModifiers.SongSpeed.Slower) ? GameplayModifierMask.SlowerSong : GameplayModifierMask.None) | ((gameplayModifiers.songSpeed == GameplayModifiers.SongSpeed.SuperFast) ? GameplayModifierMask.SuperFastSong : GameplayModifierMask.None) | (gameplayModifiers.noArrows ? GameplayModifierMask.NoArrows : GameplayModifierMask.None) | (gameplayModifiers.ghostNotes ? GameplayModifierMask.GhostNotes : GameplayModifierMask.None) | (gameplayModifiers.proMode ? GameplayModifierMask.ProMode : GameplayModifierMask.None) | (gameplayModifiers.zenMode ? GameplayModifierMask.ZenMode : GameplayModifierMask.None) | (gameplayModifiers.smallCubes ? GameplayModifierMask.SmallCubes : GameplayModifierMask.None);
    }
}
