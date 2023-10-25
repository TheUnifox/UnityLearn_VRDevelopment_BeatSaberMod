// Decompiled with JetBrains decompiler
// Type: JumpMarker
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[DisplayName("Jump/JumpMarker")]
public class JumpMarker : Marker, INotification
{
  [SerializeField]
  protected JumpDestinationMarker _destination;
  [CompilerGenerated]
  protected readonly PropertyName m_id;

  public PropertyName id => this.m_id;

  public JumpDestinationMarker jumpDestination => this._destination;
}
