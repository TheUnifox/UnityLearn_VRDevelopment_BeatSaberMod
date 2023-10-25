// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.RotationEventBoxGroupTrackMouseInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using UnityEngine;

namespace BeatmapEditor3D.InputSignals
{
  public class RotationEventBoxGroupTrackMouseInputController : 
    EventBoxGroupTrackMouseInputController
  {
    [SerializeField]
    private RotationEventBoxGroupTrackView _rotationEventBoxGroupTrackView;

    protected override int id => this._rotationEventBoxGroupTrackView.id.groupId;

    protected override EventBoxGroupEditorData.EventBoxGroupType type => this._rotationEventBoxGroupTrackView.type;

    protected override void HandleMouseInputEventSourceObjectPointerHover(
      MouseInputType mouseInputType,
      BeatmapEditorObjectId objectId)
    {
      base.HandleMouseInputEventSourceObjectPointerHover(mouseInputType, objectId);
      this._rotationEventBoxGroupTrackView.eventObjects[objectId].SetHighlight(mouseInputType == MouseInputType.Enter);
    }
  }
}
