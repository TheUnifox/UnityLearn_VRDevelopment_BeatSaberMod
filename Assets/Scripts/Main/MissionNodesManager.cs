// Decompiled with JetBrains decompiler
// Type: MissionNodesManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class MissionNodesManager : MonoBehaviour
{
  [SerializeField]
  protected MissionNode _rootMissionNode;
  [SerializeField]
  protected MissionNode _finalMissionNode;
  [SerializeField]
  protected MissionStagesManager _missionStagesManager;
  [SerializeField]
  protected GameObject _connectionsParentObject;
  [SerializeField]
  protected GameObject _missionNodesParentObject;
  [Inject]
  protected CampaignProgressModel _missionProgressModel;
  protected MissionNodeConnection[] _allMissionNodeConnections;
  protected MissionNode[] _allMissionNodes;
  protected bool _isInitialized;

  public MissionNode rootMissionNode => this._rootMissionNode;

  public MissionNode finalMissionNode => this._finalMissionNode;

  public MissionStagesManager missionStagesManager => this._missionStagesManager;

  public CampaignProgressModel missionProgressModel => this._missionProgressModel;

  public MissionNode[] allMissionNodes => this._allMissionNodes;

  public bool IsInitialized => this._isInitialized;

  public virtual void Awake()
  {
    this.GetAllMissionNodes();
    this.RegisterAllNodes();
    this.SetupNodeMap();
    this._isInitialized = true;
  }

  public virtual void SetupNodeMap()
  {
    this.ResetAllNodes();
    this.SetupStages();
    this.SetupNodeTree(this.rootMissionNode.missionNodeVisualController, true);
    this.SetupNodeConnections();
  }

  public virtual bool MissionWasCleared(MissionNode missionNode) => !missionNode.missionNodeVisualController.cleared && this.missionProgressModel.IsMissionCleared(missionNode.missionId);

  public virtual MissionNode GetMissionNodeWithModelClearedStateInconsistency()
  {
    foreach (MissionNode allMissionNode in this._allMissionNodes)
    {
      if (!allMissionNode.missionNodeVisualController.cleared && this.missionProgressModel.IsMissionCleared(allMissionNode.missionId))
        return allMissionNode;
    }
    return (MissionNode) null;
  }

  public virtual bool DidFirstLockedMissionStageChange()
  {
    MissionStage lockedMissionStage1 = this.missionStagesManager.firstLockedMissionStage;
    this.missionStagesManager.UpdateFirtsLockedMissionStage(this.missionProgressModel.numberOfClearedMissions);
    MissionStage lockedMissionStage2 = this.missionStagesManager.firstLockedMissionStage;
    return (Object) lockedMissionStage1 != (Object) lockedMissionStage2;
  }

  public virtual void UpdateStageLockText() => this.missionStagesManager.UpdateStageLockText(this.missionProgressModel.numberOfClearedMissions);

  public virtual MissionNode GetTopMostNotClearedMissionNode()
  {
    MissionNode clearedMissionNode = (MissionNode) null;
    foreach (MissionNode allMissionNode in this._allMissionNodes)
    {
      MissionNodeVisualController visualController = allMissionNode.missionNodeVisualController;
      if (visualController.isInitialized && !allMissionNode.missionNodeVisualController.cleared && visualController.interactable && ((Object) clearedMissionNode == (Object) null || (double) clearedMissionNode.transform.transform.position.y < (double) visualController.transform.transform.position.y))
        clearedMissionNode = allMissionNode;
    }
    return clearedMissionNode;
  }

  public virtual void GetAllMissionNodes() => this._allMissionNodes = this.GetAllMissionNodes(this.rootMissionNode, new HashSet<MissionNode>()).ToArray<MissionNode>();

  public virtual HashSet<MissionNode> GetAllMissionNodes(
    MissionNode node,
    HashSet<MissionNode> visited)
  {
    if (visited.Contains(node))
      return visited;
    visited.Add(node);
    foreach (MissionNode childNode in node.childNodes)
      this.GetAllMissionNodes(childNode, visited);
    return visited;
  }

  public virtual MissionNodeConnection[] GetNewEnabledConnection()
  {
    List<MissionNodeConnection> missionNodeConnectionList = new List<MissionNodeConnection>();
    foreach (MissionNodeConnection missionNodeConnection in this._allMissionNodeConnections)
    {
      bool cleared = missionNodeConnection.parentMissionNode.cleared;
      bool flag1 = this.IsNodeInteractable(missionNodeConnection.childMissionNode, cleared);
      bool flag2 = cleared & flag1;
      if (!missionNodeConnection.isActive & flag2)
        missionNodeConnectionList.Add(missionNodeConnection);
    }
    return missionNodeConnectionList.ToArray();
  }

  public virtual void ResetAllNodes()
  {
    foreach (MissionNode allMissionNode in this._allMissionNodes)
      allMissionNode.missionNodeVisualController.Reset();
  }

  public virtual void SetupStages()
  {
    int ofClearedMissions = this.missionProgressModel.numberOfClearedMissions;
    this.missionStagesManager.UpdateFirtsLockedMissionStage(ofClearedMissions);
    this.missionStagesManager.UpdateStageLockPosition();
    this.missionStagesManager.UpdateStageLockText(ofClearedMissions);
  }

  public virtual void RegisterAllNodes()
  {
    foreach (MissionNode allMissionNode in this._allMissionNodes)
      this.missionProgressModel.RegisterMissionId(allMissionNode.missionId);
    this.missionProgressModel.SetFinalMissionId(this._finalMissionNode.missionId);
  }

  public virtual void SetupNodeTree(MissionNodeVisualController node, bool parentCleared)
  {
    if (node.isInitialized)
    {
      if (!this.IsNodeInteractable(node, parentCleared))
        return;
      node.SetInteractable();
    }
    else
    {
      bool interactable = this.IsNodeInteractable(node, parentCleared);
      bool flag = this.missionProgressModel.IsMissionCleared(node.missionNode.missionId);
      node.Setup(flag, interactable);
      foreach (MissionNode childNode in node.missionNode.childNodes)
        this.SetupNodeTree(childNode.missionNodeVisualController, flag);
    }
  }

  public virtual bool IsNodeInteractable(MissionNodeVisualController node, bool parentCleared)
  {
    if (!parentCleared)
      return false;
    MissionStage lockedMissionStage = this.missionStagesManager.firstLockedMissionStage;
    return (double) node.missionNode.position.y < (double) lockedMissionStage.position.y;
  }

  public virtual void SetupNodeConnections()
  {
    this._allMissionNodeConnections = this._connectionsParentObject.GetComponentsInChildren<MissionNodeConnection>();
    foreach (MissionNodeConnection missionNodeConnection in this._allMissionNodeConnections)
    {
      if ((!missionNodeConnection.parentMissionNode.cleared ? 0 : (missionNodeConnection.childMissionNode.interactable ? 1 : 0)) != 0)
        missionNodeConnection.SetActive(false);
    }
  }
}
