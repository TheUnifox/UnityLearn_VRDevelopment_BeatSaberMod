// Decompiled with JetBrains decompiler
// Type: NoteDebris
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class NoteDebris : MonoBehaviour
{
  [SerializeField]
  protected Transform _meshTransform;
  [SerializeField]
  protected NoteDebrisPhysics _physics;
  [SerializeField]
  protected MaterialPropertyBlockController _materialPropertyBlockController;
  [Space]
  [SerializeField]
  protected AnimationCurve _cutoutCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
  [SerializeField]
  protected float _maxCutPointCenterDistance = 0.2f;
  [Space]
  [SerializeField]
  protected Mesh _centroidComputationMesh;
  [Inject]
  protected readonly ColorManager _colorManager;
  protected float _elapsedTime;
  protected float _lifeTime;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _cutoutPropertyID = Shader.PropertyToID("_Cutout");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _colorID = Shader.PropertyToID("_Color");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _cutPlaneID = Shader.PropertyToID("_CutPlane");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _cutoutTexOffsetID = Shader.PropertyToID("_CutoutTexOffset");
  [DoesNotRequireDomainReloadInit]
  protected static Vector3[] _meshVertices;
  protected readonly LazyCopyHashSet<INoteDebrisDidFinishEvent> _didFinishEvent = new LazyCopyHashSet<INoteDebrisDidFinishEvent>();

  public ILazyCopyHashSet<INoteDebrisDidFinishEvent> didFinishEvent => (ILazyCopyHashSet<INoteDebrisDidFinishEvent>) this._didFinishEvent;

  public virtual void Awake()
  {
    if (NoteDebris._meshVertices != null)
      return;
    NoteDebris._meshVertices = this._centroidComputationMesh.vertices;
  }

  public virtual void Update()
  {
    if ((double) this._elapsedTime < (double) this._lifeTime)
    {
      this._materialPropertyBlockController.materialPropertyBlock.SetFloat(NoteDebris._cutoutPropertyID, this._cutoutCurve.Evaluate(this._elapsedTime / this._lifeTime));
      this._materialPropertyBlockController.ApplyChanges();
      this._elapsedTime += Time.deltaTime;
    }
    else
    {
      foreach (INoteDebrisDidFinishEvent debrisDidFinishEvent in this._didFinishEvent.items)
        debrisDidFinishEvent.HandleNoteDebrisDidFinish(this);
    }
  }

  public virtual void Init(
    ColorType colorType,
    Vector3 notePos,
    Quaternion noteRot,
    Vector3 noteMoveVec,
    Vector3 noteScale,
    Vector3 positionOffset,
    Quaternion rotationOffset,
    Vector3 cutPoint,
    Vector3 cutNormal,
    Vector3 force,
    Vector3 torque,
    float lifeTime)
  {
    Quaternion quaternion = Quaternion.Inverse(noteRot);
    Vector3 rhs = quaternion * (cutPoint - notePos);
    Vector3 lhs = quaternion * cutNormal;
    float sqrMagnitude = rhs.sqrMagnitude;
    if ((double) sqrMagnitude > (double) this._maxCutPointCenterDistance * (double) this._maxCutPointCenterDistance)
      rhs = this._maxCutPointCenterDistance * rhs / Mathf.Sqrt(sqrMagnitude);
    Vector4 vector4 = lhs;
        vector4.w = -Vector3.Dot(lhs, rhs);
        float num1 = Mathf.Sqrt(Vector3.Dot((Vector3) vector4, (Vector3) vector4));
    Vector3 zero = Vector3.zero;
    int length = NoteDebris._meshVertices.Length;
    for (int index = 0; index < length; ++index)
    {
      Vector3 meshVertex = NoteDebris._meshVertices[index];
      float num2 = Vector3.Dot((Vector3) vector4, meshVertex) + vector4.w;
      if ((double) num2 < 0.0)
      {
        float num3 = num2 / num1;
        Vector3 vector3 = meshVertex - (Vector3) vector4 * num3;
        zero += vector3 / (float) length;
      }
      else
        zero += meshVertex / (float) length;
    }
    Quaternion rotation = rotationOffset * noteRot;
    Transform transform = this.transform;
    transform.SetPositionAndRotation(rotationOffset * notePos + positionOffset + rotation * zero, rotation);
    transform.localScale = noteScale;
    this._meshTransform.localPosition = -zero;
    this._physics.Init(force, torque);
    Color color = this._colorManager.ColorForType(colorType);
    MaterialPropertyBlock materialPropertyBlock = this._materialPropertyBlockController.materialPropertyBlock;
    materialPropertyBlock.Clear();
    materialPropertyBlock.SetColor(NoteDebris._colorID, color);
    materialPropertyBlock.SetVector(NoteDebris._cutPlaneID, vector4);
    materialPropertyBlock.SetVector(NoteDebris._cutoutTexOffsetID, (Vector4) Random.insideUnitSphere);
    materialPropertyBlock.SetFloat(NoteDebris._cutoutPropertyID, 0.0f);
    this._materialPropertyBlockController.ApplyChanges();
    this._lifeTime = lifeTime;
    this._elapsedTime = 0.0f;
  }

  public class Pool : MonoMemoryPool<NoteDebris>
  {
  }
}
