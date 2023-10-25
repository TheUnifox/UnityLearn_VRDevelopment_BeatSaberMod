// Decompiled with JetBrains decompiler
// Type: BloomPrePassLight
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteAlways]
public abstract class BloomPrePassLight : MonoBehaviour
{
  private const int kFloatSize = 4;
  private const int kVertexOffset = 0;
  private const int kVertexSize = 12;
  private const int kViewPosOffset = 12;
  private const int kViewPosSize = 12;
  private const int kColorOffset = 24;
  private const int kColorSize = 16;
  private const int kUvOffset = 40;
  private const int kUvSize = 12;
  private const int kVertexDataSize = 52;
  [SerializeField]
  private BloomPrePassLightTypeSO _lightType;
  private static readonly Dictionary<BloomPrePassLightTypeSO, HashSet<BloomPrePassLight>> _bloomLightsDict = new Dictionary<BloomPrePassLightTypeSO, HashSet<BloomPrePassLight>>();
  private static readonly List<BloomPrePassLight.LightsDataItem> _lightsDataItems = new List<BloomPrePassLight.LightsDataItem>();
  private BloomPrePassLightTypeSO _registeredWithLightType;
  private bool _isRegistered;
  private bool _isBeingDestroyed;

  public static Dictionary<BloomPrePassLightTypeSO, HashSet<BloomPrePassLight>> bloomLightsDict => BloomPrePassLight._bloomLightsDict;

  public static List<BloomPrePassLight.LightsDataItem> lightsDataItems => BloomPrePassLight._lightsDataItems;

  public abstract bool isDirty { get; }

  protected virtual void OnEnable() => this.RegisterLight();

  protected virtual void OnDisable() => this.UnregisterLight();

  protected void OnDestroy()
  {
    this._isBeingDestroyed = true;
    this.UnregisterLight();
  }

  private void RegisterLight()
  {
    if (this._isRegistered || this._isBeingDestroyed)
      return;
    this.DidRegisterLight();
    this._isRegistered = true;
    this._registeredWithLightType = this._lightType;
    HashSet<BloomPrePassLight> lights = (HashSet<BloomPrePassLight>) null;
    if (!BloomPrePassLight._bloomLightsDict.TryGetValue(this._registeredWithLightType, out lights))
    {
      lights = new HashSet<BloomPrePassLight>();
      BloomPrePassLight._bloomLightsDict[this._registeredWithLightType] = lights;
      int index = 0;
      while (index < BloomPrePassLight._lightsDataItems.Count && BloomPrePassLight._lightsDataItems[index].lightType.renderingPriority <= this._registeredWithLightType.renderingPriority)
        ++index;
      BloomPrePassLight._lightsDataItems.Insert(index, new BloomPrePassLight.LightsDataItem(this._registeredWithLightType, lights));
    }
    lights.Add(this);
  }

  private void UnregisterLight()
  {
    if (!this._isRegistered)
      return;
    this._isRegistered = false;
    HashSet<BloomPrePassLight> bloomPrePassLightSet;
    if (!BloomPrePassLight._bloomLightsDict.TryGetValue(this._registeredWithLightType, out bloomPrePassLightSet))
      return;
    bloomPrePassLightSet.Remove(this);
  }

  protected abstract void DidRegisterLight();

  public abstract void FillMeshData(
    ref int lightNum,
    BloomPrePassLight.QuadData[] lightQuads,
    Matrix4x4 viewMatrix,
    Matrix4x4 projectionMatrix,
    float lineWidth);

  public abstract void Refresh();

  [StructLayout(LayoutKind.Explicit)]
  public struct VertexData
  {
    [FieldOffset(0)]
    public Vector3 vertex;
    [FieldOffset(12)]
    public Vector3 viewPos;
    [FieldOffset(24)]
    public Color color;
    [FieldOffset(40)]
    public Vector3 uv;
  }

  [StructLayout(LayoutKind.Explicit)]
  public struct QuadData
  {
    [FieldOffset(0)]
    public BloomPrePassLight.VertexData vertex0;
    [FieldOffset(52)]
    public BloomPrePassLight.VertexData vertex1;
    [FieldOffset(104)]
    public BloomPrePassLight.VertexData vertex2;
    [FieldOffset(156)]
    public BloomPrePassLight.VertexData vertex3;
  }

  public class LightsDataItem
  {
    public readonly BloomPrePassLightTypeSO lightType;
    public readonly HashSet<BloomPrePassLight> lights;

    public LightsDataItem(BloomPrePassLightTypeSO lightType, HashSet<BloomPrePassLight> lights)
    {
      this.lightType = lightType;
      this.lights = lights;
    }
  }
}
