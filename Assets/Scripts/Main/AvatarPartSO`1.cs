// Decompiled with JetBrains decompiler
// Type: AvatarPartSO`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using UnityEngine;

public class AvatarPartSO<T> : PersistentScriptableObject, IAvatarPart
{
  [SerializeField]
  protected string _id;
  [SerializeField]
  [LocalizationKey]
  protected string _localizationKey;
  [SerializeField]
  [NullAllowed]
  protected T _partAsset;

  public T partAsset => this._partAsset;

  public string id => this._id;

  public string localizationKey => this._localizationKey;

  public string localizedName => Localization.Get(this._localizationKey);
}
