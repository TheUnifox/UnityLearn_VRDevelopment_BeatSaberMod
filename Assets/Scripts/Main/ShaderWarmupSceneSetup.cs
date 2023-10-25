// Decompiled with JetBrains decompiler
// Type: ShaderWarmupSceneSetup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class ShaderWarmupSceneSetup : MonoInstaller
{
  [SerializeField]
  protected ColorSchemeSO _sharedWarmupColorScheme;

  public override void InstallBindings()
  {
    this.Container.Bind<ColorScheme>().FromInstance(this._sharedWarmupColorScheme.colorScheme).AsSingle();
    this.Container.Bind<ColorManager>().AsSingle();
  }
}
