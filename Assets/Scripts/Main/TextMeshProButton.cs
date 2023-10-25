// Decompiled with JetBrains decompiler
// Type: TextMeshProButton
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextMeshProButton : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected Button _button;

  public TextMeshProUGUI text => this._text;

  public Button button => this._button;
}
