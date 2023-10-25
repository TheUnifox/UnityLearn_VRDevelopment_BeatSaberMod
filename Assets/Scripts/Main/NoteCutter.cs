// Decompiled with JetBrains decompiler
// Type: NoteCutter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using UnityEngine;

public class NoteCutter
{
  protected const int kMaxNumberOfColliders = 16;
  protected readonly Collider[] _colliders = new Collider[16];
  protected readonly NoteCutter.CuttableBySaberSortParams[] _cuttableBySaberSortParams = new NoteCutter.CuttableBySaberSortParams[16];
  protected readonly NoteCutter.CuttableBySaberSortParamsComparer _comparer = new NoteCutter.CuttableBySaberSortParamsComparer();

  public NoteCutter()
  {
    for (int index = 0; index < 16; ++index)
      this._cuttableBySaberSortParams[index] = new NoteCutter.CuttableBySaberSortParams();
  }

  public virtual void Cut(Saber saber)
  {
    Vector3 saberBladeTopPos = saber.saberBladeTopPos;
    Vector3 saberBladeBottomPos = saber.saberBladeBottomPos;
    BladeMovementDataElement prevAddedData = saber.movementData.prevAddedData;
    Vector3 topPos = prevAddedData.topPos;
    Vector3 bottomPos = prevAddedData.bottomPos;
    Vector3 center;
    Vector3 halfSize;
    Quaternion orientation;
    if (!GeometryTools.ThreePointsToBox(saberBladeTopPos, saberBladeBottomPos, (bottomPos + topPos) * 0.5f, out center, out halfSize, out orientation))
      return;
    int length = Physics.OverlapBoxNonAlloc(center, halfSize, this._colliders, orientation, (int) LayerMasks.noteLayerMask);
    switch (length)
    {
      case 0:
        break;
      case 1:
        this._colliders[0].gameObject.GetComponent<CuttableBySaber>().Cut(saber, center, orientation, saberBladeTopPos - topPos);
        break;
      default:
        for (int index = 0; index < length; ++index)
        {
          CuttableBySaber component = this._colliders[index].gameObject.GetComponent<CuttableBySaber>();
          Vector3 position = component.transform.position;
          NoteCutter.CuttableBySaberSortParams bySaberSortParam = this._cuttableBySaberSortParams[index];
          bySaberSortParam.cuttableBySaber = component;
          bySaberSortParam.distance = (topPos - position).sqrMagnitude - component.radius * component.radius;
          bySaberSortParam.pos = position;
        }
        if (length == 2)
        {
          if (this._comparer.Compare((object) this._cuttableBySaberSortParams[0], (object) this._cuttableBySaberSortParams[1]) > 0)
          {
            this._cuttableBySaberSortParams[0].cuttableBySaber.Cut(saber, center, orientation, saberBladeTopPos - topPos);
            this._cuttableBySaberSortParams[1].cuttableBySaber.Cut(saber, center, orientation, saberBladeTopPos - topPos);
            break;
          }
          this._cuttableBySaberSortParams[1].cuttableBySaber.Cut(saber, center, orientation, saberBladeTopPos - topPos);
          this._cuttableBySaberSortParams[0].cuttableBySaber.Cut(saber, center, orientation, saberBladeTopPos - topPos);
          break;
        }
        Array.Sort((Array) this._cuttableBySaberSortParams, 0, length, (IComparer) this._comparer);
        for (int index = 0; index < length; ++index)
          this._cuttableBySaberSortParams[index].cuttableBySaber.Cut(saber, center, orientation, saberBladeTopPos - topPos);
        break;
    }
  }

  public class CuttableBySaberSortParams
  {
    public CuttableBySaber cuttableBySaber;
    public float distance;
    public Vector3 pos;
  }

  public class CuttableBySaberSortParamsComparer : IComparer
  {
    public virtual int Compare(object p0, object p1)
    {
      NoteCutter.CuttableBySaberSortParams bySaberSortParams1 = (NoteCutter.CuttableBySaberSortParams) p0;
      NoteCutter.CuttableBySaberSortParams bySaberSortParams2 = (NoteCutter.CuttableBySaberSortParams) p1;
      if ((double) bySaberSortParams1.distance > (double) bySaberSortParams2.distance)
        return 1;
      if ((double) bySaberSortParams1.distance < (double) bySaberSortParams2.distance)
        return -1;
      if ((double) bySaberSortParams1.pos.x < (double) bySaberSortParams2.pos.x)
        return 1;
      if ((double) bySaberSortParams1.pos.x > (double) bySaberSortParams2.pos.x)
        return -1;
      if ((double) bySaberSortParams1.pos.y < (double) bySaberSortParams2.pos.y)
        return 1;
      if ((double) bySaberSortParams1.pos.y > (double) bySaberSortParams2.pos.y)
        return -1;
      if ((double) bySaberSortParams1.pos.z < (double) bySaberSortParams2.pos.z)
        return 1;
      return (double) bySaberSortParams1.pos.z > (double) bySaberSortParams2.pos.z ? -1 : 0;
    }
  }
}
