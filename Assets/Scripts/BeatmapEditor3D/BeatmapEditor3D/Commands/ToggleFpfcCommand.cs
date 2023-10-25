// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ToggleFpfcCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ToggleFpfcCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly ToggleFpfcSignal _signal;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;

    public void Execute() => this._beatmapEditorSettingsDataModel.SetEnableFpfc(this._signal.enableFpfc);
  }
}
