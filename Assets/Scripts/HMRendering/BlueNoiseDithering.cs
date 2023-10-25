// Decompiled with JetBrains decompiler
// Type: BlueNoiseDithering
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class BlueNoiseDithering : PersistentScriptableObject
{
  [SerializeField]
  protected Texture2D _noiseTexture;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _noiseParamsID = Shader.PropertyToID("_GlobalBlueNoiseParams");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _globalNoiseTextureID = Shader.PropertyToID("_GlobalBlueNoiseTex");

  public virtual void SetBlueNoiseShaderParams(int cameraPixelWidth, int cameraPixelHeight)
  {
    Shader.SetGlobalVector(BlueNoiseDithering._noiseParamsID, new Vector4((float) cameraPixelWidth / (float) this._noiseTexture.width, (float) cameraPixelHeight / (float) this._noiseTexture.height, 0.0f, 0.0f));
    Shader.SetGlobalTexture(BlueNoiseDithering._globalNoiseTextureID, (Texture) this._noiseTexture);
  }
}
