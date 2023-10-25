// Decompiled with JetBrains decompiler
// Type: GameScenesManager
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;

public class GameScenesManager : MonoBehaviour
{
  [SerializeField]
  protected SceneInfo _emptyTransitionSceneInfo;
  [Inject]
  protected readonly ZenjectSceneLoader _zenjectSceneLoader;
  public const float kStandardTransitionLength = 0.7f;
  public const float kShortTransitionLength = 0.35f;
  public const float kLongTransitionLength = 1.3f;
  protected bool _inTransition;
  protected readonly List<GameScenesManager.ScenesStackData> _scenesStack = new List<GameScenesManager.ScenesStackData>();
  protected readonly HashSet<string> _neverUnloadScenes = new HashSet<string>();
  protected const string kRootContainerGOName = "RootContainer";

  public event System.Action<float> transitionDidStartEvent;

  public event System.Action beforeDismissingScenesEvent;

  public event System.Action<ScenesTransitionSetupDataSO, DiContainer> transitionDidFinishEvent;

  public event System.Action<ScenesTransitionSetupDataSO, DiContainer> installEarlyBindingsEvent;

  public DiContainer currentScenesContainer => this._scenesStack.Last<GameScenesManager.ScenesStackData>().container;

  public bool isInTransition => this._inTransition;

  public WaitUntil waitUntilSceneTransitionFinish => new WaitUntil((Func<bool>) (() => !this.isInTransition));

  public virtual void MarkSceneAsPersistent(string sceneName) => this._neverUnloadScenes.Add(sceneName);

  public virtual List<string> GetCurrentlyLoadedSceneNames()
  {
    List<string> loadedSceneNames = new List<string>(SceneManager.sceneCount);
    for (int index = 0; index < SceneManager.sceneCount; ++index)
    {
      Scene sceneAt = SceneManager.GetSceneAt(index);
      if (!sceneAt.name.Contains("InitTestScene"))
        loadedSceneNames.Add(sceneAt.name);
    }
    return loadedSceneNames;
  }

  public virtual void PushScenes(
    ScenesTransitionSetupDataSO scenesTransitionSetupData,
    float minDuration = 0.0f,
    System.Action afterMinDurationCallback = null,
    System.Action<DiContainer> finishCallback = null)
  {
    if (this._inTransition)
      return;
    this._inTransition = true;
    System.Action<float> transitionDidStartEvent = this.transitionDidStartEvent;
    if (transitionDidStartEvent != null)
      transitionDidStartEvent(minDuration);
    List<string> stringList1 = this.SceneNamesFromSceneInfoArray(scenesTransitionSetupData.scenes);
    List<string> stringList2;
    if (this._scenesStack.Count > 0)
    {
      stringList2 = this._scenesStack[this._scenesStack.Count - 1].sceneNames;
    }
    else
    {
      stringList2 = this.GetCurrentlyLoadedSceneNames();
      this._scenesStack.Add(new GameScenesManager.ScenesStackData(stringList2));
    }
    GameScenesManager.ScenesStackData scenesStackData = new GameScenesManager.ScenesStackData(stringList1.ToList<string>());
    this._scenesStack.Add(scenesStackData);
    this.StartCoroutine(this.ScenesTransitionCoroutine(scenesTransitionSetupData, stringList1, GameScenesManager.ScenePresentType.Load, stringList2, GameScenesManager.SceneDismissType.Deactivate, minDuration, afterMinDurationCallback, (System.Action<DiContainer>) (container =>
    {
      scenesStackData.SetDiContainer(container);
      scenesTransitionSetupData.InstallBindings(container);
      System.Action<ScenesTransitionSetupDataSO, DiContainer> earlyBindingsEvent = this.installEarlyBindingsEvent;
      if (earlyBindingsEvent == null)
        return;
      earlyBindingsEvent(scenesTransitionSetupData, container);
    }), (System.Action<DiContainer>) (container =>
    {
      this._inTransition = false;
      System.Action<ScenesTransitionSetupDataSO, DiContainer> transitionDidFinishEvent = this.transitionDidFinishEvent;
      if (transitionDidFinishEvent != null)
        transitionDidFinishEvent(scenesTransitionSetupData, scenesStackData.container);
      System.Action<DiContainer> action = finishCallback;
      if (action == null)
        return;
      action(container);
    })));
  }

