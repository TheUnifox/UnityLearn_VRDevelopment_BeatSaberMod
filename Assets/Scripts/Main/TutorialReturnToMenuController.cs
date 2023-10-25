// Decompiled with JetBrains decompiler
// Type: TutorialReturnToMenuController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class TutorialReturnToMenuController : MonoBehaviour, IReturnToMenuController
{
  [SerializeField]
  protected TutorialScenesTransitionSetupDataSO _tutorialSceneSetupData;

  public virtual void ReturnToMenu() => this._tutorialSceneSetupData.Finish(TutorialScenesTransitionSetupDataSO.TutorialEndStateType.ReturnToMenu);
}
