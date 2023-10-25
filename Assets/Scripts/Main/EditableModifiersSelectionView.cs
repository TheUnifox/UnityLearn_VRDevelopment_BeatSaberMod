// Decompiled with JetBrains decompiler
// Type: EditableModifiersSelectionView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EditableModifiersSelectionView : ModifiersSelectionView
{
  [Space]
  [SerializeField]
  protected Button _editButton;
  [SerializeField]
  protected Button _clearButton;
  [SerializeField]
  protected CanvasGroup _modifiersListCanvasGroup;
  protected bool _interactable = true;
  [CompilerGenerated]
  protected bool m_CshowClearButton;

  public Button editButton => this._editButton;

  public Button clearButton => this._clearButton;

  public bool interactable
  {
    get => this._interactable;
    set
    {
      this._interactable = value;
      this._editButton.interactable = this._interactable;
      this._clearButton.interactable = this._interactable;
      this._modifiersListCanvasGroup.alpha = this._interactable ? 1f : 0.25f;
    }
  }

  public bool showClearButton
  {
    get => this.m_CshowClearButton;
    set => this.m_CshowClearButton = value;
  }

  public virtual void SetVisibility(bool visible) => this.gameObject.SetActive(visible);

  public virtual void Setup(bool showClearButton) => this.showClearButton = showClearButton;

  public override void SetGameplayModifiers(GameplayModifiers gameplayModifiers)
  {
    base.SetGameplayModifiers(gameplayModifiers);
    this._clearButton.gameObject.SetActive(this.showClearButton && gameplayModifiers != null && !gameplayModifiers.IsWithoutModifiers());
  }
}
