// Decompiled with JetBrains decompiler
// Type: LightColorGroup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightColorGroup : LightGroupSubsystem, IEditTimeValidated
{
  [Tooltip("Whether this groups is controlling light with ids defined in List of parents or collection in LightWithIds")]
  [SerializeField]
  protected LightColorGroup.LightColorGroupControlType _lightColorGroupControlType;
  [Header("Single Lights")]
  [SerializeField]
  protected bool _disableAutomaticIdAssignment;
  [SerializeField]
  protected List<LightColorGroupParent> _lightColorGroupParents = new List<LightColorGroupParent>();
  [Header("Light Collections")]
  [SerializeField]
  [NullAllowed]
  protected LightWithIds _lightWithIds;

  public enum LightColorGroupControlType
  {
    LightWithIdsFromHierarchy,
    LightWithIdsCollection,
  }
}
