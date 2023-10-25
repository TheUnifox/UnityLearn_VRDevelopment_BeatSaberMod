// Decompiled with JetBrains decompiler
// Type: NamedValueChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Text;

public class NamedValueChecker
{
  public static bool Check(
    string fieldName,
    object value,
    object expectedValue,
    StringBuilder assertMessageSb)
  {
    if ((value == null || value.Equals(expectedValue)) && (value != null || expectedValue == null))
      return true;
    assertMessageSb.Append(string.Format("{0}: {1}, expected: {2}\n", (object) fieldName, value, expectedValue));
    return false;
  }
}
