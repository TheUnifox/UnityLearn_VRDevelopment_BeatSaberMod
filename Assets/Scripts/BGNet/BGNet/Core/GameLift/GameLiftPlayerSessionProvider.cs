using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace BGNet.Core.GameLift
{
    // Token: 0x020000C3 RID: 195
    public class GameLiftPlayerSessionProvider : IGameLiftPlayerSessionProvider, IPollable
    {
        // Token: 0x060006F4 RID: 1780 RVA: 0x00012D44 File Offset: 0x00010F44
        public GameLiftPlayerSessionProvider(INetworkConfig networkConfig)
        {
            this._networkConfig = networkConfig;
            foreach (string key in GameLiftPlayerSessionProvider._awsGameLiftRegions)
            {
                this._pingAverages[key] = new RollingAverage(10);
            }
        }

        // Token: 0x060006F5 RID: 1781 RVA: 0x00012DA0 File Offset: 0x00010FA0
        public void PollUpdate()
        {
            long num = DateTime.UtcNow.Ticks / 10000L;
            if (this._pingCount >= 10 || num <= this._lastPingTime + 3000L)
            {
                return;
            }
            this._lastPingTime = num;
            this._pingCount++;
            this.PingAllAwsGameLiftRegions();
        }

        // Token: 0x060006F6 RID: 1782 RVA: 0x00012DF8 File Offset: 0x00010FF8
        public async Task<PlayerSessionInfo> GetGameLiftPlayerSessionInfo(IAuthenticationTokenProvider authenticationTokenProvider, string userId, BeatmapLevelSelectionMask beatmapLevelSelectionMask, GameplayServerConfiguration gameplayServerConfiguration, string secret, string code, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string ticketId = null;
            string placementId = null;
            while (stopwatch.ElapsedMilliseconds < 120000L && !cancellationToken.IsCancellationRequested)
            {
                AuthenticationToken authenticationToken = await authenticationTokenProvider.GetAuthenticationToken();
                string sessionToken = authenticationToken.sessionToken;
                GetMultiplayerInstanceResponse getMultiplayerInstanceResponse = await this.Post<GetMultiplayerInstanceRequest, GetMultiplayerInstanceResponse>("beat_saber_get_multiplayer_instance", new GetMultiplayerInstanceRequest(Application.version, this._networkConfig.serviceEnvironment, userId, beatmapLevelSelectionMask, gameplayServerConfiguration, authenticationToken.platform, authenticationToken.userId, sessionToken, secret, code, this.GetAverageLatencies(), ticketId, placementId));
                if (getMultiplayerInstanceResponse.errorCode != MultiplayerPlacementErrorCode.Success && getMultiplayerInstanceResponse.errorCode != MultiplayerPlacementErrorCode.RequestTimeout)
                {
                    throw new ConnectionFailedException(getMultiplayerInstanceResponse.errorCode.ToConnectionFailedReason());
                }
                if (getMultiplayerInstanceResponse.playerSessionInfo != null && !string.IsNullOrEmpty(getMultiplayerInstanceResponse.playerSessionInfo.gameSessionId) && !string.IsNullOrEmpty(getMultiplayerInstanceResponse.playerSessionInfo.playerSessionId))
                {
                    return getMultiplayerInstanceResponse.playerSessionInfo;
                }
                if (string.IsNullOrEmpty(getMultiplayerInstanceResponse.ticketId) && string.IsNullOrEmpty(getMultiplayerInstanceResponse.placementId))
                {
                    throw new ConnectionFailedException(ConnectionFailedReason.Timeout);
                }
                ticketId = getMultiplayerInstanceResponse.ticketId;
                placementId = getMultiplayerInstanceResponse.placementId;
                if (getMultiplayerInstanceResponse.pollIntervalMs > 0)
                {
                    try
                    {
                        await Task.Delay(getMultiplayerInstanceResponse.pollIntervalMs, cancellationToken);
                    }
                    catch (TaskCanceledException)
                    {
                    }
                }
            }
            if (!cancellationToken.IsCancellationRequested)
            {
                throw new ConnectionFailedException(ConnectionFailedReason.FailedToFindMatch);
            }
            if (!string.IsNullOrEmpty(ticketId) || !string.IsNullOrEmpty(placementId))
            {
                AuthenticationToken authenticationToken2 = await authenticationTokenProvider.GetAuthenticationToken();
                string sessionToken2 = authenticationToken2.sessionToken;
                try
                {
                    await this.Post<GetMultiplayerInstanceRequest, GetMultiplayerInstanceResponse>("beat_saber_multiplayer_cancel_matchmaking_ticket", new GetMultiplayerInstanceRequest(Application.version, this._networkConfig.serviceEnvironment, userId, beatmapLevelSelectionMask, gameplayServerConfiguration, authenticationToken2.platform, authenticationToken2.userId, sessionToken2, secret, code, this.GetAverageLatencies(), ticketId, placementId));
                }
                catch
                {
                }
            }
            throw new TaskCanceledException("Cancelled Request");
        }

        // Token: 0x060006F7 RID: 1783 RVA: 0x00012E7C File Offset: 0x0001107C
        private async Task<TResponse> Post<TRequest, TResponse>(string path, TRequest request)
        {
            UriBuilder uriBuilder = new UriBuilder(this._networkConfig.graphUrl)
            {
                Path = path,
                Query = "access_token=" + this._networkConfig.graphAccessToken
            };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await this._client.PostAsync(uriBuilder.Uri, content);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new ConnectionFailedException(ConnectionFailedReason.MultiplayerApiUnreachable);
            }
            return JsonConvert.DeserializeObject<TResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
        }

        // Token: 0x060006F8 RID: 1784 RVA: 0x00012ED4 File Offset: 0x000110D4
        private async Task<TResponse> Get<TResponse>(string path, Dictionary<string, string> query = null)
        {
            UriBuilder uriBuilder = new UriBuilder(this._networkConfig.graphUrl);
            uriBuilder.Path = path;
            UriBuilder uriBuilder2 = uriBuilder;
            string query2;
            if (query == null || query.Count == 0)
            {
                query2 = "access_token=" + this._networkConfig.graphAccessToken;
            }
            else
            {
                string str = this._networkConfig.graphAccessToken;
                string str2 = await new FormUrlEncodedContent(query).ReadAsStringAsync();
                query2 = "access_token=" + str + "&" + str2;
                str = null;
            }
            uriBuilder2.Query = query2;
            UriBuilder uriBuilder3 = uriBuilder;
            uriBuilder2 = null;
            uriBuilder = null;
            return JsonConvert.DeserializeObject<TResponse>(await this._client.GetStringAsync(uriBuilder3.Uri));
        }

        // Token: 0x060006F9 RID: 1785 RVA: 0x00012F2C File Offset: 0x0001112C
        private async void PingAllAwsGameLiftRegions()
        {
            foreach (ValueTuple<string, long> valueTuple in await Task.WhenAll<ValueTuple<string, long>>(from region in GameLiftPlayerSessionProvider._awsGameLiftRegions
                                                                                                         select this.PingRegionAsync(region)))
            {
                if (valueTuple.Item2 != -1L)
                {
                    this._pingAverages[valueTuple.Item1].Update((float)valueTuple.Item2);
                }
            }
        }

        // Token: 0x060006FA RID: 1786 RVA: 0x00012F68 File Offset: 0x00011168
        private Dictionary<string, long> GetAverageLatencies()
        {
            return (from kvp in this._pingAverages
                    where kvp.Value.hasValue
                    select kvp).ToDictionary((KeyValuePair<string, RollingAverage> kvp) => kvp.Key, (KeyValuePair<string, RollingAverage> kvp) => (long)kvp.Value.currentAverage);
        }

        // Token: 0x060006FB RID: 1787 RVA: 0x00012FE4 File Offset: 0x000111E4
        private async Task<(string region, long latency)> PingRegionAsync(string awsRegion)
        {
            ValueTuple<string, long> result;
            try
            {
                long item = await PingUtility.PingAsync(GameLiftPlayerSessionProvider.GetAwsGameLiftRegionEndpoint(awsRegion));
                result = new ValueTuple<string, long>(awsRegion, item);
            }
            catch
            {
                result = new ValueTuple<string, long>(awsRegion, -1L);
            }
            return result;
        }

        // Token: 0x060006FC RID: 1788 RVA: 0x00013029 File Offset: 0x00011229
        private static string GetAwsGameLiftRegionEndpoint(string awsRegion)
        {
            return "gamelift." + awsRegion + ".amazonaws.com";
        }

        // Token: 0x040002C1 RID: 705
        private const int kMatchmakingTimeoutMs = 120000;

        // Token: 0x040002C2 RID: 706
        private const int kPingFrequencyMs = 3000;

        // Token: 0x040002C3 RID: 707
        private const int kMaxPingCount = 10;

        // Token: 0x040002C4 RID: 708
        private const string kGetMatchmakingInstancePath = "beat_saber_get_multiplayer_instance";

        // Token: 0x040002C5 RID: 709
        private const string kCancelMatchmakingTicketPath = "beat_saber_multiplayer_cancel_matchmaking_ticket";

        // Token: 0x040002C6 RID: 710
        [DoesNotRequireDomainReloadInit]
        private static readonly string[] _awsGameLiftRegions = new string[]
        {
            "ap-northeast-1",
            "ap-northeast-2",
            "ap-south-1",
            "ap-southeast-1",
            "ap-southeast-2",
            "ca-central-1",
            "eu-central-1",
            "eu-west-1",
            "eu-west-2",
            "sa-east-1",
            "us-east-1",
            "us-east-2",
            "us-west-1",
            "us-west-2"
        };

        // Token: 0x040002C7 RID: 711
        private readonly INetworkConfig _networkConfig;

        // Token: 0x040002C8 RID: 712
        private readonly HttpClient _client = new HttpClient();

        // Token: 0x040002C9 RID: 713
        private readonly Dictionary<string, RollingAverage> _pingAverages = new Dictionary<string, RollingAverage>();

        // Token: 0x040002CA RID: 714
        private int _pingCount;

        // Token: 0x040002CB RID: 715
        private long _lastPingTime;
    }
}
