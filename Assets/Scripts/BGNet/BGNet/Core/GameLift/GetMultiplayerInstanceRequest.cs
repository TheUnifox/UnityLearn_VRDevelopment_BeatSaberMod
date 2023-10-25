using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace BGNet.Core.GameLift
{
    // Token: 0x020000C4 RID: 196
    [Preserve]
    [Serializable]
    public struct GetMultiplayerInstanceRequest
    {
        // Token: 0x060006FF RID: 1791 RVA: 0x000130D4 File Offset: 0x000112D4
        [JsonConstructor]
        public GetMultiplayerInstanceRequest(string version, ServiceEnvironment serviceEnvironment, string userId, BeatmapLevelSelectionMask beatmapLevelSelectionMask, GameplayServerConfiguration gameplayServerConfiguration, AuthenticationToken.Platform platform, string authUserId, string singleUseAuthToken, string privateGameSecret, string privateGameCode, Dictionary<string, long> gameliftRegionLatencies, string ticketId = null, string placementId = null)
        {
            this.version = version;
            this.serviceEnvironment = serviceEnvironment;
            this.singleUseAuthToken = singleUseAuthToken;
            this.userId = userId;
            this.beatmapLevelSelectionMask = beatmapLevelSelectionMask;
            this.gameplayServerConfiguration = gameplayServerConfiguration;
            this.privateGameSecret = privateGameSecret;
            this.privateGameCode = privateGameCode;
            this.gameliftRegionLatencies = gameliftRegionLatencies;
            this.authUserId = authUserId;
            this.platform = platform;
            this.ticketId = ticketId;
            this.placementId = placementId;
        }

        // Token: 0x040002CC RID: 716
        [JsonProperty("version")]
        public readonly string version;

        // Token: 0x040002CD RID: 717
        [JsonProperty("service_environment")]
        public readonly ServiceEnvironment serviceEnvironment;

        // Token: 0x040002CE RID: 718
        [JsonProperty("single_use_auth_token")]
        public readonly string singleUseAuthToken;

        // Token: 0x040002CF RID: 719
        [JsonProperty("beatmap_level_selection_mask")]
        public readonly BeatmapLevelSelectionMask beatmapLevelSelectionMask;

        // Token: 0x040002D0 RID: 720
        [JsonProperty("gameplay_server_configuration")]
        public readonly GameplayServerConfiguration gameplayServerConfiguration;

        // Token: 0x040002D1 RID: 721
        [JsonProperty("user_id")]
        public readonly string userId;

        // Token: 0x040002D2 RID: 722
        [JsonProperty("private_game_secret")]
        public readonly string privateGameSecret;

        // Token: 0x040002D3 RID: 723
        [JsonProperty("private_game_code")]
        public readonly string privateGameCode;

        // Token: 0x040002D4 RID: 724
        [JsonProperty("platform")]
        public readonly AuthenticationToken.Platform platform;

        // Token: 0x040002D5 RID: 725
        [JsonProperty("auth_user_id")]
        public readonly string authUserId;

        // Token: 0x040002D6 RID: 726
        [JsonProperty("gamelift_region_latencies")]
        public readonly Dictionary<string, long> gameliftRegionLatencies;

        // Token: 0x040002D7 RID: 727
        [JsonProperty("ticket_id")]
        public readonly string ticketId;

        // Token: 0x040002D8 RID: 728
        [JsonProperty("placement_id")]
        public readonly string placementId;
    }
}
