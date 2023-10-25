// Decompiled with JetBrains decompiler
// Type: ObjectiveListItem
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class ObjectiveListItem : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _titleText;
  [SerializeField]
  protected TextMeshProUGUI _conditionText;

  public string title
  {
    set => this._titleText.text = value;
  }

  public string conditionText
  {
    set => this._conditionText.text = value;
  }

  public bool hideCondition
  {
    set => this._conditionText.gameObject.SetActive(!value);
  }
}
