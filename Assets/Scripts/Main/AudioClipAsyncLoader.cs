// Decompiled with JetBrains decompiler
// Type: AudioClipAsyncLoader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

public class AudioClipAsyncLoader
{
  protected readonly IReferenceCountingCache<int, Task<AudioClip>> _cache;
  protected readonly IMediaAsyncLoader _mediaAsyncLoader;

  public AudioClipAsyncLoader(
    IReferenceCountingCache<int, Task<AudioClip>> cache,
    IMediaAsyncLoader mediaAsyncLoader)
  {
    this._cache = cache;
    this._mediaAsyncLoader = mediaAsyncLoader;
  }

  public virtual Task<AudioClip> LoadPreview(IPreviewBeatmapLevel beatmapLevel)
  {
    switch (beatmapLevel)
    {
      case IAssetSongPreviewAudioClipProvider source1 when (UnityEngine.Object) source1.songPreviewAudioClip != (UnityEngine.Object) null:
        return this.Load(source1);
      case IFilePathSongPreviewAudioClipProvider source2 when !string.IsNullOrEmpty(source2.songPreviewAudioClipPath):
        return this.Load(source2);
      default:
        return Task.FromResult<AudioClip>((AudioClip) null);
    }
  }

  public virtual Task<AudioClip> LoadSong(IBeatmapLevel beatmapLevel)
  {
    switch (beatmapLevel)
    {
      case IAssetSongAudioClipProvider source1 when (UnityEngine.Object) source1.songAudioClip != (UnityEngine.Object) null:
        return this.Load(source1);
      case IFilePathSongAudioClipProvider source2 when !string.IsNullOrEmpty(source2.songAudioClipPath):
        return this.Load(source2);
      default:
        return Task.FromResult<AudioClip>((AudioClip) null);
    }
  }

  public virtual void UnloadPreview(IPreviewBeatmapLevel beatmapLevel)
  {
    switch (beatmapLevel)
    {
      case IAssetSongPreviewAudioClipProvider source1 when (UnityEngine.Object) source1.songPreviewAudioClip != (UnityEngine.Object) null:
        this.Unload(source1);
        break;
      case IFilePathSongPreviewAudioClipProvider source2 when !string.IsNullOrEmpty(source2.songPreviewAudioClipPath):
        this.Unload(source2);
        break;
    }
  }

  public virtual void UnloadSong(IBeatmapLevel beatmapLevel)
  {
    switch (beatmapLevel)
    {
      case IAssetSongAudioClipProvider source1 when (UnityEngine.Object) source1.songAudioClip != (UnityEngine.Object) null:
        this.Unload(source1);
        break;
      case IFilePathSongAudioClipProvider source2 when !string.IsNullOrEmpty(source2.songAudioClipPath):
        this.Unload(source2);
        break;
    }
  }

  public virtual Task<AudioClip> Load(IAssetSongPreviewAudioClipProvider source) => this.Load(source.songPreviewAudioClip);

  public virtual Task<AudioClip> Load(IAssetSongAudioClipProvider source) => this.Load(source.songAudioClip);

  public virtual Task<AudioClip> Load(IFilePathSongPreviewAudioClipProvider source) => this.Load(source.songPreviewAudioClipPath);

  public virtual Task<AudioClip> Load(IFilePathSongAudioClipProvider source) => this.Load(source.songAudioClipPath);

  public virtual void Unload(IAssetSongPreviewAudioClipProvider source) => this.Unload(source.songPreviewAudioClip);

  public virtual void Unload(IAssetSongAudioClipProvider source) => this.Unload(source.songAudioClip);

  public virtual void Unload(IFilePathSongPreviewAudioClipProvider source) => this.Unload(source.songPreviewAudioClipPath);

  public virtual void Unload(IFilePathSongAudioClipProvider source) => this.Unload(source.songAudioClipPath);

  public virtual Task<AudioClip> Load(AudioClip audioClip) => this.Load(this.GetCacheKey(audioClip), (AudioClipAsyncLoader.LoadMethodDelegate) (() => Task.FromResult<AudioClip>(audioClip)));

  public virtual Task<AudioClip> Load(string audioClipFilePath) => this.Load(this.GetCacheKey(audioClipFilePath), (AudioClipAsyncLoader.LoadMethodDelegate) (() => this._mediaAsyncLoader.LoadAudioClipFromFilePathAsync(audioClipFilePath)));

  public virtual Task<AudioClip> Load(
    int cacheKey,
    AudioClipAsyncLoader.LoadMethodDelegate loadMethodDelegate)
  {
    Task<AudioClip> result;
    if (this._cache.TryGet(cacheKey, out result))
    {
      this._cache.AddReference(cacheKey);
      return result;
    }
    Task<AudioClip> task = loadMethodDelegate();
    this._cache.Insert(cacheKey, task);
    return task;
  }

  public virtual void Unload(AudioClip audioClip) => this.Unload(this.GetCacheKey(audioClip), (System.Action<AudioClip>) (loadedAudioClip => loadedAudioClip.UnloadAudioData()));

  public virtual void Unload(string audioClipFilePath) => this.Unload(this.GetCacheKey(audioClipFilePath), new System.Action<AudioClip>(UnityEngine.Object.Destroy));

  public virtual async void Unload(int cacheKey, System.Action<AudioClip> onDelete)
  {
    Task<AudioClip> result;
    if (!this._cache.TryGet(cacheKey, out result) || this._cache.RemoveReference(cacheKey) != 0)
      return;
    AudioClip audioClip = await result;
    if (this._cache.GetReferenceCount(cacheKey) != 0)
      return;
    onDelete(audioClip);
  }

  public virtual int GetCacheKey(AudioClip audioClip) => ((object) audioClip).GetHashCode();

  public virtual int GetCacheKey(string audioClipFilePath) => audioClipFilePath.GetHashCode();

  [Conditional("AUDIO_ASYNC_LOADER_LOG_ENABLED")]
  public static void LogError(string message) => UnityEngine.Debug.LogError((object) message);

  public delegate Task<AudioClip> LoadMethodDelegate();
}
