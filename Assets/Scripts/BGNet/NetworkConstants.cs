using System;

// Token: 0x02000062 RID: 98
public static class NetworkConstants
{
    // Token: 0x0400018D RID: 397
    public const uint kProtocolVersion = 8U;

    // Token: 0x0400018E RID: 398
    public const uint kHandshakeMessageType = 3192347326U;

    // Token: 0x0400018F RID: 399
    public const uint kUserMasterServerMessageType = 1U;

    // Token: 0x04000190 RID: 400
    public const uint kDedicatedServerMasterServerMessageType = 2U;

    // Token: 0x04000191 RID: 401
    public const uint kGameLiftMessageType = 3U;

    // Token: 0x04000192 RID: 402
    public const string dedicatedServerState = "dedicated_server";

    // Token: 0x04000193 RID: 403
    public const string playerState = "player";

    // Token: 0x04000194 RID: 404
    public const string spectatingState = "spectating";

    // Token: 0x04000195 RID: 405
    public const string backgroundedState = "backgrounded";

    // Token: 0x04000196 RID: 406
    public const string terminatingState = "terminating";

    // Token: 0x04000197 RID: 407
    public const string wantsToPlayNextLevel = "wants_to_play_next_level";

    // Token: 0x04000198 RID: 408
    public const string wasActiveAtLevelStart = "was_active_at_level_start";

    // Token: 0x04000199 RID: 409
    public const string isActive = "is_active";

    // Token: 0x0400019A RID: 410
    public const string finishedLevel = "finished_level";
}
