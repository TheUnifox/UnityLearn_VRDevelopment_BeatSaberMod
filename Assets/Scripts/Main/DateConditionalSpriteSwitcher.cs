// Decompiled with JetBrains decompiler
// Type: DateConditionalSpriteSwitcher
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class DateConditionalSpriteSwitcher : MonoBehaviour
{
  [SerializeField]
  protected int _day;
  [SerializeField]
  protected int _month;
  [Space]
  [SerializeField]
  protected Sprite _falseSprite;
  [SerializeField]
  protected Sprite _trueSprite;
  [Space]
  [SerializeField]
  protected ConditionalSpriteSwitcher _conditionalSpriteSwitcher;

  public virtual void Awake()
  {
    DateTime now = DateTime.Now;
    if (now.Day != this._day || now.Month != this._month)
      return;
    this._conditionalSpriteSwitcher.trueSprite = this._trueSprite;
    this._conditionalSpriteSwitcher.falseSprite = this._falseSprite;
    this._conditionalSpriteSwitcher.Apply();
  }
}
