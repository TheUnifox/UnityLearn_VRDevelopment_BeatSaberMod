// Decompiled with JetBrains decompiler
// Type: MultiplayerLobbyAvatarController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class MultiplayerLobbyAvatarController : MonoBehaviour
{
  [SerializeField]
  protected PlayableDirector _spawnEffect;
  [SerializeField]
  protected VFXController _despawnVFXController;
  [SerializeField]
  protected float _spawnAvatarDelay = 0.05f;
  [SerializeField]
  protected float _despawnAvatarDelay = 0.05f;
  [SerializeField]
  protected float _destroyAvatarDelay = 2.5f;
  [SerializeField]
  protected GameObject[] _visualObjects;

  public virtual void ShowSpawnAnimation(Vector3 position, Quaternion rotation)
  {
    this.transform.SetPositionAndRotation(position, rotation);
    this._spawnEffect.Play();
    this.ActivateVisualObjects(false);
    this.StartCoroutine(this.SpawnAnimationCoroutine());
  }

  public virtual IEnumerator SpawnAnimationCoroutine()
  {
    yield return (object) new WaitForSeconds(this._spawnAvatarDelay);
    this.ActivateVisualObjects(true);
  }

  public virtual void ActivateVisualObjects(bool on)
  {
    foreach (GameObject visualObject in this._visualObjects)
      visualObject.SetActive(on);
  }

  public virtual IEnumerator ShowDespawnAnimationAndDestroy()
  {
    this._despawnVFXController.Play();
    yield return (object) this.DespawnAnimationCoroutine();
  }

  public virtual void DestroySelf() => Object.Destroy((Object) this.gameObject);

  public virtual IEnumerator DespawnAnimationCoroutine()
  {
    yield return (object) new WaitForSeconds(this._despawnAvatarDelay);
    this.ActivateVisualObjects(false);
    yield return (object) new WaitForSeconds(this._destroyAvatarDelay - this._despawnAvatarDelay);
    this.DestroySelf();
  }

  public class Factory : PlaceholderFactory<IConnectedPlayer, MultiplayerLobbyAvatarController>
  {
  }
}
