// Decompiled with JetBrains decompiler
// Type: AudioWaveformController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

public class AudioWaveformController : MonoBehaviour
{
  [SerializeField]
  private ComputeShader _computeShader;
  [Header("Colors")]
  [SerializeField]
  private Color _rmsColor = Color.red;
  [SerializeField]
  private Color _minMaxColor = Color.blue;
  [SerializeField]
  private Color _emptyColor = Color.clear;
  [Header("Settings")]
  [SerializeField]
  private int _thickness = 5;
  [DoesNotRequireDomainReloadInit]
  private static readonly int _outputTextureId = Shader.PropertyToID("_OutputTexture");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _monoSamplesId = Shader.PropertyToID("_MonoSamples");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _monoSamplesLengthId = Shader.PropertyToID("_MonoSamplesLength");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _sampleBracketsId = Shader.PropertyToID("_SampleBracketsCount");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _textureResolutionId = Shader.PropertyToID("_TextureResolution");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _halfThicknessId = Shader.PropertyToID("_HalfThickness");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _emptyColorId = Shader.PropertyToID("_EmptyColor");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _minMaxColorId = Shader.PropertyToID("_MinMaxColor");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _rmsColorId = Shader.PropertyToID("_RMSColor");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _startSampleId = Shader.PropertyToID("_StartSample");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _endSampleId = Shader.PropertyToID("_EndSample");
  private ComputeShader _computeShaderInstance;
  private RenderTexture _renderTexture;
  private int _frequency;
  private Vector4 _renderTextureSize;
  private int _monoSamplesCount;
  private int _waveformTextureFrequencyLimitKernelId;
  private int _waveformTextureFullKernelId;
  private uint _kernelXThreadGroup;
  private uint _kernelYThreadGroup;
  private uint _kernelZThreadGroup;

  public void Setup(RenderTexture renderTexture, ComputeBuffer computeBuffer, int frequency)
  {
    this._frequency = frequency;
    this._monoSamplesCount = computeBuffer.count;
    this._computeShaderInstance = Object.Instantiate<ComputeShader>(this._computeShader);
    this._renderTexture = renderTexture;
    this._renderTextureSize = new Vector4((float) this._renderTexture.width, (float) this._renderTexture.height, 1f / (float) this._renderTexture.width, 1f / (float) this._renderTexture.height);
    this._waveformTextureFrequencyLimitKernelId = this._computeShaderInstance.FindKernel("WaveformTextureFrequencyLimit");
    this._waveformTextureFullKernelId = this._computeShaderInstance.FindKernel("WaveformTextureFull");
    this._computeShaderInstance.GetKernelThreadGroupSizes(this._waveformTextureFullKernelId, out this._kernelXThreadGroup, out this._kernelYThreadGroup, out this._kernelZThreadGroup);
    this._computeShaderInstance.SetVector(AudioWaveformController._textureResolutionId, this._renderTextureSize);
    this._computeShaderInstance.SetFloat(AudioWaveformController._halfThicknessId, (float) ((double) this._thickness * (double) this._renderTextureSize.z * 0.5));
    this._computeShaderInstance.SetVector(AudioWaveformController._emptyColorId, new Vector4(this._emptyColor.r, this._emptyColor.g, this._emptyColor.b, this._emptyColor.a));
    this._computeShaderInstance.SetVector(AudioWaveformController._minMaxColorId, new Vector4(this._minMaxColor.r, this._minMaxColor.g, this._minMaxColor.b, this._minMaxColor.a));
    this._computeShaderInstance.SetVector(AudioWaveformController._rmsColorId, new Vector4(this._rmsColor.r, this._rmsColor.g, this._rmsColor.b, this._rmsColor.a));
    this._computeShaderInstance.SetInt(AudioWaveformController._monoSamplesLengthId, this._monoSamplesCount);
    this._computeShaderInstance.SetBuffer(this._waveformTextureFrequencyLimitKernelId, AudioWaveformController._monoSamplesId, computeBuffer);
    this._computeShaderInstance.SetBuffer(this._waveformTextureFullKernelId, AudioWaveformController._monoSamplesId, computeBuffer);
    this._computeShaderInstance.SetTexture(this._waveformTextureFrequencyLimitKernelId, AudioWaveformController._outputTextureId, (Texture) this._renderTexture);
    this._computeShaderInstance.SetTexture(this._waveformTextureFullKernelId, AudioWaveformController._outputTextureId, (Texture) this._renderTexture);
  }

  public void RenderWaveform(int lodStartIndex, int lodEndIndex, int bracketSize, int length)
  {
    this._computeShaderInstance.SetInt(AudioWaveformController._startSampleId, lodStartIndex);
    this._computeShaderInstance.SetInt(AudioWaveformController._endSampleId, lodEndIndex);
    this._computeShaderInstance.SetInt(AudioWaveformController._sampleBracketsId, bracketSize);
    int kernelIndex = this._waveformTextureFullKernelId;
    if (length <= this._frequency)
      kernelIndex = this._waveformTextureFrequencyLimitKernelId;
    this._computeShaderInstance.Dispatch(kernelIndex, (int) ((long) this._renderTexture.width / (long) this._kernelXThreadGroup), (int) this._kernelYThreadGroup, (int) this._kernelZThreadGroup);
  }

  protected void OnDestroy() => Object.Destroy((Object) this._computeShaderInstance);
}
