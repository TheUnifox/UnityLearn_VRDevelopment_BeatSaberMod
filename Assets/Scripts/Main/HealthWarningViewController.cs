// Decompiled with JetBrains decompiler
// Type: HealthWarningViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthWarningViewController : ViewController
{
  [SerializeField]
  protected Button _continueButton;
  [Header("Safe Area Checker")]
  [SerializeField]
  protected SafeAreaRectChecker _safeAreaRectChecker;
  [Header("Auto Hide Settings")]
  [SerializeField]
  protected float _duration;
  [Header("Texts")]
  [SerializeField]
  protected TextMeshProUGUI _healthAndSafetyTextMesh;
  [SerializeField]
  [LocalizationKey]
  protected string _healthAndSafetyFullLocalizationKey;
  [SerializeField]
  [LocalizationKey]
  protected string _healthAndSafetyShortLocalizationKey;
  protected bool _showShortHealthAndSafety;
  protected Coroutine _dismissCoroutine;

  public event System.Action didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      if (this._showShortHealthAndSafety)
      {
        this._healthAndSafetyTextMesh.SetText(Localization.Get(this._healthAndSafetyShortLocalizationKey));
        this._continueButton.gameObject.SetActive(false);
      }
      else
      {
        this._healthAndSafetyTextMesh.SetText(Localization.Get(this._healthAndSafetyFullLocalizationKey));
        this._safeAreaRectChecker.enabled = true;
        this.buttonBinder.AddBinding(this._continueButton, (System.Action) (() =>
        {
          System.Action didFinishEvent = this.didFinishEvent;
          if (didFinishEvent == null)
            return;
          didFinishEvent();
        }));
      }
    }
    if (!addedToHierarchy || !this._showShortHealthAndSafety)
      return;
    this._dismissCoroutine = PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(this.DismissHealthAndSafety());
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy || this._dismissCoroutine == null || !PersistentSingleton<SharedCoroutineStarter>.IsSingletonAvailable)
      return;
    PersistentSingleton<SharedCoroutineStarter>.instance.StopCoroutine(this._dismissCoroutine);
  }

  public virtual void Init(bool showShortHealthAndSafety) => this._showShortHealthAndSafety = showShortHealthAndSafety;

  public virtual IEnumerator DismissHealthAndSafety()
  {
    yield return (object) new WaitForSeconds(this._duration);
    this._dismissCoroutine = (Coroutine) null;
    System.Action didFinishEvent = this.didFinishEvent;
    if (didFinishEvent != null)
      didFinishEvent();
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__11_0()
  {
    System.Action didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent();
  }
}
