// Decompiled with JetBrains decompiler
// Type: SpriteSwapTransitionSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SpriteSwapTransitionSO : BaseTransitionSO
{
  [SerializeField]
  protected Sprite _normalSprite;
  [SerializeField]
  protected Sprite _highlightedSprite;
  [SerializeField]
  protected Sprite _pressedSprite;
  [SerializeField]
  protected Sprite _disabledSprite;
  [SerializeField]
  protected Sprite _selectedSprite;
  [SerializeField]
  protected Sprite _selectedAndHighlightedSprite;

  public Sprite normalSprite => this._normalSprite;

  public Sprite highlightedSprite => this._highlightedSprite;

  public Sprite pressedSprite => this._pressedSprite;

  public Sprite disabledSprite => this._disabledSprite;

  public Sprite selectedSprite => this._selectedSprite;

  public Sprite selectedAndHighlightedSprite => this._selectedAndHighlightedSprite;
}
