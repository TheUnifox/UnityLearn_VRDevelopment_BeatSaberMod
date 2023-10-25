// Decompiled with JetBrains decompiler
// Type: MultiplayerResultsPyramidViewAvatar
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Linq;
using TMPro;
using Tweening;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Zenject;

public class MultiplayerResultsPyramidViewAvatar : MonoBehaviour
{
  [Header("New")]
  [SerializeField]
  protected PlayableDirector _badgeDirector;
  [SerializeField]
  protected string _ghostFirstTrackName;
  [SerializeField]
  protected string _ghostSecondTrackName;
  [Header("Old")]
  [SerializeField]
  protected Transform _standWithAvatarTransform;
  [SerializeField]
  protected float _perPositionRotation = 10f;
  [SerializeField]
  protected Color _localPlayerColor = Color.magenta;
  [Header("Name")]
  [SerializeField]
  protected TextMeshProUGUI _positionText;
  [SerializeField]
  protected TextMeshProUGUI _nameText;
  [SerializeField]
  protected ImageView _nameBackground;
  [Header("Badge Tweens")]
  [SerializeField]
  protected CanvasGroup _badgeCanvas;
  [SerializeField]
  protected TextMeshProUGUI[] _badgeTitles;
  [SerializeField]
  protected ImageView[] _badgeImages;
  [Header("Subtitle")]
  [SerializeField]
  protected TextMeshProUGUI _badgeSubtitleText;
  [SerializeField]
  protected CanvasGroup _badgeSubtitleCanvas;
  [Header("Ghost Effect")]
  [SerializeField]
  protected GhostDuplicationEffect _ghostDuplicationEffect;
  [SerializeField]
  protected GhostDuplicationEffect.GhostEffectParams _ghostAppear;
  [SerializeField]
  protected GhostDuplicationEffect.GhostEffectParams _ghostReceive;
  [Header("Trophy")]
  [SerializeField]
  protected ImageView _trophyImage;
  [SerializeField]
  protected Sprite _firstPlaceTrophy;
  [SerializeField]
  protected Sprite _secondPlaceTrophy;
  [SerializeField]
  protected Sprite _thirdPlaceTrophy;
  [SerializeField]
  protected Color _firstPlaceColor = Color.yellow;
  [SerializeField]
  protected GameObject _personalBestVisual;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  [Inject]
  protected readonly IDifficultyBeatmap _difficultyBeatmap;
  protected Vector3Tween _riseTween;
  protected Vector3Tween _avatarRiseTween;
  protected Vector3Tween _badgePositionTween;
  protected Tweening.FloatTween _badgeOpacityTween;
  protected Tweening.FloatTween _nameOpacityTween;
  protected Vector3Tween _namePositionTween;
  protected Tweening.ColorTween _localGlowTween;
  protected Vector3Tween _titleMakingSpaceForBadgeTween;
  protected Vector3 _originalBadgeLocalPos;
  [Inject]
  protected readonly IConnectedPlayer _connectedPlayer;

  public PlayableDirector badgeDirector => this._badgeDirector;

  public virtual void Awake() => this._personalBestVisual.SetActive(false);

  public virtual void Setup(MultiplayerPlayerResultsData resultData, int position, int playerCount)
  {
    this._standWithAvatarTransform.localEulerAngles = new Vector3(0.0f, this._perPositionRotation * Mathf.Sign(this.transform.position.x) * Mathf.Ceil((float) (position - playerCount % 2) / 2f) * (playerCount % 2 != 0 || (double) Mathf.Ceil((float) (position - playerCount % 2) / 2f) != 1.0 ? 1f : 0.5f), 0.0f);
    this._positionText.text = position.ToString();
    switch (position)
    {
      case 1:
        this._trophyImage.sprite = this._firstPlaceTrophy;
        this._positionText.color = this._firstPlaceColor;
        this._nameText.alpha = 1f;
        break;
      case 2:
        this._trophyImage.sprite = this._secondPlaceTrophy;
        this._positionText.color = Color.white;
        this._nameText.alpha = 0.8f;
        break;
      case 3:
        this._trophyImage.sprite = this._thirdPlaceTrophy;
        this._positionText.color = Color.white;
        this._nameText.alpha = 0.8f;
        break;
      default:
        this._trophyImage.gameObject.SetActive(false);
        this._nameText.alpha = 0.6f;
        this._positionText.alpha = 0.6f;
        break;
    }
    if (resultData.connectedPlayer.isMe)
    {
      this._localPlayerColor.a = 0.75f;
      this._nameBackground.color0 = this._localPlayerColor;
      this._localPlayerColor.a = 0.125f;
      this._nameBackground.color1 = this._localPlayerColor;
      if (this._difficultyBeatmap != null && resultData.multiplayerLevelCompletionResults.hasAnyResults && this._playerDataModel.playerData.GetPlayerLevelStatsData(this._difficultyBeatmap).highScore < resultData.multiplayerLevelCompletionResults.levelCompletionResults.modifiedScore)
        this._personalBestVisual.SetActive(true);
    }
    if (resultData.badge == null)
    {
      this._badgeCanvas.gameObject.SetActive(false);
      this._badgeSubtitleCanvas.gameObject.SetActive(false);
    }
    else
    {
      MultiplayerBadgeAwardData badge = resultData.badge;
      this._badgeSubtitleText.text = badge.subtitle;
      this._badgeSubtitleCanvas.alpha = 0.0f;
      foreach (Image badgeImage in this._badgeImages)
        badgeImage.sprite = badge.icon;
      foreach (TMP_Text badgeTitle in this._badgeTitles)
        badgeTitle.text = badge.title;
    }
  }

  public virtual void SetupBadgeTimeline(Transform startTransform, Transform midTransform)
  {
    if (!(this._badgeDirector.playableAsset is TimelineAsset playableAsset))
      return;
    foreach (TrackAsset outputTrack in playableAsset.GetOutputTracks())
    {
      if (outputTrack is PlayableTrack playableTrack)
      {
        if (this._ghostFirstTrackName == playableTrack.name)
        {
          GhostEffectAsset asset = playableTrack.GetClips().First<TimelineClip>().asset as GhostEffectAsset;
          asset.template._startTransform = startTransform;
          asset.template._endTransform = midTransform;
        }
        else if (this._ghostSecondTrackName == playableTrack.name)
        {
          GhostEffectAsset asset = playableTrack.GetClips().First<TimelineClip>().asset as GhostEffectAsset;
          asset.template._startTransform = midTransform;
          asset.template._endLocalPosition = this.transform.InverseTransformPoint(this._ghostDuplicationEffect.transform.position);
        }
      }
    }
  }

  public class Factory : PlaceholderFactory<IConnectedPlayer, MultiplayerResultsPyramidViewAvatar>
  {
  }
}
