using System;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace BGNet.Core.GameLift
{
    // Token: 0x020000C7 RID: 199
    [Preserve]
    [Serializable]
    public class PlayerSessionInfo
    {
        // Token: 0x040002E0 RID: 736
        [JsonProperty("player_session_id")]
        public string playerSessionId;

        // Token: 0x040002E1 RID: 737
        [JsonProperty("game_session_id")]
        public string gameSessionId;

        // Token: 0x040002E2 RID: 738
        [JsonProperty("dns_name")]
        public string dnsName;

        // Token: 0x040002E3 RID: 739
        [JsonProperty("port")]
        public int port;

        // Token: 0x040002E4 RID: 740
        [JsonProperty("beatmap_level_selection_mask")]
        public BeatmapLevelSelectionMask beatmapLevelSelectionMask;

        // Token: 0x040002E5 RID: 741
        [JsonProperty("gameplay_server_configuration")]
        public GameplayServerConfiguration gameplayServerConfiguration;

        // Token: 0x040002E6 RID: 742
        [JsonProperty("private_game_secret")]
        public string privateGameSecret;

        // Token: 0x040002E7 RID: 743
        [JsonProperty("private_game_code")]
        public string privateGameCode;
    }
}
