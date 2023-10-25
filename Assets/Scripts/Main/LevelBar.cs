// Decompiled with JetBrains decompiler
// Type: LevelBar
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class LevelBar : MonoBehaviour
{
  [SerializeField]
  protected ImageView _songArtworkImageView;
  [SerializeField]
  protected TextMeshProUGUI _songNameText;
  [SerializeField]
  protected TextMeshProUGUI _authorNameText;
  [Space]
  [SerializeField]
  protected bool _showSongSubName;
  [DrawIf("_showSongSubName", true, DrawIfAttribute.DisablingType.DontDraw)]
  [NullAllowed("_showSongSubName", true)]
  [SerializeField]
  protected GameObject _singleLineSongInfoContainer;
  [DrawIf("_showSongSubName", true, DrawIfAttribute.DisablingType.DontDraw)]
  [NullAllowed("_showSongSubName", true)]
  [SerializeField]
  protected GameObject _multiLineSongInfoContainer;
  [DrawIf("_showSongSubName", true, DrawIfAttribute.DisablingType.DontDraw)]
  [NullAllowed("_showSongSubName", true)]
  [SerializeField]
  protected TextMeshProUGUI _multiLineSongNameText;
  [DrawIf("_showSongSubName", true, DrawIfAttribute.DisablingType.DontDraw)]
  [NullAllowed("_showSongSubName", true)]
  [SerializeField]
  protected TextMeshProUGUI _multiLineAuthorNameText;
  [Space]
  [SerializeField]
  protected bool _showDifficultyAndCharacteristic;
  [DrawIf("_showDifficultyAndCharacteristic", true, DrawIfAttribute.DisablingType.DontDraw)]
  [NullAllowed("_showDifficultyAndCharacteristic", true)]
  [SerializeField]
  protected TextMeshProUGUI _difficultyText;
  [DrawIf("_showDifficultyAndCharacteristic", true, DrawIfAttribute.DisablingType.DontDraw)]
  [NullAllowed("_showDifficultyAndCharacteristic", true)]
  [SerializeField]
  protected ImageView _characteristicIconImageView;
  [SerializeField]
  protected bool _useArtworkBackground;
  [DrawIf("_useArtworkBackground", true, DrawIfAttribute.DisablingType.DontDraw)]
  [NullAllowed("_useArtworkBackground", true)]
  [SerializeField]
  protected ImageView _artworkBackgroundImage;
  protected CancellationTokenSource _cancellationTokenSource;

  public bool hide
  {
    set => this.gameObject.SetActive(!value);
  }

  public virtual void Setup(IPreviewBeatmapLevel previewBeatmapLevel) => this.Setup(previewBeatmapLevel, (BeatmapCharacteristicSO) null, BeatmapDifficulty.Easy);

  public virtual async void Setup(
    IPreviewBeatmapLevel previewBeatmapLevel,
    BeatmapCharacteristicSO beatmapCharacteristic,
    BeatmapDifficulty beatmapDifficulty)
  {
    try
    {
      this._cancellationTokenSource?.Cancel();
      this._cancellationTokenSource = new CancellationTokenSource();
      CancellationToken cancellationToken = this._cancellationTokenSource.Token;
      Sprite coverImageAsync = await previewBeatmapLevel.GetCoverImageAsync(this._cancellationTokenSource.Token);
      cancellationToken.ThrowIfCancellationRequested();
      this._songArtworkImageView.sprite = coverImageAsync;
      bool flag = this._showSongSubName && previewBeatmapLevel.songSubName.Length > 0;
      if ((UnityEngine.Object) this._singleLineSongInfoContainer != (UnityEngine.Object) null)
        this._singleLineSongInfoContainer.SetActive(!flag);
      if ((UnityEngine.Object) this._multiLineSongInfoContainer != (UnityEngine.Object) null)
        this._multiLineSongInfoContainer.SetActive(flag);
      if (this._showSongSubName && previewBeatmapLevel.songSubName.Length > 0)
      {
        this._multiLineSongNameText.text = " " + previewBeatmapLevel.songName + "\n<size=80%><alpha=#BB>" + previewBeatmapLevel.songSubName;
        this._multiLineAuthorNameText.text = previewBeatmapLevel.songAuthorName;
      }
      else
      {
        this._songNameText.text = previewBeatmapLevel.songName;
        this._authorNameText.text = previewBeatmapLevel.songAuthorName;
      }
      if (this._showDifficultyAndCharacteristic)
      {
        this._difficultyText.text = beatmapDifficulty.Name();
        this._characteristicIconImageView.sprite = (UnityEngine.Object) beatmapCharacteristic != (UnityEngine.Object) null ? beatmapCharacteristic.icon : (Sprite) null;
      }
      if (this._useArtworkBackground)
        this._artworkBackgroundImage.sprite = coverImageAsync;
      cancellationToken = new CancellationToken();
    }
    catch (OperationCanceledException ex)
    {
    }
  }

  public virtual void OnDestroy() => this._cancellationTokenSource?.Cancel();
}
