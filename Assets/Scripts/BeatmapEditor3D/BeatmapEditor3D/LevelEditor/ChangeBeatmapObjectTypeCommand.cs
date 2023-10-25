// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.ChangeBeatmapObjectTypeCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class ChangeBeatmapObjectTypeCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly ChangeBeatmapObjectTypeSignal _signal;

    public void Execute()
    {
      this._readonlyBeatmapObjectsState.beatmapObjectType = this._signal.editorBeatmapObjectType;
      this._signalBus.Fire<BeatmapObjectTypeChangedSignal>(new BeatmapObjectTypeChangedSignal(this._signal.editorBeatmapObjectType));
      if (this._beatmapState.interactionMode != InteractionMode.Delete)
        return;
      this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(InteractionMode.Place));
    }
  }
}
