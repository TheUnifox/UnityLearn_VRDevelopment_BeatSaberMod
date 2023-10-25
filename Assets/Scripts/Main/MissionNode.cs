// Decompiled with JetBrains decompiler
// Type: MissionNode
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[SelectionBase]
public class MissionNode : MonoBehaviour
{
  [SerializeField]
  protected MissionDataSO _missionDataSO;
  [SerializeField]
  protected string _letterPartName;
  [SerializeField]
  protected int _numberPartName;
  [SerializeField]
  protected RectTransform _rectTransform;
  [SerializeField]
  protected MissionNodeVisualController _missionNodeVisualController;
  [Space]
  [SerializeField]
  protected MissionNode[] _childNodes;

  public MissionDataSO missionData => this._missionDataSO;

  public MissionNode[] childNodes => this._childNodes;

  public MissionNodeVisualController missionNodeVisualController => this._missionNodeVisualController;

  public string letterPartName => this._letterPartName;

  public int numberPartName => this._numberPartName;

  public string missionId => this._numberPartName.ToString() + this._letterPartName;

  public string formattedMissionNodeName => string.Format("{0}<size=66%>{1}</size>", (object) this.numberPartName, (object) this.letterPartName);

  public Vector2 position => (Vector2) this._rectTransform.localPosition;

  public float radius => this._rectTransform.rect.width;
}
