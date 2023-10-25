// Decompiled with JetBrains decompiler
// Type: CoreGameHUDController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class CoreGameHUDController : MonoBehaviour
{
  [SerializeField]
  protected GameObject _songProgressPanelGO;
  [SerializeField]
  protected GameObject _relativeScoreGO;
  [SerializeField]
  protected GameObject _immediateRankGO;
  [SerializeField]
  protected GameObject _energyPanelGO;
  [SerializeField]
  protected CanvasGroup _canvasGroup;

  public GameObject songProgressPanelGO => this._songProgressPanelGO;

  public GameObject relativeScoreGo => this._relativeScoreGO;

  public GameObject immediateRankGo => this._immediateRankGO;

  public GameObject energyPanelGo => this._energyPanelGO;

  public float alpha
  {
    set => this._canvasGroup.alpha = value;
  }

  [Inject]
  public virtual void Initialize(CoreGameHUDController.InitData initData)
  {
    if (initData.hide)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this._songProgressPanelGO.SetActive(initData.advancedHUD);
      this._relativeScoreGO.SetActive(initData.advancedHUD);
      this._immediateRankGO.SetActive(initData.advancedHUD);
      this._energyPanelGO.SetActive(initData.showEnergyPanel);
    }
  }

  public class InitData
  {
    public readonly bool hide;
    public readonly bool showEnergyPanel;
    public readonly bool advancedHUD;

    public InitData(bool hide, bool showEnergyPanel, bool advancedHUD)
    {
      this.hide = hide;
      this.showEnergyPanel = showEnergyPanel;
      this.advancedHUD = advancedHUD;
    }
  }
}
