// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.DifficultyBeatmapView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D.Views
{
  public class DifficultyBeatmapView : BeatmapEditorView
  {
    [Header("Edit Beatmap Level View")]
    [SerializeField]
    private GameObject _editBeatmapLevelViewWrapper;
    [SerializeField]
    private FloatInputFieldValidator _noteJumpMovementSpeedInputValidator;
    [SerializeField]
    private FloatInputFieldValidator _noteJumpStartBeatOffsetInputValidator;
    [SerializeField]
    private Button _editBeatmapLevelButton;
    [SerializeField]
    private Button _deleteBeatmapLevelButton;
    [Header("New Beatmap Level View")]
    [SerializeField]
    private GameObject _newBeatmapLevelViewWrapper;
    [SerializeField]
    private Button _newBeatmapLevelButton;
    private BeatmapDifficulty _beatmapDifficulty;
    private IDifficultyBeatmapData _beatmapData;

    public event Action<BeatmapDifficulty> newBeatmapEvent;

    public event Action<BeatmapDifficulty> editBeatmapEvent;

    public event Action<BeatmapDifficulty> deleteBeatmapEvent;

    public event Action<BeatmapDifficulty, float, float> beatmapDataChangedEvent;

    public void SetData(BeatmapDifficulty beatmapDifficulty, IDifficultyBeatmapData beatmapData)
    {
      this._beatmapDifficulty = beatmapDifficulty;
      this._beatmapData = beatmapData;
      this._newBeatmapLevelViewWrapper.SetActive(this._beatmapData == null);
      this._editBeatmapLevelViewWrapper.SetActive(this._beatmapData != null);
      if (this._beatmapData == null)
        return;
      this._noteJumpMovementSpeedInputValidator.SetValueWithoutNotify(this._beatmapData.noteJumpMovementSpeed, true);
      this._noteJumpStartBeatOffsetInputValidator.SetValueWithoutNotify(this._beatmapData.noteJumpStartBeatOffset, true);
    }

    public void SetState(bool canEditLevel, bool deleteMode)
    {
      this._editBeatmapLevelButton.enabled = canEditLevel;
      this._deleteBeatmapLevelButton.enabled = canEditLevel;
      this._editBeatmapLevelButton.gameObject.SetActive(!deleteMode);
      this._deleteBeatmapLevelButton.gameObject.SetActive(deleteMode);
    }

    public void ClearDirtyState()
    {
      this._noteJumpMovementSpeedInputValidator.ClearDirtyState();
      this._noteJumpStartBeatOffsetInputValidator.ClearDirtyState();
    }

    protected override void DidActivate()
    {
      this._newBeatmapLevelButton.onClick.AddListener(new UnityAction(this.HandleNewBeatmapLevelButtonClicked));
      this._editBeatmapLevelButton.onClick.AddListener(new UnityAction(this.HandleEditBeatmapLevelButtonClicked));
      this._deleteBeatmapLevelButton.onClick.AddListener(new UnityAction(this.HandleDeleteBeatmapLevelButtonClicked));
      this._noteJumpMovementSpeedInputValidator.onInputValidated += new Action<float>(this.HandleDataValidated);
      this._noteJumpStartBeatOffsetInputValidator.onInputValidated += new Action<float>(this.HandleDataValidated);
    }

    protected override void DidDeactivate()
    {
      this._newBeatmapLevelButton.onClick.RemoveListener(new UnityAction(this.HandleNewBeatmapLevelButtonClicked));
      this._editBeatmapLevelButton.onClick.RemoveListener(new UnityAction(this.HandleEditBeatmapLevelButtonClicked));
      this._deleteBeatmapLevelButton.onClick.RemoveListener(new UnityAction(this.HandleDeleteBeatmapLevelButtonClicked));
      this._noteJumpMovementSpeedInputValidator.onInputValidated -= new Action<float>(this.HandleDataValidated);
      this._noteJumpStartBeatOffsetInputValidator.onInputValidated -= new Action<float>(this.HandleDataValidated);
    }

    private void HandleNewBeatmapLevelButtonClicked()
    {
      Action<BeatmapDifficulty> newBeatmapEvent = this.newBeatmapEvent;
      if (newBeatmapEvent == null)
        return;
      newBeatmapEvent(this._beatmapDifficulty);
    }

    private void HandleEditBeatmapLevelButtonClicked()
    {
      Action<BeatmapDifficulty> editBeatmapEvent = this.editBeatmapEvent;
      if (editBeatmapEvent == null)
        return;
      editBeatmapEvent(this._beatmapDifficulty);
    }

    private void HandleDeleteBeatmapLevelButtonClicked()
    {
      Action<BeatmapDifficulty> deleteBeatmapEvent = this.deleteBeatmapEvent;
      if (deleteBeatmapEvent == null)
        return;
      deleteBeatmapEvent(this._beatmapDifficulty);
    }

    private void HandleDataValidated(float _)
    {
      Action<BeatmapDifficulty, float, float> dataChangedEvent = this.beatmapDataChangedEvent;
      if (dataChangedEvent == null)
        return;
      dataChangedEvent(this._beatmapDifficulty, this._noteJumpMovementSpeedInputValidator.value, this._noteJumpStartBeatOffsetInputValidator.value);
    }
  }
}
