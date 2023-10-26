// Decompiled with JetBrains decompiler
// Type: SimpleTemporalFiltering
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;

public class SimpleTemporalFiltering
{
  private RenderTexture[] _temporalFilteringTextures;
  private int _prevTemporalFilteringTextureIdx = -1;
  private Material _temporalFilteringMaterial;
  private int _bufferTexID;

  public SimpleTemporalFiltering()
  {
    this._temporalFilteringMaterial = new Material(Shader.Find("Hidden/TemporalFiltering"));
    this._bufferTexID = Shader.PropertyToID("_BufferTex");
  }

  public RenderTexture FilterTexture(RenderTexture src)
  {
    this.CreateRenderTexturesIfNeeded(src.width, src.height);
    RenderTexture filteringTexture;
    if (this._prevTemporalFilteringTextureIdx >= 0)
    {
      this._temporalFilteringMaterial.SetTexture(this._bufferTexID, (Texture) this._temporalFilteringTextures[this._prevTemporalFilteringTextureIdx]);
      filteringTexture = this._temporalFilteringTextures[(this._prevTemporalFilteringTextureIdx + 1) % 2];
      Graphics.Blit((Texture) src, filteringTexture, this._temporalFilteringMaterial);
    }
    else
    {
      filteringTexture = this._temporalFilteringTextures[0];
      Graphics.Blit((Texture) src, filteringTexture);
    }
    this._prevTemporalFilteringTextureIdx = (this._prevTemporalFilteringTextureIdx + 1) % 2;
    return filteringTexture;
  }

  private void CreateRenderTexturesIfNeeded(int width, int height)
  {
    if (this._temporalFilteringTextures == null)
      this._temporalFilteringTextures = new RenderTexture[2];
    for (int index = 0; index < 2; ++index)
    {
      if ((Object) this._temporalFilteringTextures[index] == (Object) null || this._temporalFilteringTextures[index].width != width || this._temporalFilteringTextures[index].height != height)
      {
        if ((Object) this._temporalFilteringTextures[index] != (Object) null)
          this._temporalFilteringTextures[index].Release();
        this._temporalFilteringTextures[index] = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        this._temporalFilteringTextures[index].wrapMode = TextureWrapMode.Mirror;
        this._temporalFilteringTextures[index].Create();
      }
    }
  }
}
