// Decompiled with JetBrains decompiler
// Type: AvatarPartsModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class AvatarPartsModelSO : PersistentScriptableObject
{
  [SerializeField]
  protected AvatarMeshPartSO[] _headTops;
  [SerializeField]
  protected AvatarSpritePartSO[] _eyes;
  [SerializeField]
  protected AvatarSpritePartSO[] _mouths;
  [SerializeField]
  protected AvatarMeshPartSO[] _glasses;
  [SerializeField]
  protected AvatarMeshPartSO[] _facialHair;
  [SerializeField]
  protected AvatarMeshPartSO[] _hands;
  [SerializeField]
  protected AvatarMeshPartSO[] _clothes;

  public AvatarMeshPartSO[] headTops => this._headTops;

  public AvatarSpritePartSO[] Eyes => this._eyes;

  public AvatarSpritePartSO[] Mouths => this._mouths;

  public AvatarMeshPartSO[] Glasses => this._glasses;

  public AvatarMeshPartSO[] FacialHair => this._facialHair;

  public AvatarMeshPartSO[] Hands => this._hands;

  public AvatarMeshPartSO[] Clothes => this._clothes;
}
