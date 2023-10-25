// Decompiled with JetBrains decompiler
// Type: SaberManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SaberManager : MonoBehaviour
{
  [SerializeField]
  protected Saber _leftSaber;
  [SerializeField]
  protected Saber _rightSaber;
  [Inject]
  protected readonly SaberManager.InitData _initData;
  protected bool _started;

  public Saber leftSaber => this._leftSaber;

  public Saber rightSaber => this._rightSaber;

  public event System.Action<Saber, Saber> didUpdateSaberPositionsEvent;

  public bool disableSabers
  {
    set => this.enabled = !value;
  }

  public virtual void Start()
  {
    this.RefreshSabers();
    this._started = true;
  }

  public virtual void OnDisable() => this.RefreshSabers();

  public virtual void OnEnable()
  {
    if (!this._started)
      return;
    this.RefreshSabers();
  }

  public virtual void Update()
  {
    if (this._initData.oneSaberMode)
    {
      if (this._leftSaber.saberType == this._initData.oneSaberType)
        this._leftSaber.ManualUpdate();
      else if (this._rightSaber.saberType == this._initData.oneSaberType)
        this._rightSaber.ManualUpdate();
    }
    else
    {
      this._leftSaber.ManualUpdate();
      this._rightSaber.ManualUpdate();
    }
    System.Action<Saber, Saber> saberPositionsEvent = this.didUpdateSaberPositionsEvent;
    if (saberPositionsEvent == null)
      return;
    saberPositionsEvent(this._leftSaber, this._rightSaber);
  }

  public virtual Saber SaberForType(SaberType saberType)
  {
    if (this._leftSaber.saberType == saberType)
      return this._leftSaber;
    return this._rightSaber.saberType == saberType ? this._rightSaber : (Saber) null;
  }

  public virtual void RefreshSabers()
  {
    if (this.isActiveAndEnabled)
    {
      if (this._initData.oneSaberMode)
      {
        this._leftSaber.gameObject.SetActive(this._initData.oneSaberType == this._leftSaber.saberType);
        this._rightSaber.gameObject.SetActive(this._initData.oneSaberType == this._rightSaber.saberType);
      }
      else
      {
        this._leftSaber.gameObject.SetActive(true);
        this._rightSaber.gameObject.SetActive(true);
      }
    }
    else
    {
      this._leftSaber.gameObject.SetActive(false);
      this._rightSaber.gameObject.SetActive(false);
    }
  }

  public class InitData
  {
    public readonly bool oneSaberMode;
    public readonly SaberType oneSaberType;

    public InitData(bool oneSaberMode, SaberType oneSaberType = SaberType.SaberA)
    {
      this.oneSaberMode = oneSaberMode;
      this.oneSaberType = oneSaberType;
    }
  }
}
