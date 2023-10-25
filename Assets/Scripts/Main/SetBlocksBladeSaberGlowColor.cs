// Decompiled with JetBrains decompiler
// Type: SetBlocksBladeSaberGlowColor
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SetBlocksBladeSaberGlowColor : MonoBehaviour
{
  [SerializeField]
  protected SaberTypeObject _saber;
  [SerializeField]
  protected ColorManager _colorManager;
  [SerializeField]
  protected BlocksBlade _blocksBlade;

  public virtual void Start() => this._blocksBlade.color = this._colorManager.ColorForSaberType(this._saber.saberType);
}
