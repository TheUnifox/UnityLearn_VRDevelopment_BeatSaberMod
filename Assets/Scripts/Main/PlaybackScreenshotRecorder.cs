// Decompiled with JetBrains decompiler
// Type: PlaybackScreenshotRecorder
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.IO;
using UnityEngine;

public class PlaybackScreenshotRecorder : MonoBehaviour
{
  [SerializeField]
  protected string _directory = "Screenshots";
  protected PlaybackRenderer _playbackRenderer;
  protected int _frameNumber;

  public string directory => this._directory;

  public virtual void OnEnable() => this._playbackRenderer.texturesReadyEvent += new System.Action(this.HandleTexturesReady);

  public virtual void OnDisable() => this._playbackRenderer.texturesReadyEvent -= new System.Action(this.HandleTexturesReady);

  public virtual void Init(string directory, int framerate, PlaybackRenderer playbackRenderer)
  {
    this._playbackRenderer = playbackRenderer;
    this._directory = directory;
    foreach (PlaybackRenderer.PlaybackScreenshot screenshot in playbackRenderer.screenshots)
    {
      screenshot.path = Path.Combine(this._directory, screenshot.name);
      Directory.CreateDirectory(screenshot.path);
    }
    Time.captureFramerate = framerate;
    this._frameNumber = 0;
  }

  public virtual void HandleTexturesReady()
  {
    foreach (PlaybackRenderer.PlaybackScreenshot screenshot in this._playbackRenderer.screenshots)
      this.SaveScreenshot(screenshot.texture, screenshot.path);
    ++this._frameNumber;
  }

  public virtual void SaveScreenshot(RenderTexture renderTexture, string directory)
  {
    Texture2D tex = PlaybackScreenshotRecorder.ConvertRenderTexture(renderTexture);
    File.WriteAllBytes(Path.Combine(directory, string.Format("{0:D05}.png", (object) this._frameNumber)), tex.EncodeToPNG());
    UnityEngine.Object.Destroy((UnityEngine.Object) tex);
  }

  private static Texture2D ConvertRenderTexture(RenderTexture renderTexture)
  {
    Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
    RenderTexture.active = renderTexture;
    texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) renderTexture.width, (float) renderTexture.height), 0, 0);
    return texture2D;
  }
}
