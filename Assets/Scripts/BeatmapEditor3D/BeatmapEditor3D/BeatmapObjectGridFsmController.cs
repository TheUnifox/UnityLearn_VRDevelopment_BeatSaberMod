// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectGridFsmController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using BeatmapEditor3D.Views;
using HMUI;
using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapObjectGridFsmController : MonoBehaviour
  {
    [Inject]
    private readonly BeatmapObjectGridHoverView _beatmapObjectGridHoverView;
    [Inject]
    private readonly BeatmapObjectEditGridView _beatmapObjectEditGridView;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly IReadonlyBeatmapObjectsState _readonlyBeatmapObjectsState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly BeatmapObjectGridFsm _beatmapObjectGridFsm;
    [Inject]
    private readonly PlaceNoteBeatmapObjectGridFsmState.Factory _placeNoteBeatmapObjectGridFsmStateFactory;
    [Inject]
    private readonly PlaceBombBeatmapObjectGridFsmState.Factory _placeBombBeatmapObjectGridFsmStateFactory;
    [Inject]
    private readonly PlaceObstacleBeatmapObjectGridFsmState.Factory _placeObstacleBeatmapObjectGridFsmStateFactory;
    [Inject]
    private readonly PlaceArcBeatmapObjectGridFsmState.Factory _placeArcBeatmapObjectGridFsmStateFactory;
    [Inject]
    private readonly BeatmapObjectGridFsmStateHidden.Factory _beatmapObjectGridFsmStateHidden;
    [Inject]
    private readonly MoveBeatmapObjectOnGridFsmState.Factory _moveBeatmapObjectOnGridFsmState;
    private readonly MouseBinder _mouseBinder = new MouseBinder();
    private readonly KeyboardBinder _keyboardBinder = new KeyboardBinder();

    protected void Start()
    {
      this._mouseBinder.AddButtonBinding(MouseBinder.ButtonType.Primary, MouseBinder.MouseEventType.ButtonDown, new UnityAction(this._beatmapObjectGridFsm.HandleMousePointerDown));
      this._mouseBinder.AddButtonBinding(MouseBinder.ButtonType.Primary, MouseBinder.MouseEventType.ButtonUp, new UnityAction(this._beatmapObjectGridFsm.HandleMousePointerUp));
      this._mouseBinder.AddScrollBinding(new UnityAction<float>(this._beatmapObjectGridFsm.HandleMouseScroll));
      this._mouseBinder.AddScrollBinding(new UnityAction<float>(this.HandleMouseScroll));
      this._keyboardBinder.AddBinding(KeyCode.Escape, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this._beatmapObjectGridFsm.HandleCancelAction));
      this._keyboardBinder.AddBinding(KeyCode.Escape, KeyboardBinder.KeyBindingType.KeyUp, new Action<bool>(this.HandleEscapePressed));
      this._signalBus.Subscribe<BeatmapObjectTypeChangedSignal>(new Action<BeatmapObjectTypeChangedSignal>(this._beatmapObjectGridFsm.HandleBeatmapObjectTypeChanged));
      this._signalBus.Subscribe<InteractionModeChangedSignal>(new Action<InteractionModeChangedSignal>(this._beatmapObjectGridFsm.HandleBeatmapObjectModeChanged));
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action<BeatmapEditingModeSwitched>(this._beatmapObjectGridFsm.HandleLevelEditorModeSwitched));
      this._signalBus.Subscribe<BeatmapLevelStateTimeUpdated>(new Action(this._beatmapObjectGridFsm.HandleBeatmapLevelStateTimeUpdated));
      this._signalBus.Subscribe<BeatmapTimeScaleChangedSignal>(new Action(this._beatmapObjectGridFsm.HandleBeatmapTimeScaleChanged));
      this._signalBus.Subscribe<LevelEditorStateZenModeUpdatedSignal>(new Action<LevelEditorStateZenModeUpdatedSignal>(this._beatmapObjectGridFsm.HandleLevelEditorStateZenModeUpdated));
      this._signalBus.Subscribe<ObstacleDurationChangedSignal>(new Action(this._beatmapObjectGridFsm.HandleObstacleDurationChanged));
      this._signalBus.Subscribe<BeatmapObjectTypeChangedSignal>(new Action<BeatmapObjectTypeChangedSignal>(this.HandleBeatmapObjectTypeChanged));
      this._signalBus.Subscribe<InteractionModeChangedSignal>(new Action(this.HandleBeatmapObjectModeChanged));
      this._signalBus.Subscribe<LevelEditorStateZenModeUpdatedSignal>(new Action(this.HandleLevelEditorStateZenModeUpdated));
      this._beatmapObjectEditGridView.pointerDownEvent += new Action<int, int>(this._beatmapObjectGridFsm.HandleGridCellPointerDown);
      this._beatmapObjectEditGridView.pointerUpEvent += new Action<int, int>(this._beatmapObjectGridFsm.HandleGridCellPointerUp);
      this._beatmapObjectEditGridView.pointerEnterEvent += new Action<int, int>(this._beatmapObjectGridFsm.HandleGridCellPointerEnter);
      this._beatmapObjectEditGridView.pointerExitEvent += new Action<int, int>(this._beatmapObjectGridFsm.HandleGridCellPointerExit);
      this._beatmapObjectGridHoverView.SetObstacleData(0.0f, this._readonlyBeatmapObjectsState.obstacleDuration);
      this.SwitchBeatmapObjectGridFsmStateToHidden();
    }

    protected void OnDestroy()
    {
      this._mouseBinder.ClearBindings();
      this._keyboardBinder.ClearBindings();
      this._signalBus.TryUnsubscribe<BeatmapObjectTypeChangedSignal>(new Action<BeatmapObjectTypeChangedSignal>(this._beatmapObjectGridFsm.HandleBeatmapObjectTypeChanged));
      this._signalBus.TryUnsubscribe<InteractionModeChangedSignal>(new Action<InteractionModeChangedSignal>(this._beatmapObjectGridFsm.HandleBeatmapObjectModeChanged));
      this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action<BeatmapEditingModeSwitched>(this._beatmapObjectGridFsm.HandleLevelEditorModeSwitched));
      this._signalBus.TryUnsubscribe<BeatmapLevelStateTimeUpdated>(new Action(this._beatmapObjectGridFsm.HandleBeatmapLevelStateTimeUpdated));
      this._signalBus.TryUnsubscribe<BeatmapTimeScaleChangedSignal>(new Action(this._beatmapObjectGridFsm.HandleBeatmapTimeScaleChanged));
      this._signalBus.TryUnsubscribe<LevelEditorStateZenModeUpdatedSignal>(new Action<LevelEditorStateZenModeUpdatedSignal>(this._beatmapObjectGridFsm.HandleLevelEditorStateZenModeUpdated));
      this._signalBus.TryUnsubscribe<ObstacleDurationChangedSignal>(new Action(this._beatmapObjectGridFsm.HandleObstacleDurationChanged));
      this._signalBus.TryUnsubscribe<BeatmapObjectTypeChangedSignal>(new Action<BeatmapObjectTypeChangedSignal>(this.HandleBeatmapObjectTypeChanged));
      this._beatmapObjectEditGridView.pointerDownEvent -= new Action<int, int>(this._beatmapObjectGridFsm.HandleGridCellPointerDown);
      this._beatmapObjectEditGridView.pointerUpEvent -= new Action<int, int>(this._beatmapObjectGridFsm.HandleGridCellPointerUp);
      this._beatmapObjectEditGridView.pointerEnterEvent -= new Action<int, int>(this._beatmapObjectGridFsm.HandleGridCellPointerEnter);
      this._beatmapObjectEditGridView.pointerExitEvent -= new Action<int, int>(this._beatmapObjectGridFsm.HandleGridCellPointerExit);
    }

    protected void Update()
    {
      this._mouseBinder.ManualUpdate();
      this._keyboardBinder.ManualUpdate();
      this._beatmapObjectGridFsm?.Update();
    }

    private void HandleBeatmapObjectTypeChanged(BeatmapObjectTypeChangedSignal signal) => this.SwitchBeatmapObjectGridFsmStateToHidden();

    private void HandleBeatmapObjectModeChanged() => this.SwitchBeatmapObjectGridFsmStateToHidden();

    private void HandleLevelEditorStateZenModeUpdated() => this.SwitchBeatmapObjectGridFsmStateToHidden();

    private void HandleMouseScroll(float delta)
    {
      if (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
        return;
      this._signalBus.Fire<ChangeHoveredObstacleDurationSignal>(new ChangeHoveredObstacleDurationSignal((int) delta));
    }

    private void HandleEscapePressed(bool pressed)
    {
      if (this._beatmapState.interactionMode != InteractionMode.ClickSelect)
        return;
      this._signalBus.Fire<EndBeatmapObjectsSelectionSignal>(new EndBeatmapObjectsSelectionSignal(0.0f, false));
    }

    private void SwitchBeatmapObjectGridFsmStateToHidden()
    {
      if ((this._beatmapEditorSettingsDataModel.zenMode || this._beatmapState.editingMode != BeatmapEditingMode.Objects ? 1 : (this._beatmapState.interactionMode == InteractionMode.Delete ? 1 : 0)) != 0)
        this._beatmapObjectGridFsm.SwitchState((IBeatmapObjectGridFsmState) this._beatmapObjectGridFsmStateHidden.Create());
      else
        this.SwitchBeatmapObjectGridFsmState();
    }

    private void SwitchBeatmapObjectGridFsmState()
    {
      if (this._beatmapState.interactionMode == InteractionMode.Modify)
      {
        this._beatmapObjectGridFsm.SwitchState((IBeatmapObjectGridFsmState) this._moveBeatmapObjectOnGridFsmState.Create());
      }
      else
      {
        switch (this._readonlyBeatmapObjectsState.beatmapObjectType)
        {
          case BeatmapObjectType.Note:
            this._beatmapObjectGridFsm.SwitchState((IBeatmapObjectGridFsmState) this._placeNoteBeatmapObjectGridFsmStateFactory.Create());
            break;
          case BeatmapObjectType.Bomb:
            this._beatmapObjectGridFsm.SwitchState((IBeatmapObjectGridFsmState) this._placeBombBeatmapObjectGridFsmStateFactory.Create());
            break;
          case BeatmapObjectType.Obstacle:
            this._beatmapObjectGridFsm.SwitchState((IBeatmapObjectGridFsmState) this._placeObstacleBeatmapObjectGridFsmStateFactory.Create());
            break;
          case BeatmapObjectType.Arc:
            this._beatmapObjectGridFsm.SwitchState((IBeatmapObjectGridFsmState) this._placeArcBeatmapObjectGridFsmStateFactory.Create());
            break;
        }
      }
    }
  }
}
