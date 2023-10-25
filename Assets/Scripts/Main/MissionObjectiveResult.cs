// Decompiled with JetBrains decompiler
// Type: MissionObjectiveResult
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

public class MissionObjectiveResult
{
  [CompilerGenerated]
  protected MissionObjective m_CmissionObjective;
  [CompilerGenerated]
  protected bool m_Ccleared;
  [CompilerGenerated]
  protected int m_Cvalue;

  public MissionObjective missionObjective
  {
    get => this.m_CmissionObjective;
    private set => this.m_CmissionObjective = value;
  }

  public bool cleared
  {
    get => this.m_Ccleared;
    private set => this.m_Ccleared = value;
  }

  public int value
  {
    get => this.m_Cvalue;
    private set => this.m_Cvalue = value;
  }

  public MissionObjectiveResult(MissionObjective missionObjective, bool cleared, int value)
  {
    this.missionObjective = missionObjective;
    this.cleared = cleared;
    this.value = value;
  }
}
