// Decompiled with JetBrains decompiler
// Type: AssertExtensions
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Diagnostics;

[DebuggerStepThrough]
public abstract class AssertExtensions
{
  private const string kUnityAssertions = "UNITY_ASSERTIONS";

  [Conditional("UNITY_ASSERTIONS")]
  public static void LessThan(float expected, float value, string message = null)
  {
  }
}
