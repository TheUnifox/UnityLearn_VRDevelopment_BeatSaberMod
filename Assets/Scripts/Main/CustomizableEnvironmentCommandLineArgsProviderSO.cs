// Decompiled with JetBrains decompiler
// Type: CustomizableEnvironmentCommandLineArgsProviderSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizableEnvironmentCommandLineArgsProviderSO : 
  PersistentScriptableObject,
  ICommandLineArgsProvider
{
  [SerializeField]
  protected bool _useCustomCommandLineArgs;
  [SerializeField]
  protected bool _useEnvironmentCommandLineArgs;
  [SerializeField]
  protected string _customCommandLineArgs;
  protected readonly EnvironmentCommandLineArgsProvider _environmentCommandLineArgsProvider = new EnvironmentCommandLineArgsProvider();

  public virtual string[] GetCommandLineArgs()
  {
    string[] first = this._useEnvironmentCommandLineArgs ? this._environmentCommandLineArgsProvider.GetCommandLineArgs() : new string[0];
    if (this._useCustomCommandLineArgs && !string.IsNullOrWhiteSpace(this._customCommandLineArgs))
      first = ((IEnumerable<string>) first).Concat<string>((IEnumerable<string>) this._customCommandLineArgs.Split()).ToArray<string>();
    return first;
  }
}
