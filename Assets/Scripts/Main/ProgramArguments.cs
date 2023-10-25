// Decompiled with JetBrains decompiler
// Type: ProgramArguments
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;

public class ProgramArguments
{
  protected readonly IReadOnlyList<string> _arguments;

  public IReadOnlyList<string> arguments => this._arguments;

  public ProgramArguments(IReadOnlyList<string> arguments) => this._arguments = arguments;

  public static ProgramArguments Empty() => new ProgramArguments((IReadOnlyList<string>) Array.Empty<string>());
}
