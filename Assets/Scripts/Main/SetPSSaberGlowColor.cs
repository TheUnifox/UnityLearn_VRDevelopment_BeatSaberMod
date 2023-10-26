// Decompiled with JetBrains decompiler
// Type: SetPSSaberGlowColor
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SetPSSaberGlowColor : MonoBehaviour
{
  [SerializeField]
  protected SaberTypeObject _saber;
  [SerializeField]
  protected ColorManager _colorManager;
  [SerializeField]
  protected ParticleSystem _particleSystem;

    public virtual void Start()
    {
        ParticleSystem.MainModule main = this._particleSystem.main;
        main.startColor = (ParticleSystem.MinMaxGradient)this._colorManager.ColorForSaberType(this._saber.saberType);
    }
}
