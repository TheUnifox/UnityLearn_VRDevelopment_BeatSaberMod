// Decompiled with JetBrains decompiler
// Type: ScenesTransitionSetupDataSO
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Threading.Tasks;
using Zenject;

public abstract class ScenesTransitionSetupDataSO : PersistentScriptableObject
{
  private SceneSetupData[] _sceneSetupDataArray;

  public SceneInfo[] scenes { get; private set; }

  public bool beforeScenesWillBeActivatedTaskIsComplete { get; private set; }

  protected void Init(SceneInfo[] scenes, SceneSetupData[] sceneSetupData)
  {
    this.scenes = scenes;
    this._sceneSetupDataArray = sceneSetupData;
    this.beforeScenesWillBeActivatedTaskIsComplete = false;
  }

    public async void BeforeScenesWillBeActivated(bool runAsync)
    {
        this.beforeScenesWillBeActivatedTaskIsComplete = false;
        if (runAsync)
        {
            await this.BeforeScenesWillBeActivatedAsync();
            this.beforeScenesWillBeActivatedTaskIsComplete = true;
        }
        else
        {
            Task.Run(async delegate ()
            {
                await this.BeforeScenesWillBeActivatedAsync();
            }).Wait();
            this.beforeScenesWillBeActivatedTaskIsComplete = true;
        }
    }

    protected virtual Task BeforeScenesWillBeActivatedAsync() => Task.CompletedTask;

  public void InstallBindings(DiContainer container)
  {
    if (this._sceneSetupDataArray == null)
      return;
    foreach (SceneSetupData sceneSetupData in this._sceneSetupDataArray)
    {
      if (sceneSetupData != null)
      {
        Type type = sceneSetupData.GetType();
        container.Bind(type).FromInstance((object) sceneSetupData).AsSingle();
      }
    }
  }
}
