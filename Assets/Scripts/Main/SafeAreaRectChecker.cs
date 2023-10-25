// Decompiled with JetBrains decompiler
// Type: SafeAreaRectChecker
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;
using Zenject;

public class SafeAreaRectChecker : MonoBehaviour
{
  [SerializeField]
  protected float _minAngleX = -27f;
  [SerializeField]
  protected float _maxAngleX = 27f;
  [SerializeField]
  protected float _minAngleY = -22f;
  [SerializeField]
  protected float _maxAngleY = 17f;
  [SerializeField]
  protected GameObject _activeObjectWhenInsideSafeArea;
  [SerializeField]
  protected GameObject _activeObjectWhenNotInsideSafeArea;
  [SerializeField]
  protected RectTransform _rectTransformToCheck;
  protected readonly Vector3[] _corners = new Vector3[4];
  [Inject]
  protected readonly MainCamera _mainCamera;
  [Inject]
  protected readonly SafeAreaRectChecker.InitData _initData;

  public virtual void Start()
  {
    this.enabled = this._initData.checkingEnabled;
    if (!this.enabled)
      return;
    this._activeObjectWhenInsideSafeArea.SetActive(false);
    this._activeObjectWhenNotInsideSafeArea.SetActive(true);
  }

  public virtual void Update()
  {
    this._rectTransformToCheck.GetWorldCorners(this._corners);
    Matrix4x4 worldToLocalMatrix = this._mainCamera.transform.worldToLocalMatrix;
    bool flag = true;
    foreach (Vector3 corner in this._corners)
    {
      Vector3 vector3 = worldToLocalMatrix.MultiplyPoint3x4(corner);
      if ((double) vector3.z < 0.0 || (double) Mathf.Atan(vector3.x / vector3.z) < (double) this._minAngleX * (Math.PI / 180.0) || (double) Mathf.Atan(vector3.x / vector3.z) > (double) this._maxAngleX * (Math.PI / 180.0) || (double) Mathf.Atan(vector3.y / vector3.z) < (double) this._minAngleY * (Math.PI / 180.0) || (double) Mathf.Atan(vector3.y / vector3.z) > (double) this._maxAngleY * (Math.PI / 180.0))
      {
        flag = false;
        break;
      }
    }
    if (!flag)
      return;
    this._activeObjectWhenInsideSafeArea.SetActive(true);
    this._activeObjectWhenNotInsideSafeArea.SetActive(false);
    this.enabled = false;
  }

  public class InitData
  {
    public readonly bool checkingEnabled;

    public InitData(bool checkingEnabled) => this.checkingEnabled = checkingEnabled;
  }
}
