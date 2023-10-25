// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.TimeControlsStopCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Controller;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class TimeControlsStopCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;

    public void Execute()
    {
      this._songPreviewController.Stop();
      this._signalBus.Fire<UpdatePlayHeadSignal>(new UpdatePlayHeadSignal(0, UpdatePlayHeadSignal.SnapType.None, false));
    }
  }
}
