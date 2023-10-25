// Decompiled with JetBrains decompiler
// Type: BasicLevelParamsPanel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class BasicLevelParamsPanel : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _durationText;
  [SerializeField]
  protected TextMeshProUGUI _bpmText;

  public float duration
  {
    set => this._durationText.text = value.MinSecDurationText();
  }

  public float bpm
  {
    set => this._bpmText.text = value.ToString();
  }
}
