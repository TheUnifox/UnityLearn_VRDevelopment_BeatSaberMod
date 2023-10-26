// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.AudioClipLoader
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace BeatmapEditor3D
{
  public class AudioClipLoader : MonoBehaviour
  {
    private bool _isLoading;

    public void LoadAudioFile(string filePath, Action<AudioClip> finishCallback)
    {
      if (this._isLoading)
      {
        if (finishCallback == null)
          return;
        finishCallback((AudioClip) null);
      }
      else
        this.StartCoroutine(this.LoadAudioFileCoroutine(filePath, finishCallback));
    }

    public IEnumerator LoadAudioFileCoroutine(string filePath, Action<AudioClip> finishCallback)
    {
      if (!File.Exists(filePath))
      {
        Action<AudioClip> action = finishCallback;
        if (action != null)
          action(null);
      }
      else
      {
        this._isLoading = true;
        AudioType audioTypeFromPath = AudioTypeHelper.GetAudioTypeFromPath(filePath);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(FileHelpers.GetEscapedURLForFilePath(filePath), audioTypeFromPath))
        {
          yield return (object) www.SendWebRequest();
          this._isLoading = false;
          AudioClip audioClip = null;
          if (www.result != UnityWebRequest.Result.ConnectionError)
          {
            audioClip = DownloadHandlerAudioClip.GetContent(www);
            if ( audioClip != null && audioClip.loadState != AudioDataLoadState.Loaded)
              audioClip = null;
          }
          Action<AudioClip> action = finishCallback;
          if (action != null)
            action(audioClip);
        }
      }
    }
  }
}
