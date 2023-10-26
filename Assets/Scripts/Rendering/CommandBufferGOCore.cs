// Decompiled with JetBrains decompiler
// Type: CommandBufferGOCore
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class CommandBufferGOCore : MonoBehaviour
{
  private Dictionary<Camera, CommandBufferOwners> _cameras;
  [DoesNotRequireDomainReloadInit]
  private static Material _material;
  private UnityEngine.Mesh _mesh;

  protected void OnEnable()
  {
    if ((Object) this.GetComponent<MeshRenderer>() == (Object) null)
    {
      MeshRenderer meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
      if (!(bool) (Object) CommandBufferGOCore._material)
      {
        CommandBufferGOCore._material = new Material(Shader.Find("Diffuse"));
        CommandBufferGOCore._material.renderQueue = 0;
      }
      Material material = CommandBufferGOCore._material;
      meshRenderer.material = material;
    }
    MeshFilter meshFilter = this.GetComponent<MeshFilter>();
    if ((Object) meshFilter == (Object) null)
      meshFilter = this.gameObject.AddComponent<MeshFilter>();
    if ((Object) meshFilter.sharedMesh == (Object) null)
    {
      this._mesh = new UnityEngine.Mesh();
      this._mesh.name = nameof (CommandBufferGOCore);
      this._mesh.hideFlags = HideFlags.HideAndDontSave;
      this._mesh.bounds = new Bounds(Vector3.zero, new Vector3(9999999f, 9999999f, 9999999f));
      meshFilter.sharedMesh = this._mesh;
    }
    else
      meshFilter.sharedMesh.bounds = new Bounds(Vector3.zero, new Vector3(9999999f, 9999999f, 9999999f));
    this._cameras = this.CamerasDict();
  }

  protected void OnDisable()
  {
    List<Camera> cameraList = new List<Camera>();
    foreach (KeyValuePair<Camera, CommandBufferOwners> camera in this._cameras)
    {
      if ((bool) (Object) camera.Key)
      {
        CommandBufferOwners commandBufferOwners = camera.Value;
        commandBufferOwners.RemoveOwner((Object) this);
        if (commandBufferOwners.NumberOfOwners == 0)
        {
          camera.Key.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, commandBufferOwners.commandBuffer);
          cameraList.Add(camera.Key);
        }
      }
    }
    foreach (Camera key in cameraList)
      this._cameras.Remove(key);
    EssentialHelpers.SafeDestroy((Object) CommandBufferGOCore._material);
    EssentialHelpers.SafeDestroy((Object) this._mesh);
  }

  protected virtual void OnWillRenderObject()
  {
    if ((!this.gameObject.activeInHierarchy ? 0 : (this.enabled ? 1 : 0)) == 0)
      return;
    Camera current = Camera.current;
    if (!(bool) (Object) current)
      return;
    CommandBufferOwners commandBufferOwners;
    if (this._cameras.TryGetValue(current, out commandBufferOwners))
    {
      if (commandBufferOwners.ContainsOwner((Object) this))
        return;
      commandBufferOwners.AddOwner((Object) this);
    }
    else
    {
      commandBufferOwners = new CommandBufferOwners();
      CommandBuffer commandBuffer = this.CreateCommandBuffer(current);
      current.AddCommandBuffer(this.CommandBufferCameraEvent(), commandBuffer);
      commandBufferOwners.commandBuffer = commandBuffer;
      commandBufferOwners.AddOwner((Object) this);
      this._cameras[current] = commandBufferOwners;
    }
  }

  protected abstract CameraEvent CommandBufferCameraEvent();

  protected abstract CommandBuffer CreateCommandBuffer(Camera camera);

  protected abstract Dictionary<Camera, CommandBufferOwners> CamerasDict();
}