  public virtual void PopScenes(
    float minDuration = 0.0f,
    System.Action afterMinDurationCallback = null,
    System.Action<DiContainer> finishCallback = null)
  {
    if (this._inTransition)
      return;
    this._inTransition = true;
    System.Action<float> transitionDidStartEvent = this.transitionDidStartEvent;
    if (transitionDidStartEvent != null)
      transitionDidStartEvent(minDuration);
    List<string> sceneNames1 = this._scenesStack[this._scenesStack.Count - 1].sceneNames;
    List<string> sceneNames2 = this._scenesStack[this._scenesStack.Count - 2].sceneNames;
    this._scenesStack.RemoveAt(this._scenesStack.Count - 1);
    this.StartCoroutine(this.ScenesTransitionCoroutine((ScenesTransitionSetupDataSO) null, sceneNames2, GameScenesManager.ScenePresentType.Activate, sceneNames1, GameScenesManager.SceneDismissType.Unload, minDuration, afterMinDurationCallback, (System.Action<DiContainer>) null, (System.Action<DiContainer>) (container =>
    {
      this._inTransition = false;
      System.Action<ScenesTransitionSetupDataSO, DiContainer> transitionDidFinishEvent = this.transitionDidFinishEvent;
      if (transitionDidFinishEvent != null)
        transitionDidFinishEvent((ScenesTransitionSetupDataSO) null, this._scenesStack.Last<GameScenesManager.ScenesStackData>().container);
      System.Action<DiContainer> action = finishCallback;
      if (action == null)
        return;
      action(container);
    })));
  }

  public virtual void ReplaceScenes(
    ScenesTransitionSetupDataSO scenesTransitionSetupData,
    IEnumerator[] beforeNewScenesActivateRoutines = null,
    float minDuration = 0.0f,
    System.Action afterMinDurationCallback = null,
    System.Action<DiContainer> finishCallback = null)
  {
    if (this._inTransition)
      return;
    this._inTransition = true;
    System.Action<float> transitionDidStartEvent = this.transitionDidStartEvent;
    if (transitionDidStartEvent != null)
      transitionDidStartEvent(minDuration);
    List<string> newSceneNames = this.SceneNamesFromSceneInfoArray(scenesTransitionSetupData.scenes);
    GameScenesManager.ScenesStackData scenesStackData = new GameScenesManager.ScenesStackData(newSceneNames);
    List<string> scenesToDismiss;
    if (this._scenesStack.Count > 0)
    {
      scenesToDismiss = this._scenesStack[this._scenesStack.Count - 1].sceneNames;
      this._scenesStack[this._scenesStack.Count - 1] = scenesStackData;
    }
    else
    {
      scenesToDismiss = this.GetCurrentlyLoadedSceneNames();
      this._scenesStack.Add(scenesStackData);
    }
    List<string> emptyTransitionSceneNameList = new List<string>()
    {
      this._emptyTransitionSceneInfo.sceneName
    };
    this.StartCoroutine(this.ScenesTransitionCoroutine((ScenesTransitionSetupDataSO) null, emptyTransitionSceneNameList, GameScenesManager.ScenePresentType.Load, scenesToDismiss, GameScenesManager.SceneDismissType.Unload, minDuration, afterMinDurationCallback, (System.Action<DiContainer>) null, (System.Action<DiContainer>) (emptySceneContainer => this.StartCoroutine(this.ScenesTransitionCoroutine(scenesTransitionSetupData, newSceneNames, GameScenesManager.ScenePresentType.Load, emptyTransitionSceneNameList, GameScenesManager.SceneDismissType.Unload, 0.0f, (System.Action) null, (System.Action<DiContainer>) (container =>
    {
      scenesStackData.SetDiContainer(container);
      scenesTransitionSetupData.InstallBindings(container);
      System.Action<ScenesTransitionSetupDataSO, DiContainer> earlyBindingsEvent = this.installEarlyBindingsEvent;
      if (earlyBindingsEvent == null)
        return;
      earlyBindingsEvent(scenesTransitionSetupData, container);
    }), (System.Action<DiContainer>) (container =>
    {
      this._inTransition = false;
      System.Action<ScenesTransitionSetupDataSO, DiContainer> transitionDidFinishEvent = this.transitionDidFinishEvent;
      if (transitionDidFinishEvent != null)
        transitionDidFinishEvent(scenesTransitionSetupData, scenesStackData.container);
      System.Action<DiContainer> action = finishCallback;
      if (action == null)
        return;
      action(container);
    }))))));
  }

