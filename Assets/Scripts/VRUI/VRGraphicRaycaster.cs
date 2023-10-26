// Decompiled with JetBrains decompiler
// Type: VRUIControls.VRGraphicRaycaster
// Assembly: VRUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BA7334A-77F9-4425-988C-69CB2EE35CCF
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\VRUI.dll

using HMUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace VRUIControls
{
  [RequireComponent(typeof (Canvas))]
  public class VRGraphicRaycaster : BaseRaycaster
  {
    [SerializeField]
    protected LayerMask _blockingMask = (LayerMask)(-1);
    [Inject]
    protected readonly PhysicsRaycasterWithCache _physicsRaycaster;
    protected Canvas _canvas;
    protected readonly List<VRGraphicRaycaster.VRGraphicRaycastResult> _raycastResults = new List<VRGraphicRaycaster.VRGraphicRaycastResult>();
    protected readonly CurvedCanvasSettingsHelper _curvedCanvasSettingsHelper = new CurvedCanvasSettingsHelper();
    protected const float kPhysics3DRaycastDistance = 6f;
    [DoesNotRequireDomainReloadInit]
    protected static readonly float[] _ray2DCircleIntersectionDistances = new float[2];

    public override Camera eventCamera => (Camera) null;

    protected override void OnEnable()
    {
      base.OnEnable();
      this._curvedCanvasSettingsHelper.Reset();
      this._canvas = this.GetComponent<Canvas>();
    }

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
      CurvedCanvasSettings curvedCanvasSettings = this._curvedCanvasSettingsHelper.GetCurvedCanvasSettings(this._canvas);
      float curvedUIRadius = (UnityEngine.Object) curvedCanvasSettings == (UnityEngine.Object) null ? 0.0f : curvedCanvasSettings.radius;
      float hitDistance = float.MaxValue;
      Ray ray = new Ray(eventData.pointerCurrentRaycast.worldPosition, eventData.pointerCurrentRaycast.worldNormal);
      RaycastHit hitInfo;
      RaycastResult raycastResult1;
      if (this._physicsRaycaster.Raycast(ray, out hitInfo, 6f, (int) this._blockingMask))
      {
        hitDistance = hitInfo.distance;
        raycastResult1 = new RaycastResult();
        raycastResult1.gameObject = hitInfo.collider.gameObject;
        raycastResult1.module = (BaseRaycaster) this;
        raycastResult1.distance = hitDistance;
        raycastResult1.screenPosition = new Vector2(float.MaxValue, float.MaxValue);
        raycastResult1.worldPosition = hitInfo.point;
        raycastResult1.index = (float) resultAppendList.Count;
        raycastResult1.depth = 0;
        raycastResult1.sortingLayer = this._canvas.sortingLayerID;
        raycastResult1.sortingOrder = this._canvas.sortingOrder;
        RaycastResult raycastResult2 = raycastResult1;
        resultAppendList.Add(raycastResult2);
      }
      RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) ray.origin, (Vector2) ray.direction, (float) eventData.pointerCurrentRaycast.depth, (int) this._blockingMask);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      {
        float num = raycastHit2D.fraction * (float) eventData.pointerCurrentRaycast.depth;
        if ((double) num < (double) hitDistance)
          hitDistance = num;
      }
      VRGraphicRaycaster.RaycastCanvas(this._canvas, ray, hitDistance, curvedUIRadius, this._raycastResults);
      for (int index = 0; index < this._raycastResults.Count; ++index)
      {
        GameObject gameObject = this._raycastResults[index].graphic.gameObject;
        raycastResult1 = new RaycastResult();
        raycastResult1.gameObject = gameObject;
        raycastResult1.module = (BaseRaycaster) this;
        raycastResult1.distance = this._raycastResults[index].distance;
        raycastResult1.screenPosition = this._raycastResults[index].insideRootCanvasPosition;
        raycastResult1.worldPosition = this._raycastResults[index].position;
        raycastResult1.index = (float) resultAppendList.Count;
        raycastResult1.depth = this._raycastResults[index].graphic.depth;
        raycastResult1.sortingLayer = this._canvas.sortingLayerID;
        raycastResult1.sortingOrder = this._canvas.sortingOrder;
        RaycastResult raycastResult3 = raycastResult1;
        resultAppendList.Add(raycastResult3);
      }
    }

    private static void RaycastCanvas(
      Canvas canvas,
      Ray ray,
      float hitDistance,
      float curvedUIRadius,
      List<VRGraphicRaycaster.VRGraphicRaycastResult> results)
    {
      results.Clear();
      RectTransform transform1 = (RectTransform) canvas.transform;
      Vector3 position1 = transform1.position;
      Vector3 forward1 = transform1.forward;
      float distance;
      Vector3 position2;
      Vector3 vector3_1;
      if ((double) Math.Abs(curvedUIRadius) < 0.10000000149011612)
      {
        distance = Vector3.Dot(forward1, position1 - ray.origin) / Vector3.Dot(forward1, ray.direction);
        if ((double) distance < 0.0 || (double) distance >= (double) hitDistance)
          return;
        position2 = ray.GetPoint(distance);
        vector3_1 = position2;
      }
      else
      {
        RectTransform transform2 = (RectTransform) canvas.rootCanvas.transform;
        Vector3 position3 = transform2.position;
        Quaternion rotation = transform2.rotation;
        float radius = curvedUIRadius * transform2.lossyScale.z;
        Vector2 circleCenter = new Vector2(0.0f, -radius);
        Ray ray1 = new Ray()
        {
          origin = Quaternion.Inverse(rotation) * (ray.origin - position3),
          direction = transform2.InverseTransformDirection(ray.direction)
        };
        Ray2D ray2 = new Ray2D(new Vector2(ray1.origin.x, ray1.origin.z), new Vector2(ray1.direction.x, ray1.direction.z));
        if (ray2.CircleIntersections(circleCenter, radius, VRGraphicRaycaster._ray2DCircleIntersectionDistances) != 1)
          return;
        Vector2 point = ray2.GetPoint(VRGraphicRaycaster._ray2DCircleIntersectionDistances[0]);
        float y = (point - ray2.origin).magnitude * ray1.direction.y / Mathf.Sqrt((float) ((double) ray1.direction.x * (double) ray1.direction.x + (double) ray1.direction.z * (double) ray1.direction.z)) + ray1.origin.y;
        Vector3 vector3_2 = new Vector3(point.x, y, point.y);
        position2 = rotation * vector3_2 + position3;
        distance = (position2 - ray.origin).magnitude;
        Vector3 forward2 = Vector3.forward;
        Vector3 vector3_3 = new Vector3(-Vector2.SignedAngle(new Vector2(forward2.x, forward2.z), point - circleCenter) * ((float) Math.PI / 180f) * radius, vector3_2.y, vector3_2.z);
        vector3_1 = rotation * vector3_3 + position3;
        RectTransform transform3 = (RectTransform) canvas.transform;
        if (!transform3.rect.Contains(transform3.InverseTransformPoint(vector3_1)))
          return;
      }
      IList<Graphic> graphicsForCanvas = GraphicRegistry.GetGraphicsForCanvas(canvas);
      for (int index = 0; index < graphicsForCanvas.Count; ++index)
      {
        Graphic graphic = graphicsForCanvas[index];
        if (graphic.depth != -1 && graphic.raycastTarget)
        {
          RectTransform transform4 = (RectTransform) graphic.transform;
          Vector3 point = transform4.InverseTransformPoint(vector3_1);
          Rect rect = transform4.rect;
          if (graphic is ImageView imageView)
            point.x -= imageView.skew * point.y;
          else if (graphic is Touchable touchable)
            point.x -= touchable.skew * point.y;
          if (rect.Contains(point))
            results.Add(new VRGraphicRaycaster.VRGraphicRaycastResult(graphic, distance, position2, (Vector2) vector3_1));
        }
      }
      results.Sort((Comparison<VRGraphicRaycaster.VRGraphicRaycastResult>) ((g1, g2) => g2.graphic.depth.CompareTo(g1.graphic.depth)));
    }

    public struct VRGraphicRaycastResult
    {
      public readonly Graphic graphic;
      public readonly float distance;
      public readonly Vector3 position;
      public readonly Vector2 insideRootCanvasPosition;

      public VRGraphicRaycastResult(
        Graphic graphic,
        float distance,
        Vector3 position,
        Vector2 insideRootCanvasPosition)
      {
        this.graphic = graphic;
        this.distance = distance;
        this.position = position;
        this.insideRootCanvasPosition = insideRootCanvasPosition;
      }
    }
  }
}
