// Decompiled with JetBrains decompiler
// Type: BtsResultsEnvironmentController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BtsResultsEnvironmentController : BaseResultsEnvironmentController
{
  [SerializeField]
  protected GameObject _btsCharacterAnimationWrapper;
  [SerializeField]
  protected BTSCharacterDataModel _btsCharacterDataModel;
  [SerializeField]
  protected PlayableDirector _btsCharactersResultsAppearPlayableDirector;
  [Space]
  [SerializeField]
  protected BtsResultsEnvironmentController.BTSResultCharacterPlaceWithAnimation[] _resultPlacesWithAnimations;
  protected const BasicBeatmapEventType kCharacterDisplayEventType = BasicBeatmapEventType.Special0;
  protected List<AsyncOperationHandle> _handles = new List<AsyncOperationHandle>();

  public override void Setup(IReadonlyBeatmapData beatmapData)
  {
    if (beatmapData == null)
      return;
    List<int> intList = new List<int>();
    foreach (BasicBeatmapEventData beatmapDataItem in beatmapData.GetBeatmapDataItems<BasicBeatmapEventData>(40))
    {
      int prefabId = BTSCharacterSpawnEventValueParser.GetPrefabId(beatmapDataItem.value);
      bool alternativeMaterial = BTSCharacterSpawnEventValueParser.GetIsAlternativeMaterial(beatmapDataItem.value);
      BTSCharacterDataModel.PrefabWithId btsCharacterPrefabToSpawn = (BTSCharacterDataModel.PrefabWithId) null;
      for (int index = 0; index < this._btsCharacterDataModel.prefabsWithIds.Length; ++index)
      {
        if (this._btsCharacterDataModel.prefabsWithIds[index].id == prefabId)
        {
          btsCharacterPrefabToSpawn = this._btsCharacterDataModel.prefabsWithIds[index];
          break;
        }
      }
      if (btsCharacterPrefabToSpawn != null && !intList.Contains(prefabId))
      {
        intList.Add(prefabId);
        this.SpawnBtsCharacters(btsCharacterPrefabToSpawn, alternativeMaterial);
      }
    }
  }

  public virtual void SpawnBtsCharacters(
    BTSCharacterDataModel.PrefabWithId btsCharacterPrefabToSpawn,
    bool useAlternativeMaterial)
  {
    AsyncOperationHandle<GameObject> asyncOperationHandle = btsCharacterPrefabToSpawn.prefabAssetReference.InstantiateAsync();
    asyncOperationHandle.WaitForCompletion();
    this._handles.Add((AsyncOperationHandle) asyncOperationHandle);
    BTSCharacter component = asyncOperationHandle.Result.GetComponent<BTSCharacter>();
    BtsResultsEnvironmentController.BTSResultCharacterPlaceWithAnimation placeWithAnimation = (BtsResultsEnvironmentController.BTSResultCharacterPlaceWithAnimation) null;
    for (int index = 0; index < this._resultPlacesWithAnimations.Length; ++index)
    {
      if (this._resultPlacesWithAnimations[index].name == component.characterName)
      {
        placeWithAnimation = this._resultPlacesWithAnimations[index];
        break;
      }
    }
    placeWithAnimation?.SetCharacter(btsCharacterPrefabToSpawn.prefabAssetReference, component, useAlternativeMaterial);
  }

  public override void Activate(bool immediately = false)
  {
    this._btsCharacterAnimationWrapper.SetActive(true);
    this._btsCharactersResultsAppearPlayableDirector.Play();
    this.StartUniqueCoroutine(new Func<IEnumerator>(this.DestroyCharactersDelayed));
  }

  public override void Deactivate(bool immediately = false)
  {
    for (int index = 0; index < this._handles.Count; ++index)
    {
      if (this._handles[index].IsValid())
        Addressables.Release(this._handles[index]);
    }
    this._handles.Clear();
    this.StopUniqueCoroutine(new Func<IEnumerator>(this.DestroyCharactersDelayed));
    this.DestroyCharactersAndStopAnimations();
    this._btsCharacterAnimationWrapper.SetActive(false);
  }

  public virtual IEnumerator DestroyCharactersDelayed()
  {
    yield return (object) new WaitForSeconds((float) this._btsCharactersResultsAppearPlayableDirector.duration);
    this.DestroyCharactersAndStopAnimations();
  }

  public virtual void DestroyCharactersAndStopAnimations()
  {
    this._btsCharactersResultsAppearPlayableDirector.Stop();
    this._btsCharactersResultsAppearPlayableDirector.time = 0.0;
    foreach (BtsResultsEnvironmentController.BTSResultCharacterPlaceWithAnimation placesWithAnimation in this._resultPlacesWithAnimations)
      placesWithAnimation.Clean();
  }

  [Serializable]
  public class BTSResultCharacterPlaceWithAnimation
  {
    [SerializeField]
    protected string _name;
    [SerializeField]
    protected Transform _placeTransform;
    [SerializeField]
    protected BTSCharacterResultAnimationController _animationController;
    [SerializeField]
    protected AnimationClip _animationClip;
    protected BTSCharacter _btsCharacter;
    protected AssetReference _btsCharacterAssetReference;

    public string name => this._name;

    public virtual void Clean()
    {
      if ((UnityEngine.Object) this._btsCharacter == (UnityEngine.Object) null)
        return;
      this._animationController.StopAnimation();
      this._btsCharacterAssetReference.ReleaseInstance(this._btsCharacter.gameObject);
      UnityEngine.Object.Destroy((UnityEngine.Object) this._btsCharacter.gameObject);
      this._btsCharacterAssetReference = (AssetReference) null;
      this._btsCharacter = (BTSCharacter) null;
    }

    public virtual void SetCharacter(
      AssetReference assetReference,
      BTSCharacter btsCharacter,
      bool alternativeMaterial)
    {
      this._btsCharacterAssetReference = assetReference;
      this._btsCharacter = btsCharacter;
      this._btsCharacter.transform.SetParent(this._placeTransform, false);
      this._btsCharacter.transform.localPosition = Vector3.zero;
      this._animationController.SetCharacter(this._btsCharacter);
      this._btsCharacter.SetAlternativeAnimationAndMaterial(this._animationClip, alternativeMaterial);
    }
  }
}
