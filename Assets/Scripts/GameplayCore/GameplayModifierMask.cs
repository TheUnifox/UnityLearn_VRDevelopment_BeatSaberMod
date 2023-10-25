using System;

// Token: 0x0200000B RID: 11
[Flags]
public enum GameplayModifierMask : ushort
{
    // Token: 0x04000019 RID: 25
    None = 0b0,
    // Token: 0x0400001A RID: 26
    BatteryEnergy = 0b1,
    // Token: 0x0400001B RID: 27
    NoFail = 0b10,
    // Token: 0x0400001C RID: 28
    InstaFail = 0b100,
    // Token: 0x0400001D RID: 29
    NoObstacles = 0b1000,
    // Token: 0x0400001E RID: 30
    NoBombs = 0b10000,
    // Token: 0x0400001F RID: 31
    FastNotes = 0b100000,
    // Token: 0x04000020 RID: 32
    StrictAngles = 0b1000000,
    // Token: 0x04000021 RID: 33
    DisappearingArrows = 0b10000000,
    // Token: 0x04000022 RID: 34
    FasterSong = 0b100000000,
    // Token: 0x04000023 RID: 35
    SlowerSong = 0b1000000000,
    // Token: 0x04000024 RID: 36
    NoArrows = 0b10000000000,
    // Token: 0x04000025 RID: 37
    GhostNotes = 0b100000000000,
    // Token: 0x04000026 RID: 38
    SuperFastSong = 0b1000000000000,
    // Token: 0x04000027 RID: 39
    ProMode = 0b10000000000000,
    // Token: 0x04000028 RID: 40
    ZenMode = 0b100000000000000,
    // Token: 0x04000029 RID: 41
    SmallCubes = 0b1000000000000000,
    // Token: 0x0400002A RID: 42
    All = 0b1111111111111111
}