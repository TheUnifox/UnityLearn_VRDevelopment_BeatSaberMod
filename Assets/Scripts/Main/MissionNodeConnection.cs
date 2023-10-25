// Decompiled with JetBrains decompiler
// Type: MissionNodeConnection
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.UI;

public class MissionNodeConnection : MonoBehaviour
{
  [SerializeField]
  protected float _separator = 2f;
  [SerializeField]
  protected float _width = 0.8f;
  [SerializeField]
  protected RectTransform _rectTransform;
  [SerializeField]
  protected Image _image;
  [SerializeField]
  [NullAllowed(NullAllowed.Context.Prefab)]
  protected MissionNodeVisualController _parentMissionNode;
  [SerializeField]
  [NullAllowed(NullAllowed.Context.Prefab)]
  protected MissionNodeVisualController _childMissionNode;
  [SerializeField]
  protected Animator _animator;
  protected Vector2 _parentMissionNodePosition;
  protected Vector2 _childMissionNodePosition;
  protected bool _isActive;

  public MissionNodeVisualController parentMissionNode => this._parentMissionNode;

  public MissionNodeVisualController childMissionNode => this._childMissionNode;

  public bool isActive => this._isActive;

  public virtual void Setup(
    MissionNodeVisualController parentMissionNode,
    MissionNodeVisualController childMissionNode)
  {
    this._parentMissionNode = parentMissionNode;
    this._childMissionNode = childMissionNode;
    this.UpdateConnectionRectTransform();
  }

  public virtual void UpdateConnectionRectTransform()
  {
    if (this._parentMissionNodePosition == this._parentMissionNode.missionNode.position && this._childMissionNodePosition == this._childMissionNode.missionNode.position)
      return;
    this._parentMissionNodePosition = this._parentMissionNode.missionNode.position;
    this._childMissionNodePosition = this._childMissionNode.missionNode.position;
    Vector2 vector2 = this._childMissionNodePosition - this._parentMissionNodePosition;
    float z = 57.29578f * Mathf.Atan2(vector2.y, vector2.x);
    this._rectTransform.localPosition = Vector3.zero;
    this._rectTransform.anchoredPosition = (this._parentMissionNodePosition + this._childMissionNodePosition) / 2f;
    this._rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, z);
    this._rectTransform.localScale = Vector3.one;
    this._rectTransform.sizeDelta = new Vector2((float) ((double) vector2.magnitude - (double) this._childMissionNode.missionNode.radius - (double) this._separator * 2.0), this._width);
  }

  public virtual void SetActive(bool animated)
  {
    this._isActive = true;
    if (animated && this.gameObject.activeInHierarchy)
    {
      this._animator.enabled = true;
      this._animator.SetBool("MissionConnectionEnabled", true);
    }
    else
      this._image.color = Color.white.ColorWithAlpha(0.35f);
  }

  public virtual void MissionConnectionEnabledDidFinish() => this._animator.enabled = false;
}
