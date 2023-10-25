// Decompiled with JetBrains decompiler
// Type: EnabledTransitionSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class EnabledTransitionSO : BaseTransitionSO
{
  [SerializeField]
  protected bool _normalState;
  [SerializeField]
  protected bool _highlightedState;
  [SerializeField]
  protected bool _pressedState;
  [SerializeField]
  protected bool _disabledState;
  [SerializeField]
  protected bool _selectedState;
  [SerializeField]
  protected bool _selectedAndHighlightedState;

  public bool normalState => this._normalState;

  public bool highlightedState => this._highlightedState;

  public bool pressedState => this._pressedState;

  public bool disabledState => this._disabledState;

  public bool selectedState => this._selectedState;

  public bool selectedAndHighlightedState => this._selectedAndHighlightedState;
}
