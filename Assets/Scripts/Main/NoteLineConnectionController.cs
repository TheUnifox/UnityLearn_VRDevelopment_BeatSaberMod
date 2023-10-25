// Decompiled with JetBrains decompiler
// Type: NoteLineConnectionController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class NoteLineConnectionController : MonoBehaviour
{
  [SerializeField]
  protected LineRenderer _lineRenderer;
  [Inject]
  protected readonly PlayerTransforms _playerTransforms;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  [Inject]
  protected readonly ColorManager _colorManager;
  protected NoteController _noteController0;
  protected NoteController _noteController1;
  protected Color _color0;
  protected Color _color1;
  protected float _fadeOutStartDistance;
  protected float _fadeOutEndDistance;
  protected float _noteTime;
  protected bool _didFinish;

  public event System.Action<NoteLineConnectionController> didFinishEvent;

  public virtual void Setup(
    NoteController noteController0,
    NoteController noteController1,
    float fadeOutStartDistance,
    float fadeOutEndDistance,
    float noteTime)
  {
    this._noteController0 = noteController0;
    this._noteController1 = noteController1;
    this._fadeOutStartDistance = fadeOutStartDistance;
    this._fadeOutEndDistance = fadeOutEndDistance;
    this._noteTime = noteTime;
    this._didFinish = false;
    this._color0 = this._colorManager.ColorForType(this._noteController0.noteData.colorType);
    this._color1 = this._colorManager.ColorForType(this._noteController1.noteData.colorType);
    this.UpdatePositionsAndColors();
  }

  public virtual void Update()
  {
    if (this._didFinish)
      return;
    if ((double) this._audioTimeSyncController.songTime >= (double) this._noteTime)
    {
      this._didFinish = true;
      System.Action<NoteLineConnectionController> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent != null)
        didFinishEvent(this);
    }
    this.UpdatePositionsAndColors();
  }

  public virtual void UpdatePositionsAndColors()
  {
    Vector3 position1 = this._noteController0.transform.position;
    Vector3 position2 = this._noteController1.transform.position;
    this._lineRenderer.SetPosition(0, position1);
    this._lineRenderer.SetPosition(1, position2);
    double num1 = (double) Vector3.Magnitude(this._playerTransforms.headPseudoLocalPos - (position1 + position2) * 0.5f);
    float num2 = this._fadeOutStartDistance - this._fadeOutEndDistance;
    double fadeOutEndDistance = (double) this._fadeOutEndDistance;
    float num3 = (float) (num1 - fadeOutEndDistance) / num2;
    this._color0.a = num3;
    this._color1.a = num3;
    this._lineRenderer.startColor = this._color0;
    this._lineRenderer.endColor = this._color1;
    this._lineRenderer.enabled = (double) num3 > 0.0;
  }

  public class Pool : MonoMemoryPool<NoteLineConnectionController>
  {
  }
}
