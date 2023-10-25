// Decompiled with JetBrains decompiler
// Type: MissionNodeVisualController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class MissionNodeVisualController : MonoBehaviour
{
  [SerializeField]
  protected MissionToggle _missionToggle;
  [SerializeField]
  protected MissionNode _missionNode;
  protected bool _selected;
  protected bool _isInitialized;
  protected bool _cleared;
  protected bool _interactable;

  public event System.Action<MissionNodeVisualController> nodeWasSelectEvent;

  public event System.Action<MissionNodeVisualController> nodeWasDisplayedEvent;

  public MissionNode missionNode => this._missionNode;

  public bool selected => this._selected;

  public bool isInitialized => this._isInitialized;

  public bool cleared => this._cleared;

  public bool interactable => this._interactable;

  public virtual void SetSelected(bool value)
  {
    this._selected = value;
    this._missionToggle.selected = this._selected;
  }

  public virtual void OnEnable()
  {
    System.Action<MissionNodeVisualController> wasDisplayedEvent = this.nodeWasDisplayedEvent;
    if (wasDisplayedEvent == null)
      return;
    wasDisplayedEvent(this);
  }

  public virtual void Awake()
  {
    this._missionToggle.selectionDidChangeEvent += new System.Action<MissionToggle>(this.HandleMissionToggleSelectionDidChange);
    this._missionToggle.interactable = true;
  }

  public virtual void Start() => this.Init();

  public virtual void Reset() => this._isInitialized = false;

  public virtual void OnDestroy()
  {
    if (!(bool) (UnityEngine.Object) this._missionToggle)
      return;
    this._missionToggle.selectionDidChangeEvent -= new System.Action<MissionToggle>(this.HandleMissionToggleSelectionDidChange);
  }

  public virtual void Init()
  {
    this._missionToggle.SetText(this._missionNode.formattedMissionNodeName);
    this.SetupToggle();
    this.ChangeNodeSelection(this.selected);
  }

  public virtual void Setup(bool cleared, bool interactable)
  {
    this._cleared = cleared;
    this._interactable = interactable;
    this._isInitialized = true;
    this.SetupToggle();
  }

  public virtual void SetupToggle()
  {
    this._missionToggle.missionCleared = this._cleared;
    this._missionToggle.interactable = this._interactable;
  }

  public virtual void SetMissionCleared()
  {
    this._cleared = true;
    this.SetupToggle();
  }

  public virtual void SetInteractable()
  {
    this._interactable = true;
    this.SetupToggle();
  }

  public virtual void ChangeNodeSelection(bool selected) => this._missionToggle.ChangeSelection(selected, false, false);

  public virtual void HandleMissionToggleSelectionDidChange(MissionToggle toggle)
  {
    if (!toggle.selected)
      return;
    System.Action<MissionNodeVisualController> nodeWasSelectEvent = this.nodeWasSelectEvent;
    if (nodeWasSelectEvent == null)
      return;
    nodeWasSelectEvent(this);
  }
}