  public virtual void ClearAndOpenScenes(
    ScenesTransitionSetupDataSO scenesTransitionSetupData,
    float minDuration = 0.0f,
    System.Action afterMinDurationCallback = null,
    System.Action<DiContainer> finishCallback = null,
    bool unloadAllScenes = true)
  {
    if (this._inTransition)
      return;
    this._inTransition = true;
    System.Action<float> transitionDidStartEvent = this.transitionDidStartEvent;
    if (transitionDidStartEvent != null)
      transitionDidStartEvent(minDuration);
    List<string> newSceneNames = this.SceneNamesFromSceneInfoArray(scenesTransitionSetupData.scenes);
    List<string> scenesToDismiss;
    if (this._scenesStack.Count > 0)
    {
      scenesToDismiss = new List<string>();
      foreach (GameScenesManager.ScenesStackData scenes in this._scenesStack)
        scenesToDismiss.AddRange((IEnumerable<string>) scenes.sceneNames);
    }
    else
      scenesToDismiss = this.GetCurrentlyLoadedSceneNames();
    if (unloadAllScenes)
    {
      foreach (string neverUnloadScene in this._neverUnloadScenes)
        scenesToDismiss.Add(neverUnloadScene);
      this._neverUnloadScenes.Clear();
    }
    this._scenesStack.Clear();
    GameScenesManager.ScenesStackData scenesStackData = new GameScenesManager.ScenesStackData(newSceneNames.ToList<string>());
    this._scenesStack.Add(scenesStackData);
    List<string> emptyTransitionSceneNameList = new List<string>()
    {
      this._emptyTransitionSceneInfo.sceneName
    };
    this.StartCoroutine(this.ScenesTransitionCoroutine((ScenesTransitionSetupDataSO) null, emptyTransitionSceneNameList, GameScenesManager.ScenePresentType.Load, scenesToDismiss, GameScenesManager.SceneDismissType.Unload, minDuration, afterMinDurationCallback, (System.Action<DiContainer>) null, (System.Action<DiContainer>) (emptySceneContainer => this.StartCoroutine(this.ScenesTransitionCoroutine(scenesTransitionSetupData, newSceneNames, GameScenesManager.ScenePresentType.Load, emptyTransitionSceneNameList, GameScenesManager.SceneDismissType.Unload, 0.0f, (System.Action) null, (System.Action<DiContainer>) (container =>
    {
      scenesStackData.SetDiContainer(container);
      scenesTransitionSetupData.InstallBindings(container);
      System.Action<ScenesTransitionSetupDataSO, DiContainer> earlyBindingsEvent = this.installEarlyBindingsEvent;
      if (earlyBindingsEvent == null)
        return;
      earlyBindingsEvent(scenesTransitionSetupData, container);
    }), (System.Action<DiContainer>) (container =>
    {
      this._inTransition = false;
      System.Action<ScenesTransitionSetupDataSO, DiContainer> transitionDidFinishEvent = this.transitionDidFinishEvent;
      if (transitionDidFinishEvent != null)
        transitionDidFinishEvent(scenesTransitionSetupData, scenesStackData.container);
      System.Action<DiContainer> action = finishCallback;
      if (action == null)
        return;
      action(container);
    }))))));
  }

