// Decompiled with JetBrains decompiler
// Type: BTSCharacterSpawnEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class BTSCharacterSpawnEventEffect : MonoBehaviour
{
  [SerializeField]
  protected float _animationStartAheadTime = 0.5f;
  [SerializeField]
  protected BTSCharacterDataModel _btsCharacterDataModel;
  [SerializeField]
  [NullAllowed(NullAllowed.Context.Prefab)]
  protected Transform _characterWrapper;
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly IReadonlyBeatmapData _beatmapData;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  [Inject]
  protected readonly EnvironmentContext _environmentContext;
  protected const BasicBeatmapEventType kCharacterDisplayEventType = BasicBeatmapEventType.Special0;
  protected readonly Dictionary<int, BTSCharacter> _idsToCharacterPrefabsDictionary = new Dictionary<int, BTSCharacter>();
  protected readonly Dictionary<int, BTSCharacter> _idsToCharactersDictionary = new Dictionary<int, BTSCharacter>();
  protected readonly Dictionary<int, AnimationClip> _idsToAnimationClipsDictionary = new Dictionary<int, AnimationClip>();
  protected bool _isInitialized;
  protected BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;
  protected readonly List<AsyncOperationHandle> _asyncOperationHandles = new List<AsyncOperationHandle>();

  public event System.Action<BTSCharacter> startCharacterAnimationEvent;

  public bool isInitialized => this._isInitialized;

  public virtual void Start()
  {
    this.LoadAddressables();
    foreach (BasicBeatmapEventData beatmapDataItem in this._beatmapData.GetBeatmapDataItems<BasicBeatmapEventData>(40))
    {
      int prefabId = BTSCharacterSpawnEventValueParser.GetPrefabId(beatmapDataItem.value);
      int animationId = BTSCharacterSpawnEventValueParser.GetAnimationId(beatmapDataItem.value);
      Tuple<BTSCharacter, AnimationClip> withAnimationClip = BTSCharacterSpawnEventEffect.GetCharacterWithAnimationClip(this._idsToCharacterPrefabsDictionary, this._idsToAnimationClipsDictionary, prefabId, animationId);
      if (withAnimationClip == null)
        return;
      BTSCharacter btsCharacter = UnityEngine.Object.Instantiate<BTSCharacter>(withAnimationClip.Item1, this._characterWrapper);
      btsCharacter.gameObject.SetActive(false);
      this._idsToCharactersDictionary[prefabId] = btsCharacter;
    }
    this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(this._animationStartAheadTime, new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(BasicBeatmapEventType.Special0));
    this._isInitialized = true;
  }

  public virtual void LoadAddressables()
  {
    IEnumerable<BasicBeatmapEventData> beatmapDataItems = this._beatmapData.GetBeatmapDataItems<BasicBeatmapEventData>(40);
    HashSet<int> intSet1 = new HashSet<int>();
    HashSet<int> intSet2 = new HashSet<int>();
    foreach (BasicBeatmapEventData beatmapEventData in beatmapDataItems)
    {
      int prefabId = BTSCharacterSpawnEventValueParser.GetPrefabId(beatmapEventData.value);
      int animationId = BTSCharacterSpawnEventValueParser.GetAnimationId(beatmapEventData.value);
      intSet1.Add(prefabId);
      intSet2.Add(animationId);
    }
    for (int index = 0; index < this._btsCharacterDataModel.prefabsWithIds.Length; ++index)
    {
      BTSCharacterDataModel.PrefabWithId prefabsWithId = this._btsCharacterDataModel.prefabsWithIds[index];
      if (intSet1.Contains(prefabsWithId.id))
      {
        AsyncOperationHandle<GameObject> asyncOperationHandle = prefabsWithId.prefabAssetReference.LoadAssetAsync<GameObject>();
        this._asyncOperationHandles.Add((AsyncOperationHandle) asyncOperationHandle);
        GameObject gameObject = asyncOperationHandle.WaitForCompletion();
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          this._idsToCharacterPrefabsDictionary[prefabsWithId.id] = gameObject.GetComponent<BTSCharacter>();
      }
    }
    for (int index = 0; index < this._btsCharacterDataModel.animationClipsWithIds.Length; ++index)
    {
      BTSCharacterDataModel.AnimationClipWithId animationClipsWithId = this._btsCharacterDataModel.animationClipsWithIds[index];
      if (intSet2.Contains(animationClipsWithId.id))
      {
        AsyncOperationHandle<AnimationClip> asyncOperationHandle = animationClipsWithId.animationClipAssetReference.LoadAssetAsync<AnimationClip>();
        this._asyncOperationHandles.Add((AsyncOperationHandle) asyncOperationHandle);
        AnimationClip animationClip = asyncOperationHandle.WaitForCompletion();
        this._idsToAnimationClipsDictionary[animationClipsWithId.id] = animationClip;
      }
    }
  }

  public virtual void CleanupAddressables()
  {
    for (int index = 0; index < this._asyncOperationHandles.Count; ++index)
    {
      if (this._asyncOperationHandles[index].IsValid())
        Addressables.Release(this._asyncOperationHandles[index]);
    }
    this._asyncOperationHandles.Clear();
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapCallbacksController != null)
      this._beatmapCallbacksController.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
    this.CleanupAddressables();
  }

  public virtual void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
  {
    if ((double) Mathf.Abs(basicBeatmapEventData.time - this._audioTimeSource.songTime) > (double) Time.deltaTime + (double) this._animationStartAheadTime)
      return;
    int prefabId = BTSCharacterSpawnEventValueParser.GetPrefabId(basicBeatmapEventData.value);
    int animationId = BTSCharacterSpawnEventValueParser.GetAnimationId(basicBeatmapEventData.value);
    bool alternativeMaterial = BTSCharacterSpawnEventValueParser.GetIsAlternativeMaterial(basicBeatmapEventData.value);
    Tuple<BTSCharacter, AnimationClip> withAnimationClip = BTSCharacterSpawnEventEffect.GetCharacterWithAnimationClip(this._idsToCharactersDictionary, this._idsToAnimationClipsDictionary, prefabId, animationId);
    if (withAnimationClip == null)
      return;
    withAnimationClip.Item1.SetAlternativeAnimationAndMaterial(withAnimationClip.Item2, alternativeMaterial);
    System.Action<BTSCharacter> characterAnimationEvent = this.startCharacterAnimationEvent;
    if (characterAnimationEvent == null)
      return;
    characterAnimationEvent(withAnimationClip.Item1);
  }

  private static Tuple<BTSCharacter, AnimationClip> GetCharacterWithAnimationClip(
    Dictionary<int, BTSCharacter> charDictionary,
    Dictionary<int, AnimationClip> animDictionary,
    int prefabId,
    int animationId)
  {
    BTSCharacter btsCharacter;
    if (!charDictionary.TryGetValue(prefabId, out btsCharacter))
    {
      Debug.LogError((object) "Non-existing character prefab id");
      return (Tuple<BTSCharacter, AnimationClip>) null;
    }
    AnimationClip animationClip;
    if (animDictionary.TryGetValue(animationId, out animationClip))
      return new Tuple<BTSCharacter, AnimationClip>(btsCharacter, animationClip);
    Debug.LogError((object) "Non-existing character animation id");
    return (Tuple<BTSCharacter, AnimationClip>) null;
  }
}
