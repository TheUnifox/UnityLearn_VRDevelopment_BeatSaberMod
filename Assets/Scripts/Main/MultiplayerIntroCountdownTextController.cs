// Decompiled with JetBrains decompiler
// Type: MultiplayerIntroCountdownTextController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class MultiplayerIntroCountdownTextController : MonoBehaviour
{
  [SerializeField]
  protected TextMeshPro[] _texts;

  public virtual void SetText(string text)
  {
    foreach (TMP_Text text1 in this._texts)
      text1.text = text;
  }

  public virtual void SetDistances(float distance)
  {
    for (int index = 1; index < this._texts.Length; ++index)
      this._texts[index].transform.localPosition = new Vector3(0.0f, 0.0f, (float) index * distance);
  }

  public bool hide
  {
    set => this.gameObject.SetActive(!value);
  }

  public float fontSize
  {
    set
    {
      foreach (TMP_Text text in this._texts)
        text.fontSize = value;
    }
    get => this._texts != null && this._texts.Length != 0 ? this._texts[0].fontSize : 0.0f;
  }

  public float alpha
  {
    set
    {
      for (int p = 0; p < this._texts.Length; ++p)
        this._texts[p].alpha = value / Mathf.Pow(2f, (float) p);
    }
    get => this._texts != null && this._texts.Length != 0 ? this._texts[0].alpha : 0.0f;
  }
}
