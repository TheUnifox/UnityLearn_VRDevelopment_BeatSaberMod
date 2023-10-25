// Decompiled with JetBrains decompiler
// Type: TrailElementCollection
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class TrailElementCollection
{
  protected readonly int _capacity;
  protected readonly TrailElement[] _snapshots;
  protected int _headIndex;
  protected float _totalDistance;

  public TrailElementCollection(
    int capacity,
    Vector3 defaultStartPosition,
    Vector3 defaultEndPosition,
    float time)
  {
    this._capacity = capacity;
    this._snapshots = new TrailElement[capacity];
    for (int index = 0; index < capacity; ++index)
    {
      this._snapshots[index] = new TrailElement();
      this._snapshots[index].SetData(defaultStartPosition, defaultEndPosition, time);
    }
  }

  public virtual void InitSnapshots(
    Vector3 defaultStartPosition,
    Vector3 defaultEndPosition,
    float time)
  {
    for (int index = 0; index < this._capacity; ++index)
      this._snapshots[index].SetData(defaultStartPosition, defaultEndPosition, time);
  }

  public virtual void SetHeadData(Vector3 start, Vector3 end, float time)
  {
    TrailElement element = this.GetElement(0);
    element.SetData(start, end, time);
    this.GetElement(1).UpdateLocalDistance(element);
  }

  public virtual void MoveTailToHead()
  {
    this._headIndex = (this._headIndex + this._capacity - 1) % this._capacity;
    this.GetElement(0).CopyFrom(this.GetElement(1));
  }

  public virtual void UpdateDistances()
  {
    float num = 0.0f;
    for (int index = 0; index < this._capacity; ++index)
    {
      TrailElement element = this.GetElement(index);
      num += element.localDistance;
      element.SetDistance(num);
    }
    this._totalDistance = num;
  }

  public virtual void Interpolate(
    float t,
    ref TrailElementCollection.InterpolationState lerpState,
    out Vector3 position,
    out Vector3 normal,
    out float time)
  {
    this.UpdateLerpState(t, ref lerpState);
    TrailElement element1 = this.GetElement(lerpState.segmentIndex > 0 ? lerpState.segmentIndex - 1 : 0);
    TrailElement element2 = this.GetElement(lerpState.segmentIndex);
    TrailElement element3 = this.GetElement(lerpState.segmentIndex < this._capacity - 1 ? lerpState.segmentIndex + 1 : this._capacity - 1);
    TrailElement element4 = this.GetElement(lerpState.segmentIndex < this._capacity - 2 ? lerpState.segmentIndex + 2 : this._capacity - 1);
    position = SplineUtils.Interpolate(element1.position, element2.position, element3.position, element4.position, lerpState.segmentLerp);
    normal = SplineUtils.Interpolate(element1.normal, element2.normal, element3.normal, element4.normal, lerpState.segmentLerp).normalized;
    time = Mathf.Lerp(element2.time, element3.time, lerpState.segmentLerp);
  }

  public virtual void UpdateLerpState(
    float t,
    ref TrailElementCollection.InterpolationState interpolationState)
  {
    float num1 = t * this._totalDistance;
    if ((double) num1 < (double) Mathf.Epsilon)
    {
      interpolationState.segmentIndex = 0;
      interpolationState.segmentLerp = 0.0f;
    }
    else
    {
      if ((double) num1 > (double) this._totalDistance)
        num1 = this._totalDistance;
      int index = interpolationState.segmentIndex + 1;
      float num2 = 0.0f;
      float num3 = 0.0f;
      for (; index < this._capacity; ++index)
      {
        TrailElement element = this.GetElement(index);
        if ((double) element.distance >= (double) num1)
        {
          num2 = element.distance;
          num3 = element.localDistance;
          break;
        }
      }
      interpolationState.segmentIndex = index - 1;
      interpolationState.segmentLerp = (num1 - num2 + num3) / num3;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual TrailElement GetElement(int index) => this._snapshots[(this._headIndex + index) % this._capacity];

  public struct InterpolationState
  {
    public int segmentIndex;
    public float segmentLerp;
  }
}
