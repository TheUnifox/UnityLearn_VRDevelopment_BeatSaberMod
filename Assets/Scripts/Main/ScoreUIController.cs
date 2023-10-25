// Decompiled with JetBrains decompiler
// Type: ScoreUIController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Text;
using TMPro;
using UnityEngine;
using Zenject;

public class ScoreUIController : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _scoreText;
  [InjectOptional]
  protected readonly ScoreUIController.InitData _initData = new ScoreUIController.InitData(ScoreUIController.ScoreDisplayType.ModifiedScore);
  [Inject]
  protected readonly IScoreController _scoreController;
  protected StringBuilder _stringBuilder;
  protected const int kMaxNumberOfDigits = 9;

  public virtual void Start()
  {
    this.RegisterForEvents();
    this._stringBuilder = new StringBuilder(40);
    this.UpdateScore(0, 0);
  }

  public virtual void OnEnable() => this.RegisterForEvents();

  public virtual void OnDisable() => this.UnregisterFromEvents();

  public virtual void RegisterForEvents()
  {
    if (this._scoreController == null)
      return;
    this._scoreController.scoreDidChangeEvent -= new System.Action<int, int>(this.HandleScoreDidChangeRealtime);
    this._scoreController.scoreDidChangeEvent += new System.Action<int, int>(this.HandleScoreDidChangeRealtime);
  }

  public virtual void UnregisterFromEvents()
  {
    if (this._scoreController == null)
      return;
    this._scoreController.scoreDidChangeEvent -= new System.Action<int, int>(this.HandleScoreDidChangeRealtime);
  }

  public virtual void HandleScoreDidChangeRealtime(int multipliedScore, int modifiedScore) => this.UpdateScore(multipliedScore, modifiedScore);

  public virtual void UpdateScore(int multipliedScore, int modifiedScore)
  {
    int number = this._initData.scoreDisplayType == ScoreUIController.ScoreDisplayType.ModifiedScore ? modifiedScore : multipliedScore;
    this._stringBuilder.Remove(0, this._stringBuilder.Length);
    if (number > 999999)
    {
      this._stringBuilder.AppendNumber(number / 1000000 - number / 1000000000 * 1000000);
      this._stringBuilder.Append(' ');
      ScoreUIController.Append000Number(this._stringBuilder, number / 1000 - number / 1000000 * 1000);
      this._stringBuilder.Append(' ');
      ScoreUIController.Append000Number(this._stringBuilder, number - number / 1000 * 1000);
    }
    else if (number > 999)
    {
      this._stringBuilder.AppendNumber(number / 1000 - number / 1000000 * 1000);
      this._stringBuilder.Append(' ');
      ScoreUIController.Append000Number(this._stringBuilder, number - number / 1000 * 1000);
    }
    else
      this._stringBuilder.AppendNumber(number);
    this._scoreText.text = this._stringBuilder.ToString();
  }

  private static void Append000Number(StringBuilder stringBuilder, int number)
  {
    if (number < 100)
      stringBuilder.Append('0');
    if (number < 10)
      stringBuilder.Append('0');
    stringBuilder.AppendNumber(number);
  }

  public class InitData
  {
    public readonly ScoreUIController.ScoreDisplayType scoreDisplayType;

    public InitData(
      ScoreUIController.ScoreDisplayType scoreDisplayType)
    {
      this.scoreDisplayType = scoreDisplayType;
    }
  }

  public enum ScoreDisplayType
  {
    MultipliedScore,
    ModifiedScore,
  }
}
