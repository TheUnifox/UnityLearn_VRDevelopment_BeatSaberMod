// Decompiled with JetBrains decompiler
// Type: BloomPrePass
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof (Camera))]
public class BloomPrePass : MonoBehaviour
{
  [SerializeField]
  protected BloomPrePassRendererSO _bloomPrepassRenderer;
  [SerializeField]
  protected BloomPrePassEffectContainerSO _bloomPrePassEffectContainer;
  [Tooltip("This is used to share same render data with two BloomPrePass objects. We need this for efficient implementation of mixed reality background and foreground camera. Null is allowed.")]
  [SerializeField]
  [NullAllowed]
  protected BloomPrePassRenderDataSO _bloomPrePassRenderData;
  [SerializeField]
  protected BloomPrePass.Mode _mode;
  protected BloomPrePassRenderDataSO.Data _renderData;

  public virtual void Awake() => this.LazyInit();

  public virtual void LazyInit()
  {
    if (this._renderData != null)
      return;
    this._renderData = (Object) this._bloomPrePassRenderData == (Object) null ? new BloomPrePassRenderDataSO.Data() : this._bloomPrePassRenderData.data;
    this._bloomPrepassRenderer.Init();
  }

  public virtual void OnDestroy()
  {
    if (this._renderData == null || (Object) this._renderData.bloomPrePassRenderTexture == (Object) null)
      return;
    this._renderData.bloomPrePassRenderTexture.Release();
    EssentialHelpers.SafeDestroy((Object) this._renderData.bloomPrePassRenderTexture);
    this._renderData.bloomPrePassRenderTexture = (RenderTexture) null;
  }

  public virtual void OnPreRender()
  {
    if (this._mode == BloomPrePass.Mode.RenderAndSetData)
    {
      Camera current = Camera.current;
      this._bloomPrepassRenderer.GetCameraParams(current, out this._renderData.projectionMatrix, out this._renderData.viewMatrix, out this._renderData.stereoCameraEyeOffset);
      this._renderData.bloomPrePassRenderTexture = this._bloomPrepassRenderer.CreateBloomPrePassRenderTextureIfNeeded(this._renderData.bloomPrePassRenderTexture, (IBloomPrePassParams) this._bloomPrePassEffectContainer.bloomPrePassEffect);
      this._bloomPrepassRenderer.RenderAndSetData(current.transform.position, this._renderData.projectionMatrix, this._renderData.viewMatrix, this._renderData.stereoCameraEyeOffset, (IBloomPrePassParams) this._bloomPrePassEffectContainer.bloomPrePassEffect, this._renderData.bloomPrePassRenderTexture, out this._renderData.textureToScreenRatio, out this._renderData.toneMapping);
      this._bloomPrepassRenderer.EnableBloomFog();
      BloomPrePassRendererSO.SetDataToShaders(this._renderData.stereoCameraEyeOffset, this._renderData.textureToScreenRatio, (Texture) this._renderData.bloomPrePassRenderTexture, this._renderData.toneMapping);
    }
    else
    {
      if (!((Object) this._renderData.bloomPrePassRenderTexture != (Object) null))
        return;
      this._bloomPrepassRenderer.EnableBloomFog();
      BloomPrePassRendererSO.SetDataToShaders(this._renderData.stereoCameraEyeOffset, this._renderData.textureToScreenRatio, (Texture) this._renderData.bloomPrePassRenderTexture, this._renderData.toneMapping);
    }
  }

  public virtual void OnPostRender()
  {
    if ((Object) this._renderData.bloomPrePassRenderTexture != (Object) null)
      this._renderData.bloomPrePassRenderTexture.DiscardContents();
    this._bloomPrepassRenderer.DisableBloomFog();
  }

  public virtual void SetMode(BloomPrePass.Mode mode) => this._mode = mode;

  public enum Mode
  {
    RenderAndSetData,
    SetDataOnly,
  }
}
