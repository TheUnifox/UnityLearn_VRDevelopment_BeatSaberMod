// Decompiled with JetBrains decompiler
// Type: SaberBurnMarkSparkles
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SaberBurnMarkSparkles : MonoBehaviour
{
  [SerializeField]
  protected ParticleSystem _sparklesPS;
  [SerializeField]
  protected ParticleSystem _burnMarksPSPrefab;
  [SerializeField]
  protected BoxCollider _boxCollider;
  [Inject]
  protected readonly ColorManager _colorManager;
  [Inject]
  protected readonly SaberManager _saberManager;
  protected Saber[] _sabers;
  protected Plane _plane;
  protected Vector3[] _prevBurnMarkPos;
  protected bool[] _prevBurnMarkPosValid;
  protected ParticleSystem[] _burnMarksPS;
  protected ParticleSystem.EmissionModule[] _burnMarksEmissionModules;
  protected ParticleSystem.EmitParams _sparklesEmitParams;

  public virtual void Start()
  {
    if ((Object) this._saberManager == (Object) null)
    {
      this.enabled = false;
    }
    else
    {
      this._sabers = new Saber[2];
      this._sabers[0] = this._saberManager.leftSaber;
      this._sabers[1] = this._saberManager.rightSaber;
      this._sparklesEmitParams = new ParticleSystem.EmitParams();
      this._sparklesEmitParams.applyShapeToPosition = true;
      this._prevBurnMarkPos = new Vector3[2];
      this._prevBurnMarkPosValid = new bool[2];
      this._plane = new Plane(this.transform.up, this.transform.position);
      this._burnMarksPS = new ParticleSystem[2];
      this._burnMarksEmissionModules = new ParticleSystem.EmissionModule[2];
      for (int index = 0; index < 2; ++index)
      {
        this._burnMarksPS[index] = Object.Instantiate<ParticleSystem>(this._burnMarksPSPrefab, Vector3.zero, new Quaternion()
        {
          eulerAngles = new Vector3(-90f, 0.0f, 0.0f)
        }, (Transform) null);
        this._burnMarksEmissionModules[index] = this._burnMarksPS[index].emission;
        this._burnMarksPS[index].main.startColor = (ParticleSystem.MinMaxGradient) this._colorManager.EffectsColorForSaberType(this._sabers[index].saberType);
        this._prevBurnMarkPosValid[index] = false;
      }
    }
  }

  public virtual void OnDestroy()
  {
    for (int index = 0; index < 2; ++index)
    {
      if (this._burnMarksPS != null && (Object) this._burnMarksPS[index] != (Object) null)
        Object.Destroy((Object) this._burnMarksPS[index].gameObject);
    }
  }

  public virtual void OnEnable()
  {
    for (int index = 0; index < 2; ++index)
    {
      if (this._burnMarksPS != null && (Object) this._burnMarksPS[index] != (Object) null)
        this._burnMarksPS[index].gameObject.SetActive(true);
    }
  }

  public virtual void OnDisable()
  {
    for (int index = 0; index < 2; ++index)
    {
      if (this._burnMarksPS != null && (Object) this._burnMarksPS[index] != (Object) null)
        this._burnMarksPS[index].gameObject.SetActive(false);
    }
  }

  public virtual bool GetBurnMarkPos(
    Vector3 bladeBottomPos,
    Vector3 bladeTopPos,
    out Vector3 burnMarkPos)
  {
    float num = Vector3.Distance(bladeBottomPos, bladeTopPos);
    Vector3 direction = (bladeTopPos - bladeBottomPos) / num;
    float enter;
    if (this._plane.Raycast(new Ray(bladeBottomPos, direction), out enter) && (double) enter <= (double) num)
    {
      burnMarkPos = bladeBottomPos + direction * enter;
      Bounds bounds = this._boxCollider.bounds;
      return (double) bounds.min.x < (double) burnMarkPos.x && (double) bounds.max.x > (double) burnMarkPos.x && (double) bounds.min.z < (double) burnMarkPos.z && (double) bounds.max.z > (double) burnMarkPos.z;
    }
    burnMarkPos = new Vector3(0.0f, 0.0f, 0.0f);
    return false;
  }

  public virtual void LateUpdate()
  {
    for (int index1 = 0; index1 < 2; ++index1)
    {
      Vector3 burnMarkPos = new Vector3(0.0f, 0.0f, 0.0f);
      bool flag = this._sabers[index1].isActiveAndEnabled && this.GetBurnMarkPos(this._sabers[index1].saberBladeBottomPos, this._sabers[index1].saberBladeTopPos, out burnMarkPos);
      if (flag)
        this._burnMarksPS[index1].transform.localPosition = burnMarkPos;
      if (flag && !this._prevBurnMarkPosValid[index1])
        this._burnMarksEmissionModules[index1].enabled = true;
      else if (!flag && !this._prevBurnMarkPosValid[index1])
      {
        this._burnMarksEmissionModules[index1].enabled = false;
        this._burnMarksPS[index1].Clear();
      }
      this._sparklesEmitParams.startColor = (Color32) this._colorManager.ColorForSaberType(this._sabers[index1].saberType);
      if (flag && this._prevBurnMarkPosValid[index1])
      {
        Vector3 vector3 = burnMarkPos - this._prevBurnMarkPos[index1];
        int num1 = (int) ((double) vector3.magnitude / 0.05000000074505806);
        int num2 = num1 > 0 ? num1 : 1;
        for (int index2 = 0; index2 <= num1; ++index2)
        {
          this._sparklesEmitParams.position = this._prevBurnMarkPos[index1] + vector3 * (float) index2 / (float) num2;
          this._sparklesPS.Emit(this._sparklesEmitParams, 1);
        }
      }
      this._prevBurnMarkPosValid[index1] = flag;
      this._prevBurnMarkPos[index1] = burnMarkPos;
    }
  }
}
