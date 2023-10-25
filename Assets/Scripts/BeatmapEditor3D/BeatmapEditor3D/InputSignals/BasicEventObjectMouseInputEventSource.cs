// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.BasicEventObjectMouseInputEventSource
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;

namespace BeatmapEditor3D.InputSignals
{
  public class BasicEventObjectMouseInputEventSource : AbstractMouseInputEventSource
  {
    [SerializeField]
    private LayerMask _eventObjectLayerMask;
    private GameObject _prevHoveredGameObject;
    private GameObject _hoveredGameObject;

    public event Action<MouseInputType, BasicEventEditorData> pointerDownEvent;

    public event Action<MouseInputType, BasicEventEditorData> pointerUpEvent;

    public event Action<MouseInputType, BasicEventEditorData> pointerHoverEvent;

    protected void Update()
    {
      this._hoveredGameObject = this.RaycastEventObjects();
      BasicEventEditorData basicEventEditorData = (BasicEventEditorData) null;
      if ((UnityEngine.Object) this._hoveredGameObject != (UnityEngine.Object) null)
      {
        IBasicEventObjectDataProvider componentInParent = this._hoveredGameObject.GetComponentInParent<IBasicEventObjectDataProvider>();
        if (componentInParent != null)
          basicEventEditorData = componentInParent.GetEventData();
      }
      if ((UnityEngine.Object) this._hoveredGameObject != (UnityEngine.Object) this._prevHoveredGameObject)
      {
        if ((UnityEngine.Object) this._prevHoveredGameObject != (UnityEngine.Object) null)
        {
          IBasicEventObjectDataProvider componentInParent = this._prevHoveredGameObject.GetComponentInParent<IBasicEventObjectDataProvider>();
          if (componentInParent != null)
          {
            Action<MouseInputType, BasicEventEditorData> pointerHoverEvent = this.pointerHoverEvent;
            if (pointerHoverEvent != null)
              pointerHoverEvent(MouseInputType.Exit, componentInParent.GetEventData());
          }
        }
        if ((UnityEngine.Object) this._hoveredGameObject != (UnityEngine.Object) null)
        {
          Action<MouseInputType, BasicEventEditorData> pointerHoverEvent = this.pointerHoverEvent;
          if (pointerHoverEvent != null)
            pointerHoverEvent(MouseInputType.Enter, basicEventEditorData);
        }
      }
      this._prevHoveredGameObject = this._hoveredGameObject;
      if ((UnityEngine.Object) this._hoveredGameObject == (UnityEngine.Object) null)
        return;
      (MouseInputType mouseDown, MouseInputType mouseUp, MouseInputType _) = this.GetMouseInputs();
      if (mouseDown != MouseInputType.None)
      {
        Action<MouseInputType, BasicEventEditorData> pointerDownEvent = this.pointerDownEvent;
        if (pointerDownEvent != null)
          pointerDownEvent(mouseDown, basicEventEditorData);
      }
      if (mouseUp == MouseInputType.None)
        return;
      Action<MouseInputType, BasicEventEditorData> pointerUpEvent = this.pointerUpEvent;
      if (pointerUpEvent == null)
        return;
      pointerUpEvent(mouseUp, basicEventEditorData);
    }

    private GameObject RaycastEventObjects()
    {
      RaycastHit hitInfo;
      return !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity, (int) this._eventObjectLayerMask) ? (GameObject) null : hitInfo.collider.gameObject;
    }
  }
}
