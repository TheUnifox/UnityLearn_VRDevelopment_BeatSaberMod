// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.EventBoxesView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.DataModels;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class EventBoxesView : MonoBehaviour
  {
    [SerializeField]
    private EventBoxView _eventBoxView;
    [Space]
    [SerializeField]
    private ScrollView _eventBoxButtonsScrollView;
    [SerializeField]
    private TextSegmentedControl _eventBoxButtonsTextSegmentedControl;
    [SerializeField]
    private float _segmentedControlCellSize = 40f;
    [Space]
    [SerializeField]
    private Button _addEventBoxButton;
    [SerializeField]
    private Button _superAddEventBoxButton;
    [SerializeField]
    private Button _deleteEventBoxButton;
    [SerializeField]
    private Button _pruneEventBoxesButton;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();
    private List<EventBoxEditorData> _eventBoxes;
    private int _activeEventBoxIdx;

    protected void OnEnable()
    {
      if (this._eventBoxGroupsState.eventBoxGroupContext == (EventBoxGroupEditorData) null)
        return;
      this._eventBoxes = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupsState.eventBoxGroupContext.id);
      this._activeEventBoxIdx = 0;
      this._addEventBoxButton.onClick.AddListener(new UnityAction(this.HandleAddEventBoxButtonClick));
      this._superAddEventBoxButton.onClick.AddListener(new UnityAction(this.HandleSuperAddEventBoxButtonClick));
      this._deleteEventBoxButton.onClick.AddListener(new UnityAction(this.HandleDeleteEventBoxButtonClick));
      this._pruneEventBoxesButton.onClick.AddListener(new UnityAction(this.HandlePruneEventBoxesButtonClick));
      this._eventBoxButtonsTextSegmentedControl.didSelectCellEvent += new Action<SegmentedControl, int>(this.HandleEventBoxButtonsTextSegmentedControlDidSelectCell);
      this._eventBoxView.saveEventBoxEvent += new Action<EventBoxEditorData>(this.HandleEventBoxViewSaveEventBox);
      this.DisplayEventBoxes();
      this.ToggleAlternativeButtons(Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift));
      this._signalBus.Subscribe<EventBoxesUpdatedSignal>(new Action<EventBoxesUpdatedSignal>(this.HandleEventBoxesUpdated));
      this._signalBus.Subscribe<EditingEventBoxGroupChangedSignal>(new Action(this.HandleEditingEventBoxGroupChanged));
      this._keyboardBinder.AddBinding(KeyCode.PageUp, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.MoveCurrentEventBoxUp));
      this._keyboardBinder.AddBinding(KeyCode.PageDown, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.MoveCurrentEventBoxDown));
      this._keyboardBinder.AddBinding(KeyCode.LeftShift, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.ToggleAlternativeButtons));
      this._keyboardBinder.AddBinding(KeyCode.RightShift, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.ToggleAlternativeButtons));
      this._keyboardBinder.AddBinding(KeyCode.LeftShift, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.ToggleAlternativeButtons));
      this._keyboardBinder.AddBinding(KeyCode.RightShift, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.ToggleAlternativeButtons));
    }

    protected void OnDisable()
    {
      this._addEventBoxButton.onClick.RemoveListener(new UnityAction(this.HandleAddEventBoxButtonClick));
      this._superAddEventBoxButton.onClick.RemoveListener(new UnityAction(this.HandleSuperAddEventBoxButtonClick));
      this._deleteEventBoxButton.onClick.RemoveListener(new UnityAction(this.HandleDeleteEventBoxButtonClick));
      this._pruneEventBoxesButton.onClick.RemoveListener(new UnityAction(this.HandlePruneEventBoxesButtonClick));
      this._eventBoxButtonsTextSegmentedControl.didSelectCellEvent -= new Action<SegmentedControl, int>(this.HandleEventBoxButtonsTextSegmentedControlDidSelectCell);
      this._eventBoxView.saveEventBoxEvent -= new Action<EventBoxEditorData>(this.HandleEventBoxViewSaveEventBox);
      this._signalBus.TryUnsubscribe<EventBoxesUpdatedSignal>(new Action<EventBoxesUpdatedSignal>(this.HandleEventBoxesUpdated));
      this._signalBus.TryUnsubscribe<EditingEventBoxGroupChangedSignal>(new Action(this.HandleEditingEventBoxGroupChanged));
      this._keyboardBinder.ClearBindings();
    }

    protected void Update() => this._keyboardBinder.ManualUpdate();

    private void DisplayEventBoxes(int? eventBoxIdOverride = null)
    {
      this._eventBoxButtonsTextSegmentedControl.SetTexts((IReadOnlyList<string>) Enumerable.Range(0, this._eventBoxes.Count).Select<int, string>((Func<int, string>) (i => i.ToString())).ToList<string>());
      this.SetEventBoxData(eventBoxIdOverride);
    }

    private void SetEventBoxData(int? eventBoxIdOverride = null)
    {
      this._activeEventBoxIdx = Mathf.Clamp(eventBoxIdOverride ?? this._activeEventBoxIdx, 0, this._eventBoxes.Count - 1);
      this._eventBoxView.gameObject.SetActive(this._eventBoxes.Count > 0);
      if (this._eventBoxes.Count == 0)
        return;
      EventBoxEditorData eventBox = this._eventBoxes[this._activeEventBoxIdx];
      int groupSize;
      if (!this._beatmapEventBoxGroupsDataModel.TryGetGroupSizeByEventBoxGroupId(this._eventBoxGroupsState.eventBoxGroupContext.groupId, out groupSize))
        return;
      switch (eventBox)
      {
        case LightColorEventBoxEditorData lightColorEventBox:
          this._eventBoxView.SetData(this._eventBoxGroupsState.eventBoxGroupContext.beat, lightColorEventBox, groupSize);
          break;
        case LightRotationEventBoxEditorData lightRotationEventBox:
          this._eventBoxView.SetData(this._eventBoxGroupsState.eventBoxGroupContext.beat, lightRotationEventBox, groupSize);
          break;
        case LightTranslationEventBoxEditorData lightTranslationEventBox:
          this._eventBoxView.SetData(this._eventBoxGroupsState.eventBoxGroupContext.beat, lightTranslationEventBox, groupSize);
          break;
      }
      this.ScrollAndSelectCell();
    }

    private void HandleEventBoxesUpdated(EventBoxesUpdatedSignal signal)
    {
      this._eventBoxes = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupsState.eventBoxGroupContext.id);
      this.DisplayEventBoxes(new int?(signal.newEventBoxId));
    }

    private void HandleEditingEventBoxGroupChanged()
    {
      this._eventBoxes = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupsState.eventBoxGroupContext.id);
      this.DisplayEventBoxes(new int?(0));
    }

    private void HandleEventBoxViewSaveEventBox(EventBoxEditorData modifiedEventBox) => this._signalBus.Fire<ModifyEventBoxSignal>(new ModifyEventBoxSignal(modifiedEventBox));

    private void HandleDeleteEventBoxButtonClick() => this._signalBus.Fire<DeleteEventBoxSignal>(new DeleteEventBoxSignal(this._eventBoxes[this._activeEventBoxIdx]));

    private void HandlePruneEventBoxesButtonClick() => this._signalBus.Fire<PruneEventBoxesSignal>();

    private void HandleAddEventBoxButtonClick() => this._signalBus.Fire<InsertEventBoxSignal>();

    private void HandleSuperAddEventBoxButtonClick() => this._signalBus.Fire<SuperInsertEventBoxSignal>();

    private void MoveCurrentEventBoxUp(bool _)
    {
      this._activeEventBoxIdx = Mathf.Clamp(this._activeEventBoxIdx - 1, 0, this._eventBoxes.Count - 1);
      this.SetEventBoxData();
    }

    private void MoveCurrentEventBoxDown(bool _)
    {
      this._activeEventBoxIdx = Mathf.Clamp(this._activeEventBoxIdx + 1, 0, this._eventBoxes.Count - 1);
      this.SetEventBoxData();
    }

    private void ToggleAlternativeButtons(bool enableAlternativeButtons)
    {
      this._addEventBoxButton.gameObject.SetActive(!enableAlternativeButtons);
      this._superAddEventBoxButton.gameObject.SetActive(enableAlternativeButtons);
      this._deleteEventBoxButton.gameObject.SetActive(!enableAlternativeButtons);
      this._pruneEventBoxesButton.gameObject.SetActive(enableAlternativeButtons);
    }

    private void ScrollAndSelectCell()
    {
      this._eventBoxButtonsTextSegmentedControl.SelectCellWithNumber(this._activeEventBoxIdx);
      this._eventBoxButtonsScrollView.ScrollTo((float) this._activeEventBoxIdx * this._segmentedControlCellSize, true);
    }

    private void HandleEventBoxButtonsTextSegmentedControlDidSelectCell(SegmentedControl _, int idx)
    {
      if (this._activeEventBoxIdx == idx)
        return;
      this._activeEventBoxIdx = idx;
      this.SetEventBoxData();
    }
  }
}
