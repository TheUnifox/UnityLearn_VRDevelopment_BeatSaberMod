// Decompiled with JetBrains decompiler
// Type: LevelParamsPanel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class LevelParamsPanel : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _notesPerSecondText;
  [SerializeField]
  protected TextMeshProUGUI _notesCountText;
  [SerializeField]
  protected TextMeshProUGUI _obstaclesCountText;
  [SerializeField]
  protected TextMeshProUGUI _bombsCountText;

  public float notesPerSecond
  {
    set => this._notesPerSecondText.text = value.ToString("F2");
  }

  public int notesCount
  {
    set => this._notesCountText.text = value.ToString();
  }

  public int obstaclesCount
  {
    set => this._obstaclesCountText.text = value.ToString();
  }

  public int bombsCount
  {
    set => this._bombsCountText.text = value.ToString();
  }
}
