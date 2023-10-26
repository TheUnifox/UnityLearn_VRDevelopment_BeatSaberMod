// Decompiled with JetBrains decompiler
// Type: VRUIControls.VRInputModule
// Assembly: VRUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3BA7334A-77F9-4425-988C-69CB2EE35CCF
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\VRUI.dll

using HMUI;
using Libraries.HM.HMLib.VR;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace VRUIControls
{
  public class VRInputModule : BaseInputModule
  {
    [SerializeField]
    protected VRPointer _vrPointer;
    [SerializeField]
    protected HapticPresetSO _rumblePreset;
    [Inject]
    protected readonly HapticFeedbackController _hapticFeedbackController;
    [CompilerGenerated]
    protected bool useMouseForPressInput_k__BackingField;
    protected const int kMouseLeftId = -1;
    protected const float kMinPressValue = 0.9f;
    protected readonly Dictionary<int, PointerEventData> _pointerData = new Dictionary<int, PointerEventData>();
    protected readonly List<Component> _componentList = new List<Component>(20);
    protected readonly MouseState _mouseState = new MouseState();
    [DoesNotRequireDomainReloadInit]
    protected static readonly Comparison<RaycastResult> _raycastComparer = new Comparison<RaycastResult>(VRInputModule.RaycastComparer);

    public bool useMouseForPressInput
    {
      get => this.useMouseForPressInput_k__BackingField;
      set => this.useMouseForPressInput_k__BackingField = value;
    }

    public event Action<GameObject> onProcessMousePressEvent;

    protected override void OnDisable()
    {
      base.OnDisable();
      this.ClearSelection();
    }

    public virtual bool GetPointerData(int id, out PointerEventData data, bool create)
    {
      if (!(!this._pointerData.TryGetValue(id, out data) & create))
        return false;
      data = new PointerEventData(this.eventSystem)
      {
        pointerId = id
      };
      this._pointerData.Add(id, data);
      return true;
    }

    protected virtual MouseState GetMousePointerEventData(int id)
    {
      PointerEventData data;
      int num1 = this.GetPointerData(-1, out data, true) ? 1 : 0;
      data.Reset();
      data.button = PointerEventData.InputButton.Left;
      VRController vrController = this._vrPointer.vrController;
      if (vrController.active)
      {
        data.pointerCurrentRaycast = new RaycastResult()
        {
          worldPosition = vrController.position,
          worldNormal = vrController.forward
        };
        Vector2 vector2 = new Vector2(vrController.horizontalAxisValue, vrController.verticalAxisValue) * 1f;
        vector2.x *= -1f;
        data.scrollDelta = vector2;
      }
      this.eventSystem.RaycastAll(data, this.m_RaycastResultCache);
      this.m_RaycastResultCache.Sort(VRInputModule._raycastComparer);
      RaycastResult firstRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
      data.pointerCurrentRaycast = firstRaycast;
      this.m_RaycastResultCache.Clear();
      Vector2 screenPosition = firstRaycast.screenPosition;
      data.delta = num1 == 0 ? screenPosition - data.position : new Vector2(0.0f, 0.0f);
      data.position = screenPosition;
      PointerEventData.FramePressState stateForMouseButton = PointerEventData.FramePressState.NotChanged;
      if (vrController.active)
      {
        float num2 = !this.enabled ? 0.0f : (!this.useMouseForPressInput ? vrController.triggerValue : (Input.GetMouseButton(0) ? 1f : 0.0f));
        ButtonState buttonState = this._mouseState.GetButtonState(PointerEventData.InputButton.Left);
        if ((double) buttonState.pressedValue < 0.89999997615814209 && (double) num2 >= 0.89999997615814209)
          stateForMouseButton = PointerEventData.FramePressState.Pressed;
        else if ((double) buttonState.pressedValue >= 0.89999997615814209 && (double) num2 < 0.89999997615814209)
          stateForMouseButton = PointerEventData.FramePressState.Released;
        buttonState.pressedValue = num2;
      }
      this._mouseState.SetButtonState(PointerEventData.InputButton.Left, stateForMouseButton, data);
      return this._mouseState;
    }

    public virtual PointerEventData GetLastPointerEventData(int id)
    {
      PointerEventData data;
      this.GetPointerData(id, out data, false);
      return data;
    }

    public virtual bool ShouldStartDrag(
      Vector2 pressPos,
      Vector2 currentPos,
      float threshold,
      bool useDragThreshold)
    {
      return !useDragThreshold || (double) (pressPos - currentPos).sqrMagnitude >= (double) threshold * (double) threshold;
    }

    protected virtual void ProcessMove(PointerEventData pointerEvent)
    {
      GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
      this.HandlePointerExitAndEnter(pointerEvent, gameObject);
    }

    protected virtual void ProcessDrag(PointerEventData pointerEvent)
    {
      bool flag = pointerEvent.IsPointerMoving();
      if (flag && (UnityEngine.Object) pointerEvent.pointerDrag != (UnityEngine.Object) null && !pointerEvent.dragging && this.ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, (float) this.eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
      {
        ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.pointerDrag, (BaseEventData) pointerEvent, ExecuteEvents.beginDragHandler);
        pointerEvent.dragging = true;
      }
      if (!(pointerEvent.dragging & flag) || !((UnityEngine.Object) pointerEvent.pointerDrag != (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) pointerEvent.pointerPress != (UnityEngine.Object) pointerEvent.pointerDrag)
      {
        ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, (BaseEventData) pointerEvent, ExecuteEvents.pointerUpHandler);
        pointerEvent.eligibleForClick = false;
        pointerEvent.pointerPress = (GameObject) null;
        pointerEvent.rawPointerPress = (GameObject) null;
      }
      ExecuteEvents.Execute<IDragHandler>(pointerEvent.pointerDrag, (BaseEventData) pointerEvent, ExecuteEvents.dragHandler);
    }

    public override bool IsPointerOverGameObject(int pointerId)
    {
      PointerEventData pointerEventData = this.GetLastPointerEventData(pointerId);
      return pointerEventData != null && (UnityEngine.Object) pointerEventData.pointerEnter != (UnityEngine.Object) null;
    }

    public virtual void ClearSelection()
    {
      BaseEventData baseEventData = this.GetBaseEventData();
      foreach (PointerEventData currentPointerData in this._pointerData.Values)
        this.HandlePointerExitAndEnter(currentPointerData, (GameObject) null);
      this._pointerData.Clear();
      if (!((UnityEngine.Object) this.eventSystem != (UnityEngine.Object) null))
        return;
      this.eventSystem.SetSelectedGameObject((GameObject) null, baseEventData);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder("<b>Pointer Input Module of type: </b>" + (object) this.GetType());
      stringBuilder.AppendLine();
      foreach (KeyValuePair<int, PointerEventData> keyValuePair in this._pointerData)
      {
        if (keyValuePair.Value != null)
        {
          stringBuilder.AppendLine("<B>Pointer:</b> " + (object) keyValuePair.Key);
          stringBuilder.AppendLine(keyValuePair.Value.ToString());
        }
      }
      return stringBuilder.ToString();
    }

    public virtual void DeselectIfSelectionChanged(
      GameObject currentOverGo,
      BaseEventData pointerEvent)
    {
      GameObject eventHandler = ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo);
      if ((UnityEngine.Object) eventHandler == (UnityEngine.Object) this.eventSystem.currentSelectedGameObject)
        return;
      this.eventSystem.SetSelectedGameObject(eventHandler, pointerEvent);
    }

    public override void Process()
    {
      MouseButtonEventData eventData = this.GetMousePointerEventData(0).GetButtonState(PointerEventData.InputButton.Left).eventData;
      this.ProcessMousePress(eventData);
      this.ProcessMove(eventData.buttonData);
      this.ProcessDrag(eventData.buttonData);
      if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0.0f))
        ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), (BaseEventData) eventData.buttonData, ExecuteEvents.scrollHandler);
      this._vrPointer.Process(eventData.buttonData);
    }

    public virtual bool SendUpdateEventToSelectedObject()
    {
      if ((UnityEngine.Object) this.eventSystem.currentSelectedGameObject == (UnityEngine.Object) null)
        return false;
      BaseEventData baseEventData = this.GetBaseEventData();
      ExecuteEvents.Execute<IUpdateSelectedHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
      return baseEventData.used;
    }

    public virtual void ProcessMousePress(MouseButtonEventData data)
    {
      PointerEventData buttonData = data.buttonData;
      GameObject gameObject1 = buttonData.pointerCurrentRaycast.gameObject;
      bool flag = false;
      if (data.PressedThisFrame())
      {
        buttonData.eligibleForClick = true;
        buttonData.delta = Vector2.zero;
        buttonData.dragging = false;
        buttonData.useDragThreshold = true;
        buttonData.pressPosition = buttonData.position;
        buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
        this.DeselectIfSelectionChanged(gameObject1, (BaseEventData) buttonData);
        GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData) buttonData, ExecuteEvents.pointerDownHandler);
        if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null)
          gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
        if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
        {
          Action<GameObject> processMousePressEvent = this.onProcessMousePressEvent;
          if (processMousePressEvent != null)
            processMousePressEvent(gameObject1);
        }
        float unscaledTime = Time.unscaledTime;
        if ((UnityEngine.Object) gameObject2 == (UnityEngine.Object) buttonData.lastPress)
        {
          if ((double) unscaledTime - (double) buttonData.clickTime < 0.30000001192092896)
            ++buttonData.clickCount;
          else
            buttonData.clickCount = 1;
          buttonData.clickTime = unscaledTime;
        }
        else
          buttonData.clickCount = 1;
        buttonData.pointerPress = gameObject2;
        buttonData.rawPointerPress = gameObject1;
        buttonData.clickTime = unscaledTime;
        buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1);
        if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null)
          ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, (BaseEventData) buttonData, ExecuteEvents.initializePotentialDrag);
        buttonData.eligibleForClick = true;
        flag = (UnityEngine.Object) buttonData.pointerPress != (UnityEngine.Object) null && buttonData.pointerPress.GetComponent<IPointerClickHandler>() != null;
      }
      if (!(data.ReleasedThisFrame() | flag))
        return;
      ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, (BaseEventData) buttonData, ExecuteEvents.pointerUpHandler);
      GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
      if ((UnityEngine.Object) buttonData.pointerPress == (UnityEngine.Object) eventHandler && buttonData.eligibleForClick)
        ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, (BaseEventData) buttonData, ExecuteEvents.pointerClickHandler);
      else if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null && buttonData.dragging)
        ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject1, (BaseEventData) buttonData, ExecuteEvents.dropHandler);
      this.eventSystem.SetSelectedGameObject((GameObject) null, (BaseEventData) null);
      buttonData.eligibleForClick = false;
      buttonData.pointerPress = (GameObject) null;
      buttonData.rawPointerPress = (GameObject) null;
      if ((UnityEngine.Object) buttonData.pointerDrag != (UnityEngine.Object) null && buttonData.dragging)
        ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, (BaseEventData) buttonData, ExecuteEvents.endDragHandler);
      buttonData.dragging = false;
      buttonData.pointerDrag = (GameObject) null;
      if (!((UnityEngine.Object) gameObject1 != (UnityEngine.Object) buttonData.pointerEnter))
        return;
      this.HandlePointerExitAndEnter(buttonData, (GameObject) null);
      this.HandlePointerExitAndEnter(buttonData, gameObject1);
    }

    public new virtual void HandlePointerExitAndEnter(
      PointerEventData currentPointerData,
      GameObject newEnterTarget)
    {
      if ((UnityEngine.Object) newEnterTarget == (UnityEngine.Object) null || (UnityEngine.Object) currentPointerData.pointerEnter == (UnityEngine.Object) null)
      {
        for (int index = 0; index < currentPointerData.hovered.Count; ++index)
          ExecuteEvents.Execute<IPointerExitHandler>(currentPointerData.hovered[index], (BaseEventData) currentPointerData, ExecuteEvents.pointerExitHandler);
        currentPointerData.hovered.Clear();
        if ((UnityEngine.Object) newEnterTarget == (UnityEngine.Object) null)
        {
          currentPointerData.pointerEnter = newEnterTarget;
          return;
        }
      }
      if ((UnityEngine.Object) currentPointerData.pointerEnter == (UnityEngine.Object) newEnterTarget && (bool) (UnityEngine.Object) newEnterTarget)
        return;
      GameObject commonRoot = BaseInputModule.FindCommonRoot(currentPointerData.pointerEnter, newEnterTarget);
      if ((UnityEngine.Object) currentPointerData.pointerEnter != (UnityEngine.Object) null)
      {
        for (Transform transform = currentPointerData.pointerEnter.transform; (UnityEngine.Object) transform != (UnityEngine.Object) null && (!((UnityEngine.Object) commonRoot != (UnityEngine.Object) null) || !((UnityEngine.Object) commonRoot.transform == (UnityEngine.Object) transform)); transform = transform.parent)
        {
          ExecuteEvents.Execute<IPointerExitHandler>(transform.gameObject, (BaseEventData) currentPointerData, ExecuteEvents.pointerExitHandler);
          currentPointerData.hovered.Remove(transform.gameObject);
        }
      }
      if (!this.enabled)
        return;
      currentPointerData.pointerEnter = newEnterTarget;
      if (!((UnityEngine.Object) newEnterTarget != (UnityEngine.Object) null))
        return;
      Transform transform1 = newEnterTarget.transform;
      bool flag1 = false;
      bool flag2 = false;
      for (; (UnityEngine.Object) transform1 != (UnityEngine.Object) null; transform1 = transform1.parent)
      {
        this._componentList.Clear();
        transform1.gameObject.GetComponents<Component>(this._componentList);
        for (int index = 0; index < this._componentList.Count; ++index)
        {
          Selectable component1 = this._componentList[index] as Selectable;
          Interactable component2 = this._componentList[index] as Interactable;
          CanvasGroup component3 = this._componentList[index] as CanvasGroup;
          flag2 = flag2 || (UnityEngine.Object) component1 != (UnityEngine.Object) null && !component1.interactable || (UnityEngine.Object) component2 != (UnityEngine.Object) null && !component2.interactable || (UnityEngine.Object) component3 != (UnityEngine.Object) null && !component3.interactable;
          flag1 = flag1 || (UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.isActiveAndEnabled && component1.interactable || (UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.isActiveAndEnabled && component2.interactable;
        }
        if (!((UnityEngine.Object) transform1.gameObject == (UnityEngine.Object) commonRoot))
        {
          ExecuteEvents.Execute<IPointerEnterHandler>(transform1.gameObject, (BaseEventData) currentPointerData, ExecuteEvents.pointerEnterHandler);
          currentPointerData.hovered.Add(transform1.gameObject);
        }
        else
          break;
      }
      if (!(!flag2 & flag1))
        return;
      this._hapticFeedbackController.PlayHapticFeedback(this._vrPointer.vrController.node, this._rumblePreset);
    }

    private static int RaycastComparer(RaycastResult lhs, RaycastResult rhs)
    {
      if ((UnityEngine.Object) lhs.module != (UnityEngine.Object) rhs.module)
      {
        Camera eventCamera1 = lhs.module.eventCamera;
        Camera eventCamera2 = rhs.module.eventCamera;
        if ((UnityEngine.Object) eventCamera1 != (UnityEngine.Object) null && (UnityEngine.Object) eventCamera2 != (UnityEngine.Object) null && (double) eventCamera1.depth != (double) eventCamera2.depth)
        {
          if ((double) eventCamera1.depth < (double) eventCamera2.depth)
            return 1;
          return (double) eventCamera1.depth == (double) eventCamera2.depth ? 0 : -1;
        }
        if (lhs.module.sortOrderPriority != rhs.module.sortOrderPriority)
          return rhs.module.sortOrderPriority.CompareTo(lhs.module.sortOrderPriority);
        if (lhs.module.renderOrderPriority != rhs.module.renderOrderPriority)
          return rhs.module.renderOrderPriority.CompareTo(lhs.module.renderOrderPriority);
      }
      if (!Mathf.Approximately(lhs.distance, rhs.distance))
        return lhs.distance.CompareTo(rhs.distance);
      if (lhs.sortingLayer != rhs.sortingLayer)
        return UnityEngine.SortingLayer.GetLayerValueFromID(rhs.sortingLayer).CompareTo(UnityEngine.SortingLayer.GetLayerValueFromID(lhs.sortingLayer));
      if (lhs.sortingOrder != rhs.sortingOrder)
        return rhs.sortingOrder.CompareTo(lhs.sortingOrder);
      return lhs.depth != rhs.depth && (UnityEngine.Object) lhs.module.rootRaycaster == (UnityEngine.Object) rhs.module.rootRaycaster ? rhs.depth.CompareTo(lhs.depth) : lhs.index.CompareTo(rhs.index);
    }
  }
}
