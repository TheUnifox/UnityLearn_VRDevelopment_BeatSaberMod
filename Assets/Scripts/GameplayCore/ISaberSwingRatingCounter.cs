using System;

// Token: 0x0200001E RID: 30
public interface ISaberSwingRatingCounter
{
    // Token: 0x17000021 RID: 33
    // (get) Token: 0x06000097 RID: 151
    float beforeCutRating { get; }

    // Token: 0x17000022 RID: 34
    // (get) Token: 0x06000098 RID: 152
    float afterCutRating { get; }

    // Token: 0x06000099 RID: 153
    void RegisterDidChangeReceiver(ISaberSwingRatingCounterDidChangeReceiver receiver);

    // Token: 0x0600009A RID: 154
    void RegisterDidFinishReceiver(ISaberSwingRatingCounterDidFinishReceiver receiver);

    // Token: 0x0600009B RID: 155
    void UnregisterDidChangeReceiver(ISaberSwingRatingCounterDidChangeReceiver receiver);

    // Token: 0x0600009C RID: 156
    void UnregisterDidFinishReceiver(ISaberSwingRatingCounterDidFinishReceiver receiver);
}
