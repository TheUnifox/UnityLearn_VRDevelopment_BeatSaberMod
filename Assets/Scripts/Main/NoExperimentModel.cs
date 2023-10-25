// Decompiled with JetBrains decompiler
// Type: NoExperimentModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Threading.Tasks;

public class NoExperimentModel : IExperimentModel
{
  public virtual Task<bool> IsEmployee() => Task.FromResult<bool>(false);

  public virtual Task<bool> IsInTest(IExperimentData data) => Task.FromResult<bool>(false);
}
