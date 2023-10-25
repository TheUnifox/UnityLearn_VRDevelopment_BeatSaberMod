// Decompiled with JetBrains decompiler
// Type: BloomPrePassBackgroundParticleSystemRenderer
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[ExecuteAlways]
public class BloomPrePassBackgroundParticleSystemRenderer : 
  BloomPrePassBackgroundNonLightRendererCore
{
  [SerializeField]
  protected ParticleSystem _particleSystem;
  protected Renderer _renderer;

  public override Renderer renderer => this._renderer;

  protected override void Awake()
  {
    this._renderer = (Renderer) this._particleSystem.GetComponent<ParticleSystemRenderer>();
    base.Awake();
  }
}
