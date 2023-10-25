// Decompiled with JetBrains decompiler
// Type: MissionConnectionsGenerator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MissionConnectionsGenerator : MonoBehaviour
{
  [SerializeField]
  protected MissionNodesManager _missionNodesManager;
  [SerializeField]
  protected MissionNodeConnection _nodeConnectionPref;
  [SerializeField]
  protected GameObject _connectionsCanvas;
  protected List<MissionNode> _missionNodes;

  private MissionNode _rootMissionNode => this._missionNodesManager.rootMissionNode;

  public virtual void CreateNodeConnections()
  {
    this.RemoveOldConnections();
    this._missionNodes = new List<MissionNode>();
    this.CreateConnections(this._rootMissionNode, this._missionNodes);
  }

  public virtual void RemoveOldConnections()
  {
    List<Transform> transformList = new List<Transform>();
    foreach (Transform transform in this._connectionsCanvas.transform)
      transformList.Add(transform);
    foreach (Component component in transformList)
      Object.DestroyImmediate((Object) component.gameObject);
  }

  public virtual void CreateConnections(MissionNode missionNode, List<MissionNode> visitedNodes)
  {
    if (visitedNodes.Contains(missionNode))
      return;
    visitedNodes.Add(missionNode);
    if (missionNode.childNodes == null)
      return;
    foreach (MissionNode childNode in missionNode.childNodes)
    {
      this.CreateConnectionBetweenNodes(missionNode, childNode).gameObject.name = "NodeConnection_" + missionNode.missionId + "-" + childNode.missionId;
      this.CreateConnections(childNode, visitedNodes);
    }
  }

  public virtual MissionNodeConnection CreateConnectionBetweenNodes(
    MissionNode parentMissionNode,
    MissionNode childMissionNode)
  {
    MissionNodeConnection connectionBetweenNodes = Object.Instantiate<MissionNodeConnection>(this._nodeConnectionPref, this._connectionsCanvas.transform, false);
    connectionBetweenNodes.Setup(parentMissionNode.missionNodeVisualController, childMissionNode.missionNodeVisualController);
    return connectionBetweenNodes;
  }
}
