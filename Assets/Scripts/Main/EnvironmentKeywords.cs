// Decompiled with JetBrains decompiler
// Type: EnvironmentKeywords
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class EnvironmentKeywords
{
  protected readonly IReadOnlyList<string> _environmentKeywords;
  protected readonly HashSet<string> _environmentKeywordsSet;

  public IReadOnlyList<string> environmentKeywords => this._environmentKeywords;

  public EnvironmentKeywords(IReadOnlyList<string> environmentKeywords)
  {
    if (environmentKeywords != null)
    {
      this._environmentKeywords = environmentKeywords;
      this._environmentKeywordsSet = new HashSet<string>((IEnumerable<string>) environmentKeywords);
    }
    else
    {
      this._environmentKeywords = (IReadOnlyList<string>) new List<string>();
      this._environmentKeywordsSet = new HashSet<string>();
    }
  }

  public virtual bool HasKeyword(string keyword) => this._environmentKeywordsSet.Contains(keyword);
}
