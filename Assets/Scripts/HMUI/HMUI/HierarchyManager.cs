// Decompiled with JetBrains decompiler
// Type: HMUI.HierarchyManager
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using UnityEngine;
using Zenject;

namespace HMUI
{
  public class HierarchyManager : MonoBehaviour
  {
    [SerializeField]
    protected ScreenSystem _screenSystem;
    [Inject]
    protected GameScenesManager _gameScenesManager;
    protected FlowCoordinator _rootFlowCoordinator;

    public virtual void Start()
    {
      this._gameScenesManager.transitionDidFinishEvent += new Action<ScenesTransitionSetupDataSO, DiContainer>(this.HandleSceneTransitionDidFinish);
      this._gameScenesManager.beforeDismissingScenesEvent += new Action(this.HandleBeforeDismissingScenes);
      if (this._gameScenesManager.isInTransition)
        return;
      this.HandleSceneTransitionDidFinish((ScenesTransitionSetupDataSO) null, (DiContainer) null);
    }

    public virtual void OnDestroy()
    {
      this._gameScenesManager.transitionDidFinishEvent -= new Action<ScenesTransitionSetupDataSO, DiContainer>(this.HandleSceneTransitionDidFinish);
      this._gameScenesManager.beforeDismissingScenesEvent -= new Action(this.HandleBeforeDismissingScenes);
    }

    public virtual void HandleSceneTransitionDidFinish(
      ScenesTransitionSetupDataSO scenesTransitionSetupData,
      DiContainer container)
    {
      if (!this.gameObject.activeInHierarchy || !((UnityEngine.Object) this._rootFlowCoordinator != (UnityEngine.Object) null))
        return;
      FlowCoordinator flowCoordinator = this._rootFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();
      if (flowCoordinator.isActivated)
        return;
      flowCoordinator.__ExternalActivate();
    }

    public virtual void HandleBeforeDismissingScenes()
    {
      if (!this.gameObject.activeInHierarchy || !((UnityEngine.Object) this._rootFlowCoordinator != (UnityEngine.Object) null))
        return;
      FlowCoordinator flowCoordinator = this._rootFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();
      if (!flowCoordinator.isActivated)
        return;
      flowCoordinator.__ExternalDeactivate();
    }

    public virtual void StartWithFlowCoordinator(FlowCoordinator flowCoordinator)
    {
      this._rootFlowCoordinator = flowCoordinator;
      flowCoordinator.__StartOnScreenSystem(this._screenSystem);
    }
  }
}
