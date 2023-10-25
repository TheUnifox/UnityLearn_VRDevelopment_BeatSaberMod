using System;

// Token: 0x02000020 RID: 32
public static class MultiplayerPlacementErrorCodeMethods
{
    // Token: 0x06000125 RID: 293 RVA: 0x00005CEC File Offset: 0x00003EEC
    public static ConnectionFailedReason ToConnectionFailedReason(this MultiplayerPlacementErrorCode errorCode)
    {
        switch (errorCode)
        {
            case MultiplayerPlacementErrorCode.Success:
                return ConnectionFailedReason.None;
            case MultiplayerPlacementErrorCode.Unknown:
                return ConnectionFailedReason.Unknown;
            case MultiplayerPlacementErrorCode.ConnectionCanceled:
                return ConnectionFailedReason.ConnectionCanceled;
            case MultiplayerPlacementErrorCode.ServerDoesNotExist:
                return ConnectionFailedReason.ServerDoesNotExist;
            case MultiplayerPlacementErrorCode.ServerAtCapacity:
                return ConnectionFailedReason.ServerAtCapacity;
            case MultiplayerPlacementErrorCode.AuthenticationFailed:
                return ConnectionFailedReason.InvalidPassword;
            case MultiplayerPlacementErrorCode.RequestTimeout:
                return ConnectionFailedReason.Timeout;
            case MultiplayerPlacementErrorCode.MatchmakingTimeout:
                return ConnectionFailedReason.FailedToFindMatch;
            default:
                return ConnectionFailedReason.Unknown;
        }
    }
}
