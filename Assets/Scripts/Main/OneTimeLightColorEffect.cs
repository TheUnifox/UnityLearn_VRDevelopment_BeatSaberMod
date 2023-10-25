// Decompiled with JetBrains decompiler
// Type: OneTimeLightColorEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class OneTimeLightColorEffect : MonoBehaviour
{
  [SerializeField]
  protected ColorSO _color;
  [SerializeField]
  protected float _alpha = 1f;
  [SerializeField]
  protected int _lightsId;
  [Inject]
  protected LightWithIdManager _lightWithIdManager;

  public virtual void Update()
  {
    this._lightWithIdManager.SetColorForId(this._lightsId, this._color.color.ColorWithAlpha(this._alpha));
    this.enabled = false;
  }
}
