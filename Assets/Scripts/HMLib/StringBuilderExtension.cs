// Decompiled with JetBrains decompiler
// Type: StringBuilderExtension
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Text;

public static class StringBuilderExtension
{
  [DoesNotRequireDomainReloadInit]
  private static char[] charToInt = new char[10]
  {
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9'
  };

  public static void Swap(this StringBuilder sb, int startIndex, int endIndex)
  {
    int num = (endIndex - startIndex + 1) / 2;
    for (int index = 0; index < num; ++index)
    {
      char ch = sb[startIndex + index];
      sb[startIndex + index] = sb[endIndex - index];
      sb[endIndex - index] = ch;
    }
  }

  public static void AppendNumber(this StringBuilder sb, int number)
  {
    int length = sb.Length;
    uint num;
    bool flag;
    if (number < 0)
    {
      num = number == int.MinValue ? (uint) number : (uint) -number;
      flag = true;
    }
    else
    {
      num = (uint) number;
      flag = false;
    }
    do
    {
      sb.Append(StringBuilderExtension.charToInt[(int) (num % 10U)]);
      num /= 10U;
    }
    while (num != 0U);
    if (flag)
      sb.Append('-');
    sb.Swap(length, sb.Length - 1);
  }

  public static void AppendNumber(this StringBuilder sb, uint unumber)
  {
    int length = sb.Length;
    do
    {
      sb.Append(StringBuilderExtension.charToInt[(int) (unumber % 10U)]);
      unumber /= 10U;
    }
    while (unumber != 0U);
    sb.Swap(length, sb.Length - 1);
  }
}
