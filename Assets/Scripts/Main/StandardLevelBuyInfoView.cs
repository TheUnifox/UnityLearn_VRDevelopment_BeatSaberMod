// Decompiled with JetBrains decompiler
// Type: StandardLevelBuyInfoView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StandardLevelBuyInfoView : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected Button _buyLevelButton;
  [SerializeField]
  protected Button _openPackButton;
  [SerializeField]
  protected Button _buyPackButton;

  public Button buyLevelButton => this._buyLevelButton;

  public Button openPackButton => this._openPackButton;

  public Button buyPackButton => this._buyPackButton;

  public virtual void RefreshView(string infoText, bool canBuyPack)
  {
    this._text.text = infoText;
    this._buyPackButton.gameObject.SetActive(canBuyPack);
    this._openPackButton.gameObject.SetActive(!canBuyPack);
  }
}
