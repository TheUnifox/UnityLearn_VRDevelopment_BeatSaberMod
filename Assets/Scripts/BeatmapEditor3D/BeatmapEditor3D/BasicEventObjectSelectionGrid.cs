// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BasicEventObjectSelectionGrid
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using HMUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BasicEventObjectSelectionGrid : MonoBehaviour
  {
    [SerializeField]
    private BasicEventObjectHoverView _basicEventObjectHoverView;
    [Space]
    [SerializeField]
    private RectTransform _cellParent;
    [SerializeField]
    private RectTransform _canvasTransform;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private float _trackWidth;
    [SerializeField]
    private float _trackSpacing;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly EventObjectSelectionCellView.Pool _eventObjectSelectionCellViewPool;
    private EnvironmentTracksDefinitionSO.BasicEventTrackPage _currentPage;
    private Dictionary<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, EventObjectSelectionCellView> _spawnedEventObjectSelectionCells;
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();
    private EventObjectSelectionCellView _prevSelectedCell;

    protected void Start()
    {
      this._signalBus.Subscribe<EventsPageChangedSignal>(new Action<EventsPageChangedSignal>(this.HandleEventsPageChanged));
      this._signalBus.Subscribe<InteractionModeChangedSignal>(new Action(this.HandleInteractionModeChanged));
      this._signalBus.Subscribe<EventHoverUpdatedSignal>(new Action(this.HandleEventHoverUpdated));
      this._keyboardBinder.AddBinding(KeyCode.LeftAlt, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleAltPressed));
      this._keyboardBinder.AddBinding(KeyCode.LeftAlt, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleAltPressed));
      this._keyboardBinder.AddBinding(KeyCode.RightAlt, KeyboardBinder.KeyBindingType.KeyDown, new Action<bool>(this.HandleAltPressed));
      this._keyboardBinder.AddBinding(KeyCode.RightAlt, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleAltPressed));
      this._keyboardBinder.AddBinding(KeyCode.Escape, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleEscapePressed));
      this.SpawnCells();
    }

    protected void OnDestroy()
    {
      this._keyboardBinder.ClearBindings();
      this._signalBus.TryUnsubscribe<EventsPageChangedSignal>(new Action<EventsPageChangedSignal>(this.HandleEventsPageChanged));
      this._signalBus.TryUnsubscribe<InteractionModeChangedSignal>(new Action(this.HandleInteractionModeChanged));
      this._signalBus.TryUnsubscribe<EventHoverUpdatedSignal>(new Action(this.HandleEventHoverUpdated));
    }

    protected void Update() => this._keyboardBinder.ManualUpdate();

        private void SpawnCells()
        {
            this._currentPage = this._basicEventsState.currentEventsPage;
            List<EnvironmentTracksDefinitionSO.BasicEventTrackInfo> list = this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage];
            float x = (float)list.Count * this._trackWidth + (float)(list.Count - 1) * this._trackSpacing;
            Vector2 sizeDelta = this._canvasTransform.sizeDelta;
            sizeDelta.x = x;
            this._canvasTransform.sizeDelta = sizeDelta;
            this._spawnedEventObjectSelectionCells = new Dictionary<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, EventObjectSelectionCellView>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                EventObjectSelectionCellView eventObjectSelectionCellView = this._eventObjectSelectionCellViewPool.Spawn(i);
                this._spawnedEventObjectSelectionCells.Add(list[i], eventObjectSelectionCellView);
                eventObjectSelectionCellView.pointerEnterEvent += this.HandleEventObjectSelectionCellViewEnter;
                eventObjectSelectionCellView.pointerExitEvent += this.HandleEventObjectSelectionCellViewExit;
                eventObjectSelectionCellView.pointerUpEvent += this.HandleEventObjectSelectionCellViewUp;
                eventObjectSelectionCellView.transform.SetParent(this._cellParent, false);
            }
        }

        private void ClearCells()
    {
      foreach (KeyValuePair<EnvironmentTracksDefinitionSO.BasicEventTrackInfo, EventObjectSelectionCellView> objectSelectionCell in this._spawnedEventObjectSelectionCells)
      {
        objectSelectionCell.Value.pointerEnterEvent -= new Action<int>(this.HandleEventObjectSelectionCellViewEnter);
        objectSelectionCell.Value.pointerExitEvent -= new Action<int>(this.HandleEventObjectSelectionCellViewExit);
        objectSelectionCell.Value.pointerUpEvent -= new Action<int>(this.HandleEventObjectSelectionCellViewUp);
        this._eventObjectSelectionCellViewPool.Despawn(objectSelectionCell.Value);
      }
      this._spawnedEventObjectSelectionCells.Clear();
    }

    private void HandleEventHoverUpdated()
    {
      if ((UnityEngine.Object) this._prevSelectedCell != (UnityEngine.Object) null)
        this._prevSelectedCell.SetHighlighted(false);
      this._prevSelectedCell = this._spawnedEventObjectSelectionCells[this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage][this._basicEventsState.currentHoverPageTrackId]];
      this._prevSelectedCell.SetHighlighted(true);
      this._basicEventObjectHoverView.ShowPreview();
    }

    private void HandleEventObjectSelectionCellViewEnter(int index)
    {
      this._spawnedEventObjectSelectionCells[this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage][index]].SetHighlighted(true);
      this._basicEventObjectHoverView.ShowPreview();
    }

    private void HandleEventObjectSelectionCellViewExit(int index)
    {
      this._signalBus.Fire<UpdateHoverBeatAndTrackSignal>();
      this._spawnedEventObjectSelectionCells[this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage][index]].SetHighlighted(false);
      this._basicEventObjectHoverView.HidePreview();
    }

    private void HandleEventObjectSelectionCellViewUp(int index)
    {
      if (this._beatmapState.interactionMode != InteractionMode.Place)
        return;
      this._signalBus.Fire<PlaceEventSignal>(new PlaceEventSignal()
      {
        basicEventTrack = this._beatmapDataModel.environmentTrackDefinition[this._basicEventsState.currentEventsPage][index]
      });
    }

    private void HandleEventsPageChanged(EventsPageChangedSignal signal)
    {
      if (this._currentPage == signal.newPage)
        return;
      this.ClearCells();
      this.SpawnCells();
    }

    private void HandleInteractionModeChanged() => this._canvas.enabled = this._beatmapState.interactionMode == InteractionMode.Place;

    private void HandleEscapePressed(bool pressed)
    {
      if (this._beatmapState.interactionMode != InteractionMode.ClickSelect)
        return;
      this._signalBus.Fire<EndEventsSelectionSignal>(new EndEventsSelectionSignal());
    }

    private void HandleAltPressed(bool pressed) => this._canvas.enabled = !pressed && this._beatmapState.interactionMode != InteractionMode.ClickSelect;
  }
}
