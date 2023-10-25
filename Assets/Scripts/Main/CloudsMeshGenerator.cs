// Decompiled with JetBrains decompiler
// Type: CloudsMeshGenerator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class CloudsMeshGenerator : MonoBehaviour
{
  [SerializeField]
  protected MeshFilter _meshFilter;
  [SerializeField]
  protected string _meshName;
  [Header("Pause")]
  [SerializeField]
  protected bool _pauseGenerator;
  [Header("Bottom Push")]
  [SerializeField]
  protected bool _bottomPushEnabled;
  [SerializeField]
  [DrawIf("_bottomPushEnabled", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _bottomPushDistance = 0.2f;
  [SerializeField]
  [DrawIf("_bottomPushEnabled", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected Vector2 _bottomScaleTopBottom = new Vector2(-0.1f, -0.5f);
  [SerializeField]
  [DrawIf("_bottomPushEnabled", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected float _bottomHorizontalScale = 1.3f;
  [Header("Radius Prohibition")]
  [SerializeField]
  protected bool _drawRingGizmos;
  [SerializeField]
  protected CloudsMeshGenerator.ProhibitedRadius[] _prohibitedRadii;
  [Header("Size Settings")]
  [SerializeField]
  protected Vector2 _meshSize = Vector2.one;
  [SerializeField]
  [Range(0.0f, 2f)]
  protected float _sizeRandomness = 0.5f;
  [SerializeField]
  [Range(0.0f, 1f)]
  protected float _ratioRandomness = 0.5f;
  [Header("Settings")]
  [SerializeField]
  [Tooltip("To prevent clipping")]
  [Range(0.01f, 0.4f)]
  protected float _perMeshRadiusOffset = 0.03f;
  [SerializeField]
  protected Gradient _possibleColors;
  [SerializeField]
  [Range(0.0f, 256f)]
  protected int _randomSeed;
  [SerializeField]
  protected float _heightRandomness = 0.5f;
  [SerializeField]
  protected float _ringRotationRandomness = 2f;
  [SerializeField]
  [Min(1f)]
  protected int _ringCount = 10;
  [SerializeField]
  protected float _meshesPerRadius = 1f;
  [Header("Close Far Settings")]
  [SerializeField]
  protected Vector2 _radiusCloseFar = new Vector2(12f, 80f);
  [SerializeField]
  protected Vector2 _sizeCloseFar = new Vector2(0.7f, 1.5f);
  [SerializeField]
  protected AnimationCurve _heightCloseFar;
  [SerializeField]
  protected float _lowPolyThreshold = 30f;
  [Header("Debug")]
  [SerializeField]
  protected bool _flipNormals;
  [SerializeField]
  protected bool _curveMesh;
  [SerializeField]
  protected CloudsMeshGenerator.Cloud[] _clouds;
  [Header("Info")]
  [SerializeField]
  protected int _meshCount;
  [SerializeField]
  protected int _vertexCount;
  protected Mesh _generatedMesh;
  protected Bounds _meshBounds;
  protected CloudsMeshGenerator.RadiusChunk[] _radiusChunks;
  protected CloudsMeshGenerator.Ring[] _rings;
  protected CloudsMeshGenerator.ProhibitedRadius[] _sortedProhibitedRadii;

  [Serializable]
  public struct Cloud
  {
    public Mesh precisionOpaqueMesh;
    public Mesh lowPolyMesh;
    public float sizeModifier;
    public float bottomThreshold;
    public int weight;
    [Header("Information only")]
    public int precisionVertexCount;
    public int lowPolyVertexCount;
    public int generatedCount;
  }

  public struct Ring
  {
    public float radius;
    public float normalizedRadius;
    public int meshCount;
    public int[] cloudIDs;
    public float sizeMultiplier;
  }

  [Serializable]
  public struct ProhibitedRadius
  {
    [NullAllowed]
    public Transform transform;
    public float distance;
    [Min(0.1f)]
    public float radius;
  }

  public struct RadiusChunk
  {
    public float normalizedStart;
    public float normalizedEnd;
    public float absoluteStart;
    public float absoluteEnd;
  }
}