  public virtual void AppendScenes(
    ScenesTransitionSetupDataSO scenesTransitionSetupData,
    float minDuration = 0.0f,
    System.Action afterMinDurationCallback = null,
    System.Action<DiContainer> finishCallback = null)
  {
    if (this._inTransition)
      return;
    this._inTransition = true;
    System.Action<float> transitionDidStartEvent = this.transitionDidStartEvent;
    if (transitionDidStartEvent != null)
      transitionDidStartEvent(minDuration);
    List<string> stringList1;
    if (this._scenesStack.Count > 0)
    {
      stringList1 = this._scenesStack[this._scenesStack.Count - 1].sceneNames;
    }
    else
    {
      stringList1 = this.GetCurrentlyLoadedSceneNames();
      this._scenesStack.Add(new GameScenesManager.ScenesStackData(stringList1));
    }
    List<string> stringList2 = this.SceneNamesFromSceneInfoArray(scenesTransitionSetupData.scenes);
    List<string> sceneNames = new List<string>((IEnumerable<string>) stringList1);
    sceneNames.AddRange((IEnumerable<string>) stringList2);
    GameScenesManager.ScenesStackData scenesStackData = new GameScenesManager.ScenesStackData(sceneNames);
    this._scenesStack.Add(scenesStackData);
    this.StartCoroutine(this.ScenesTransitionCoroutine(scenesTransitionSetupData, stringList2, GameScenesManager.ScenePresentType.Load, new List<string>(), GameScenesManager.SceneDismissType.DoNotUnload, minDuration, (System.Action) null, (System.Action<DiContainer>) (container =>
    {
      scenesStackData.SetDiContainer(container);
      scenesTransitionSetupData.InstallBindings(container);
      System.Action<ScenesTransitionSetupDataSO, DiContainer> earlyBindingsEvent = this.installEarlyBindingsEvent;
      if (earlyBindingsEvent == null)
        return;
      earlyBindingsEvent(scenesTransitionSetupData, container);
    }), (System.Action<DiContainer>) (container =>
    {
      this._inTransition = false;
      System.Action<ScenesTransitionSetupDataSO, DiContainer> transitionDidFinishEvent = this.transitionDidFinishEvent;
      if (transitionDidFinishEvent != null)
        transitionDidFinishEvent(scenesTransitionSetupData, scenesStackData.container);
      System.Action<DiContainer> action = finishCallback;
      if (action == null)
        return;
      action(container);
    })));
  }

  public virtual void RemoveScenes(
    ScenesTransitionSetupDataSO scenesTransitionSetupDataSo,
    float minDuration = 0.0f,
    System.Action afterMinDurationCallback = null,
    System.Action<DiContainer> finishCallback = null)
  {
    if (this._inTransition)
      return;
    this._inTransition = true;
    System.Action<float> transitionDidStartEvent = this.transitionDidStartEvent;
    if (transitionDidStartEvent != null)
      transitionDidStartEvent(minDuration);
    List<string> sceneNames1 = this._scenesStack[this._scenesStack.Count - 1].sceneNames;
    List<string> sceneNames2 = this._scenesStack[this._scenesStack.Count - 2].sceneNames;
    this._scenesStack.RemoveAt(this._scenesStack.Count - 1);
    List<string> sceneNamesToRemove = this.SceneNamesFromSceneInfoArray(scenesTransitionSetupDataSo.scenes);
    this.StartCoroutine(this.ScenesTransitionCoroutine((ScenesTransitionSetupDataSO) null, new List<string>(), GameScenesManager.ScenePresentType.DoNotLoad, sceneNamesToRemove, GameScenesManager.SceneDismissType.Unload, minDuration, afterMinDurationCallback, (System.Action<DiContainer>) null, (System.Action<DiContainer>) (container =>
    {
      this._inTransition = false;
      System.Action<ScenesTransitionSetupDataSO, DiContainer> transitionDidFinishEvent = this.transitionDidFinishEvent;
      if (transitionDidFinishEvent != null)
        transitionDidFinishEvent((ScenesTransitionSetupDataSO) null, this._scenesStack.Last<GameScenesManager.ScenesStackData>().container);
      System.Action<DiContainer> action = finishCallback;
      if (action == null)
        return;
      action(container);
    })));
    List<string> list = sceneNames1.Where<string>((Func<string, bool>) (scene => !sceneNamesToRemove.Contains(scene))).ToList<string>();
    List<string> second = list;
    if (sceneNames2.SequenceEqual<string>((IEnumerable<string>) second))
      return;
    this._scenesStack.Add(new GameScenesManager.ScenesStackData(list));
  }

