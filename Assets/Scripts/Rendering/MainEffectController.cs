// Decompiled with JetBrains decompiler
// Type: MainEffectController
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class MainEffectController : MonoBehaviour
{
  [SerializeField]
  private MainEffectContainerSO _mainEffectContainer;
  [SerializeField]
  private FloatSO _fadeValue;
  private ImageEffectController _imageEffectController;
  private const string kMainEffectEnabledKeyword = "MAIN_EFFECT_ENABLED";

  public event Action<RenderTexture> afterImageEffectEvent;

  protected void OnEnable()
  {
    this.LazySetupImageEffectController();
    this._imageEffectController.enabled = true;
  }

  protected void OnDisable()
  {
    if (!((UnityEngine.Object) this._imageEffectController != (UnityEngine.Object) null))
      return;
    this._imageEffectController.enabled = false;
  }

  protected void OnValidate()
  {
    if (!this.gameObject.scene.IsValid())
      return;
    this.LazySetupImageEffectController();
  }

  private void LazySetupImageEffectController()
  {
    if ((UnityEngine.Object) this._imageEffectController != (UnityEngine.Object) null)
      return;
    this._imageEffectController = this.GetComponent<ImageEffectController>();
    if ((UnityEngine.Object) this._imageEffectController != (UnityEngine.Object) null)
    {
      this._imageEffectController.SetCallback(new ImageEffectController.RenderImageCallback(this.ImageEffectControllerCallback));
    }
    else
    {
      this._imageEffectController = this.gameObject.AddComponent<ImageEffectController>();
      this._imageEffectController.hideFlags = HideFlags.HideAndDontSave;
      this._imageEffectController.SetCallback(new ImageEffectController.RenderImageCallback(this.ImageEffectControllerCallback));
    }
  }

  private void ImageEffectControllerCallback(RenderTexture src, RenderTexture dest)
  {
    this._mainEffectContainer.mainEffect.Render(src, dest, (float) (ObservableVariableSO<float>) this._fadeValue);
    Action<RenderTexture> imageEffectEvent = this.afterImageEffectEvent;
    if (imageEffectEvent == null)
      return;
    imageEffectEvent(dest);
  }

  protected void OnPreRender()
  {
    this._mainEffectContainer.mainEffect.PreRender();
    if (this._mainEffectContainer.mainEffect.hasPostProcessEffect)
    {
      Shader.EnableKeyword("MAIN_EFFECT_ENABLED");
      this._imageEffectController.enabled = true;
    }
    else
    {
      Shader.DisableKeyword("MAIN_EFFECT_ENABLED");
      this._imageEffectController.enabled = false;
    }
  }

  protected void OnPostRender() => this._mainEffectContainer.mainEffect.PostRender((float) (ObservableVariableSO<float>) this._fadeValue);
}
