// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.ColorEventBoxGroupTrackMouseInputController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using System;
using UnityEngine;

namespace BeatmapEditor3D.InputSignals
{
  public class ColorEventBoxGroupTrackMouseInputController : EventBoxGroupTrackMouseInputController
  {
    [SerializeField]
    private ColorEventBoxGroupTrackView _colorEventBoxGroupTrackView;

    protected override int id => this._colorEventBoxGroupTrackView.id.groupId;

    protected override EventBoxGroupEditorData.EventBoxGroupType type => this._colorEventBoxGroupTrackView.type;

    protected override void OnEnable()
    {
      base.OnEnable();
      this.objectActions.Add(AbstractMouseInputController<BeatmapEditorObjectId>.HashInput(MouseInputEventOrigin.Object, MouseInputType.Middle, MouseEventType.Up, false, false, false), new Action<BeatmapEditorObjectId>(this.HandleEventBoxGroupObjectSwapColor));
    }

    protected override void HandleMouseInputEventSourceObjectPointerHover(
      MouseInputType mouseInputType,
      BeatmapEditorObjectId objectId)
    {
      base.HandleMouseInputEventSourceObjectPointerHover(mouseInputType, objectId);
      this._colorEventBoxGroupTrackView.eventObjects[objectId].SetHighlight(mouseInputType == MouseInputType.Enter);
    }

    private void HandleEventBoxGroupObjectSwapColor(BeatmapEditorObjectId id) => this.signalBus.Fire<SwapLightColorEventBoxesEditorGroupSignal>(new SwapLightColorEventBoxesEditorGroupSignal(id));
  }
}
