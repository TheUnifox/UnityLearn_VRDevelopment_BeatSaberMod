// Decompiled with JetBrains decompiler
// Type: ResultObjectiveListItem
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultObjectiveListItem : MonoBehaviour
{
  [SerializeField]
  protected Image _icon;
  [SerializeField]
  protected Image _iconGlow;
  [SerializeField]
  protected TextMeshProUGUI _titleText;
  [SerializeField]
  protected TextMeshProUGUI _conditionText;
  [SerializeField]
  protected TextMeshProUGUI _valueText;

  public Color iconColor
  {
    set => this._iconGlow.color = value;
  }

  public Sprite icon
  {
    set => this._icon.sprite = value;
  }

  public Sprite iconGlow
  {
    set => this._iconGlow.sprite = value;
  }

  public string title
  {
    set => this._titleText.text = value;
  }

  public string conditionText
  {
    set => this._conditionText.text = value;
  }

  public bool hideConditionText
  {
    set => this._conditionText.gameObject.SetActive(!value);
  }

  public string valueText
  {
    set => this._valueText.text = value;
  }

  public bool hideValueText
  {
    set => this._valueText.gameObject.SetActive(!value);
  }
}
