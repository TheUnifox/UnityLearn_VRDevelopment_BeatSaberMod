// Decompiled with JetBrains decompiler
// Type: BTSCharacterDataModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BTSCharacterDataModel : MonoBehaviour
{
  [SerializeField]
  protected BTSCharacterDataModel.PrefabWithId[] _prefabsWithIds;
  [SerializeField]
  protected BTSCharacterDataModel.AnimationClipWithId[] _animationClipsWithIds;

  public BTSCharacterDataModel.PrefabWithId[] prefabsWithIds => this._prefabsWithIds;

  public BTSCharacterDataModel.AnimationClipWithId[] animationClipsWithIds => this._animationClipsWithIds;

  [Serializable]
  public class PrefabWithId
  {
    [SerializeField]
    protected int _id;
    [SerializeField]
    protected AssetReference _prefabAssetReference;

    public int id => this._id;

    public AssetReference prefabAssetReference => this._prefabAssetReference;
  }

  [Serializable]
  public class AnimationClipWithId
  {
    [SerializeField]
    protected int _id;
    [SerializeField]
    protected AssetReference _animationClipAssetReference;

    public int id => this._id;

    public AssetReference animationClipAssetReference => this._animationClipAssetReference;
  }
}
