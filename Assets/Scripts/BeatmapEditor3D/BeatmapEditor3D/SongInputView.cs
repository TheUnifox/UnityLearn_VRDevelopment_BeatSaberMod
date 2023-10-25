// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.SongInputView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using SFB;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class SongInputView : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _songLabelText;
    [SerializeField]
    private TextMeshProUGUI _songLengthText;
    [SerializeField]
    private Button _openFileButton;
    [SerializeField]
    [NullAllowed]
    private GameObject _valueModifiedHintGo;
    [SerializeField]
    private GameObject _loadingIndicator;
    [Space]
    [SerializeField]
    private string _openDialogTitle;
    [SerializeField]
    private string _filterName;
    [SerializeField]
    private string[] _extensions = new string[2]
    {
      "ogg",
      "wav"
    };
    [Inject]
    private readonly AudioClipLoader _audioClipLoader;
    private bool _songValid;
    private string _songPath;

    public event Action<string, AudioClip> songLoadedEvent;

    public string songPath => this._songPath;

    public bool songValid => this._songValid;

    public void SetAudioPath(string path)
    {
      this._songPath = path;
      this._songValid = true;
      this.LoadAudioClip(path, false);
      this.ClearDirtyState();
    }

    public void ClearDirtyState()
    {
      if (!((UnityEngine.Object) this._valueModifiedHintGo != (UnityEngine.Object) null))
        return;
      this._valueModifiedHintGo.SetActive(false);
    }

    protected void Start()
    {
      this._loadingIndicator.SetActive(false);
      this._openFileButton.onClick.AddListener(new UnityAction(this.HandleOpenFileButtonClicked));
    }

    protected void OnDestroy() => this._openFileButton.onClick.RemoveListener(new UnityAction(this.HandleOpenFileButtonClicked));

    private void HandleOpenFileButtonClicked()
    {
      string songPath = NativeFileDialogs.OpenFileDialog(this._openDialogTitle, new ExtensionFilter[1]
      {
        new ExtensionFilter(this._filterName, this._extensions)
      }, (string) null);
      if (string.IsNullOrEmpty(songPath))
        return;
      this._songValid = false;
      if ((UnityEngine.Object) this._valueModifiedHintGo != (UnityEngine.Object) null)
        this._valueModifiedHintGo.SetActive(true);
      this.LoadAudioClip(songPath, true);
    }

    private void LoadAudioClip(string songPath, bool triggerLoadedEvent)
    {
      if (string.IsNullOrEmpty(songPath))
        return;
      this._loadingIndicator.SetActive(true);
      string songName = Path.GetFileName(songPath);
      this._audioClipLoader.LoadAudioFile(songPath, (Action<AudioClip>) (clip =>
      {
        this._loadingIndicator.SetActive(false);
        if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
        {
          this._songPath = (string) null;
        }
        else
        {
          this._songValid = true;
          this._songPath = songPath;
          this._songLabelText.text = songName ?? "";
          this._songLengthText.text = string.Format("({0:mm\\:ss})", (object) TimeSpan.FromSeconds((double) clip.length));
          if (!triggerLoadedEvent)
            return;
          Action<string, AudioClip> songLoadedEvent = this.songLoadedEvent;
          if (songLoadedEvent == null)
            return;
          songLoadedEvent(songPath, clip);
        }
      }));
    }
  }
}
