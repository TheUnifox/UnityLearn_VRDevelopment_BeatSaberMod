// Decompiled with JetBrains decompiler
// Type: BloomPrePassDoubleKawaseBlurTextureEffectSO
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;

public class BloomPrePassDoubleKawaseBlurTextureEffectSO : BloomPrePassEffectSO
{
  [Space]
  [SerializeField]
  private KawaseBlurRendererSO.KernelSize _bloom1KernelSize = KawaseBlurRendererSO.KernelSize.Kernel63;
  [SerializeField]
  private float _bloom1Boost = 0.02f;
  [SerializeField]
  private KawaseBlurRendererSO.KernelSize _bloom2KernelSize = KawaseBlurRendererSO.KernelSize.Kernel15;
  [SerializeField]
  private float _bloom2Boost;
  [SerializeField]
  private float _bloom2Alpha = 1f;
  [SerializeField]
  private int _downsample;
  [SerializeField]
  private bool _gammaCorrection = true;
  [Space]
  [SerializeField]
  private KawaseBlurRendererSO _kawaseBlurRenderer;

  public override void Render(RenderTexture src, RenderTexture dest) => this._kawaseBlurRenderer.DoubleBlur(src, dest, this._bloom1KernelSize, this._bloom1Boost, this._bloom2KernelSize, this._bloom2Boost, this._bloom2Alpha, this._downsample, this._gammaCorrection);
}
