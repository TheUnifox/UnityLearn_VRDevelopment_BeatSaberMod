// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.SetAllTracksCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class SetAllTracksCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SetAllTracksSignal _signal;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;

    public void Execute()
    {
      this._basicEventsState.allEventTypes = this._signal.allEvents;
      this._basicEventsState.activeEventTypes = this._signal.allEvents;
    }
  }
}
