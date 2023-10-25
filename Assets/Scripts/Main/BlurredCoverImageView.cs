// Decompiled with JetBrains decompiler
// Type: BlurredCoverImageView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using System.Threading;
using UnityEngine;

public class BlurredCoverImageView : MonoBehaviour
{
  [SerializeField]
  protected ImageView _coverImage;
  [Space]
  [SerializeField]
  protected KawaseBlurRendererSO _kawaseBlurRenderer;
  protected string _settingTextureForLevelId;
  protected CancellationTokenSource _cancellationTokenSource;
  protected Texture2D _blurredCoverTexture;

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._blurredCoverTexture != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this._blurredCoverTexture);
      this._blurredCoverTexture = (Texture2D) null;
    }
    this._cancellationTokenSource?.Cancel();
  }

  public virtual async void SetTextureAsync(IPreviewBeatmapLevel level)
  {
    if (this._settingTextureForLevelId == level.levelID)
      return;
    try
    {
      this._settingTextureForLevelId = level.levelID;
      this._cancellationTokenSource?.Cancel();
      this._cancellationTokenSource = new CancellationTokenSource();
      this._coverImage.enabled = false;
      this._coverImage.sprite = (Sprite) null;
      CancellationToken cancellationToken = this._cancellationTokenSource.Token;
      Sprite coverImageAsync = await level.GetCoverImageAsync(cancellationToken);
      cancellationToken.ThrowIfCancellationRequested();
      if ((UnityEngine.Object) this._blurredCoverTexture != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this._blurredCoverTexture);
      this._blurredCoverTexture = this._kawaseBlurRenderer.Blur((Texture) coverImageAsync.texture, KawaseBlurRendererSO.KernelSize.Kernel7);
      Sprite sprite = Sprite.Create(this._blurredCoverTexture, new Rect(0.0f, 0.0f, (float) this._blurredCoverTexture.width, (float) this._blurredCoverTexture.height), new Vector2(0.5f, 0.5f), 256f, 0U, SpriteMeshType.FullRect, new Vector4(0.0f, 0.0f, 0.0f, 0.0f), false);
      if ((UnityEngine.Object) this._coverImage.sprite != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this._coverImage.sprite);
      this._coverImage.sprite = sprite;
      this._coverImage.enabled = true;
      cancellationToken = new CancellationToken();
    }
    catch (OperationCanceledException ex)
    {
    }
    finally
    {
      if (this._settingTextureForLevelId == level.levelID)
        this._settingTextureForLevelId = (string) null;
    }
  }
}
