// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.SoloEventTrackCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class SoloEventTrackCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SoloEventTrackSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;

    public void Execute()
    {
      if (this._signal.isSolo)
        this._basicEventsState.activeEventTypes = new List<BasicBeatmapEventType>()
        {
          this._signal.type
        };
      else
        this._basicEventsState.activeEventTypes = this._basicEventsState.allEventTypes;
      this._signalBus.Fire<EventsTrackStateUpdated>();
    }
  }
}
