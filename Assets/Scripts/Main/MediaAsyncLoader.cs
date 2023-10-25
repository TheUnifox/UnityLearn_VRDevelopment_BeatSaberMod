// Decompiled with JetBrains decompiler
// Type: MediaAsyncLoader
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class MediaAsyncLoader : IMediaAsyncLoader
{
  public static async Task<string> LoadWebpage(string uri, CancellationToken cancellationToken)
  {
    string text;
    using (UnityWebRequest www = UnityWebRequest.Get(uri))
    {
      AsyncOperation request = (AsyncOperation) www.SendWebRequest();
      while (!request.isDone)
      {
        cancellationToken.ThrowIfCancellationRequested();
        await Task.Delay(100);
      }
      cancellationToken.ThrowIfCancellationRequested();
      text = www.isNetworkError || www.isHttpError ? (string) null : www.downloadHandler.text;
    }
    return text;
  }

  public virtual async Task<AudioClip> LoadAudioClipFromFilePathAsync(string filePath)
  {
    AudioType audioTypeFromPath = AudioTypeHelper.GetAudioTypeFromPath(filePath);
    AudioClip content;
    using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(FileHelpers.GetEscapedURLForFilePath(filePath), audioTypeFromPath))
    {
      ((DownloadHandlerAudioClip) www.downloadHandler).streamAudio = true;
      AsyncOperation request = (AsyncOperation) www.SendWebRequest();
      while (!request.isDone)
        await Task.Delay(100);
      content = www.isNetworkError || www.isHttpError ? (AudioClip) null : DownloadHandlerAudioClip.GetContent(www);
    }
    return content;
  }

  public static async Task<Texture2D> LoadTextureAsync(
    string path,
    CancellationToken cancellationToken)
  {
    Texture2D content;
    using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(FileHelpers.GetEscapedURLForFilePath(path)))
    {
      AsyncOperation request = (AsyncOperation) www.SendWebRequest();
      while (!request.isDone)
      {
        cancellationToken.ThrowIfCancellationRequested();
        await Task.Delay(100);
      }
      cancellationToken.ThrowIfCancellationRequested();
      content = www.isNetworkError || www.isHttpError ? (Texture2D) null : DownloadHandlerTexture.GetContent(www);
    }
    return content;
  }

  public static async Task<Sprite> LoadSpriteAsync(string path, CancellationToken cancellationToken)
  {
    using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(FileHelpers.GetEscapedURLForFilePath(path)))
    {
      AsyncOperation request = (AsyncOperation) www.SendWebRequest();
      while (!request.isDone)
      {
        cancellationToken.ThrowIfCancellationRequested();
        await Task.Delay(100);
      }
      cancellationToken.ThrowIfCancellationRequested();
      if (www.isNetworkError || www.isHttpError)
        return (Sprite) null;
      Texture2D content = DownloadHandlerTexture.GetContent(www);
      content.hideFlags = HideFlags.DontSave;
      return Sprite.Create(content, new Rect(0.0f, 0.0f, (float) content.width, (float) content.height), new Vector2(0.5f, 0.5f), 256f, 0U, SpriteMeshType.FullRect, new Vector4(0.0f, 0.0f, 0.0f, 0.0f), false);
    }
  }

  [Conditional("MediaAsyncLoaderLog")]
  public static void Log(string message) => UnityEngine.Debug.Log((object) message);
}
