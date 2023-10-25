// Decompiled with JetBrains decompiler
// Type: PoseObject
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class PoseObject
{
  [SerializeField]
  protected Transform _transform;
  [SerializeField]
  protected PoseObjectIdSO _poseObjectId;

  public Transform objectTransform => this._transform;

  public string id => this._poseObjectId.id;

  public PoseObject(Transform transform, PoseObjectIdSO poseObjectId)
  {
    this._transform = transform;
    this._poseObjectId = poseObjectId;
  }
}
