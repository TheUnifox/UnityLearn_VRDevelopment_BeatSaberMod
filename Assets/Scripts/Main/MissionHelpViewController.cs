// Decompiled with JetBrains decompiler
// Type: MissionHelpViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MissionHelpViewController : ViewController
{
  [SerializeField]
  protected Button _okButton;
  [SerializeField]
  protected MissionHelpViewController.MissionHelpGameObjectPair[] _missionHelpGameObjectPairs;
  protected MissionHelpSO _missionHelp;

  public event System.Action<MissionHelpViewController> didFinishEvent;

  public virtual void Setup(MissionHelpSO missionHelp)
  {
    this._missionHelp = missionHelp;
    if (!this.isInViewControllerHierarchy)
      return;
    this.RefreshContent();
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
      this.buttonBinder.AddBinding(this._okButton, new System.Action(this.OkButtonPressed));
    this.RefreshContent();
  }

  public virtual void RefreshContent()
  {
    foreach (MissionHelpViewController.MissionHelpGameObjectPair helpGameObjectPair in this._missionHelpGameObjectPairs)
      helpGameObjectPair.gameObject.SetActive((UnityEngine.Object) helpGameObjectPair.missionHelp == (UnityEngine.Object) this._missionHelp);
  }

  public virtual void OkButtonPressed()
  {
    System.Action<MissionHelpViewController> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this);
  }

  [Serializable]
  public class MissionHelpGameObjectPair
  {
    public MissionHelpSO missionHelp;
    public GameObject gameObject;
  }
}
