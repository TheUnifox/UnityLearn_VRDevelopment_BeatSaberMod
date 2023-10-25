// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.TimeControlsPauseCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Controller;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class TimeControlsPauseCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      int currentSample = this._songPreviewController.currentSample;
      this._songPreviewController.Stop();
      this._signalBus.Fire<UpdatePlayHeadSignal>(new UpdatePlayHeadSignal(currentSample, UpdatePlayHeadSignal.SnapType.CurrentSubdivision, false));
    }
  }
}
