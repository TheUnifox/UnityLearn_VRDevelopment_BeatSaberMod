// Decompiled with JetBrains decompiler
// Type: SaberSwingRatingCounter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class SaberSwingRatingCounter : ISaberSwingRatingCounter, ISaberMovementDataProcessor
{
  protected ISaberMovementData _saberMovementData;
  protected Vector3 _cutPlaneNormal;
  protected float _cutTime;
  protected float _afterCutRating;
  protected float _beforeCutRating;
  protected Plane _notePlane;
  protected bool _notePlaneWasCut;
  protected Vector3 _noteForward;
  protected bool _rateBeforeCut;
  protected bool _rateAfterCut;
  protected readonly LazyCopyHashSet<ISaberSwingRatingCounterDidChangeReceiver> _didChangeReceivers = new LazyCopyHashSet<ISaberSwingRatingCounterDidChangeReceiver>();
  protected readonly LazyCopyHashSet<ISaberSwingRatingCounterDidFinishReceiver> _didFinishReceivers = new LazyCopyHashSet<ISaberSwingRatingCounterDidFinishReceiver>();
  protected Vector3 _notePlaneCenter;
  protected Vector3 _beforeCutTopPos;
  protected Vector3 _beforeCutBottomPos;
  protected Vector3 _afterCutTopPos;
  protected Vector3 _afterCutBottomPos;
  protected Vector3 _newPlaneNormal;
  protected Vector3 _cutTopPos;
  protected Vector3 _cutBottomPos;
  protected bool _finished;

  public float beforeCutRating => this._beforeCutRating;

  public float afterCutRating => this._afterCutRating;

  public virtual void RegisterDidChangeReceiver(ISaberSwingRatingCounterDidChangeReceiver receiver) => this._didChangeReceivers.Add(receiver);

  public virtual void RegisterDidFinishReceiver(ISaberSwingRatingCounterDidFinishReceiver receiver) => this._didFinishReceivers.Add(receiver);

  public virtual void UnregisterDidChangeReceiver(ISaberSwingRatingCounterDidChangeReceiver receiver) => this._didChangeReceivers.Remove(receiver);

  public virtual void UnregisterDidFinishReceiver(ISaberSwingRatingCounterDidFinishReceiver receiver) => this._didFinishReceivers.Remove(receiver);

  public virtual void Init(
    ISaberMovementData saberMovementData,
    Vector3 notePosition,
    Quaternion noteRotation,
    bool rateBeforeCut,
    bool rateAfterCut)
  {
    this._finished = false;
    this._notePlaneWasCut = false;
    BladeMovementDataElement lastAddedData = saberMovementData.lastAddedData;
    this._cutPlaneNormal = lastAddedData.segmentNormal;
    this._cutTime = lastAddedData.time;
    this._rateBeforeCut = rateBeforeCut;
    this._rateAfterCut = rateAfterCut;
    this._beforeCutRating = !rateBeforeCut ? 1f : saberMovementData.ComputeSwingRating();
    this._afterCutRating = !rateAfterCut ? 1f : 0.0f;
    this._notePlaneCenter = notePosition;
    this._notePlane = new Plane(noteRotation * Vector3.up, this._notePlaneCenter);
    this._noteForward = noteRotation * Vector3.forward;
    this._saberMovementData = saberMovementData;
    this._saberMovementData.AddDataProcessor((ISaberMovementDataProcessor) this);
    this._saberMovementData.RequestLastDataProcessing((ISaberMovementDataProcessor) this);
  }

  public virtual void ProcessNewData(
    BladeMovementDataElement newData,
    BladeMovementDataElement prevData,
    bool prevDataAreValid)
  {
    if ((double) newData.time - (double) this._cutTime > 0.40000000596046448)
      this.Finish();
    else if (this._notePlaneWasCut && (!this._rateAfterCut || this._rateAfterCut && (double) this._afterCutRating >= 1.0))
    {
      this.Finish();
    }
    else
    {
      if (!prevDataAreValid)
        return;
      if (!this._notePlaneWasCut)
      {
        this._newPlaneNormal = Vector3.Cross(this._cutPlaneNormal, this._noteForward);
        this._notePlane = new Plane(this._newPlaneNormal, this._notePlaneCenter);
      }
      if (!this._notePlaneWasCut && !this._notePlane.SameSide(newData.topPos, prevData.topPos))
      {
        this._beforeCutTopPos = prevData.topPos;
        this._beforeCutBottomPos = prevData.bottomPos;
        this._afterCutTopPos = newData.topPos;
        this._afterCutBottomPos = newData.bottomPos;
        Ray ray = new Ray(prevData.topPos, newData.topPos - prevData.topPos);
        float enter;
        this._notePlane.Raycast(ray, out enter);
        this._cutTopPos = ray.GetPoint(enter);
        this._cutBottomPos = (prevData.bottomPos + newData.bottomPos) * 0.5f;
        float overrideSegmentAngle = Vector3.Angle(this._cutTopPos - this._cutBottomPos, this._beforeCutTopPos - this._beforeCutBottomPos);
        float angleDiff = Vector3.Angle(this._cutTopPos - this._cutBottomPos, this._afterCutTopPos - this._afterCutBottomPos);
        this._cutTime = newData.time;
        if (this._rateBeforeCut)
          this._beforeCutRating = this._saberMovementData.ComputeSwingRating(overrideSegmentAngle);
        if (this._rateAfterCut)
        {
          this._afterCutRating = SaberSwingRating.AfterCutStepRating(angleDiff, 0.0f);
          if ((double) this._afterCutRating > 1.0)
            this._afterCutRating = 1f;
        }
        this._notePlaneWasCut = true;
      }
      else
      {
        float normalDiff = Vector3.Angle(newData.segmentNormal, this._cutPlaneNormal);
        if ((double) normalDiff > 90.0)
        {
          this.Finish();
          return;
        }
        if (this._rateAfterCut)
        {
          this._afterCutRating += SaberSwingRating.AfterCutStepRating(newData.segmentAngle, normalDiff);
          if ((double) this._afterCutRating > 1.0)
            this._afterCutRating = 1f;
        }
      }
      List<ISaberSwingRatingCounterDidChangeReceiver> items = this._didChangeReceivers.items;
      for (int index = items.Count - 1; index >= 0; --index)
        items[index].HandleSaberSwingRatingCounterDidChange((ISaberSwingRatingCounter) this, this._afterCutRating);
    }
  }

  public virtual void Finish()
  {
    this._saberMovementData?.RemoveDataProcessor((ISaberMovementDataProcessor) this);
    List<ISaberSwingRatingCounterDidFinishReceiver> items = this._didFinishReceivers.items;
    for (int index = items.Count - 1; index >= 0; --index)
      items[index].HandleSaberSwingRatingCounterDidFinish((ISaberSwingRatingCounter) this);
    this._finished = true;
  }

  public virtual void DrawGizmos()
  {
    Gizmos.matrix = Matrix4x4.TRS(this._notePlaneCenter, Quaternion.LookRotation(this._notePlane.normal), Vector3.one);
    Gizmos.color = (Color) ((Color32) Color.blue with
    {
      a = (byte) 125
    });
    Gizmos.DrawCube(Vector3.zero, new Vector3(1f, 1f, 0.0001f));
    Gizmos.matrix = Matrix4x4.identity;
    Gizmos.color = Color.white;
    Gizmos.color = Color.magenta;
    Gizmos.DrawLine(this._beforeCutBottomPos, this._beforeCutTopPos);
    Gizmos.color = Color.white;
    Gizmos.DrawLine(this._afterCutBottomPos, this._afterCutTopPos);
    Gizmos.color = Color.red;
    Gizmos.DrawLine(this._notePlaneCenter, this._notePlaneCenter + this._newPlaneNormal);
    Gizmos.color = Color.green;
    Gizmos.DrawLine(this._cutTopPos, this._cutBottomPos);
  }
}
