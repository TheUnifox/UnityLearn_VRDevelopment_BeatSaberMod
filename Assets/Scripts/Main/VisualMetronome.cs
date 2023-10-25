// Decompiled with JetBrains decompiler
// Type: VisualMetronome
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.UI;

public class VisualMetronome : MonoBehaviour
{
  [SerializeField]
  protected AudioSource _audioSource;
  [SerializeField]
  protected float _leftPadding;
  [SerializeField]
  protected float _rightPadding;
  [SerializeField]
  protected RectTransform _ticker;
  [SerializeField]
  protected RectTransform _movingTicker;
  [SerializeField]
  protected Image _tickerImage;
  [SerializeField]
  protected Image _movingTickerImage;
  [SerializeField]
  protected float _metronomeInterval = 0.5f;
  [SerializeField]
  protected Vector2 _normalTickerSize = new Vector2(1.2f, 3f);
  [SerializeField]
  protected Vector2 _tickTickerSize0 = new Vector2(40f, 4f);
  [SerializeField]
  protected Vector2 _tickTickerSize1 = new Vector2(0.8f, 5f);
  [SerializeField]
  protected float _smooth = 16f;
  protected float _prevAudioTime;
  protected float _zeroOffset;
  protected float _direction = 1f;
  protected bool _dontTickThisFrame;

  public Color tickerColor
  {
    set => this._tickerImage.color = value;
  }

  public Color movingTickerColor
  {
    set => this._movingTickerImage.color = value;
  }

  public float zeroOffset
  {
    set
    {
      this._zeroOffset = value;
      this._dontTickThisFrame = true;
    }
    get => this._zeroOffset;
  }

  public virtual void Awake()
  {
    this._ticker.anchorMin = new Vector2(0.5f, 0.5f);
    this._ticker.anchorMax = new Vector2(0.5f, 0.5f);
    this._ticker.sizeDelta = this._normalTickerSize;
    this._movingTicker.anchorMin = new Vector2(0.0f, 0.5f);
    this._movingTicker.anchorMax = new Vector2(0.0f, 0.5f);
    this._movingTicker.sizeDelta = this._normalTickerSize;
  }

  public virtual void OnEnable()
  {
    this._audioSource.Play();
    this._tickerImage.enabled = true;
    this._movingTickerImage.enabled = true;
  }

  public virtual void OnDisable()
  {
    this._audioSource.Stop();
    this._tickerImage.enabled = false;
    this._movingTickerImage.enabled = false;
  }

  public virtual void Update()
  {
    double num1 = (double) this._audioSource.time - (double) this._zeroOffset;
    float num2 = (float) (num1 - (double) Mathf.Floor((float) num1 / this._metronomeInterval) * (double) this._metronomeInterval);
    if ((double) num2 < (double) this._prevAudioTime)
    {
      this._direction *= -1f;
      if (!this._dontTickThisFrame)
        this._ticker.sizeDelta = this._tickTickerSize0;
    }
    if ((double) this._prevAudioTime <= 0.5 && (double) num2 >= 0.5)
    {
      if (!this._dontTickThisFrame)
        this._ticker.sizeDelta = this._tickTickerSize1;
    }
    else
      this._ticker.sizeDelta = Vector2.Lerp(this._ticker.sizeDelta, this._normalTickerSize, Time.deltaTime * this._smooth);
    float t = num2 / this._metronomeInterval;
    if ((double) this._direction < 0.0)
      t = 1f - t;
    this.SetMovingTickerNormalizedPosition(t);
    this._dontTickThisFrame = false;
    this._prevAudioTime = num2;
  }

  public virtual void SetMovingTickerNormalizedPosition(float t) => this._movingTicker.anchoredPosition = new Vector2(t * (((RectTransform) this.transform).rect.width - (this._leftPadding + this._rightPadding)) + this._leftPadding, 0.0f);
}
