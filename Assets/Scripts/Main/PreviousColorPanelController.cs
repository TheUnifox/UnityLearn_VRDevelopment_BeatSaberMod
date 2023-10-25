// Decompiled with JetBrains decompiler
// Type: PreviousColorPanelController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PreviousColorPanelController : MonoBehaviour
{
  [SerializeField]
  protected Graphic[] _graphics;
  [SerializeField]
  protected Button _button;
  protected const int kMaxColors = 2;
  protected ButtonBinder _buttonBinder;
  protected Color _color = Color.black;
  protected Color _graphicsColor = Color.black;

  public event System.Action<Color> colorWasSelectedEvent;

  public virtual void Awake()
  {
    this._buttonBinder = new ButtonBinder();
    this._buttonBinder.AddBinding(this._button, (System.Action) (() =>
    {
      System.Action<Color> wasSelectedEvent = this.colorWasSelectedEvent;
      if (wasSelectedEvent == null)
        return;
      wasSelectedEvent(this._graphicsColor);
    }));
  }

  public virtual void OnDestroy() => this._buttonBinder.ClearBindings();

  public virtual void AddColor(Color color)
  {
    this._graphicsColor = this._color;
    foreach (Graphic graphic in this._graphics)
      graphic.color = this._graphicsColor;
    this._color = color;
  }

  public virtual void DiscardUpcomingColor() => this._color = this._graphicsColor;

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__9_0()
  {
    System.Action<Color> wasSelectedEvent = this.colorWasSelectedEvent;
    if (wasSelectedEvent == null)
      return;
    wasSelectedEvent(this._graphicsColor);
  }
}
