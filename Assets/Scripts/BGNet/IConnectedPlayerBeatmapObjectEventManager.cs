using System;

// Token: 0x02000034 RID: 52
public interface IConnectedPlayerBeatmapObjectEventManager
{
    // Token: 0x14000022 RID: 34
    // (add) Token: 0x060001A2 RID: 418
    // (remove) Token: 0x060001A3 RID: 419
    event Action<NoteSpawnInfoNetSerializable> connectedPlayerNoteWasSpawnedEvent;

    // Token: 0x14000023 RID: 35
    // (add) Token: 0x060001A4 RID: 420
    // (remove) Token: 0x060001A5 RID: 421
    event Action<ObstacleSpawnInfoNetSerializable> connectedPlayerObstacleWasSpawnedEvent;

    // Token: 0x14000024 RID: 36
    // (add) Token: 0x060001A6 RID: 422
    // (remove) Token: 0x060001A7 RID: 423
    event Action<SliderSpawnInfoNetSerializable> connectedPlayerSliderWasSpawnedEvent;

    // Token: 0x14000025 RID: 37
    // (add) Token: 0x060001A8 RID: 424
    // (remove) Token: 0x060001A9 RID: 425
    event Action<NoteCutInfoNetSerializable> connectedPlayerNoteWasCutEvent;

    // Token: 0x14000026 RID: 38
    // (add) Token: 0x060001AA RID: 426
    // (remove) Token: 0x060001AB RID: 427
    event Action<NoteMissInfoNetSerializable> connectedPlayerNoteWasMissedEvent;

    // Token: 0x060001AC RID: 428
    void Pause();

    // Token: 0x060001AD RID: 429
    void Resume();
}
