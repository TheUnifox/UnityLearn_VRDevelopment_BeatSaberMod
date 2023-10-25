// Decompiled with JetBrains decompiler
// Type: VideoProjectionEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Video;
using Zenject;

public class VideoProjectionEffect : MonoBehaviour
{
  [SerializeField]
  protected VideoProjectionDataModelSO _videoProjectionDataModel;
  [SerializeField]
  protected SongTimeSyncedVideoPlayer _videoPlayer;
  [SerializeField]
  protected BasicBeatmapEventType _videoEventType;
  [Inject]
  protected readonly VideoProjectionEffect.InitData _initData;
  [Inject]
  protected readonly EnvironmentContext _environmentContext;
  [Inject]
  protected readonly IReadonlyBeatmapData _beatmapData;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  protected VideoProjectionEffect.VideoProjectionBehavior _behavior;

  public virtual void Start()
  {
    switch (this._environmentContext)
    {
      case EnvironmentContext.Gameplay:
        this._behavior = (VideoProjectionEffect.VideoProjectionBehavior) new VideoProjectionEffect.GameplayVideoProjectionBehavior(this._beatmapData, this._videoProjectionDataModel, this._videoPlayer, this._videoEventType, this._initData.previewBeatmapLevel);
        break;
      case EnvironmentContext.BeatmapEditor:
        this._behavior = (VideoProjectionEffect.VideoProjectionBehavior) new VideoProjectionEffect.BeatmapEditorVideoProjectionBehavior(this._beatmapData, this._videoProjectionDataModel, this._videoPlayer, this._videoEventType, this._beatmapCallbacksController, this._initData.previewBeatmapLevel);
        break;
    }
  }

  public virtual void OnDestroy() => this._behavior?.Dispose();

  public class InitData
  {
    public readonly IPreviewBeatmapLevel previewBeatmapLevel;

    public InitData(IPreviewBeatmapLevel previewBeatmapLevel) => this.previewBeatmapLevel = previewBeatmapLevel;
  }

  public abstract class VideoProjectionBehavior
  {
    protected int _eventValue;
    protected AsyncOperationHandle<VideoClip> _videoClipHandle;
    private readonly VideoProjectionDataModelSO _model;
    private readonly SongTimeSyncedVideoPlayer _videoPlayer;
    private readonly IPreviewBeatmapLevel _previewBeatmapLevel;

    protected VideoProjectionBehavior(
      VideoProjectionDataModelSO dataModel,
      SongTimeSyncedVideoPlayer videoPlayer,
      IPreviewBeatmapLevel previewBeatmapLevel)
    {
      this._model = dataModel;
      this._videoPlayer = videoPlayer;
      this._previewBeatmapLevel = previewBeatmapLevel;
    }

    public virtual void Dispose()
    {
      if (!this._videoClipHandle.IsValid())
        return;
      Addressables.Release<VideoClip>(this._videoClipHandle);
    }

    protected void LoadVideoFromModel(int eventValue)
    {
      VideoProjectionDataModelSO.VideoClipWithId videoClipWithId = ((IEnumerable<VideoProjectionDataModelSO.VideoClipWithId>) this._model.videoClipWithIds).FirstOrDefault<VideoProjectionDataModelSO.VideoClipWithId>((Func<VideoProjectionDataModelSO.VideoClipWithId, bool>) (clipWithId => clipWithId.id == eventValue));
      if (videoClipWithId == null)
      {
        Debug.LogWarning((object) string.Format("Unable to find video with {0} id.", (object) eventValue));
        this.LoadPreviewCoverAsset();
      }
      else
      {
        this._videoClipHandle = videoClipWithId.videoAssetReference.LoadAssetAsync<VideoClip>();
        VideoClip videoClip = this._videoClipHandle.WaitForCompletion();
        if ((UnityEngine.Object) videoClip == (UnityEngine.Object) null)
        {
          Debug.LogWarning((object) string.Format("Could not load video clip {0}.", (object) videoClipWithId.id));
          this.LoadPreviewCoverAsset();
        }
        else
          this._videoPlayer.SetVideoClip(videoClip);
      }
    }

    protected async void LoadPreviewCoverAsset()
    {
      if (this._previewBeatmapLevel == null)
        return;
      Sprite coverImageAsync = await this._previewBeatmapLevel.GetCoverImageAsync(CancellationToken.None);
      if ((UnityEngine.Object) coverImageAsync == (UnityEngine.Object) null)
        return;
      this._videoPlayer.SetSpriteAndStopVideo(coverImageAsync);
    }
  }

  public class BeatmapEditorVideoProjectionBehavior : VideoProjectionEffect.VideoProjectionBehavior
  {
    protected readonly BeatmapCallbacksController _beatmapCallbacksController;
    protected readonly BeatmapDataCallbackWrapper _callbackWrapper;

    public BeatmapEditorVideoProjectionBehavior(
      IReadonlyBeatmapData beatmapData,
      VideoProjectionDataModelSO dataModel,
      SongTimeSyncedVideoPlayer videoPlayer,
      BasicBeatmapEventType videoEventType,
      BeatmapCallbacksController beatmapCallbacksController,
      IPreviewBeatmapLevel previewBeatmapLevel)
      : base(dataModel, videoPlayer, previewBeatmapLevel)
    {
      this._beatmapCallbacksController = beatmapCallbacksController;
      BasicBeatmapEventData beatmapEventData = beatmapData.GetBeatmapDataItems<BasicBeatmapEventData>((int) videoEventType).FirstOrDefault<BasicBeatmapEventData>();
      if (beatmapEventData != null)
      {
        this._eventValue = beatmapEventData.value;
        this.LoadVideoFromModel(this._eventValue);
      }
      else
        this.LoadPreviewCoverAsset();
      this._callbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(videoEventType));
    }

    public override void Dispose()
    {
      base.Dispose();
      this._beatmapCallbacksController.RemoveBeatmapCallback(this._callbackWrapper);
    }

    public virtual void HandleBeatmapEvent(BasicBeatmapEventData data)
    {
      if (data.previousSameTypeEventData != null || data.value == this._eventValue)
        return;
      this._eventValue = data.value;
      if (this._videoClipHandle.IsValid())
        Addressables.Release<VideoClip>(this._videoClipHandle);
      this.LoadVideoFromModel(this._eventValue);
    }
  }

  public class GameplayVideoProjectionBehavior : VideoProjectionEffect.VideoProjectionBehavior
  {
    public GameplayVideoProjectionBehavior(
      IReadonlyBeatmapData beatmapData,
      VideoProjectionDataModelSO dataModel,
      SongTimeSyncedVideoPlayer videoPlayer,
      BasicBeatmapEventType videoEventType,
      IPreviewBeatmapLevel previewBeatmapLevel)
      : base(dataModel, videoPlayer, previewBeatmapLevel)
    {
      BasicBeatmapEventData beatmapEventData = beatmapData.GetBeatmapDataItems<BasicBeatmapEventData>((int) videoEventType).FirstOrDefault<BasicBeatmapEventData>();
      if (beatmapEventData == null)
      {
        this.LoadPreviewCoverAsset();
      }
      else
      {
        this._eventValue = beatmapEventData.value;
        this.LoadVideoFromModel(this._eventValue);
      }
    }
  }
}
