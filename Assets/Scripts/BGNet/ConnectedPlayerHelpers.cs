using System;

// Token: 0x0200000F RID: 15
public static class ConnectedPlayerHelpers
{
    // Token: 0x06000093 RID: 147 RVA: 0x000043EA File Offset: 0x000025EA
    public static bool WantsToPlayNextLevel(this IConnectedPlayer connectedPlayer)
    {
        return connectedPlayer.HasState("wants_to_play_next_level");
    }

    // Token: 0x06000094 RID: 148 RVA: 0x000043F7 File Offset: 0x000025F7
    public static bool WasActiveAtLevelStart(this IConnectedPlayer connectedPlayer)
    {
        return connectedPlayer.HasState("was_active_at_level_start");
    }

    // Token: 0x06000095 RID: 149 RVA: 0x00004404 File Offset: 0x00002604
    public static bool IsActive(this IConnectedPlayer connectedPlayer)
    {
        return connectedPlayer.HasState("is_active");
    }

    // Token: 0x06000096 RID: 150 RVA: 0x00004411 File Offset: 0x00002611
    public static bool HasFinishedLevel(this IConnectedPlayer connectedPlayer)
    {
        return connectedPlayer.HasState("finished_level");
    }

    // Token: 0x06000097 RID: 151 RVA: 0x0000441E File Offset: 0x0000261E
    public static bool IsActiveOrFinished(this IConnectedPlayer connectedPlayer)
    {
        return connectedPlayer.IsActive() || connectedPlayer.HasFinishedLevel();
    }

    // Token: 0x06000098 RID: 152 RVA: 0x00004430 File Offset: 0x00002630
    public static bool IsFailed(this IConnectedPlayer connectedPlayer)
    {
        return (connectedPlayer.WasActiveAtLevelStart() && !connectedPlayer.IsActiveOrFinished()) || !connectedPlayer.isConnected;
    }
}
