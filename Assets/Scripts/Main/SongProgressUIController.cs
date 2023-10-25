// Decompiled with JetBrains decompiler
// Type: SongProgressUIController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SongProgressUIController : MonoBehaviour
{
  [SerializeField]
  protected Slider _slider;
  [SerializeField]
  protected Image _progressImage;
  [SerializeField]
  protected TextMeshProUGUI _durationMinutesText;
  [SerializeField]
  protected TextMeshProUGUI _durationSecondsText;
  [SerializeField]
  protected TextMeshProUGUI _progressMinutesText;
  [SerializeField]
  protected TextMeshProUGUI _progressSecondsText;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  protected int _prevMinutes = -1;
  protected int _prevSeconds = -1;
  protected StringBuilder _stringBuilder;
  protected RectTransform _progressImageRectTransform;

  public virtual void Start()
  {
    float songLength = this._audioTimeSource.songLength;
    this._durationMinutesText.text = songLength.Minutes().ToString("0");
    this._durationSecondsText.text = songLength.Seconds().ToString("00");
    this._stringBuilder = new StringBuilder(4);
    this._progressImageRectTransform = this._progressImage.rectTransform;
  }

  public virtual void Update()
  {
    double songTime = (double) this._audioTimeSource.songTime;
    int num = ((float) songTime).Minutes();
    int number = ((float) songTime).Seconds();
    if (number < 0)
      number = 0;
    if (this._prevMinutes != num)
    {
      this._progressMinutesText.text = num.ToString();
      this._prevMinutes = num;
    }
    if (this._prevSeconds == number)
      return;
    this._stringBuilder.Remove(0, this._stringBuilder.Length);
    if (number < 10)
      this._stringBuilder.Append('0');
    this._stringBuilder.AppendNumber(number);
    this._progressSecondsText.text = this._stringBuilder.ToString();
    this._prevSeconds = number;
    float x = this._audioTimeSource.songTime / this._audioTimeSource.songLength;
    this._slider.value = x;
    this._progressImageRectTransform.anchorMax = new Vector2(x, 1f);
  }
}
