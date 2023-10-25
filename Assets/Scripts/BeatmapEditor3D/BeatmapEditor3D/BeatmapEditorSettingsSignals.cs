// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorSettingsSignals
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorSettingsSignals
  {
    public class UpdateSettingsSignal
    {
      public readonly string customLevelsFolder;
      public readonly Vector2Int editorWindowResolution;
      public readonly bool invertSubdivisionScroll;
      public readonly bool invertTimelineScroll;

      public UpdateSettingsSignal(
        string customLevelsFolder,
        Vector2Int editorWindowResolution,
        bool invertSubdivisionScroll,
        bool invertTimelineScroll)
      {
        this.customLevelsFolder = customLevelsFolder;
        this.editorWindowResolution = editorWindowResolution;
        this.invertSubdivisionScroll = invertSubdivisionScroll;
        this.invertTimelineScroll = invertTimelineScroll;
      }
    }

    public class UpdateSettingsCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
      [Inject]
      private readonly BeatmapEditorSettingsSignals.UpdateSettingsSignal _signal;

      public void Execute() => this._beatmapEditorSettingsDataModel.UpdateWith(this._signal.customLevelsFolder, this._signal.editorWindowResolution, this._signal.invertSubdivisionScroll, this._signal.invertTimelineScroll);
    }
  }
}
