// Decompiled with JetBrains decompiler
// Type: Vector3TransitionSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class Vector3TransitionSO : BaseTransitionSO
{
  [SerializeField]
  protected Vector3 _normalState;
  [SerializeField]
  protected Vector3 _highlightedState;
  [SerializeField]
  protected Vector3 _pressedState;
  [SerializeField]
  protected Vector3 _disabledState;
  [SerializeField]
  protected Vector3 _selectedState;
  [SerializeField]
  protected Vector3 _selectedAndHighlightedState;

  public Vector3 normalState => this._normalState;

  public Vector3 highlightedState => this._highlightedState;

  public Vector3 pressedState => this._pressedState;

  public Vector3 disabledState => this._disabledState;

  public Vector3 selectedState => this._selectedState;

  public Vector3 selectedAndHighlightedState => this._selectedAndHighlightedState;
}
