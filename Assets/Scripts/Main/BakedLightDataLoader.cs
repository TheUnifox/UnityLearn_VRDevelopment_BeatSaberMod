// Decompiled with JetBrains decompiler
// Type: BakedLightDataLoader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteAlways]
public class BakedLightDataLoader : MonoBehaviour
{
  [NullAllowed(NullAllowed.Context.Prefab)]
  [SerializeField]
  protected LightmapDataSO _lightmapData;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _lightMap1PropertyId = Shader.PropertyToID("_LightMap1");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _lightMap2PropertyId = Shader.PropertyToID("_LightMap2");
  protected Texture2D _blackTexture;

  public LightmapDataSO lightmapData
  {
    get => this._lightmapData;
    set => this._lightmapData = value;
  }

  public virtual void Start()
  {
    if ((Object) this._lightmapData != (Object) null)
      this.SetTextureDataToShaders();
    else
      Debug.LogWarning((object) "Lightmap data not set");
  }

  public virtual void SetTextureDataToShaders()
  {
    Shader.SetGlobalTexture(BakedLightDataLoader._lightMap1PropertyId, (Object) this._lightmapData.lightmap1 != (Object) null ? (Texture) this._lightmapData.lightmap1 : (Texture) this._blackTexture);
    Shader.SetGlobalTexture(BakedLightDataLoader._lightMap2PropertyId, (Object) this._lightmapData.lightmap2 != (Object) null ? (Texture) this._lightmapData.lightmap2 : (Texture) this._blackTexture);
  }
}
