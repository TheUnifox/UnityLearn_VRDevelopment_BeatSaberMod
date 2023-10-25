// Decompiled with JetBrains decompiler
// Type: TextButton
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.UI;

public class TextButton : MonoBehaviour
{
  [SerializeField]
  protected UnityEngine.UI.Text _text;
  [SerializeField]
  protected Button _button;

  public UnityEngine.UI.Text text => this._text;

  public Button button => this._button;
}
