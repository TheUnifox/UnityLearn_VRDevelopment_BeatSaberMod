// Decompiled with JetBrains decompiler
// Type: SpawnChevronObjectPoolsInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SpawnChevronObjectPoolsInstaller : MonoInstaller
{
  [SerializeField]
  protected SpawnRotationChevron _spawnRotationChevronPrefab;

  public override void InstallBindings() => this.Container.BindMemoryPool<SpawnRotationChevron, SpawnRotationChevron.Pool>().WithInitialSize(9).FromComponentInNewPrefab((Object) this._spawnRotationChevronPrefab);
}
