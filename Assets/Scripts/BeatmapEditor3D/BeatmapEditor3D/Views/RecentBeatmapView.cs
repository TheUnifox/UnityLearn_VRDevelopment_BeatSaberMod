// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.RecentBeatmapView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class RecentBeatmapView : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private RawImage _rawImage;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Texture _noImageTexture;
    [SerializeField]
    private Color _noImageColor;
    private IBeatmapInfoData _beatmapInfoData;
    private Coroutine _loadImageCoroutine;

    public void SetData(
      IBeatmapInfoData beatmapInfoData,
      Action<string> recentBeatmapClickedCallback)
    {
      this._nameText.text = beatmapInfoData.songName;
      this._beatmapInfoData = beatmapInfoData;
      this._rawImage.texture = this._noImageTexture;
      this._rawImage.color = this._noImageColor;
      this._button.onClick.RemoveAllListeners();
      this._button.onClick.AddListener((UnityAction) (() =>
      {
        Action<string> action = recentBeatmapClickedCallback;
        if (action == null)
          return;
        action(beatmapInfoData.beatmapFolderPath);
      }));
      this.gameObject.SetActive(true);
      this.LoadImage();
    }

    protected void Awake() => this.gameObject.SetActive(this._beatmapInfoData != null);

    protected void OnDestroy() => this._button.onClick.RemoveAllListeners();

    private void LoadImage()
    {
      if (this._loadImageCoroutine != null)
        this.StopCoroutine(this._loadImageCoroutine);
      this._loadImageCoroutine = this.StartCoroutine(this.LoadCoverImageCoroutine());
    }

    private IEnumerator LoadCoverImageCoroutine()
    {
      RecentBeatmapView recentBeatmapView = this;
      if (!string.IsNullOrEmpty(recentBeatmapView._beatmapInfoData.beatmapFolderPath) && !string.IsNullOrEmpty(recentBeatmapView._beatmapInfoData.coverImagePath) && BeatmapProjectFileHelper.FileExists(recentBeatmapView._beatmapInfoData.beatmapFolderPath, recentBeatmapView._beatmapInfoData.coverImagePath))
      {
        // ISSUE: reference to a compiler-generated method
        yield return (object) SimpleTextureLoader.LoadTextureCoroutine(Path.Combine(recentBeatmapView._beatmapInfoData.beatmapFolderPath, recentBeatmapView._beatmapInfoData.coverImagePath), false, new Action<Texture2D>(recentBeatmapView.m_CLoadCoverImageCoroutinem_Eb__11_0));
        recentBeatmapView._loadImageCoroutine = (Coroutine) null;
      }
    }
  }
}
