// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.CopyDifficultyBeatmapView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class CopyDifficultyBeatmapView : BeatmapEditorView
  {
    [SerializeField]
    private TextMeshProUGUI _difficultyText;
    [SerializeField]
    private Toggle _notesToggle;
    [SerializeField]
    private Toggle _waypointsToggle;
    [SerializeField]
    private Toggle _obstaclesToggle;
    [SerializeField]
    private Toggle _chainsToggle;
    [SerializeField]
    private Toggle _arcsToggle;
    [SerializeField]
    private Toggle _eventsToggle;
    [SerializeField]
    private Button _copyButton;
    private BeatmapDifficulty _beatmapDifficulty;

    public event Action<BeatmapDifficulty, bool, bool, bool, bool, bool, bool> copyBeatmapEvent;

    public void SetData(BeatmapDifficulty beatmapDifficulty)
    {
      this._difficultyText.text = beatmapDifficulty.Name();
      this._beatmapDifficulty = beatmapDifficulty;
      this._notesToggle.SetIsOnWithoutNotify(false);
      this._waypointsToggle.SetIsOnWithoutNotify(false);
      this._obstaclesToggle.SetIsOnWithoutNotify(false);
      this._chainsToggle.SetIsOnWithoutNotify(false);
      this._arcsToggle.SetIsOnWithoutNotify(false);
      this._eventsToggle.SetIsOnWithoutNotify(false);
    }

    protected override void DidActivate() => this._copyButton.onClick.AddListener(new UnityAction(this.HandleCopyButtonClicked));

    protected override void DidDeactivate() => this._copyButton.onClick.RemoveListener(new UnityAction(this.HandleCopyButtonClicked));

    private void HandleCopyButtonClicked()
    {
      Action<BeatmapDifficulty, bool, bool, bool, bool, bool, bool> copyBeatmapEvent = this.copyBeatmapEvent;
      if (copyBeatmapEvent == null)
        return;
      copyBeatmapEvent(this._beatmapDifficulty, this._notesToggle.isOn, this._waypointsToggle.isOn, this._obstaclesToggle.isOn, this._chainsToggle.isOn, this._arcsToggle.isOn, this._eventsToggle.isOn);
    }
  }
}
