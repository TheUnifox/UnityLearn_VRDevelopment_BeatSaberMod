// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ChangeLightRotationEventCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class ChangeLightRotationEventCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly ChangeLightRotationEventSignal _signal;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      this._eventBoxGroupsState.lightRotationEaseType = this._signal.easeType;
      this._eventBoxGroupsState.lightRotationLoopCount = this._signal.loopCount;
      this._eventBoxGroupsState.lightRotation = this._signal.rotation;
      this._eventBoxGroupsState.lightRotationDirection = this._signal.rotationDirection;
      this._eventBoxGroupsState.eventBoxGroupExtension = this._signal.extension;
      if (this._beatmapState.interactionMode == InteractionMode.Delete)
        this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(InteractionMode.Place));
      this._signalBus.Fire<LightRotationEventChangedSignal>();
    }
  }
}
