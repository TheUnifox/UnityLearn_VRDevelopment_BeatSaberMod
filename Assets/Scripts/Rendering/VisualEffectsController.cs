// Decompiled with JetBrains decompiler
// Type: VisualEffectsController
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
public class VisualEffectsController : MonoBehaviour
{
  [SerializeField]
  private BoolSO _depthTextureEnabled;
  private Camera _camera;
  private const string kDepthTextureEnabledKeyword = "DEPTH_TEXTURE_ENABLED";

  protected void Awake()
  {
    this._camera = this.GetComponent<Camera>();
    this.HandleDepthTextureEnabledDidChange();
    this._depthTextureEnabled.didChangeEvent += new Action(this.HandleDepthTextureEnabledDidChange);
  }

  protected void OnDestroy()
  {
    if (!((UnityEngine.Object) this._depthTextureEnabled != (UnityEngine.Object) null))
      return;
    this._depthTextureEnabled.didChangeEvent -= new Action(this.HandleDepthTextureEnabledDidChange);
  }

  protected void OnPreRender() => this.SetShaderKeyword("DEPTH_TEXTURE_ENABLED", (bool) (ObservableVariableSO<bool>) this._depthTextureEnabled);

  private void HandleDepthTextureEnabledDidChange() => this._camera.depthTextureMode = (bool) (ObservableVariableSO<bool>) this._depthTextureEnabled ? DepthTextureMode.Depth : DepthTextureMode.None;

  private void SetShaderKeyword(string keyword, bool value)
  {
    if (value)
      Shader.EnableKeyword(keyword);
    else
      Shader.DisableKeyword(keyword);
  }
}
