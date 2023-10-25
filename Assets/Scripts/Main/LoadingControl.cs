// Decompiled with JetBrains decompiler
// Type: LoadingControl
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingControl : MonoBehaviour
{
  [SerializeField]
  protected GameObject _loadingContainer;
  [SerializeField]
  protected TextMeshProUGUI _loadingText;
  [Space]
  [SerializeField]
  protected TextMeshProUGUI _refreshText;
  [SerializeField]
  protected Button _refreshButton;
  [SerializeField]
  protected GameObject _refreshContainer;
  [Space]
  [SerializeField]
  protected GameObject _downloadingContainer;
  [SerializeField]
  protected TextMeshProUGUI _downloadingText;
  [SerializeField]
  protected Image _donwloadingProgressImage;
  protected ButtonBinder _buttonBinder;

  public event System.Action didPressRefreshButtonEvent;

  public bool isLoading => this._loadingContainer.activeSelf;

  public virtual void Awake()
  {
    this._buttonBinder = new ButtonBinder();
    this._buttonBinder.AddBinding(this._refreshButton, (System.Action) (() =>
    {
      System.Action refreshButtonEvent = this.didPressRefreshButtonEvent;
      if (refreshButtonEvent == null)
        return;
      refreshButtonEvent();
    }));
  }

  public virtual void OnDestroy() => this._buttonBinder.ClearBindings();

  public virtual void ShowLoading(string text = "")
  {
    this.gameObject.SetActive(true);
    this._loadingContainer.SetActive(true);
    this._refreshContainer.SetActive(false);
    this._downloadingContainer.SetActive(false);
    this._loadingText.text = text;
  }

  public virtual void ShowText(string text, bool showRefreshButton)
  {
    this.gameObject.SetActive(true);
    this._loadingContainer.SetActive(false);
    this._refreshContainer.SetActive(true);
    this._downloadingContainer.SetActive(false);
    this._refreshButton.gameObject.SetActive(showRefreshButton);
    this._refreshText.text = text;
  }

  public virtual void ShowDownloadingProgress(string text, float downloadingProgress)
  {
    this.gameObject.SetActive(true);
    this._loadingContainer.SetActive(false);
    this._refreshContainer.SetActive(false);
    this._downloadingContainer.SetActive(true);
    this._downloadingText.text = text;
    this._donwloadingProgressImage.fillAmount = downloadingProgress;
  }

  public virtual void Hide() => this.gameObject.SetActive(false);

  [CompilerGenerated]
  public virtual void m_CAwakem_Eb__14_0()
  {
    System.Action refreshButtonEvent = this.didPressRefreshButtonEvent;
    if (refreshButtonEvent == null)
      return;
    refreshButtonEvent();
  }
}
