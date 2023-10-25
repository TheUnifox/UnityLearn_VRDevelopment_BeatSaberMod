// Decompiled with JetBrains decompiler
// Type: SpectrogramRow
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SpectrogramRow : MonoBehaviour
{
  [SerializeField]
  protected MeshRenderer[] _meshRenderers;
  [SerializeField]
  protected int _dataIndex;
  [Inject]
  protected readonly BasicSpectrogramData _spectrogramData;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _spectrogramDataID = Shader.PropertyToID("_SpectrogramData");
  protected static MaterialPropertyBlock _materialPropertyBlock;

  public virtual void Awake()
  {
    if (SpectrogramRow._materialPropertyBlock != null)
      return;
    SpectrogramRow._materialPropertyBlock = new MaterialPropertyBlock();
  }

  public virtual void Update()
  {
    float processedSample = this._spectrogramData.ProcessedSamples[this._dataIndex];
    SpectrogramRow._materialPropertyBlock.SetFloat(SpectrogramRow._spectrogramDataID, processedSample);
    foreach (Renderer meshRenderer in this._meshRenderers)
      meshRenderer.SetPropertyBlock(SpectrogramRow._materialPropertyBlock);
  }
}
