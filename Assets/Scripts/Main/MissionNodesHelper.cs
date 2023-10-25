// Decompiled with JetBrains decompiler
// Type: MissionNodesHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class MissionNodesHelper
{
  public static HashSet<MissionNode> GetAllNodesFromRoot(MissionNode root)
  {
    HashSet<MissionNode> visitedNodes = new HashSet<MissionNode>();
    MissionNodesHelper.VisitAllTree(root, visitedNodes);
    return visitedNodes;
  }

  private static void VisitAllTree(MissionNode node, HashSet<MissionNode> visitedNodes)
  {
    if (visitedNodes.Contains(node))
      return;
    visitedNodes.Add(node);
    foreach (MissionNode childNode in node.childNodes)
      MissionNodesHelper.VisitAllTree(childNode, visitedNodes);
  }

  public static bool CycleDetection(MissionNode node) => MissionNodesHelper.CycleDetection(node, 0, new Dictionary<MissionNode, int>());

  private static bool CycleDetection(
    MissionNode node,
    int layer,
    Dictionary<MissionNode, int> layers)
  {
    if (layers.ContainsKey(node))
      return layer > layers[node];
    layers[node] = layer;
    foreach (MissionNode childNode in node.childNodes)
    {
      if (MissionNodesHelper.CycleDetection(childNode, layer + 1, layers))
        return true;
    }
    layers.Remove(node);
    return false;
  }

  public static bool FinalNodeIsFinal(MissionNode finalNode, MissionNode rootNode) => MissionNodesHelper.FinalNodeIsFinal(finalNode, rootNode, new HashSet<MissionNode>());

  private static bool FinalNodeIsFinal(
    MissionNode finalNode,
    MissionNode node,
    HashSet<MissionNode> visitedNodes)
  {
    if (visitedNodes.Contains(node))
      return true;
    if ((double) finalNode.position.y < (double) node.position.y)
      return false;
    visitedNodes.Add(node);
    foreach (MissionNode childNode in node.childNodes)
    {
      if (!MissionNodesHelper.FinalNodeIsFinal(finalNode, childNode, visitedNodes))
        return false;
    }
    return true;
  }
}
