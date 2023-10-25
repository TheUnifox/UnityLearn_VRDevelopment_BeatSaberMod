// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.OpenFilePathView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using Polyglot;
using SFB;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class OpenFilePathView : MonoBehaviour
  {
    [SerializeField]
    private TMP_InputField _filePathInput;
    [SerializeField]
    private Button _openFileButton;
    [Space]
    [SerializeField]
    [LocalizationKey]
    private string _openDialogTitleKey;
    [SerializeField]
    [LocalizationKey]
    private string _filterNameKey;
    [SerializeField]
    private string[] _extensions;

    public string filePath => this._filePathInput.text;

    public event Action<string> onFilePathLoadedEvent;

    public void SetPath(string path) => this._filePathInput.text = path;

    protected void Start()
    {
      this._filePathInput.interactable = false;
      this._openFileButton.onClick.AddListener(new UnityAction(this.HandleOpenFileButtonClicked));
      this._filePathInput.onEndEdit.AddListener(new UnityAction<string>(this.HandleFilePathInputOnEndEdit));
    }

    private void HandleOpenFileButtonClicked()
    {
      string input = NativeFileDialogs.OpenFileDialog(Localization.Get(this._openDialogTitleKey), new ExtensionFilter[1]
      {
        new ExtensionFilter(Localization.Get(this._filterNameKey), this._extensions)
      }, (string) null);
      if (string.IsNullOrEmpty(input))
        return;
      this._filePathInput.SetTextWithoutNotify(input);
      Action<string> filePathLoadedEvent = this.onFilePathLoadedEvent;
      if (filePathLoadedEvent == null)
        return;
      filePathLoadedEvent(input);
    }

    private void HandleFilePathInputOnEndEdit(string path)
    {
      Action<string> filePathLoadedEvent = this.onFilePathLoadedEvent;
      if (filePathLoadedEvent == null)
        return;
      filePathLoadedEvent(path);
    }
  }
}
