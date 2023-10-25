// Decompiled with JetBrains decompiler
// Type: VideoProjectionDataModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class VideoProjectionDataModelSO : PersistentScriptableObject
{
  [SerializeField]
  protected VideoProjectionDataModelSO.VideoClipWithId[] _videoClipsWithId;

  public VideoProjectionDataModelSO.VideoClipWithId[] videoClipWithIds => this._videoClipsWithId;

  [Serializable]
  public class VideoClipWithId
  {
    [SerializeField]
    protected int _id;
    [SerializeField]
    protected AssetReference _videoAssetReference;

    public int id => this._id;

    public AssetReference videoAssetReference => this._videoAssetReference;
  }
}
