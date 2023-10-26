// Decompiled with JetBrains decompiler
// Type: HMUI.SelectableCellStaticAnimations
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;

namespace HMUI
{
  public class SelectableCellStaticAnimations : MonoBehaviour
  {
    [SerializeField]
    protected SelectableCell _selectableCell;
    [Space]
    [SerializeField]
    protected AnimationClip _normalAnimationClip;
    [SerializeField]
    protected AnimationClip _highlightedAnimationClip;
    [SerializeField]
    protected AnimationClip _selectedAnimationClip;
    [SerializeField]
    protected AnimationClip _selectedAndHighlightedAnimationClip;

    public virtual void Awake()
    {
      this._selectableCell.selectionDidChangeEvent += new Action<SelectableCell, SelectableCell.TransitionType, object>(this.HandleSelectionDidChange);
      this._selectableCell.highlightDidChangeEvent += new Action<SelectableCell, SelectableCell.TransitionType>(this.HandleHighlightDidChange);
    }

    public virtual void Start() => this.RefreshVisuals();

    public virtual void OnDestroy()
    {
      this._selectableCell.selectionDidChangeEvent -= new Action<SelectableCell, SelectableCell.TransitionType, object>(this.HandleSelectionDidChange);
      this._selectableCell.highlightDidChangeEvent -= new Action<SelectableCell, SelectableCell.TransitionType>(this.HandleHighlightDidChange);
    }

    public virtual void HandleSelectionDidChange(
      SelectableCell selectableCell,
      SelectableCell.TransitionType transitionType,
      object changeOwner)
    {
      this.RefreshVisuals();
    }

    public virtual void HandleHighlightDidChange(
      SelectableCell selectableCell,
      SelectableCell.TransitionType transitionType)
    {
      this.RefreshVisuals();
    }

    public virtual void RefreshVisuals()
    {
      if (!this._selectableCell.selected && !this._selectableCell.highlighted)
        this._normalAnimationClip.SampleAnimation(this.gameObject, 0.0f);
      else if (!this._selectableCell.highlighted)
        this._selectedAnimationClip.SampleAnimation(this.gameObject, 0.0f);
      else if (!this._selectableCell.selected)
        this._highlightedAnimationClip.SampleAnimation(this.gameObject, 0.0f);
      else
        this._selectedAndHighlightedAnimationClip.SampleAnimation(this.gameObject, 0.0f);
    }
  }
}
