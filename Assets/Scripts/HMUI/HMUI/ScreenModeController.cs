// Decompiled with JetBrains decompiler
// Type: HMUI.ScreenModeController
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.XR;
using Zenject;

namespace HMUI
{
  public class ScreenModeController : MonoBehaviour
  {
    [SerializeField]
    protected CurvedCanvasSettings[] _curvedCanvases;
    [Inject]
    protected readonly IVRPlatformHelper _vrPlatformHelper;
    protected ScreenModeData _defaultModeData;
    protected Transform _transform;

    public virtual void Awake()
    {
      this._transform = this.transform;
      this._defaultModeData = new ScreenModeData(this._transform.localPosition, this._transform.localRotation.eulerAngles, this._transform.localScale.x, this._curvedCanvases[0].radius, false, 0.0f, 0.0f);
    }

    public virtual void SetMode(ScreenModeData screenModeData)
    {
      this._transform.localRotation = Quaternion.Euler(screenModeData.rotation);
      if (screenModeData.offsetHeightByHeadPos)
      {
        Vector3 position = screenModeData.position;
        Vector3 pos;
        this._vrPlatformHelper.GetNodePose(XRNode.Head, 0, out pos, out Quaternion _);
        position.y = Mathf.Max(pos.y + screenModeData.yOffsetRelativeToHead, screenModeData.minYPos);
        this._transform.localPosition = position;
      }
      else
        this._transform.localPosition = screenModeData.position;
      this._transform.localScale = Vector3.one * screenModeData.scale;
      foreach (CurvedCanvasSettings curvedCanvase in this._curvedCanvases)
        curvedCanvase.SetRadius(screenModeData.radius);
    }

    public virtual void SetDefaultMode() => this.SetMode(this._defaultModeData);
  }
}
