// Decompiled with JetBrains decompiler
// Type: ScoreMultiplierUIController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ScoreMultiplierUIController : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI[] _multiplierTexts;
  [SerializeField]
  protected Image _multiplierProgressImage;
  [SerializeField]
  protected Animator _multiplierAnimator;
  [Inject]
  protected IScoreController _scoreController;
  protected int _prevMultiplier;
  protected int _multiplierIncreasedTriggerId;
  protected float _progressTarget;

  public virtual void Start()
  {
    this.RegisterForEvents();
    this._prevMultiplier = 1;
    for (int index = 0; index < this._multiplierTexts.Length; ++index)
      this._multiplierTexts[index].text = "1";
    this._multiplierProgressImage.fillAmount = 0.0f;
    this._multiplierIncreasedTriggerId = Animator.StringToHash("MultiplierIncreased");
  }

  public virtual void OnEnable() => this.RegisterForEvents();

  public virtual void OnDisable() => this.UnregisterFromEvents();

  public virtual void RegisterForEvents()
  {
    if (this._scoreController == null)
      return;
    this._scoreController.multiplierDidChangeEvent -= new System.Action<int, float>(this.HandleMultiplierDidChange);
    this._scoreController.multiplierDidChangeEvent += new System.Action<int, float>(this.HandleMultiplierDidChange);
  }

  public virtual void UnregisterFromEvents()
  {
    if (this._scoreController == null)
      return;
    this._scoreController.multiplierDidChangeEvent -= new System.Action<int, float>(this.HandleMultiplierDidChange);
  }

  public virtual void Update()
  {
    if ((double) Mathf.Abs(this._progressTarget - this._multiplierProgressImage.fillAmount) <= 1.0 / 1000.0)
      return;
    this._multiplierProgressImage.fillAmount = Mathf.Lerp(this._multiplierProgressImage.fillAmount, this._progressTarget, Time.deltaTime * 4f);
  }

  public virtual void HandleMultiplierDidChange(int multiplier, float progress)
  {
    if (this._prevMultiplier < multiplier)
    {
      this._multiplierAnimator.SetTrigger(this._multiplierIncreasedTriggerId);
      this._multiplierProgressImage.fillAmount = 0.0f;
    }
    this._prevMultiplier = multiplier;
    string str = multiplier.ToString();
    for (int index = 0; index < this._multiplierTexts.Length; ++index)
      this._multiplierTexts[index].text = str;
    this._progressTarget = progress;
  }
}
