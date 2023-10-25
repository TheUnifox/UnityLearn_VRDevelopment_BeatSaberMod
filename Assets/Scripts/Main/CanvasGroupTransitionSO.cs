// Decompiled with JetBrains decompiler
// Type: CanvasGroupTransitionSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class CanvasGroupTransitionSO : BaseTransitionSO
{
  [SerializeField]
  protected float _normalAlpha;
  [SerializeField]
  protected float _highlightedAlpha;
  [SerializeField]
  protected float _pressedAlpha;
  [SerializeField]
  protected float _disabledAlpha;
  [SerializeField]
  protected float _selectedAlpha;
  [SerializeField]
  protected float _selectedAndHighlightedAlpha;

  public float normalAlpha => this._normalAlpha;

  public float highlightedAlpha => this._highlightedAlpha;

  public float pressedAlpha => this._pressedAlpha;

  public float disabledAlpha => this._disabledAlpha;

  public float selectedAlpha => this._selectedAlpha;

  public float selectedAndHighlightedAlpha => this._selectedAndHighlightedAlpha;
}
