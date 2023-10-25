// Decompiled with JetBrains decompiler
// Type: SongTimeSyncedVideoPlayer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Video;
using Zenject;

public class SongTimeSyncedVideoPlayer : LightWithIdMonoBehaviour
{
  [SerializeField]
  protected VideoPlayer _videoPlayer;
  [Space]
  [SerializeField]
  protected MaterialPropertyBlockController _materialPropertyBlockController;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  [Inject]
  protected readonly EnvironmentContext _environmentContext;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _colorId = Shader.PropertyToID("_Color");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _textureId = Shader.PropertyToID("_MainTex");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _textureStId = Shader.PropertyToID("_MainTex_ST");
  protected Color _color = Color.white;
  protected Coroutine _waitForDependenciesAndPlayCoroutine;

  protected override void Start()
  {
    base.Start();
    this._videoPlayer.timeReference = VideoTimeReference.Freerun;
    this._videoPlayer.playbackSpeed = 0.0f;
    this._waitForDependenciesAndPlayCoroutine = this.StartCoroutine(this.WaitForDependenciesAndPlay());
  }

  public virtual void OnDestroy() => this.StopVideoPlayer();

  public virtual void Update()
  {
    if (!this._videoPlayer.isPlaying)
      return;
    this._videoPlayer.frame = (long) Mathf.RoundToInt(Mathf.Clamp(this._audioTimeSource.songTime * this._videoPlayer.frameRate, 0.0f, (float) this._videoPlayer.frameCount));
  }

  public virtual void LateUpdate()
  {
    if (this._videoPlayer.isPlaying && (UnityEngine.Object) this._videoPlayer.texture != (UnityEngine.Object) null)
      this._materialPropertyBlockController.materialPropertyBlock.SetTexture(SongTimeSyncedVideoPlayer._textureId, this._videoPlayer.texture);
    this._materialPropertyBlockController.materialPropertyBlock.SetColor(SongTimeSyncedVideoPlayer._colorId, this._color);
    this._materialPropertyBlockController.ApplyChanges();
  }

  public override void ColorWasSet(Color color) => this._color.a = color.a;

  public virtual void SetVideoClip(VideoClip videoClip)
  {
    int environmentContext = (int) this._environmentContext;
    this._videoPlayer.clip = videoClip;
  }

  public virtual void SetSpriteAndStopVideo(Sprite sprite)
  {
    this._materialPropertyBlockController.materialPropertyBlock.SetTexture(SongTimeSyncedVideoPlayer._textureId, (Texture) sprite.texture);
    if (sprite.packed)
    {
      Vector4 vector4;
      ref Vector4 local = ref vector4;
      double x = (double) sprite.textureRect.width / (double) sprite.texture.width;
      Rect textureRect = sprite.textureRect;
      double y = (double) textureRect.height / (double) sprite.texture.height;
      textureRect = sprite.textureRect;
      double z = (double) textureRect.x / (double) sprite.texture.width;
      textureRect = sprite.textureRect;
      double w = (double) textureRect.y / (double) sprite.texture.height;
      local = new Vector4((float) x, (float) y, (float) z, (float) w);
      this._materialPropertyBlockController.materialPropertyBlock.SetVector(SongTimeSyncedVideoPlayer._textureStId, vector4);
    }
    this.StopVideoPlayer();
  }

  public virtual void StopVideoPlayer()
  {
    if (this._waitForDependenciesAndPlayCoroutine != null)
    {
      this.StopCoroutine(this._waitForDependenciesAndPlayCoroutine);
      this._waitForDependenciesAndPlayCoroutine = (Coroutine) null;
    }
    this._videoPlayer.Stop();
    this._videoPlayer.clip = (VideoClip) null;
  }

  public virtual IEnumerator WaitForDependenciesAndPlay()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.m_Cm_E1__state;
    SongTimeSyncedVideoPlayer syncedVideoPlayer = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Cm_E1__state = -1;
      syncedVideoPlayer._videoPlayer.Play();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.m_Cm_E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.m_Cm_E2__current = (object) new WaitUntil(new Func<bool>(syncedVideoPlayer.m_CWaitForDependenciesAndPlaym_Eb__17_0));
    // ISSUE: reference to a compiler-generated field
    this.m_Cm_E1__state = 1;
    return true;
  }

  [CompilerGenerated]
  public virtual bool m_CWaitForDependenciesAndPlaym_Eb__17_0() => this._audioTimeSource.isReady && (UnityEngine.Object) this._videoPlayer.clip != (UnityEngine.Object) null;
}
