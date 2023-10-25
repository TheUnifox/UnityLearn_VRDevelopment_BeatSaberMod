// Decompiled with JetBrains decompiler
// Type: EnvironmentShaderWarmup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class EnvironmentShaderWarmup : MonoBehaviour
{
  [SerializeField]
  protected Material[] _materials;
  [Inject]
  protected readonly MainCamera _mainCamera;
  [Inject]
  protected readonly GameScenesManager _gameScenesManager;
  protected Transform _parentingTransform;
  protected const int kNumberOfColumns = 4;
  protected const int kNumberOfRows = 4;

  public virtual IEnumerator Start()
  {
    yield return (object) this._gameScenesManager.waitUntilSceneTransitionFinish;
    this._parentingTransform = new GameObject("ShaderWarmup").transform;
    this._parentingTransform.SetParent(this._mainCamera.transform);
    float num1 = this._mainCamera.camera.nearClipPlane + 0.1f;
    float num2 = (float) ((double) Mathf.Tan((float) ((double) this._mainCamera.camera.fieldOfView * 0.5 * (Math.PI / 180.0))) * (double) num1 / 4.0);
    this._parentingTransform.localPosition = Vector3.forward * num1;
    this._parentingTransform.localRotation = Quaternion.identity;
    this._parentingTransform.localScale = Vector3.one * num2;
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int y = 0; y < 4; ++y)
      {
        int index2 = index1 * 4 + y;
        if (index2 < this._materials.Length)
        {
          Material material = this._materials[index2];
          GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Quad);
          Renderer component = primitive.GetComponent<Renderer>();
          component.allowOcclusionWhenDynamic = false;
          component.material = material;
          Transform transform = primitive.transform;
          transform.SetParent(this._parentingTransform);
          transform.localPosition = new Vector3((float) (index1 - 2), (float) y, 0.0f);
          transform.localScale = Vector3.one;
        }
        else
          break;
      }
      if (index1 * 4 >= this._materials.Length)
        break;
    }
    yield return (object) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) this._parentingTransform.gameObject);
  }
}
