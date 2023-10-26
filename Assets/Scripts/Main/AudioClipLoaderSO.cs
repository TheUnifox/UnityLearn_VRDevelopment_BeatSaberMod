// Decompiled with JetBrains decompiler
// Type: AudioClipLoaderSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AudioClipLoaderSO : PersistentScriptableObject
{
  protected bool _isLoading;

  protected override void OnEnable()
  {
    base.OnEnable();
    this._isLoading = false;
  }

  public virtual void LoadAudioFile(string filePath, System.Action<AudioClip> finishCallback)
  {
    if (this._isLoading)
      return;
    PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(this.LoadAudioFileCoroutine(filePath, finishCallback));
  }

  public virtual IEnumerator LoadAudioFileCoroutine(
    string filePath,
    System.Action<AudioClip> finishCallback)
  {
    if (!File.Exists(filePath))
    {
      finishCallback((AudioClip) null);
    }
    else
    {
      this._isLoading = true;
      using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(FileHelpers.GetEscapedURLForFilePath(filePath), AudioType.UNKNOWN))
      {
        yield return (object) www.SendWebRequest();
        this._isLoading = false;
        AudioClip audioClip = (AudioClip) null;
        if (www.result != UnityWebRequest.Result.ConnectionError)
        {
          audioClip = DownloadHandlerAudioClip.GetContent(www);
          if ((UnityEngine.Object) audioClip != (UnityEngine.Object) null && audioClip.loadState != AudioDataLoadState.Loaded)
            audioClip = (AudioClip) null;
        }
        if (finishCallback != null)
          finishCallback(audioClip);
      }
    }
  }
}
