// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapsListTableCell
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapsListTableCell : TableCell
  {
    [SerializeField]
    private CurvedTextMeshPro _songName;
    [SerializeField]
    private CurvedTextMeshPro _songAuthorName;
    [SerializeField]
    private CurvedTextMeshPro _levelAuthorName;
    [SerializeField]
    private CurvedTextMeshPro _lastModifiedTimestamp;
    [SerializeField]
    private Button _openBeatmapButton;
    private int _idx;
    private string _songNameWithPath;
    private bool _nameFixRequired;

    public event Action<int> openBeatmapButtonPressedEvent;

    public void SetData(int idx, IBeatmapInfoData beatmapInfo)
    {
      this._idx = idx;
      this._songNameWithPath = beatmapInfo.relativeFolderPath;
      this._songName.text = this._songNameWithPath;
      this._songAuthorName.text = beatmapInfo.songAuthorName;
      this._levelAuthorName.text = beatmapInfo.levelAuthorName;
      this._lastModifiedTimestamp.text = beatmapInfo.lastModifiedTimestamp.ToString("dd. MM. yyyy HH:mm");
      this._nameFixRequired = !this.gameObject.activeSelf;
      if (this._nameFixRequired)
        return;
      this._nameFixRequired = false;
      this.StartCoroutine(this.TruncateNameField(this._songNameWithPath));
    }

    private void HandleOpenBeatmapButtonPressed()
    {
      Action<int> buttonPressedEvent = this.openBeatmapButtonPressedEvent;
      if (buttonPressedEvent == null)
        return;
      buttonPressedEvent(this._idx);
    }

    private void OnEnable()
    {
      this._openBeatmapButton.onClick.AddListener(new UnityAction(this.HandleOpenBeatmapButtonPressed));
      if (!this._nameFixRequired)
        return;
      this.StartCoroutine(this.TruncateNameField(this._songNameWithPath));
      this._nameFixRequired = false;
    }

    private void OnDisable() => this._openBeatmapButton.onClick.RemoveListener(new UnityAction(this.HandleOpenBeatmapButtonPressed));

    private IEnumerator TruncateNameField(string songNameWithPath)
    {
      yield return (object) new WaitForEndOfFrame();
      if (this._songName.isTextTruncated)
      {
        int startIndex = songNameWithPath.Length - (this._songName.textInfo.characterCount - 1);
        this._songName.text = "..." + songNameWithPath.Substring(startIndex);
      }
    }

    public class Factory : PlaceholderFactory<BeatmapsListTableCell>
    {
    }
  }
}
