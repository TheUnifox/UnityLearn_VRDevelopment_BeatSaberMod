// Decompiled with JetBrains decompiler
// Type: PathsHolder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class PathsHolder
{
  protected readonly BezierPath _bezierPath;
  protected readonly VertexPath _vertexPath;

  public BezierPath bezierPath => this._bezierPath;

  public VertexPath vertexPath => this._vertexPath;

  public PathsHolder(int numberOfFixedVertexPathSegments, bool updateVertexPath = true)
  {
    this._bezierPath = new BezierPath(Vector3.zero, true);
    this._vertexPath = new VertexPath(numberOfFixedVertexPathSegments);
    if (!updateVertexPath)
      return;
    this._vertexPath.UpdateByBezierPath(this._bezierPath);
  }

  public virtual void UpdateVertexPathByBezierPath() => this.vertexPath.UpdateByBezierPath(this.bezierPath);
}
