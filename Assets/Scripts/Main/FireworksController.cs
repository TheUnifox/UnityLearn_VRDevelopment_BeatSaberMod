// Decompiled with JetBrains decompiler
// Type: FireworksController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class FireworksController : MonoBehaviour
{
  [SerializeField]
  protected Vector3 _spawnSize;
  [SerializeField]
  protected float _minSpawnInterval = 0.2f;
  [SerializeField]
  protected float _maxSpawnInterval = 1f;
  [Header("Directional Lights")]
  [SerializeField]
  [NullAllowed]
  protected DirectionalLight[] _directionalLights;
  [SerializeField]
  protected float _lightsIntensity = 1f;
  protected int _currentLightId;
  [Inject]
  protected readonly FireworkItemController.Pool _fireworkItemPool;

  public virtual void OnEnable() => this.StartCoroutine(this.SpawningCoroutine());

  public virtual IEnumerator SpawningCoroutine()
  {
    FireworksController fireworksController = this;
    while (fireworksController.enabled)
    {
      FireworkItemController fireworkItemController = fireworksController._fireworkItemPool.Spawn();
      fireworkItemController.didFinishEvent += new System.Action<FireworkItemController>(fireworksController.HandleFireworkItemControllerDidFinish);
      fireworkItemController.transform.position = fireworksController.transform.position + new Vector3(UnityEngine.Random.Range((float) (-(double) fireworksController._spawnSize.x * 0.5), fireworksController._spawnSize.x * 0.5f), UnityEngine.Random.Range((float) (-(double) fireworksController._spawnSize.y * 0.5), fireworksController._spawnSize.y * 0.5f), UnityEngine.Random.Range((float) (-(double) fireworksController._spawnSize.z * 0.5), fireworksController._spawnSize.z * 0.5f));
      fireworkItemController.Fire();
      if (fireworksController._directionalLights != null && fireworksController._directionalLights.Length != 0)
      {
        DirectionalLight directionalLight = fireworksController._directionalLights[fireworksController._currentLightId];
        fireworkItemController.directionalLight = directionalLight;
        directionalLight.transform.position = fireworkItemController.transform.position;
        directionalLight.transform.LookAt(Vector3.up);
        fireworkItemController.directionalLightIntensity = fireworksController._lightsIntensity;
        fireworksController._currentLightId = (fireworksController._currentLightId + 1) % fireworksController._directionalLights.Length;
      }
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(fireworksController._minSpawnInterval, fireworksController._maxSpawnInterval));
    }
  }

  public virtual void HandleFireworkItemControllerDidFinish(
    FireworkItemController fireworkItemController)
  {
    fireworkItemController.didFinishEvent -= new System.Action<FireworkItemController>(this.HandleFireworkItemControllerDidFinish);
    this._fireworkItemPool.Despawn(fireworkItemController);
  }

  public virtual void OnDrawGizmosSelected()
  {
    Gizmos.color = new Color(1f, 1f, 0.0f, 0.1f);
    Gizmos.DrawCube(this.transform.position, this._spawnSize);
  }
}
