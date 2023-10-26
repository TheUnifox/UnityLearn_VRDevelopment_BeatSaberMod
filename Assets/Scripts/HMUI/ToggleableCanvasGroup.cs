// Decompiled with JetBrains decompiler
// Type: ToggleableCanvasGroup
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof (CanvasGroup))]
public class ToggleableCanvasGroup : MonoBehaviour
{
  [SerializeField]
  protected CanvasGroup _canvasGroup;
  [SerializeField]
  protected Toggle _toggle;
  [Space]
  [SerializeField]
  protected bool _invertToggle;

  public virtual void OnEnable()
  {
    this._toggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleToggleValueChanged));
    this.SetCanvasGroupData(this._toggle.isOn);
  }

  public virtual void OnDisable() => this._toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleToggleValueChanged));

  public virtual void HandleToggleValueChanged(bool isOn) => this.SetCanvasGroupData(isOn);

  public virtual void SetCanvasGroupData(bool isOn) => this._canvasGroup.interactable = isOn ^ this._invertToggle;
}
