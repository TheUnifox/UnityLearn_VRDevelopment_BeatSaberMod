// Decompiled with JetBrains decompiler
// Type: Mirror
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Renderer))]
public class Mirror : MonoBehaviour
{
  [SerializeField]
  private MirrorRendererSO _mirrorRenderer;
  [SerializeField]
  private MeshRenderer _renderer;
  [SerializeField]
  private Material _mirrorMaterial;
  [SerializeField]
  private Material _noMirrorMaterial;
  [DoesNotRequireDomainReloadInit]
  private static readonly int _texturePropertyID = Shader.PropertyToID("_ReflectionTex");

  public Material mirrorMaterial => this._mirrorMaterial;

  public Material noMirrorMaterial => this._noMirrorMaterial;

  public bool isEnabled { get; private set; }

  public event Action<bool> mirrorDidChangeEnabledStateEvent;

  protected void Update() => this._mirrorRenderer.PrepareForNextFrame();

  protected void OnWillRenderObject()
  {
    if (!this.enabled || !(bool) (UnityEngine.Object) this._renderer || !this._renderer.enabled || (UnityEngine.Object) this._mirrorRenderer == (UnityEngine.Object) null)
    {
      this.ChangeMirrorEnabledState(false);
    }
    else
    {
      Vector3 position = this.transform.position;
      Vector3 up = this.transform.up;
      Texture mirrorTexture = this._mirrorRenderer.GetMirrorTexture(position - up * (1f / 1000f), up);
      if ((UnityEngine.Object) mirrorTexture == (UnityEngine.Object) null)
      {
        this._renderer.sharedMaterial = this._noMirrorMaterial;
        this.ChangeMirrorEnabledState(false);
      }
      else
      {
        this._renderer.sharedMaterial = this._mirrorMaterial;
        this._renderer.sharedMaterial.SetTexture(Mirror._texturePropertyID, mirrorTexture);
        this.ChangeMirrorEnabledState(true);
      }
    }
  }

  private void ChangeMirrorEnabledState(bool newIsEnabled)
  {
    if (this.isEnabled == newIsEnabled)
      return;
    this.isEnabled = newIsEnabled;
    Action<bool> enabledStateEvent = this.mirrorDidChangeEnabledStateEvent;
    if (enabledStateEvent == null)
      return;
    enabledStateEvent(newIsEnabled);
  }
}
