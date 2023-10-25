using System;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace BGNet.Core.GameLift
{
    // Token: 0x020000C5 RID: 197
    [Preserve]
    [Serializable]
    public struct GetMultiplayerInstanceResponse
    {
        // Token: 0x06000700 RID: 1792 RVA: 0x00013146 File Offset: 0x00011346
        [JsonConstructor]
        public GetMultiplayerInstanceResponse(MultiplayerPlacementErrorCode errorCode, PlayerSessionInfo playerSessionInfo, int pollIntervalMs, string ticketId = null, string ticketStatus = null, string placementId = null, string placementStatus = null)
        {
            this.errorCode = errorCode;
            this.playerSessionInfo = playerSessionInfo;
            this.pollIntervalMs = pollIntervalMs;
            this.ticketId = ticketId;
            this.ticketStatus = ticketStatus;
            this.placementId = placementId;
            this.placementStatus = placementStatus;
        }

        // Token: 0x040002D9 RID: 729
        [JsonProperty("error_code")]
        public readonly MultiplayerPlacementErrorCode errorCode;

        // Token: 0x040002DA RID: 730
        [JsonProperty("player_session_info")]
        public readonly PlayerSessionInfo playerSessionInfo;

        // Token: 0x040002DB RID: 731
        [JsonProperty("poll_interval_ms")]
        public readonly int pollIntervalMs;

        // Token: 0x040002DC RID: 732
        [JsonProperty("ticket_id")]
        public readonly string ticketId;

        // Token: 0x040002DD RID: 733
        [JsonProperty("ticket_status")]
        public readonly string ticketStatus;

        // Token: 0x040002DE RID: 734
        [JsonProperty("placement_id")]
        public readonly string placementId;

        // Token: 0x040002DF RID: 735
        [JsonProperty("placement_status")]
        public readonly string placementStatus;
    }
}
