// Decompiled with JetBrains decompiler
// Type: TutorialNoteCutEffectSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using UnityEngine;
using Zenject;

public class TutorialNoteCutEffectSpawner : MonoBehaviour
{
  [SerializeField]
  protected FlyingTextSpawner _failFlyingTextSpawner;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;

  public virtual void Start() => this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
  }

  public virtual void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
  {
    if (noteController.noteData.colorType == ColorType.None)
    {
      this._failFlyingTextSpawner.SpawnText(noteCutInfo.cutPoint, noteController.worldRotation, noteController.inverseWorldRotation, Localization.Get("DO_NOT_CUT_FLYING_TEXT"));
    }
    else
    {
      string text;
      switch (noteCutInfo.failReason)
      {
        case NoteCutInfo.FailReason.TooSoon:
          text = Localization.Get("TOO_SOON_FLYING_TEXT");
          break;
        case NoteCutInfo.FailReason.WrongColor:
          text = Localization.Get("WRONG_COLOR_FLYING_TEXT");
          break;
        case NoteCutInfo.FailReason.CutHarder:
          text = Localization.Get("CUT_HARDER_FLYING_TEXT");
          break;
        case NoteCutInfo.FailReason.WrongDirection:
          text = Localization.Get("WRONG_DIRECTION_FLYING_TEXT");
          break;
        default:
          text = "";
          break;
      }
      if (!(text != ""))
        return;
      this._failFlyingTextSpawner.SpawnText(noteCutInfo.cutPoint, noteController.worldRotation, noteController.inverseWorldRotation, text);
    }
  }
}
