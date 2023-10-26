// Decompiled with JetBrains decompiler
// Type: RenderTextureFromPostEffect
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;

public class RenderTextureFromPostEffect : MonoBehaviour
{
  public RenderTexture _targetTexture;
  private Camera _camera;

  public RenderTexture targetTexture => this._targetTexture;

  protected void Awake() => this._camera = this.GetComponent<Camera>();

  private void OnRenderImage(RenderTexture src, RenderTexture dst)
  {
    if ((Object) this._targetTexture != (Object) null && (this._targetTexture.width != this._camera.pixelWidth * 2 || this._targetTexture.height != this._camera.pixelHeight))
    {
      Object.Destroy((Object) this._targetTexture);
      this._targetTexture = (RenderTexture) null;
    }
    if ((Object) this._targetTexture == (Object) null)
    {
      this._targetTexture = new RenderTexture(this._camera.pixelWidth * 2, this._camera.pixelHeight, 24);
      this._targetTexture.hideFlags = HideFlags.DontSave;
    }
    Graphics.Blit((Texture) src, this._targetTexture);
    Graphics.Blit((Texture) src, dst);
  }
}
