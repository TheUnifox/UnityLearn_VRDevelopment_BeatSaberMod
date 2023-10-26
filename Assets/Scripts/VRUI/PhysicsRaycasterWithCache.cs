// Decompiled with JetBrains decompiler
// Type: VRUIControls.PhysicsRaycasterWithCache
// Assembly: VRUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BA7334A-77F9-4425-988C-69CB2EE35CCF
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\VRUI.dll

using System.Collections.Generic;
using UnityEngine;

namespace VRUIControls
{
  public class PhysicsRaycasterWithCache
  {
    protected readonly List<PhysicsRaycasterWithCache.CachedRaycast> _cachedRaycasts = new List<PhysicsRaycasterWithCache.CachedRaycast>();
    protected int _lastFrameCount = -1;

    public virtual bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask)
    {
      if (Time.frameCount != this._lastFrameCount)
      {
        this._lastFrameCount = Time.frameCount;
        this._cachedRaycasts.Clear();
      }
      foreach (PhysicsRaycasterWithCache.CachedRaycast cachedRaycast in this._cachedRaycasts)
      {
        Ray ray1 = cachedRaycast.ray;
        if (ray1.origin == ray.origin)
        {
          ray1 = cachedRaycast.ray;
          if (ray1.direction == ray.direction && Mathf.Approximately(cachedRaycast.maxDistance, maxDistance) && cachedRaycast.layerMask == layerMask)
          {
            hitInfo = cachedRaycast.hitInfo;
            return cachedRaycast.wasHit;
          }
        }
      }
      bool wasHit = Physics.Raycast(ray, out hitInfo, maxDistance, layerMask);
      this._cachedRaycasts.Add(new PhysicsRaycasterWithCache.CachedRaycast(wasHit, ray, hitInfo, maxDistance, layerMask));
      return wasHit;
    }

    public readonly struct CachedRaycast
    {
      public readonly bool wasHit;
      public readonly Ray ray;
      public readonly RaycastHit hitInfo;
      public readonly float maxDistance;
      public readonly int layerMask;

      public CachedRaycast(
        bool wasHit,
        Ray ray,
        RaycastHit hitInfo,
        float maxDistance,
        int layerMask)
      {
        this.wasHit = wasHit;
        this.ray = ray;
        this.hitInfo = hitInfo;
        this.maxDistance = maxDistance;
        this.layerMask = layerMask;
      }
    }
  }
}
