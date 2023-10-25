using System;
using System.Collections.Generic;

// Token: 0x0200004D RID: 77
public interface IServerBeatmapProvider
{
    // Token: 0x060002F4 RID: 756
    bool VerifyBeatmapForSelectionMask(BeatmapIdentifierNetSerializable beatmapId, BeatmapLevelSelectionMask selectionMask);

    // Token: 0x060002F5 RID: 757
    BeatmapIdentifierNetSerializable SelectBeatmapFromSuggestionsWithSelectionMaskAndOwnedSongPacks(int playerCount, Dictionary<string, BeatmapIdentifierNetSerializable> beatmapsSuggestedByPlayers, BeatmapLevelSelectionMask selectionMask, Dictionary<string, SongPackMask> playerOwnedSongPacks);
}
