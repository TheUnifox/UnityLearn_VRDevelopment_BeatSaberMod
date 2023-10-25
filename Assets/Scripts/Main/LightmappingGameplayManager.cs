// Decompiled with JetBrains decompiler
// Type: LightmappingGameplayManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class LightmappingGameplayManager : MonoBehaviour
{
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;

  public virtual void Start()
  {
    this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new BasicBeatmapEventData(0.0f, BasicBeatmapEventType.Event0, 1, 1f));
    this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new BasicBeatmapEventData(0.0f, BasicBeatmapEventType.Event1, 1, 1f));
    this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new BasicBeatmapEventData(0.0f, BasicBeatmapEventType.Event2, 1, 1f));
    this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new BasicBeatmapEventData(0.0f, BasicBeatmapEventType.Event3, 1, 1f));
    this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new BasicBeatmapEventData(0.0f, BasicBeatmapEventType.Event4, 1, 1f));
  }
}
