// Decompiled with JetBrains decompiler
// Type: SingleFixedSceneScenesTransitionSetupDataSO
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class SingleFixedSceneScenesTransitionSetupDataSO : ScenesTransitionSetupDataSO
{
  [SerializeField]
  protected SceneInfo _sceneInfo;

  public SceneInfo sceneInfo => this._sceneInfo;

  public virtual void Init(SceneSetupData sceneSetupData) => this.Init(new SceneInfo[1]
  {
    this._sceneInfo
  }, new SceneSetupData[1]{ sceneSetupData });
}
