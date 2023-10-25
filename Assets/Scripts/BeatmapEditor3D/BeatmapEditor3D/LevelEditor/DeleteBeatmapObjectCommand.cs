// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.DeleteBeatmapObjectCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public abstract class DeleteBeatmapObjectCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    protected readonly BeatmapLevelDataModel beatmapLevelDataModel;
    [Inject]
    protected readonly BeatmapObjectsSelectionState beatmapObjectsSelectionState;
    [Inject]
    protected readonly SignalBus signalBus;

    public bool shouldAddToHistory { get; private set; }

    protected abstract void GatherBeatmapObject();

    protected abstract bool ShouldAddToHistory();

    protected abstract void DeselectBeatmapObjectIfSelected();

    protected abstract void RemoveFromBeatmapLevelDataModel();

    protected abstract void AddToBeatmapLevelDataModel();

    public void Execute()
    {
      this.GatherBeatmapObject();
      this.shouldAddToHistory = this.ShouldAddToHistory();
      if (!this.shouldAddToHistory)
        return;
      this.DeselectBeatmapObjectIfSelected();
      this.Redo();
    }

    public void Undo()
    {
      this.AddToBeatmapLevelDataModel();
      this.signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this.RemoveFromBeatmapLevelDataModel();
      this.signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
