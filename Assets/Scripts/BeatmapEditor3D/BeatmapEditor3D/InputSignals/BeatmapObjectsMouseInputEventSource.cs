// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.InputSignals.BeatmapObjectsMouseInputEventSource
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D.InputSignals
{
  public class BeatmapObjectsMouseInputEventSource : AbstractMouseInputEventSource
  {
    [SerializeField]
    private LayerMask _notesLayerMask;
    [SerializeField]
    private LayerMask _obstaclesLayerMask;
    private BeatmapObjectMouseInputProvider _prevHoveredMouseInputDataProvider;
    private BeatmapObjectMouseInputProvider _hoveredMouseInputDataProvider;

    protected void Update()
    {
      GameObject gameObject = this.RaycastBeatmapObjects();
      BeatmapObjectMouseInputProvider mouseInputProvider = (BeatmapObjectMouseInputProvider) null;
      if ((Object) gameObject != (Object) null)
        mouseInputProvider = gameObject.GetComponentInParent<BeatmapObjectMouseInputProvider>();
      if ((Object) mouseInputProvider != (Object) null && mouseInputProvider.WasInitializedThisFrame())
        return;
      this._hoveredMouseInputDataProvider = mouseInputProvider;
      if ((Object) this._hoveredMouseInputDataProvider != (Object) this._prevHoveredMouseInputDataProvider)
      {
        if ((Object) this._prevHoveredMouseInputDataProvider != (Object) null)
          this._prevHoveredMouseInputDataProvider.PointerHover(MouseInputType.Exit);
        if ((Object) this._hoveredMouseInputDataProvider != (Object) null)
          this._hoveredMouseInputDataProvider.PointerHover(MouseInputType.Enter);
      }
      this._prevHoveredMouseInputDataProvider = this._hoveredMouseInputDataProvider;
      if ((Object) this._hoveredMouseInputDataProvider == (Object) null)
        return;
      (MouseInputType mouseInputType1, MouseInputType mouseInputType2, MouseInputType mouseInputType3) = this.GetMouseInputs();
      if (mouseInputType1 != MouseInputType.None)
        this._hoveredMouseInputDataProvider.PointerDown(mouseInputType1);
      if (mouseInputType2 != MouseInputType.None)
        this._hoveredMouseInputDataProvider.PointerUp(mouseInputType2);
      if (mouseInputType3 == MouseInputType.None)
        return;
      this._hoveredMouseInputDataProvider.PointerScroll(mouseInputType3);
    }

    private GameObject RaycastBeatmapObjects()
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      int layerMask = (int) this._notesLayerMask | (int) this._obstaclesLayerMask;
      RaycastHit hitInfo1;
      if (!Physics.Raycast(ray, out hitInfo1, float.PositiveInfinity, layerMask))
        return (GameObject) null;
      if (1 << hitInfo1.collider.gameObject.layer == (int) this._notesLayerMask)
        return hitInfo1.collider.gameObject;
      int notesLayerMask = (int) this._notesLayerMask;
      RaycastHit hitInfo2;
      return !Physics.Raycast(ray, out hitInfo2, float.PositiveInfinity, notesLayerMask) ? hitInfo1.collider.gameObject : hitInfo2.collider.gameObject;
    }
  }
}
