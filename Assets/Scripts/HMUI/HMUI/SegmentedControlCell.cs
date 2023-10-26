// Decompiled with JetBrains decompiler
// Type: HMUI.SegmentedControlCell
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Runtime.CompilerServices;
using UnityEngine;

namespace HMUI
{
  [RequireComponent(typeof (RectTransform))]
  public class SegmentedControlCell : SelectableCell
  {
    [CompilerGenerated]
    protected int cellNumber_k__BackingField;
    protected SegmentedControl _segmentedControl;

    public int cellNumber
    {
      get => this.cellNumber_k__BackingField;
      private set => this.cellNumber_k__BackingField = value;
    }

    public virtual void SegmentedControlSetup(SegmentedControl segmentedControl, int cellNumber)
    {
      this._segmentedControl = segmentedControl;
      this.cellNumber = cellNumber;
    }

    protected override void InternalToggle()
    {
      if (this.selected)
        return;
      this.SetSelected(true, SelectableCell.TransitionType.Animated, (object) this, false);
    }
  }
}
