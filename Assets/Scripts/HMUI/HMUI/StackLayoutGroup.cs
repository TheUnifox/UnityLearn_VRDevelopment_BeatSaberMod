// Decompiled with JetBrains decompiler
// Type: HMUI.StackLayoutGroup
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.UI;

namespace HMUI
{
  public class StackLayoutGroup : LayoutGroup
  {
    [SerializeField]
    protected bool m_ChildForceExpandWidth = true;
    [SerializeField]
    protected bool m_ChildForceExpandHeight = true;

    public bool childForceExpandWidth
    {
      get => this.m_ChildForceExpandWidth;
      set => this.SetProperty<bool>(ref this.m_ChildForceExpandWidth, value);
    }

    public bool childForceExpandHeight
    {
      get => this.m_ChildForceExpandHeight;
      set => this.SetProperty<bool>(ref this.m_ChildForceExpandHeight, value);
    }

    protected StackLayoutGroup()
    {
    }

    public override void CalculateLayoutInputHorizontal()
    {
      base.CalculateLayoutInputHorizontal();
      this.CalcAlongAxis(0);
    }

    public override void CalculateLayoutInputVertical() => this.CalcAlongAxis(1);

    public override void SetLayoutHorizontal() => this.SetChildrenAlongAxis(0);

    public override void SetLayoutVertical() => this.SetChildrenAlongAxis(1);

    public virtual void CalcAlongAxis(int axis)
    {
      float num1 = axis == 0 ? (float) this.padding.horizontal : (float) this.padding.vertical;
      float num2 = 0.0f;
      float b = 0.0f;
      float num3 = 0.0f;
      for (int index = 0; index < this.rectChildren.Count; ++index)
      {
        RectTransform rectChild = this.rectChildren[index];
        float minSize = LayoutUtility.GetMinSize(rectChild, axis);
        float preferredSize = LayoutUtility.GetPreferredSize(rectChild, axis);
        float a = LayoutUtility.GetFlexibleSize(rectChild, axis);
        if ((axis == 0 ? (this.childForceExpandWidth ? 1 : 0) : (this.childForceExpandHeight ? 1 : 0)) != 0)
          a = Mathf.Max(a, 1f);
        num2 = Mathf.Max(minSize + num1, num2);
        b = Mathf.Max(preferredSize + num1, b);
        num3 = Mathf.Max(a, num3);
      }
      float totalPreferred = Mathf.Max(num2, b);
      this.SetLayoutInputForAxis(num2, totalPreferred, num3, axis);
    }

    public virtual void SetChildrenAlongAxis(int axis)
    {
      float num1 = this.rectTransform.rect.size[axis];
      float num2 = num1 - (axis == 0 ? (float) this.padding.horizontal : (float) this.padding.vertical);
      for (int index = 0; index < this.rectChildren.Count; ++index)
      {
        RectTransform rectChild = this.rectChildren[index];
        float minSize = LayoutUtility.GetMinSize(rectChild, axis);
        float preferredSize = LayoutUtility.GetPreferredSize(rectChild, axis);
        float a = LayoutUtility.GetFlexibleSize(rectChild, axis);
        if ((axis == 0 ? (this.childForceExpandWidth ? 1 : 0) : (this.childForceExpandHeight ? 1 : 0)) != 0)
          a = Mathf.Max(a, 1f);
        float num3 = Mathf.Clamp(num2, minSize, (double) a > 0.0 ? num1 : preferredSize);
        float startOffset = this.GetStartOffset(axis, num3);
        this.SetChildAlongAxis(rectChild, axis, startOffset, num3);
      }
    }
  }
}
