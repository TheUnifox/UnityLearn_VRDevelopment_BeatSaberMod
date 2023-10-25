// Decompiled with JetBrains decompiler
// Type: ComboUIController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;
using Zenject;

public class ComboUIController : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _comboText;
  [SerializeField]
  protected Animator _animator;
  [Inject]
  protected readonly IComboController _comboController;
  protected int _comboLostId;
  protected bool _fullComboLost;

  public virtual void Start()
  {
    this._comboLostId = Animator.StringToHash("ComboLost");
    this.RegisterForEvents();
    this._comboText.text = "0";
  }

  public virtual void OnEnable() => this.RegisterForEvents();

  public virtual void OnDisable() => this.UnregisterFromEvents();

  public virtual void RegisterForEvents()
  {
    if (this._comboController == null)
      return;
    this._comboController.comboDidChangeEvent -= new System.Action<int>(this.HandleComboDidChange);
    this._comboController.comboDidChangeEvent += new System.Action<int>(this.HandleComboDidChange);
    this._comboController.comboBreakingEventHappenedEvent -= new System.Action(this.HandleComboBreakingEventHappened);
    this._comboController.comboBreakingEventHappenedEvent += new System.Action(this.HandleComboBreakingEventHappened);
  }

  public virtual void UnregisterFromEvents()
  {
    if (this._comboController == null)
      return;
    this._comboController.comboDidChangeEvent -= new System.Action<int>(this.HandleComboDidChange);
    this._comboController.comboBreakingEventHappenedEvent -= new System.Action(this.HandleComboBreakingEventHappened);
  }

  public virtual void HandleComboDidChange(int combo) => this._comboText.text = combo.ToString();

  public virtual void HandleComboBreakingEventHappened()
  {
    if (this._fullComboLost)
      return;
    this._fullComboLost = true;
    this._animator.SetTrigger(this._comboLostId);
  }
}
