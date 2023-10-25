// Decompiled with JetBrains decompiler
// Type: ReorderableAttribute
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Runtime.CompilerServices;
using UnityEngine;

public class ReorderableAttribute : PropertyAttribute
{
  [CompilerGenerated]
  protected string m_ElementHeader;
  [CompilerGenerated]
  protected bool m_HeaderZeroIndex;
  [CompilerGenerated]
  protected bool m_ElementSingleLine;

  public string ElementHeader
  {
    get => this.m_ElementHeader;
    protected set => this.m_ElementHeader = value;
  }

  public bool HeaderZeroIndex
  {
    get => this.m_HeaderZeroIndex;
    protected set => this.m_HeaderZeroIndex = value;
  }

  public bool ElementSingleLine
  {
    get => this.m_ElementSingleLine;
    protected set => this.m_ElementSingleLine = value;
  }

  public ReorderableAttribute()
  {
    this.ElementHeader = string.Empty;
    this.HeaderZeroIndex = false;
    this.ElementSingleLine = false;
  }

  public ReorderableAttribute(string headerString = "", bool isZeroIndex = true, bool isSingleLine = false)
  {
    this.ElementHeader = headerString;
    this.HeaderZeroIndex = isZeroIndex;
    this.ElementSingleLine = isSingleLine;
  }
}
