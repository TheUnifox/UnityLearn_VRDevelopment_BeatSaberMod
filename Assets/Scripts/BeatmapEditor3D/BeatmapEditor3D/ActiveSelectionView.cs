// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ActiveSelectionView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Types;
using BeatmapEditor3D.Views;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace BeatmapEditor3D
{
  public class ActiveSelectionView : BeatmapEditorView
  {
    [SerializeField]
    private GameObject _parentGameObject;
    [Header("Beatmap Objects")]
    [SerializeField]
    private GameObject[] _beatmapObjectsGameObjects;
    [Space]
    [SerializeField]
    private TextMeshProUGUI _notesCountText;
    [SerializeField]
    private TextMeshProUGUI _obstaclesCountText;
    [SerializeField]
    private TextMeshProUGUI _bombsCountText;
    [SerializeField]
    private TextMeshProUGUI _chainsCountText;
    [SerializeField]
    private TextMeshProUGUI _arcsCountText;
    [Space]
    [SerializeField]
    private Button _mirrorBeatmapObjectsButton;
    [SerializeField]
    private Button _deleteBeatmapObjectsButton;
    [SerializeField]
    private Button _connectSelectedNotesWithSliderButton;
    [Header("Events")]
    [SerializeField]
    private GameObject[] _eventsGameObjects;
    [Space]
    [SerializeField]
    private TextMeshProUGUI _eventsCountText;
    [Space]
    [SerializeField]
    private Button _mirrorEventsButton;
    [SerializeField]
    private Button _deleteEventsButton;
    [Header("Event Box Groups")]
    [SerializeField]
    private GameObject[] _eventBoxGroupsGameObjects;
    [Space]
    [SerializeField]
    private TextMeshProUGUI _eventBoxGroupsCountText;
    [Space]
    [SerializeField]
    private Button _mirrorEventBoxGroupsButton;
    [SerializeField]
    private Button _deleteEventBoxGroupsButton;
    [Header("Event Boxes Events")]
    [SerializeField]
    private GameObject[] _eventBoxesEventsGameObjects;
    [Space]
    [SerializeField]
    private TextMeshProUGUI _eventBoxesEventsCountText;
    [Space]
    [SerializeField]
    private Button _mirrorEventBoxGroupsEventsButton;
    [SerializeField]
    private Button _deleteEventBoxesEventsButton;
    [Inject]
    private readonly IReadonlyBeatmapObjectsSelectionState _beatmapObjectsSelectionState;
    [Inject]
    private readonly IReadonlyEventsSelectionState _eventsSelectionState;
    [Inject]
    private readonly EventBoxGroupsSelectionState _eventBoxGroupsSelectionState;
    [Inject]
    private readonly EventBoxesSelectionState _eventBoxesSelectionState;
    [Inject]
    private readonly SignalBus _signalBus;

    public event Action mirrorSelectedBeatmapObjectsEvent;

    public event Action deleteSelectedBeatmapObjectsEvent;

    public event Action connectSelectedNotesWithSliderEvent;

    public event Action mirrorSelectedEventsEvent;

    public event Action deleteSelectedEventsEvent;

    public event Action mirrorSelectedEventBoxGroupsEvent;

    public event Action deleteSelectedEventBoxGroupsEvent;

    public event Action deleteSelectedEventBoxesEventsEvent;

    public event Action mirrorSelectedEventBoxesEventsEvent;

    protected override void DidActivate()
    {
      this._signalBus.Subscribe<BeatmapObjectsSelectionStateUpdatedSignal>(new Action(this.HandleBeatmapObjectsSelectionStateUpdated));
      this._signalBus.Subscribe<EventsSelectionStateUpdatedSignal>(new Action(this.HandleEventsSelectionStateUpdated));
      this._signalBus.Subscribe<EventBoxGroupsSelectionStateUpdatedSignal>(new Action(this.HandleEventBoxGroupsSelectionStateUpdated));
      this._signalBus.Subscribe<EventBoxesSelectionStateUpdatedSignal>(new Action(this.HandleEventBoxesSelectionStateUpdated));
      this._mirrorBeatmapObjectsButton.onClick.AddListener((UnityAction) (() =>
      {
        Action beatmapObjectsEvent = this.mirrorSelectedBeatmapObjectsEvent;
        if (beatmapObjectsEvent == null)
          return;
        beatmapObjectsEvent();
      }));
      this._deleteBeatmapObjectsButton.onClick.AddListener((UnityAction) (() =>
      {
        Action beatmapObjectsEvent = this.deleteSelectedBeatmapObjectsEvent;
        if (beatmapObjectsEvent == null)
          return;
        beatmapObjectsEvent();
      }));
      this._connectSelectedNotesWithSliderButton.onClick.AddListener((UnityAction) (() =>
      {
        Action notesWithSliderEvent = this.connectSelectedNotesWithSliderEvent;
        if (notesWithSliderEvent == null)
          return;
        notesWithSliderEvent();
      }));
      this._mirrorEventsButton.onClick.AddListener((UnityAction) (() =>
      {
        Action selectedEventsEvent = this.mirrorSelectedEventsEvent;
        if (selectedEventsEvent == null)
          return;
        selectedEventsEvent();
      }));
      this._deleteEventsButton.onClick.AddListener((UnityAction) (() =>
      {
        Action selectedEventsEvent = this.deleteSelectedEventsEvent;
        if (selectedEventsEvent == null)
          return;
        selectedEventsEvent();
      }));
      this._mirrorEventBoxGroupsButton.onClick.AddListener((UnityAction) (() =>
      {
        Action eventBoxGroupsEvent = this.mirrorSelectedEventBoxGroupsEvent;
        if (eventBoxGroupsEvent == null)
          return;
        eventBoxGroupsEvent();
      }));
      this._deleteEventBoxGroupsButton.onClick.AddListener((UnityAction) (() =>
      {
        Action eventBoxGroupsEvent = this.deleteSelectedEventBoxGroupsEvent;
        if (eventBoxGroupsEvent == null)
          return;
        eventBoxGroupsEvent();
      }));
      this._deleteEventBoxesEventsButton.onClick.AddListener((UnityAction) (() =>
      {
        Action boxesEventsEvent = this.deleteSelectedEventBoxesEventsEvent;
        if (boxesEventsEvent == null)
          return;
        boxesEventsEvent();
      }));
      this._mirrorEventBoxGroupsEventsButton.onClick.AddListener((UnityAction) (() =>
      {
        Action boxesEventsEvent = this.mirrorSelectedEventBoxesEventsEvent;
        if (boxesEventsEvent == null)
          return;
        boxesEventsEvent();
      }));
    }

    protected override void DidDeactivate()
    {
      this._signalBus.TryUnsubscribe<BeatmapObjectsSelectionStateUpdatedSignal>(new Action(this.HandleBeatmapObjectsSelectionStateUpdated));
      this._signalBus.TryUnsubscribe<EventsSelectionStateUpdatedSignal>(new Action(this.HandleEventsSelectionStateUpdated));
      this._signalBus.TryUnsubscribe<EventBoxGroupsSelectionStateUpdatedSignal>(new Action(this.HandleEventBoxGroupsSelectionStateUpdated));
      this._signalBus.TryUnsubscribe<EventBoxesSelectionStateUpdatedSignal>(new Action(this.HandleEventBoxesSelectionStateUpdated));
      this._mirrorBeatmapObjectsButton.onClick.RemoveAllListeners();
      this._deleteBeatmapObjectsButton.onClick.RemoveAllListeners();
      this._connectSelectedNotesWithSliderButton.onClick.RemoveAllListeners();
      this._mirrorEventsButton.onClick.RemoveAllListeners();
      this._deleteEventsButton.onClick.RemoveAllListeners();
      this._mirrorEventBoxGroupsButton.onClick.RemoveAllListeners();
      this._deleteEventBoxGroupsButton.onClick.RemoveAllListeners();
      this._deleteEventBoxesEventsButton.onClick.RemoveAllListeners();
      this._mirrorEventBoxGroupsEventsButton.onClick.RemoveAllListeners();
    }

    private void HandleBeatmapObjectsSelectionStateUpdated() => this.SetBeatmapObjectsData();

    private void HandleEventsSelectionStateUpdated() => this.SetEventsData();

    private void HandleEventBoxGroupsSelectionStateUpdated() => this.SetEventBoxGroupsData();

    private void HandleEventBoxesSelectionStateUpdated() => this.SetEventBoxesEventsData();

    public void SetMode(BeatmapEditingMode levelEditMode) => this.SetData(levelEditMode);

    private void SetData(BeatmapEditingMode levelEditMode)
    {
      foreach (GameObject objectsGameObject in this._beatmapObjectsGameObjects)
        objectsGameObject.SetActive(levelEditMode == BeatmapEditingMode.Objects);
      foreach (GameObject eventsGameObject in this._eventsGameObjects)
        eventsGameObject.SetActive(levelEditMode == BeatmapEditingMode.BasicEvents);
      foreach (GameObject groupsGameObject in this._eventBoxGroupsGameObjects)
        groupsGameObject.SetActive(levelEditMode == BeatmapEditingMode.EventBoxGroups);
      foreach (GameObject eventsGameObject in this._eventBoxesEventsGameObjects)
        eventsGameObject.SetActive(levelEditMode == BeatmapEditingMode.EventBoxes);
      switch (levelEditMode)
      {
        case BeatmapEditingMode.Objects:
          this.SetBeatmapObjectsData();
          break;
        case BeatmapEditingMode.BasicEvents:
          this.SetEventsData();
          break;
        case BeatmapEditingMode.EventBoxGroups:
          this.SetEventBoxGroupsData();
          break;
        case BeatmapEditingMode.EventBoxes:
          this.SetEventBoxesEventsData();
          break;
      }
    }

    private void SetBeatmapObjectsData()
    {
      this._notesCountText.text = string.Format("{0}", (object) this._beatmapObjectsSelectionState.notes.Count);
      this._obstaclesCountText.text = string.Format("{0}", (object) this._beatmapObjectsSelectionState.obstacles.Count);
      this._bombsCountText.text = string.Format("{0}", (object) this._beatmapObjectsSelectionState.waypoints.Count);
      this._chainsCountText.text = string.Format("{0}", (object) this._beatmapObjectsSelectionState.chains.Count);
      this._arcsCountText.text = string.Format("{0}", (object) this._beatmapObjectsSelectionState.arcs.Count);
      this._connectSelectedNotesWithSliderButton.interactable = this._beatmapObjectsSelectionState.notes.Count + this._beatmapObjectsSelectionState.chains.Count == 2;
      this._parentGameObject.SetActive(this._beatmapObjectsSelectionState.IsAnythingSelected());
    }

    private void SetEventsData()
    {
      this._eventsCountText.text = string.Format("{0}", (object) this._eventsSelectionState.events.Count);
      this._parentGameObject.SetActive(this._eventsSelectionState.IsAnythingSelected());
    }

    private void SetEventBoxGroupsData()
    {
      this._eventBoxGroupsCountText.text = this._eventBoxGroupsSelectionState.eventBoxGroups.Count.ToString();
      this._parentGameObject.SetActive(this._eventBoxGroupsSelectionState.IsAnythingSelected());
    }

    private void SetEventBoxesEventsData()
    {
      this._eventBoxesEventsCountText.text = string.Format("{0}", (object) this._eventBoxesSelectionState.events.Count);
      this._parentGameObject.SetActive(this._eventBoxesSelectionState.IsAnythingSelected());
    }
  }
}
