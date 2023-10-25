// Decompiled with JetBrains decompiler
// Type: LightGroupGridLayouter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteAlways]
public class LightGroupGridLayouter : LightGroupSubsystem
{
  [SerializeField]
  [Min(1f)]
  protected int _columns = 2;
  [Header("Columns Positioning")]
  [SerializeField]
  protected Vector3 _columnStep = new Vector3(1f, 0.0f, 0.0f);
  [SerializeField]
  protected bool _columnsFromCenter;
  [Header("Rows Positioning")]
  [SerializeField]
  protected Vector3 _rowStep = new Vector3(0.0f, 1f, 0.0f);
  [SerializeField]
  protected bool _rowsFromCenter;
  [Header("Additional Options")]
  [SerializeField]
  protected bool _transposeOrder;
  [SerializeField]
  protected bool _alternateOrder;
  [SerializeField]
  protected Vector3 _defaultRotation = Vector3.zero;
}
