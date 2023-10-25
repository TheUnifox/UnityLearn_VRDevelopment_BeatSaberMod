// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxGroupMouseInputEventSource
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.InputSignals;
using System;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class EventBoxGroupMouseInputEventSource : AbstractMouseInputEventSource
  {
    [SerializeField]
    private LayerMask _eventBoxGroupObjectLayerMask;
    private GameObject _prevHoveredGameObject;
    private GameObject _hoveredGameObject;

    public event Action<MouseInputType, EventBoxGroupEditorData> pointerDownEvent;

    public event Action<MouseInputType, EventBoxGroupEditorData> pointerUpEvent;

    public event Action<MouseInputType, EventBoxGroupEditorData> pointerHoverEvent;

    protected void Update()
    {
      this._hoveredGameObject = this.RaycastEventBoxGroupObjects();
      EventBoxGroupEditorData boxGroupEditorData = (EventBoxGroupEditorData) null;
      if ((UnityEngine.Object) this._hoveredGameObject != (UnityEngine.Object) null)
      {
        IEventBoxGroupDataProvider componentInParent = this._hoveredGameObject.GetComponentInParent<IEventBoxGroupDataProvider>();
        if (componentInParent != null)
          boxGroupEditorData = componentInParent.GetEventBoxGroupData();
      }
      if ((UnityEngine.Object) this._hoveredGameObject != (UnityEngine.Object) this._prevHoveredGameObject)
      {
        if ((UnityEngine.Object) this._prevHoveredGameObject != (UnityEngine.Object) null)
        {
          IEventBoxGroupDataProvider componentInParent = this._prevHoveredGameObject.GetComponentInParent<IEventBoxGroupDataProvider>();
          if (componentInParent != null)
          {
            Action<MouseInputType, EventBoxGroupEditorData> pointerHoverEvent = this.pointerHoverEvent;
            if (pointerHoverEvent != null)
              pointerHoverEvent(MouseInputType.Exit, componentInParent.GetEventBoxGroupData());
          }
        }
        if ((UnityEngine.Object) this._hoveredGameObject != (UnityEngine.Object) null)
        {
          Action<MouseInputType, EventBoxGroupEditorData> pointerHoverEvent = this.pointerHoverEvent;
          if (pointerHoverEvent != null)
            pointerHoverEvent(MouseInputType.Enter, boxGroupEditorData);
        }
      }
      this._prevHoveredGameObject = this._hoveredGameObject;
      if ((UnityEngine.Object) this._hoveredGameObject == (UnityEngine.Object) null)
        return;
      (MouseInputType mouseDown, MouseInputType mouseUp, MouseInputType _) = this.GetMouseInputs();
      if (mouseDown != MouseInputType.None)
      {
        Action<MouseInputType, EventBoxGroupEditorData> pointerDownEvent = this.pointerDownEvent;
        if (pointerDownEvent != null)
          pointerDownEvent(mouseDown, boxGroupEditorData);
      }
      if (mouseUp == MouseInputType.None)
        return;
      Action<MouseInputType, EventBoxGroupEditorData> pointerUpEvent = this.pointerUpEvent;
      if (pointerUpEvent == null)
        return;
      pointerUpEvent(mouseUp, boxGroupEditorData);
    }

    private GameObject RaycastEventBoxGroupObjects()
    {
      RaycastHit hitInfo;
      return !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity, (int) this._eventBoxGroupObjectLayerMask) ? (GameObject) null : hitInfo.collider.gameObject;
    }
  }
}
