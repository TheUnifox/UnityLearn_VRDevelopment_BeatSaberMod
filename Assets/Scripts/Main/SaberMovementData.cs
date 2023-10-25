// Decompiled with JetBrains decompiler
// Type: SaberMovementData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SaberMovementData : IBladeMovementData, ISaberMovementData
{
  protected const float kOutOfRangeBladeSpeed = 100f;
  protected const float kSmoothUpBladeSpeedCoef = 24f;
  protected const float kSmoothDownBladeSpeedCoef = 2f;
  protected readonly BladeMovementDataElement[] _data;
  protected readonly LazyCopyHashSet<ISaberMovementDataProcessor> _dataProcessors = new LazyCopyHashSet<ISaberMovementDataProcessor>(32);
  protected int _nextAddIndex;
  protected int _validCount;
  protected float _bladeSpeed;

  public float bladeSpeed => this._bladeSpeed;

  public BladeMovementDataElement lastAddedData
  {
    get
    {
      int index = this._nextAddIndex - 1;
      if (index < 0)
        index += this._data.Length;
      return this._data[index];
    }
  }

  public BladeMovementDataElement prevAddedData
  {
    get
    {
      int index = this._nextAddIndex - 2;
      if (index < 0)
        index += this._data.Length;
      return this._data[index];
    }
  }

  public SaberMovementData() => this._data = new BladeMovementDataElement[500];

  public virtual void AddDataProcessor(ISaberMovementDataProcessor dataProcessor) => this._dataProcessors.Add(dataProcessor);

  public virtual void RemoveDataProcessor(ISaberMovementDataProcessor dataProcessor) => this._dataProcessors.Remove(dataProcessor);

  public virtual void RequestLastDataProcessing(ISaberMovementDataProcessor dataProcessor)
  {
    if (this._validCount <= 0)
      return;
    int length = this._data.Length;
    int index1 = this._nextAddIndex - 1;
    if (index1 < 0)
      index1 += length;
    int index2 = index1 - 1;
    if (index2 < 0)
      index2 += length;
    dataProcessor.ProcessNewData(this._data[index1], this._data[index2], this._validCount > 1);
  }

  public virtual void AddNewData(Vector3 topPos, Vector3 bottomPos, float time)
  {
    int length = this._data.Length;
    int nextAddIndex = this._nextAddIndex;
    int index = nextAddIndex - 1;
    if (index < 0)
      index += length;
    if (this._validCount > 0)
    {
      float num = time - this._data[index].time;
      if ((double) num > 9.9999997473787516E-05)
      {
        float b = ((topPos - this._data[index].topPos) / num).magnitude;
        if ((double) b > 100.0)
          b = 100f;
        this._bladeSpeed = Mathf.Lerp(this._bladeSpeed, b, num * ((double) b < (double) this._bladeSpeed ? 2f : 24f));
      }
    }
    this._data[nextAddIndex].topPos = topPos;
    this._data[nextAddIndex].bottomPos = bottomPos;
    this._data[nextAddIndex].time = time;
    this.ComputeAdditionalData(this._data[nextAddIndex].topPos, this._data[nextAddIndex].bottomPos, 0, out this._data[nextAddIndex].segmentNormal, out this._data[nextAddIndex].segmentAngle);
    this._nextAddIndex = (this._nextAddIndex + 1) % length;
    if (this._validCount < this._data.Length)
      ++this._validCount;
    foreach (ISaberMovementDataProcessor movementDataProcessor in this._dataProcessors.items)
      movementDataProcessor.ProcessNewData(this._data[nextAddIndex], this._data[index], this._validCount > 1);
  }

  public virtual void ComputeAdditionalData(
    Vector3 topPos,
    Vector3 bottomPos,
    int idxOffset,
    out Vector3 segmentNormal,
    out float segmentAngle)
  {
    int length = this._data.Length;
    int index1 = this._nextAddIndex + idxOffset;
    int index2 = index1 - 1;
    if (index2 < 0)
      index2 += length;
    if (this._validCount > 0)
    {
      Vector3 topPos1 = this._data[index1].topPos;
      Vector3 bottomPos1 = this._data[index1].bottomPos;
      Vector3 topPos2 = this._data[index2].topPos;
      Vector3 bottomPos2 = this._data[index2].bottomPos;
      segmentNormal = this.ComputePlaneNormal(topPos1, bottomPos1, topPos2, bottomPos2);
      segmentAngle = Vector3.Angle(topPos2 - bottomPos2, topPos1 - bottomPos1);
    }
    else
    {
      segmentNormal = Vector3.zero;
      segmentAngle = 0.0f;
    }
  }

  public virtual Vector3 ComputePlaneNormal(Vector3 tp0, Vector3 bp0, Vector3 tp1, Vector3 bp1) => Vector3.Cross(tp0 - bp0, (tp1 + bp1) * 0.5f - bp0).normalized;

  public virtual Vector3 ComputeCutPlaneNormal()
  {
    int length = this._data.Length;
    int index1 = this._nextAddIndex - 1;
    if (index1 < 0)
      index1 += length;
    int index2 = index1 - 1;
    if (index2 < 0)
      index2 += length;
    return this.ComputePlaneNormal(this._data[index1].topPos, this._data[index1].bottomPos, this._data[index2].topPos, this._data[index2].bottomPos);
  }

  public virtual float ComputeSwingRating(float overrideSegmentAngle) => this.ComputeSwingRating(true, overrideSegmentAngle);

  public virtual float ComputeSwingRating() => this.ComputeSwingRating(false, 0.0f);

  public virtual float ComputeSwingRating(bool overrideSegmenAngle, float overrideValue)
  {
    if (this._validCount < 2)
      return 0.0f;
    int length = this._data.Length;
    int index = this._nextAddIndex - 1;
    if (index < 0)
      index += length;
    float time = this._data[index].time;
    float num1 = time;
    float num2 = 0.0f;
    Vector3 segmentNormal1 = this._data[index].segmentNormal;
    float angleDiff = overrideSegmenAngle ? overrideValue : this._data[index].segmentAngle;
    int num3 = 2;
    float swingRating = num2 + SaberSwingRating.BeforeCutStepRating(angleDiff, 0.0f);
    for (; (double) time - (double) num1 < 0.40000000596046448 && num3 < this._validCount; ++num3)
    {
      --index;
      if (index < 0)
        index += length;
      Vector3 segmentNormal2 = this._data[index].segmentNormal;
      float segmentAngle = this._data[index].segmentAngle;
      Vector3 to = segmentNormal1;
      float normalDiff = Vector3.Angle(segmentNormal2, to);
      if ((double) normalDiff <= 90.0)
      {
        swingRating += SaberSwingRating.BeforeCutStepRating(segmentAngle, normalDiff);
        num1 = this._data[index].time;
      }
      else
        break;
    }
    if ((double) swingRating > 1.0)
      swingRating = 1f;
    return swingRating;
  }
}
