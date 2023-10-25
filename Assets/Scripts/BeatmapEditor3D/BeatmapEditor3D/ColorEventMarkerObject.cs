// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ColorEventMarkerObject
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using TMPro;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class ColorEventMarkerObject : MonoBehaviour, IEventMarkerObject
  {
    [SerializeField]
    private MaterialPropertyBlockController _materialPropertyBlockController;
    [SerializeField]
    private GameObject _selectionHighlightGameObject;
    [Space]
    [SerializeField]
    private TextMeshPro _sideText;
    [SerializeField]
    private TextMeshPro _topText;
    [Header("Top")]
    [SerializeField]
    private GameObject _topInstantSprite;
    [SerializeField]
    private GameObject _topInterpolateSprite;
    [Header("Side")]
    [SerializeField]
    private GameObject _sideInstantSprite;
    [SerializeField]
    private GameObject _sideInterpolateSprite;
    [DoesNotRequireDomainReloadInit]
    private static readonly int _colorId = Shader.PropertyToID("_Color");
    [DoesNotRequireDomainReloadInit]
    private static readonly int _highlightId = Shader.PropertyToID("_Highlight");
    [DoesNotRequireDomainReloadInit]
    private static readonly int _dimId = Shader.PropertyToID("_Dim");
    private bool _onBeat;
    private bool _pastBeat;
    private GameObject _topCurrentSprite;
    private GameObject _sideCurrentSprite;
    private ColorEventMarkerObject.EnvironmentColor _environmentColor;
    private float _brightness;
    private int _strobe;

    public void Init(
      ColorEventMarkerObject.EnvironmentColor environmentColor,
      ColorEventMarkerObject.InterpolationType interpolationType,
      float brightness,
      int strobe)
    {
      this._environmentColor = environmentColor;
      this._brightness = brightness;
      this._strobe = strobe;
      this.SetColor();
      this.SetText();
      this.SetSprite(interpolationType);
    }

    public void SetState(bool onBeat, bool pastBeat, bool selected)
    {
      if (selected != this._selectionHighlightGameObject.activeSelf)
        this._selectionHighlightGameObject.SetActive(selected);
      if (this._onBeat == onBeat && this._pastBeat == pastBeat)
        return;
      this._onBeat = onBeat;
      this._pastBeat = pastBeat;
      this._materialPropertyBlockController.materialPropertyBlock.SetFloat(ColorEventMarkerObject._highlightId, onBeat ? 1f : 0.0f);
      this._materialPropertyBlockController.materialPropertyBlock.SetFloat(ColorEventMarkerObject._dimId, pastBeat ? 1f : 0.0f);
      this._materialPropertyBlockController.ApplyChanges();
    }

    public void SetHighlight(bool highlighted)
    {
      this._materialPropertyBlockController.materialPropertyBlock.SetColor(ColorEventMarkerObject._colorId, this.GetColorValue(this._environmentColor, this._brightness) * (highlighted ? 0.8f : 1f));
      this._materialPropertyBlockController.ApplyChanges();
    }

    private void SetColor()
    {
      this._materialPropertyBlockController.materialPropertyBlock.SetColor(ColorEventMarkerObject._colorId, this.GetColorValue(this._environmentColor, this._brightness));
      this._materialPropertyBlockController.ApplyChanges();
    }

    private void SetText()
    {
      string str = string.Format("{0:F0}", (object) ((double) this._brightness * 100.0));
      if (this._strobe != 0)
        str += string.Format("/{0}", (object) this._strobe);
      this._sideText.text = str;
      this._topText.text = str;
      this._sideText.gameObject.SetActive(this._environmentColor != 0);
      this._topText.gameObject.SetActive(this._environmentColor != 0);
    }

    private void SetSprite(
      ColorEventMarkerObject.InterpolationType interpolationType)
    {
      if ((Object) this._sideCurrentSprite != (Object) null && (Object) this._topCurrentSprite != (Object) null)
      {
        this._sideCurrentSprite.SetActive(false);
        this._topCurrentSprite.SetActive(false);
      }
      this._sideCurrentSprite = (GameObject) null;
      this._topCurrentSprite = (GameObject) null;
      switch (interpolationType)
      {
        case ColorEventMarkerObject.InterpolationType.Instant:
          this._sideCurrentSprite = this._sideInstantSprite;
          this._topCurrentSprite = this._topInstantSprite;
          break;
        case ColorEventMarkerObject.InterpolationType.Interpolate:
          this._sideCurrentSprite = this._sideInterpolateSprite;
          this._topCurrentSprite = this._topInterpolateSprite;
          break;
      }
      if (!((Object) this._sideCurrentSprite != (Object) null) || !((Object) this._topCurrentSprite != (Object) null))
        return;
      this._sideCurrentSprite.SetActive(true);
      this._topCurrentSprite.SetActive(true);
    }

    private Color GetColorValue(ColorEventMarkerObject.EnvironmentColor color, float brightness)
    {
      if (color == ColorEventMarkerObject.EnvironmentColor.Default)
        return Color.gray;
      Color b = Color.white;
      switch (color - 1)
      {
        case ColorEventMarkerObject.EnvironmentColor.Default:
          b = Color.red;
          break;
        case ColorEventMarkerObject.EnvironmentColor.Color0:
          b = Color.blue;
          break;
        case ColorEventMarkerObject.EnvironmentColor.Color1:
          b = Color.white;
          break;
      }
      return Color.Lerp(Color.gray, b, Mathf.Lerp(0.3f, 1f, Mathf.Lerp(0.0f, 1f, brightness)));
    }

    public class Pool : MonoMemoryPool<ColorEventMarkerObject>
    {
    }

    public enum EnvironmentColor
    {
      Default,
      Color0,
      Color1,
      ColorW,
    }

    public enum InterpolationType
    {
      Instant,
      Interpolate,
    }
  }
}
