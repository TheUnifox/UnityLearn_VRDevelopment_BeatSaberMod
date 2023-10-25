// Decompiled with JetBrains decompiler
// Type: MissionNodeSelectionManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class MissionNodeSelectionManager : MonoBehaviour
{
  [SerializeField]
  protected MissionNodesManager _missionNodesManager;
  protected MissionNode[] _missionNodes;
  protected MissionNodeVisualController _selectedNode;

  public event System.Action<MissionNodeVisualController> didSelectMissionNodeEvent;

  public virtual void DeselectSelectedNode()
  {
    if (!((UnityEngine.Object) this._selectedNode != (UnityEngine.Object) null))
      return;
    this._selectedNode.SetSelected(false);
    this._selectedNode = (MissionNodeVisualController) null;
  }

  public virtual void Start()
  {
    this._missionNodes = this._missionNodesManager.allMissionNodes;
    foreach (MissionNode missionNode in this._missionNodes)
    {
      missionNode.missionNodeVisualController.nodeWasSelectEvent += new System.Action<MissionNodeVisualController>(this.HandleNodeWasSelect);
      missionNode.missionNodeVisualController.nodeWasDisplayedEvent += new System.Action<MissionNodeVisualController>(this.HandleNodeWasDisplayed);
    }
  }

  public virtual void OnDestroy()
  {
    foreach (MissionNode missionNode in this._missionNodes)
    {
      if ((UnityEngine.Object) missionNode != (UnityEngine.Object) null)
      {
        missionNode.missionNodeVisualController.nodeWasSelectEvent -= new System.Action<MissionNodeVisualController>(this.HandleNodeWasSelect);
        missionNode.missionNodeVisualController.nodeWasDisplayedEvent -= new System.Action<MissionNodeVisualController>(this.HandleNodeWasDisplayed);
      }
    }
  }

  public virtual void HandleNodeWasSelect(MissionNodeVisualController missionNode)
  {
    if ((UnityEngine.Object) this._selectedNode != (UnityEngine.Object) null)
      this._selectedNode.SetSelected(false);
    this._selectedNode = missionNode;
    System.Action<MissionNodeVisualController> missionNodeEvent = this.didSelectMissionNodeEvent;
    if (missionNodeEvent == null)
      return;
    missionNodeEvent(missionNode);
  }

  public virtual void HandleNodeWasDisplayed(MissionNodeVisualController missionNode) => missionNode.SetSelected((UnityEngine.Object) missionNode == (UnityEngine.Object) this._selectedNode);
}
