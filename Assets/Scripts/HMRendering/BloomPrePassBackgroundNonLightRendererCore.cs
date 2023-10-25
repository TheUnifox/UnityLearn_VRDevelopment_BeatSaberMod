// Decompiled with JetBrains decompiler
// Type: BloomPrePassBackgroundNonLightRendererCore
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public abstract class BloomPrePassBackgroundNonLightRendererCore : BloomPrePassNonLightPass
{
  [SerializeField]
  protected bool _keepDefaultRendering;
  [SerializeField]
  private bool _useCustomMaterial;
  [SerializeField]
  [DrawIf("_useCustomMaterial", true, DrawIfAttribute.DisablingType.DontDraw)]
  [NullAllowed]
  private Material _customMaterial;
  [DoesNotRequireDomainReloadInit]
  private static readonly int _worldSpaceCameraPosID = Shader.PropertyToID("_WorldSpaceCameraPos");
  private CommandBuffer _commandBuffer;

  public new abstract Renderer renderer { get; }

  protected virtual void InitIfNeeded()
  {
    if (!this._keepDefaultRendering)
      this.renderer.enabled = false;
    if (this._commandBuffer != null)
      return;
    this._commandBuffer = new CommandBuffer()
    {
      name = "BloomPrePassBackgroundNonLightRenderer"
    };
  }

  protected virtual void Awake() => this.InitIfNeeded();

  public override void Render(RenderTexture dest, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix)
  {
    this._commandBuffer.Clear();
    this._commandBuffer.SetRenderTarget((RenderTargetIdentifier) (Texture) dest);
    this._commandBuffer.SetViewProjectionMatrices(viewMatrix, projectionMatrix);
    this._commandBuffer.SetGlobalVector(BloomPrePassBackgroundNonLightRendererCore._worldSpaceCameraPosID, viewMatrix.GetColumn(3));
    this._commandBuffer.DrawRenderer(this.renderer, !this._useCustomMaterial || !(bool) (Object) this._customMaterial ? this.renderer.sharedMaterial : this._customMaterial, 0, 0);
    Graphics.ExecuteCommandBuffer(this._commandBuffer);
  }
}
