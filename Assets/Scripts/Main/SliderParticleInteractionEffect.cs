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
        ParticleSystem[] particleSystems = this._particleSystems;
        ParticleSystem.MainModule main;
        for (int i = 0; i < particleSystems.Length; i++)
        {
            main = particleSystems[i].main;
            main.startColor = color;
        }
    }

  protected override void StartEffect(float saberInteractionParam)
  {
        ParticleSystem[] particleSystems = this._particleSystems;
        ParticleSystem.EmissionModule emission;
        for (int i = 0; i < particleSystems.Length; i++)
        {
            emission = particleSystems[i].emission;
            emission.enabled = true;
        }
        this.SetPSStartColor(this._startColor * saberInteractionParam);
        base.enabled = true;
    }

  protected override void EndEffect()
  {
        ParticleSystem[] particleSystems = this._particleSystems;
        ParticleSystem.EmissionModule emission;
        for (int i = 0; i < particleSystems.Length; i++)
        {
            emission = particleSystems[i].emission;
            emission.enabled = false;
        }
        base.enabled = false;
    }
}
