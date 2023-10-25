// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.NewBeatmapViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Views;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D
{
  public class NewBeatmapViewController : BeatmapEditorViewController
  {
    [SerializeField]
    private TMP_InputField _songNameInput;
    [SerializeField]
    private TMP_InputField _customBeatmapNameInput;
    [SerializeField]
    private FloatInputFieldValidator _beatmapBpmInput;
    [Space]
    [SerializeField]
    private SongInputView _openSongView;
    [SerializeField]
    private CoverImageInputView _coverImageInputView;
    [Space]
    [SerializeField]
    private Button _saveBeatmapButton;
    [SerializeField]
    private Button _saveAndOpenBeatmapButton;
    [Space]
    [SerializeField]
    private float _defaultBpm = 60f;

    public event Action<(string songName, string customBeatmapName, float bpm, string coverImagePath, string songPath), bool> saveBeatmapEvent;

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      if (addedToHierarchy)
      {
        this._saveBeatmapButton.onClick.AddListener(new UnityAction(this.HandleSaveBeatmapButton));
        this._saveAndOpenBeatmapButton.onClick.AddListener(new UnityAction(this.HandleSaveAndOpenBeatmapButton));
        this._songNameInput.onValueChanged.AddListener(new UnityAction<string>(this.HandleSongNameInputOnEdit));
        this._customBeatmapNameInput.onValueChanged.AddListener(new UnityAction<string>(this.HandleSongNameInputOnEdit));
        this._beatmapBpmInput.onInputValidated += new Action<float>(this.HandleBeatmapBpmInputOnInputValidated);
        this._openSongView.songLoadedEvent += new Action<string, AudioClip>(this.HandleOpenSongViewSongLoaded);
        this._coverImageInputView.coverImageLoadedEvent += new Action<string, Texture2D>(this.HandleCoverImageInputViewCoverImageInputLoaded);
      }
      this._songNameInput.text = (string) null;
      this._customBeatmapNameInput.text = (string) null;
      this._beatmapBpmInput.value = this._defaultBpm;
      this.RefreshUI();
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      if (!removedFromHierarchy)
        return;
      this._saveBeatmapButton.onClick.RemoveListener(new UnityAction(this.HandleSaveBeatmapButton));
      this._saveAndOpenBeatmapButton.onClick.RemoveListener(new UnityAction(this.HandleSaveAndOpenBeatmapButton));
      this._songNameInput.onValueChanged.RemoveListener(new UnityAction<string>(this.HandleSongNameInputOnEdit));
      this._customBeatmapNameInput.onValueChanged.RemoveListener(new UnityAction<string>(this.HandleSongNameInputOnEdit));
      this._beatmapBpmInput.onInputValidated -= new Action<float>(this.HandleBeatmapBpmInputOnInputValidated);
      this._openSongView.songLoadedEvent -= new Action<string, AudioClip>(this.HandleOpenSongViewSongLoaded);
      this._coverImageInputView.coverImageLoadedEvent -= new Action<string, Texture2D>(this.HandleCoverImageInputViewCoverImageInputLoaded);
    }

    private void HandleOpenSongViewSongLoaded(string path, AudioClip audioClip) => this.RefreshUI();

    private void HandleCoverImageInputViewCoverImageInputLoaded(
      string path,
      Texture2D coverImageTexture)
    {
      this.RefreshUI();
    }

    private void HandleBeatmapBpmInputOnInputValidated(float value) => this.RefreshUI();

    private void HandleSongNameInputOnEdit(string songName) => this.RefreshUI();

    private void HandleSaveBeatmapButton()
    {
      if (!this.IsNewBeatmapValid())
        return;
      this.SaveBeatmap(false);
    }

    private void HandleSaveAndOpenBeatmapButton()
    {
      if (!this.IsNewBeatmapValid())
        return;
      this.SaveBeatmap(true);
    }

    private void SaveBeatmap(bool openBeatmap)
    {
      Action<(string, string, float, string, string), bool> saveBeatmapEvent = this.saveBeatmapEvent;
      if (saveBeatmapEvent == null)
        return;
      saveBeatmapEvent((this._songNameInput.text, this._customBeatmapNameInput.text, this._beatmapBpmInput.value, this._coverImageInputView.filePath, this._openSongView.songPath), openBeatmap);
    }

    private bool IsNewBeatmapValid() => !string.IsNullOrEmpty(this._songNameInput.text) && this._openSongView.songValid && !string.IsNullOrEmpty(this._openSongView.songPath) && (double) this._beatmapBpmInput.value != 0.0;

    private void RefreshUI()
    {
      bool flag = this.IsNewBeatmapValid();
      this._saveBeatmapButton.interactable = flag;
      this._saveAndOpenBeatmapButton.interactable = flag;
    }
  }
}