  public virtual IEnumerator ScenesTransitionCoroutine(
    ScenesTransitionSetupDataSO newScenesTransitionSetupData,
    List<string> scenesToPresent,
    GameScenesManager.ScenePresentType presentType,
    List<string> scenesToDismiss,
    GameScenesManager.SceneDismissType dismissType,
    float minDuration,
    System.Action afterMinDurationCallback,
    System.Action<DiContainer> extraBindingsCallback,
    System.Action<DiContainer> finishCallback)
  {
    scenesToDismiss = scenesToDismiss.Except<string>((IEnumerable<string>) this._neverUnloadScenes).ToList<string>();
    scenesToPresent = scenesToPresent.Except<string>((IEnumerable<string>) this._neverUnloadScenes).ToList<string>();
    EventSystem eventSystem = EventSystem.current;
    if ((UnityEngine.Object) eventSystem != (UnityEngine.Object) null)
      eventSystem.enabled = false;
    yield return (object) new WaitForSeconds(minDuration);
    switch (presentType)
    {
      case GameScenesManager.ScenePresentType.Load:
        if ((UnityEngine.Object) newScenesTransitionSetupData != (UnityEngine.Object) null)
          newScenesTransitionSetupData.BeforeScenesWillBeActivated(true);
        if (scenesToPresent.Count == 1)
        {
          string sceneName = scenesToPresent[0];
          AsyncOperation loadSceneOperation = this._zenjectSceneLoader.LoadSceneAsync(sceneName, LoadSceneMode.Additive, extraBindingsCallback, (System.Action<DiContainer>) null, LoadSceneRelationship.None, (System.Action<DiContainer>) null);
          loadSceneOperation.allowSceneActivation = false;
          loadSceneOperation.priority = int.MaxValue;
          if ((UnityEngine.Object) newScenesTransitionSetupData != (UnityEngine.Object) null)
            yield return (object) new WaitWhile((Func<bool>) (() => !newScenesTransitionSetupData.beforeScenesWillBeActivatedTaskIsComplete));
          if ((UnityEngine.Object) eventSystem != (UnityEngine.Object) null)
            eventSystem.enabled = true;
          System.Action action = afterMinDurationCallback;
          if (action != null)
            action();
          loadSceneOperation.allowSceneActivation = true;
          yield return (object) loadSceneOperation;
          SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
          GameScenesManager.ActivatePresentedSceneRootObjects(new List<string>()
          {
            sceneName
          });
          sceneName = (string) null;
          loadSceneOperation = (AsyncOperation) null;
          break;
        }
        int sceneNum = 0;
        foreach (string str in scenesToPresent)
        {
          SceneManager.GetSceneByName(str);
          AsyncOperation asyncOperation = sceneNum != 0 ? SceneManager.LoadSceneAsync(str, LoadSceneMode.Additive) : this._zenjectSceneLoader.LoadSceneAsync(str, LoadSceneMode.Additive, extraBindingsCallback, (System.Action<DiContainer>) null, LoadSceneRelationship.None, (System.Action<DiContainer>) null);
          asyncOperation.priority = int.MaxValue;
          yield return (object) asyncOperation;
          ++sceneNum;
        }
        if ((UnityEngine.Object) newScenesTransitionSetupData != (UnityEngine.Object) null)
          yield return (object) new WaitWhile((Func<bool>) (() => !newScenesTransitionSetupData.beforeScenesWillBeActivatedTaskIsComplete));
        if ((UnityEngine.Object) eventSystem != (UnityEngine.Object) null)
          eventSystem.enabled = true;
        System.Action action1 = afterMinDurationCallback;
        if (action1 != null)
          action1();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenesToPresent[scenesToPresent.Count - 1]));
        GameScenesManager.ActivatePresentedSceneRootObjects(scenesToPresent);
        break;
      case GameScenesManager.ScenePresentType.Activate:
        if ((UnityEngine.Object) eventSystem != (UnityEngine.Object) null)
          eventSystem.enabled = true;
        System.Action action2 = afterMinDurationCallback;
        if (action2 != null)
          action2();
        if (scenesToPresent.Count > 0)
        {
          SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenesToPresent[scenesToPresent.Count - 1]));
          using (List<string>.Enumerator enumerator = scenesToPresent.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.MoveGameObjectsFromContainerToSceneRoot(enumerator.Current);
            break;
          }
        }
        else
          break;
      default:
        if (presentType == GameScenesManager.ScenePresentType.DoNotLoad && (UnityEngine.Object) eventSystem != (UnityEngine.Object) null)
        {
          eventSystem.enabled = true;
          break;
        }
        break;
    }
    System.Action dismissingScenesEvent = this.beforeDismissingScenesEvent;
    if (dismissingScenesEvent != null)
      dismissingScenesEvent();
    switch (dismissType)
    {
      case GameScenesManager.SceneDismissType.Unload:
        this.SetActiveRootObjectsInScenes(scenesToDismiss, false);
        foreach (string sceneName in scenesToDismiss)
          yield return (object) SceneManager.UnloadSceneAsync(sceneName);
        break;
      case GameScenesManager.SceneDismissType.Deactivate:
        using (List<string>.Enumerator enumerator = scenesToDismiss.GetEnumerator())
        {
          while (enumerator.MoveNext())
            this.ReparentRootGameObjectsToDisabledGameObject(enumerator.Current);
          break;
        }
    }
    yield return (object) Resources.UnloadUnusedAssets();
    GC.Collect();
    UnityEngine.Random.InitState(0);
    System.Action<DiContainer> action3 = finishCallback;
    if (action3 != null)
      action3(this._scenesStack[this._scenesStack.Count - 1].container);
  }

  private static void ActivatePresentedSceneRootObjects(List<string> scenesToPresent)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    foreach (string name in scenesToPresent)
    {
      GameObject[] rootGameObjects = SceneManager.GetSceneByName(name).GetRootGameObjects();
      gameObjectList.AddRange((IEnumerable<GameObject>) rootGameObjects);
    }
    UnityEngine.Random.InitState(0);
    foreach (GameObject gameObject in gameObjectList)
      gameObject.SetActive(true);
  }

  public virtual bool IsAnySceneInStack(List<string> sceneNames)
  {
    foreach (string sceneName in sceneNames)
    {
      if (this.IsSceneInStack(sceneName))
        return true;
    }
    return false;
  }

  public virtual bool AreAllScenesInStack(List<string> sceneNames) => true;

  public virtual bool IsSceneInStack(string searchSceneName)
  {
    foreach (GameScenesManager.ScenesStackData scenes in this._scenesStack)
    {
      foreach (string sceneName in scenes.sceneNames)
      {
        if (sceneName == searchSceneName)
          return true;
      }
    }
    return false;
  }

  public virtual List<string> SceneNamesFromSceneInfoArray(SceneInfo[] sceneInfos)
  {
    List<string> stringList = new List<string>(sceneInfos.Length);
    foreach (SceneInfo sceneInfo in sceneInfos)
      stringList.Add(sceneInfo.sceneName);
    return stringList;
  }

  public virtual void SetActiveRootObjectsInScenes(List<string> sceneNames, bool value)
  {
    foreach (string sceneName in sceneNames)
    {
      int num = value ? 1 : 0;
      UnityScenesHelper.SetActiveRootObjectsInScene(SceneManager.GetSceneByName(sceneName), value);
    }
  }

  public virtual void ReparentRootGameObjectsToDisabledGameObject(string sceneName)
  {
    Scene sceneByName = SceneManager.GetSceneByName(sceneName);
    GameObject go = new GameObject("RootContainer");
    SceneManager.MoveGameObjectToScene(go, sceneByName);
    Transform transform = go.transform;
    List<GameObject> gameObjectList = new List<GameObject>(sceneByName.rootCount);
    sceneByName.GetRootGameObjects(gameObjectList);
    foreach (GameObject gameObject in gameObjectList)
      gameObject.transform.SetParent(transform, false);
    go.SetActive(false);
  }

  public virtual void MoveGameObjectsFromContainerToSceneRoot(string sceneName)
  {
    Scene sceneByName = SceneManager.GetSceneByName(sceneName);
    SceneManager.SetActiveScene(sceneByName);
    List<GameObject> gameObjectList = new List<GameObject>(sceneByName.rootCount);
    sceneByName.GetRootGameObjects(gameObjectList);
    Transform transform1 = gameObjectList[0].transform;
    List<Transform> transformList = new List<Transform>(transform1.childCount);
    for (int index = 0; index < transform1.childCount; ++index)
      transformList.Add(transform1.GetChild(index));
    foreach (Transform transform2 in transformList)
      transform2.SetParent((Transform) null, false);
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObjectList[0]);
  }

  [Conditional("GamesScenesManagerLogging")]
  private static void Log(string message) => UnityEngine.Debug.Log((object) message);

  public class ScenesStackData
  {
    [CompilerGenerated]
    protected List<string> m_sceneNames;
    [CompilerGenerated]
    protected DiContainer m_container;

    public List<string> sceneNames
    {
      get => this.m_sceneNames;
      private set => this.m_sceneNames = value;
    }

    public DiContainer container
    {
      get => this.m_container;
      private set => this.m_container = value;
    }

    public ScenesStackData(List<string> sceneNames) => this.sceneNames = sceneNames;

    public virtual void SetDiContainer(DiContainer container) => this.container = container;
  }

  public enum ScenePresentType
  {
    DoNotLoad,
    Load,
    Activate,
  }

  public enum SceneDismissType
  {
    DoNotUnload,
    Unload,
    Deactivate,
  }
}
