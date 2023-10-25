// Decompiled with JetBrains decompiler
// Type: CubemapTest
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.Rendering;

public class CubemapTest : MonoBehaviour
{
  [SerializeField]
  protected Camera _camera;
  [SerializeField]
  protected Material _cubemapMaterial;
  [SerializeField]
  protected Material _flatMaterial;
  protected RenderTexture _cubemapRenderTexture;
  protected RenderTexture _downsampledCubemapRenderTexture;
  protected RenderTexture _cubemapFlatTexture;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _cubePropertyId = Shader.PropertyToID("_Cube");

  public virtual void Start()
  {
    this._cubemapRenderTexture = new RenderTexture(512, 512, 32);
    this._cubemapRenderTexture.dimension = TextureDimension.Cube;
  }

  public virtual void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      if ((Object) this._downsampledCubemapRenderTexture != (Object) null)
      {
        Object.Destroy((Object) this._downsampledCubemapRenderTexture);
        this._downsampledCubemapRenderTexture = (RenderTexture) null;
      }
      this._downsampledCubemapRenderTexture = CubemapHelpers.CreateDownsampledCubemap(this._cubemapRenderTexture, 2);
      this._cubemapMaterial.SetTexture(CubemapTest._cubePropertyId, (Texture) this._downsampledCubemapRenderTexture);
      if ((Object) this._cubemapFlatTexture != (Object) null)
      {
        Object.Destroy((Object) this._cubemapFlatTexture);
        this._cubemapFlatTexture = (RenderTexture) null;
      }
      this._cubemapFlatTexture = CubemapHelpers.Create2DTextureFromCubemap(this._cubemapRenderTexture);
      this._flatMaterial.mainTexture = (Texture) this._cubemapFlatTexture;
    }
    if (!Input.GetKeyDown(KeyCode.A))
      return;
    this._camera.RenderToCubemap(this._cubemapRenderTexture);
    this._cubemapMaterial.SetTexture(CubemapTest._cubePropertyId, (Texture) this._cubemapRenderTexture);
  }
}
