// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerObstacleClippingController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class MultiplayerConnectedPlayerObstacleClippingController : MonoBehaviour
{
  [SerializeField]
  protected MaterialPropertyBlockController[] _materialPropertyBlockControllers;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _clippingPlanePositionID = Shader.PropertyToID("_ClippingPlanePosition");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _clippingPlaneNormalID = Shader.PropertyToID("_ClippingPlaneNormal");

  public virtual void SetClippingParams(Vector3 position, Vector3 normal)
  {
    foreach (MaterialPropertyBlockController propertyBlockController in this._materialPropertyBlockControllers)
    {
      propertyBlockController.materialPropertyBlock.SetVector(MultiplayerConnectedPlayerObstacleClippingController._clippingPlanePositionID, (Vector4) position);
      propertyBlockController.materialPropertyBlock.SetVector(MultiplayerConnectedPlayerObstacleClippingController._clippingPlaneNormalID, (Vector4) normal);
      propertyBlockController.ApplyChanges();
    }
  }
}
