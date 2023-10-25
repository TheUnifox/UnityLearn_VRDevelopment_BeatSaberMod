// Decompiled with JetBrains decompiler
// Type: Spectrogram
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class Spectrogram : MonoBehaviour
{
  [SerializeField]
  protected MeshRenderer[] _meshRenderers;
  [Space]
  [SerializeField]
  [NullAllowed]
  protected MaterialPropertyBlockController _materialPropertyBlockController;
  [Inject]
  protected readonly BasicSpectrogramData _spectrogramData;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _spectrogramDataID = Shader.PropertyToID("_SpectrogramData");
  protected static MaterialPropertyBlock _materialPropertyBlock;

  private MaterialPropertyBlock materialPropertyBlock => !((Object) this._materialPropertyBlockController != (Object) null) ? Spectrogram._materialPropertyBlock : this._materialPropertyBlockController.materialPropertyBlock;

  public virtual void Awake()
  {
    if (!((Object) this._materialPropertyBlockController == (Object) null) || Spectrogram._materialPropertyBlock != null)
      return;
    Spectrogram._materialPropertyBlock = new MaterialPropertyBlock();
  }

  public virtual void Update()
  {
    this.materialPropertyBlock.SetFloatArray(Spectrogram._spectrogramDataID, this._spectrogramData.ProcessedSamples);
    if ((Object) this._materialPropertyBlockController != (Object) null)
    {
      this._materialPropertyBlockController.ApplyChanges();
    }
    else
    {
      foreach (Renderer meshRenderer in this._meshRenderers)
        meshRenderer.SetPropertyBlock(Spectrogram._materialPropertyBlock);
    }
  }
}
