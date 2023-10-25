// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.CoverImageInputView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using SFB;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class CoverImageInputView : MonoBehaviour
  {
    [SerializeField]
    private Button _openFileButton;
    [SerializeField]
    private RawImage _loadedCoverImageTexture;
    [SerializeField]
    [NullAllowed]
    private GameObject _valueModifiedHintGo;
    [SerializeField]
    private GameObject _loadingIndicatorGo;
    [Space]
    [SerializeField]
    private string _openDialogTitle;
    [SerializeField]
    private string _filterName;
    [Header("Warnings")]
    [SerializeField]
    private TextMeshProUGUI _warningText;
    [SerializeField]
    private string _cannotLoadImageWarning;
    [SerializeField]
    private string _notSquareImageWarning;
    [SerializeField]
    private string _imageTooSmallWarning;
    [Space]
    [SerializeField]
    private int _minResolution;
    private string _filePath;
    private bool _isCorrectImage;
    private Coroutine _loadImageCoroutine;

    public event Action<string, Texture2D> coverImageLoadedEvent;

    public string filePath => !this._isCorrectImage ? "" : this._filePath;

    public void SetCoverImagePath(string coverImagePath)
    {
      this._filePath = coverImagePath;
      this.LoadImage(coverImagePath, false);
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
      this._warningText.enabled = false;
      this._loadedCoverImageTexture.enabled = false;
      this._loadingIndicatorGo.SetActive(false);
      this._openFileButton.onClick.AddListener(new UnityAction(this.HandleOpenFileButtonClicked));
    }

    private void HandleOpenFileButtonClicked()
    {
      string path = NativeFileDialogs.OpenFileDialog(this._openDialogTitle, new ExtensionFilter[1]
      {
        new ExtensionFilter(this._filterName, new string[1]
        {
          "png"
        })
      }, (string) null);
      if (string.IsNullOrEmpty(path))
        return;
      this.LoadImage(path, true);
    }

    private void LoadImage(string path, bool triggerLoadedEvent)
    {
      if (this._loadImageCoroutine != null)
        this.StopCoroutine(this._loadImageCoroutine);
      if ((UnityEngine.Object) this._valueModifiedHintGo != (UnityEngine.Object) null)
        this._valueModifiedHintGo.SetActive(true);
      this._loadImageCoroutine = this.StartCoroutine(this.LoadCoverImageCoroutine(path, triggerLoadedEvent));
    }

    private IEnumerator LoadCoverImageCoroutine(string path, bool triggerLoadedEvent)
    {
      this._loadedCoverImageTexture.enabled = false;
      if (!string.IsNullOrEmpty(path))
      {
        this._loadingIndicatorGo.SetActive(true);
        Texture2D coverImageTexture = (Texture2D) null;
        yield return (object) SimpleTextureLoader.LoadTextureCoroutine(path, false, (Action<Texture2D>) (texture => coverImageTexture = texture));
        this._loadingIndicatorGo.SetActive(false);
        bool flag1 = (UnityEngine.Object) coverImageTexture == (UnityEngine.Object) null;
        bool flag2 = !flag1 && coverImageTexture.width != coverImageTexture.height;
        bool flag3 = !flag1 && (coverImageTexture.width < this._minResolution || coverImageTexture.height < this._minResolution);
        this._isCorrectImage = !(flag1 | flag2 | flag3);
        this._warningText.enabled = !this._isCorrectImage;
        if (flag1)
          this._warningText.text = this._cannotLoadImageWarning;
        else if (flag2)
          this._warningText.text = this._notSquareImageWarning;
        else if (flag3)
          this._warningText.text = string.Format("{0} {1}x{2}", (object) this._imageTooSmallWarning, (object) this._minResolution, (object) this._minResolution);
        if (this._isCorrectImage)
        {
          this._filePath = path;
          this._loadedCoverImageTexture.texture = (Texture) coverImageTexture;
          this._loadedCoverImageTexture.enabled = true;
          if (triggerLoadedEvent)
          {
            Action<string, Texture2D> imageLoadedEvent = this.coverImageLoadedEvent;
            if (imageLoadedEvent != null)
              imageLoadedEvent(path, coverImageTexture);
          }
        }
      }
    }
  }
}
