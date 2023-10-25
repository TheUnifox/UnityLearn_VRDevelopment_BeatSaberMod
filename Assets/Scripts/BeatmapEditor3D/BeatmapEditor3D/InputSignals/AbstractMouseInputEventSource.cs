// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.AbstractMouseInputEventSource
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.InputSignals
{
  public abstract class AbstractMouseInputEventSource : MonoBehaviour
  {
    [Inject]
    private readonly BeatmapEditorGroundView _beatmapEditorGroundView;
    private const string kMouseScrollWheelId = "Mouse ScrollWheel";

    protected (MouseInputType mouseDown, MouseInputType mouseUp, MouseInputType scrollType) GetMouseInputs()
    {
      if (this._beatmapEditorGroundView.isDragging || this._beatmapEditorGroundView.wasDraggingThisFrame)
        return (MouseInputType.None, MouseInputType.None, MouseInputType.None);
      MouseInputType mouseInputType1 = MouseInputType.None;
      if (Input.GetMouseButtonDown(0))
        mouseInputType1 = MouseInputType.Left;
      else if (Input.GetMouseButtonDown(1))
        mouseInputType1 = MouseInputType.Right;
      else if (Input.GetMouseButtonDown(2))
        mouseInputType1 = MouseInputType.Middle;
      MouseInputType mouseInputType2 = MouseInputType.None;
      if (Input.GetMouseButtonUp(0))
        mouseInputType2 = MouseInputType.Left;
      else if (Input.GetMouseButtonUp(1))
        mouseInputType2 = MouseInputType.Right;
      else if (Input.GetMouseButtonUp(2))
        mouseInputType2 = MouseInputType.Middle;
      MouseInputType mouseInputType3 = MouseInputType.None;
      if ((double) Input.GetAxis("Mouse ScrollWheel") > 0.0)
        mouseInputType3 = MouseInputType.ScrollUp;
      else if ((double) Input.GetAxis("Mouse ScrollWheel") < 0.0)
        mouseInputType3 = MouseInputType.ScrollDown;
      return (mouseInputType1, mouseInputType2, mouseInputType3);
    }
  }
}
