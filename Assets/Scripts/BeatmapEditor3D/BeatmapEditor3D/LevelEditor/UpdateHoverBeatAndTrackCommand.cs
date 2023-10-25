// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.UpdateHoverBeatAndTrackCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class UpdateHoverBeatAndTrackCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly UpdateHoverBeatAndTrackSignal _signal;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;

    public void Execute()
    {
      int pageTrackId = this._signal.pageTrackId;
      int visibleTrackId = this._signal.visibleTrackId;
      if ((!AudioTimeHelper.IsBeatSame(this._signal.hoverBeat, this._basicEventsState.currentHoverBeat) || this._basicEventsState.currentHoverPageTrackId != pageTrackId ? 1 : (this._basicEventsState.currentHoverVisibleTrackId != visibleTrackId ? 1 : 0)) == 0)
        return;
      this._basicEventsState.currentHoverBeat = this._signal.hoverBeat;
      this._basicEventsState.currentHoverPageTrackId = pageTrackId;
      this._basicEventsState.currentHoverVisibleTrackId = visibleTrackId;
      this._signalBus.Fire<EventHoverUpdatedSignal>();
    }
  }
}
