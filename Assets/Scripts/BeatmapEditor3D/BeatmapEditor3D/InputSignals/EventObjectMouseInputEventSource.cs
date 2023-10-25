// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.EventObjectMouseInputEventSource
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D.InputSignals
{
  public class EventObjectMouseInputEventSource : AbstractMouseInputEventSource
  {
    [SerializeField]
    private LayerMask _eventObjectLayerMask;
    private EventObjectMouseInputController _prevHoveredMouseInputController;
    private EventObjectMouseInputController _hoveredMouseInputController;

    protected void Update()
    {
      GameObject gameObject = this.RaycastEventObjects();
      EventObjectMouseInputController mouseInputController = (EventObjectMouseInputController) null;
      if ((Object) gameObject != (Object) null)
        mouseInputController = gameObject.GetComponentInParent<EventObjectMouseInputController>();
      if ((Object) mouseInputController != (Object) null && mouseInputController.WasInitializedThisFrame())
        return;
      this._hoveredMouseInputController = mouseInputController;
      if ((Object) this._hoveredMouseInputController != (Object) this._prevHoveredMouseInputController)
      {
        if ((Object) this._prevHoveredMouseInputController != (Object) null)
          this._prevHoveredMouseInputController.PointerExit();
        if ((Object) this._hoveredMouseInputController != (Object) null)
          this._hoveredMouseInputController.PointerEnter();
      }
      this._prevHoveredMouseInputController = this._hoveredMouseInputController;
      if ((Object) this._hoveredMouseInputController == (Object) null)
        return;
      (MouseInputType mouseInputType1, MouseInputType mouseInputType2, MouseInputType mouseInputType3) = this.GetMouseInputs();
      if (mouseInputType1 != MouseInputType.None)
        this._hoveredMouseInputController.PointerDown(mouseInputType1);
      if (mouseInputType2 != MouseInputType.None)
        this._hoveredMouseInputController.PointerUp(mouseInputType2);
      if (mouseInputType3 == MouseInputType.None)
        return;
      this._hoveredMouseInputController.PointerScroll(mouseInputType3);
    }

    private GameObject RaycastEventObjects()
    {
      RaycastHit hitInfo;
      return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity, (int) this._eventObjectLayerMask) ? hitInfo.collider.gameObject : (GameObject) null;
    }
  }
}
