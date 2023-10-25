// Decompiled with JetBrains decompiler
// Type: UIKeyboardManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.UI;
using VRUIControls;
using Zenject;

public class UIKeyboardManager : MonoBehaviour
{
  [SerializeField]
  protected HMUI.UIKeyboard _uiKeyboard;
  [SerializeField]
  protected ModalView _keyboardModalView;
  [SerializeField]
  protected RectTransform _keyboardContainerTransform;
  [SerializeField]
  protected Transform _parentContainerTransform;
  [Inject]
  protected readonly VRInputModule _vrInputModule;
  protected const float kKeyboardTopOffset = 5f;
  protected InputFieldView _selectedInput;

  public HMUI.UIKeyboard keyboard => this._uiKeyboard;

  public virtual void Start() => this._vrInputModule.onProcessMousePressEvent += new System.Action<GameObject>(this.ProcessMousePress);

  public virtual void OnEnable() => this._uiKeyboard.okButtonWasPressedEvent += new System.Action(this.HandleKeyboardOkButton);

  public virtual void OnDisable() => this._uiKeyboard.okButtonWasPressedEvent -= new System.Action(this.HandleKeyboardOkButton);

  public virtual void OnDestroy()
  {
    if (!(bool) (UnityEngine.Object) this._vrInputModule)
      return;
    this._vrInputModule.onProcessMousePressEvent -= new System.Action<GameObject>(this.ProcessMousePress);
  }

  public virtual void OpenKeyboardFor(InputFieldView input)
  {
    if ((UnityEngine.Object) input == (UnityEngine.Object) this._selectedInput || !input.interactable || !input.useGlobalKeyboard)
      return;
    this._selectedInput = input;
    input.ActivateKeyboard(this._uiKeyboard);
    RectTransform transform = (RectTransform) input.transform;
    Vector3 position = new Vector3();
    Rect rect = transform.rect;
    Vector2 pivot = transform.pivot;
    position.x += (0.5f - pivot.x) * rect.width;
    position.y += (0.5f - pivot.y) * rect.height;
    position.z = 0.0f;
    Vector3 vector3 = this._parentContainerTransform.InverseTransformPoint(transform.TransformPoint(position));
    vector3.y -= (float) ((double) this._keyboardContainerTransform.rect.height / 2.0 + 5.0);
    this._keyboardContainerTransform.anchoredPosition = (Vector2) (vector3 + input.keyboardPositionOffset);
    this._keyboardModalView.Show(true);
  }

  public virtual void CloseKeyboard()
  {
    if ((UnityEngine.Object) this._selectedInput != (UnityEngine.Object) null)
      this._selectedInput.DeactivateKeyboard(this._uiKeyboard);
    this._selectedInput = (InputFieldView) null;
    this._keyboardModalView.Hide(true);
  }

  public virtual void TransferKeyboardTo(InputFieldView nextInput)
  {
    if ((UnityEngine.Object) this._selectedInput != (UnityEngine.Object) null)
      this._selectedInput.DeactivateKeyboard(this._uiKeyboard);
    this.OpenKeyboardFor(nextInput);
  }

  public virtual bool ShouldCloseKeyboard(GameObject root) => !root.transform.IsChildOf(this._keyboardContainerTransform.transform);

  public virtual void ProcessMousePress(GameObject currentOverGo)
  {
    InputFieldView component = currentOverGo.GetComponent<InputFieldView>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) this._selectedInput == (UnityEngine.Object) null)
      this.OpenKeyboardFor(component);
    else if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) this._selectedInput != (UnityEngine.Object) component)
    {
      this.TransferKeyboardTo(component);
    }
    else
    {
      if (!this.ShouldCloseKeyboard(currentOverGo))
        return;
      this.CloseKeyboard();
    }
  }

  public virtual void HandleKeyboardOkButton()
  {
    if ((UnityEngine.Object) this._selectedInput == (UnityEngine.Object) null)
      return;
    Selectable selectable = this._selectedInput.FindSelectableOnRight();
    if ((UnityEngine.Object) selectable == (UnityEngine.Object) null)
      selectable = this._selectedInput.FindSelectableOnDown();
    if ((UnityEngine.Object) selectable != (UnityEngine.Object) null && selectable.interactable && selectable is InputFieldView nextInput)
      this.TransferKeyboardTo(nextInput);
    else
      this.CloseKeyboard();
  }
}
