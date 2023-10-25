// Decompiled with JetBrains decompiler
// Type: SliderParticleInteractionEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SliderParticleInteractionEffect : SliderInteractionEffect
{
  [SerializeField]
  protected ParticleSystem[] _particleSystems;
  [Inject]
  protected readonly ColorManager _colorManager;
  [Inject]
  protected readonly SaberManager _saberManager;
  protected Color _startColor;
  protected Saber _saber;

  protected override void Start()
  {
    base.Start();
    this._startColor = this._colorManager.ColorForType(this.colorType);
    this._saber = this._saberManager.SaberForType(this.colorType.ToSaberType());
    this.SetPSStartColor(Color.clear);
    this.enabled = false;
  }

  public virtual void Update()
  {
    this.transform.localPosition = SliderController.GetSaberInteractionPoint(this._saber);
    this.SetPSStartColor(this._startColor * this.saberInteractionParam);
  }

  public virtual void SetPSStartColor(Color color)
  {
    foreach (ParticleSystem particleSystem in this._particleSystems)
      particleSystem.main.startColor = (ParticleSystem.MinMaxGradient) color;
  }

  protected override void StartEffect(float saberInteractionParam)
  {
    foreach (ParticleSystem particleSystem in this._particleSystems)
      particleSystem.emission.enabled = true;
    this.SetPSStartColor(this._startColor * saberInteractionParam);
    this.enabled = true;
  }

  protected override void EndEffect()
  {
    foreach (ParticleSystem particleSystem in this._particleSystems)
      particleSystem.emission.enabled = false;
    this.enabled = false;
  }
}
