// Decompiled with JetBrains decompiler
// Type: ProModeTrailingCollidersController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ProModeTrailingCollidersController : MonoBehaviour
{
  [SerializeField]
  protected BoxCuttableBySaber _mainSmallCuttableBySaber;
  [SerializeField]
  protected BoxCuttableBySaber[] _trailingSmallCuttableBySaberList;
  [SerializeField]
  protected NoteMovement _noteMovement;
  protected Transform _transform;

  public virtual void Start()
  {
    this._transform = this.transform;
    this._noteMovement.noteDidMoveInJumpPhaseEvent += new System.Action(this.HandleNoteDidMoveInJumpPhase);
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._noteMovement != (UnityEngine.Object) null))
      return;
    this._noteMovement.noteDidMoveInJumpPhaseEvent -= new System.Action(this.HandleNoteDidMoveInJumpPhase);
  }

  public virtual void HandleNoteDidMoveInJumpPhase()
  {
    float num1 = this._noteMovement.prevLocalPosition.z - this._noteMovement.localPosition.z - this._mainSmallCuttableBySaber.colliderSize.z;
    if ((double) num1 < 0.0)
    {
      foreach (CuttableBySaber smallCuttableBySaber in this._trailingSmallCuttableBySaberList)
        smallCuttableBySaber.canBeCut = false;
    }
    else
    {
      float num2 = num1 / (float) this._trailingSmallCuttableBySaberList.Length;
      for (int index = 0; index < this._trailingSmallCuttableBySaberList.Length; ++index)
      {
        BoxCuttableBySaber smallCuttableBySaber = this._trailingSmallCuttableBySaberList[index];
        smallCuttableBySaber.canBeCut = this._mainSmallCuttableBySaber.canBeCut;
        smallCuttableBySaber.transform.position = this._transform.TransformPoint(new Vector3(0.0f, 0.0f, num2 * (float) (index + 1)));
      }
    }
  }
}
