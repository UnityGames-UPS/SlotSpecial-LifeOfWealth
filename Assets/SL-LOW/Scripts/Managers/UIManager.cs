using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private JSFunctCalls jsFunctCalls;

    [Header("Loading & Intro")]
    [SerializeField] private GameObject gameScreen;



    [Header("Bet Controls")]
    [SerializeField] private TMP_Text betAmountText;
    [SerializeField] private Button betPlusButton;
    [SerializeField] private Button betMinusButton;
    [Header("Bet Controls - Portrait")]
    [SerializeField] private TMP_Text betAmountTextPortrait;
    [SerializeField] private Button betPlusButtonPortrait;
    [SerializeField] private Button betMinusButtonPortrait;

    [Header("Balance & Win")]
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text winAmountText;
    [SerializeField] private GameObject winTextObject;
    [SerializeField] private GameObject goodLuckObject;
    [Header("Balance & Win - Portrait")]
    [SerializeField] private TMP_Text balanceTextPortrait;
    [SerializeField] private TMP_Text winAmountTextPortrait;
    [SerializeField] private GameObject winTextObjectPortrait;
    [SerializeField] private GameObject goodLuckObjectPortrait;

    [Header("Bonus Wheel")]
    [SerializeField] private GameObject wheelScreen;
    [SerializeField] private Button wheelSpinButton;
    [SerializeField] private Button wheelSpinButton2;
    [SerializeField] private Transform wheelTitleTransform;
    [SerializeField] private List<Transform> wheelTitleObjects = new List<Transform>();
    [SerializeField] private CanvasGroup transitionBackFilm;
    [SerializeField] private StarFountain wheelCoinFountain;
    [SerializeField] private Transform wheelAnticlockwiseRotatingObject;
    [SerializeField] private float wheelRotationDuration = 8f;
    private Tween wheelAnticlockwiseRotationTween;
    private List<Tween> wheelTitleTweens = new List<Tween>();
    private Dictionary<Transform, Vector3> wheelTitleInitialScales = new Dictionary<Transform, Vector3>();
    [Header("Bonus Wheel - Portrait")]
    [SerializeField] private Button wheelSpinButtonPortrait;
    [SerializeField] private Button wheelSpinButtonPortrait2;
    [SerializeField] private Transform wheelTitleTransformPortrait;
    
    [Header("Universal Win Popup")]
    [SerializeField] private GameObject bigWinObject;
    [SerializeField] private GameObject universalWinPopup;
    [SerializeField] private RectTransform universalWinPopupRect;
    [SerializeField] private GameObject uwpCongratulationsTitle;
    [SerializeField] private GameObject uwpYouWonSubtitle;
    [SerializeField] private GameObject uwpBigWinTitle;
    [SerializeField] private TMP_Text uwpWinAmountText;
    [SerializeField] private TMP_Text uwpFreeSpinCountText;
    [SerializeField] private GameObject uwpFreeSpinObject;
    [SerializeField] private Button uwpTakeButton;
    [Header("Universal Win Popup - Portrait")]
    [SerializeField] private Button uwpTakeButtonPortrait;
    [Header("Universal Win Popup - Star Particle Burst")]
    [SerializeField] private StarFountain starFountain;

    [Header("Spin Button")]
    [SerializeField] private Button spinButton;
    [SerializeField] private Button stopButton;
    [Header("Spin Button - Portrait")]
    [SerializeField] private Button spinButtonPortrait;
    [SerializeField] private Button stopButtonPortrait;

    [Header("Auto Play Stop Control")]
    [SerializeField] private Button autoSpinStopButton;
    [SerializeField] private TMP_Text autoSpinRemainingText;
    [Header("Auto Play Stop Control - Portrait")]
    [SerializeField] private Button autoSpinStopButtonPortrait;
    [SerializeField] private TMP_Text autoSpinRemainingTextPortrait;

    [Header("Auto Play Panel")]
    [SerializeField] private GameObject autoPlayPanel;
    [SerializeField] private RectTransform autoPlayPanelRect;
    [SerializeField] private Button autoPlayCloseButton;
    [Header("Auto Play Selection Buttons")]
    [SerializeField] private Button autoPlay10Button;
    [SerializeField] private Button autoPlay50Button;
    [SerializeField] private Button autoPlay100Button;
    [SerializeField] private Button autoPlay200Button;
    [SerializeField] private Button autoPlay500Button;
    [SerializeField] private Button autoPlayInfiniteButton;

    [Header("Auto Play Panel - Portrait")]
    [SerializeField] private GameObject autoPlayPanelPortrait;
    [SerializeField] private RectTransform autoPlayPanelRectPortrait;
    [SerializeField] private Button autoPlayCloseButtonPortrait;
    [SerializeField] private Button autoPlay10ButtonPortrait;
    [SerializeField] private Button autoPlay50ButtonPortrait;
    [SerializeField] private Button autoPlay100ButtonPortrait;
    [SerializeField] private Button autoPlay200ButtonPortrait;
    [SerializeField] private Button autoPlay500ButtonPortrait;
    [SerializeField] private Button autoPlayInfiniteButtonPortrait;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private RectTransform settingsPanelRect;
    [SerializeField] private Button settingsOpenButton;
    [SerializeField] private Button settingsCloseButton;
    [SerializeField] private Button settingsBgCloseButton;
    [SerializeField] private Button gameQuitButton;
    [Header("Settings Panel - Portrait")]
    [SerializeField] private GameObject settingsPanelPortrait;
    [SerializeField] private RectTransform settingsPanelRectPortrait;
    [SerializeField] private Button settingsOpenButtonPortrait;
    [SerializeField] private Button settingsCloseButtonPortrait;
    [SerializeField] private Button settingsBgCloseButtonPortrait;
    [SerializeField] private Button gameQuitButtonPortrait;

    [Header("Speed Buttons (Three-Layer Toggle)")]
    [SerializeField] private Button normalSpeedButton;
    [SerializeField] private Button turboSpeedButton;
    [SerializeField] private Button quickSpeedButton;
    [Header("Speed Buttons - Portrait")]
    [SerializeField] private Button normalSpeedButtonPortrait;
    [SerializeField] private Button turboSpeedButtonPortrait;
    [SerializeField] private Button quickSpeedButtonPortrait;

    [Header("Sound Panel")]
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private RectTransform soundPanelRect;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button soundPanelCloseButton;
    [SerializeField] private Button soundPanelOpenButton;
    [SerializeField] private Button soundPanelOpenButtonPortrait;

    [Header("Game Rules Panel")]
    [SerializeField] private GameObject gameRulesPanel;
    [SerializeField] private RectTransform gameRulesPanelRect;
    [SerializeField] private Button gameRulesOpenButton;
    [SerializeField] private Button gameRulesBackButton;
    [Header("Game Rules Panel - Portrait")]
    [SerializeField] private Button gameRulesOpenButtonPortrait;

    [Header("Guide Panel")]
    [SerializeField] private GameObject guidePanel;
    [SerializeField] private RectTransform guidePanelRect;
    [SerializeField] private Button guideOpenButton;
    [SerializeField] private Button guideBackButton;
    [Header("Guide Panel - Portrait")]
    [SerializeField] private Button guideOpenButtonPortrait;

    [Header("Game Rules Dynamic Texts")]
    [SerializeField] private TMP_Text totalLineCountText;
    [SerializeField] private TMP_Text ruleSymbol0Text;
    [SerializeField] private TMP_Text ruleSymbol1Text;
    [SerializeField] private TMP_Text ruleSymbol2Text;
    [SerializeField] private TMP_Text ruleSymbol3Text;
    [SerializeField] private TMP_Text ruleSymbol4Text;
    [SerializeField] private TMP_Text ruleSymbol5Text;
    [SerializeField] private TMP_Text ruleSymbol6Text;
    [SerializeField] private TMP_Text ruleSymbol7Text;
    [SerializeField] private TMP_Text ruleSymbol8Text;
    [SerializeField] private TMP_Text ruleSymbol9Text;

    // Multiplier blocks appear in this order on the Info page. Entries that
    // show two icons use the first icon because both symbols share a paytable.
    private static readonly int[] InfoPageMultiplierSymbolIds = { 3, 4, 6, 8, 9, 11, 12, 1 };

    [Header("Free Games Feature Panel")]
    [SerializeField] private GameObject freeGamesFeaturePanel;
    [SerializeField] private GameObject freeGamesStartPanel;
    [SerializeField] private GameObject freeGamesMidPanel;
    [SerializeField] private GameObject freeGamesEndPanel;
    [SerializeField] private GameObject landscapeGreyFilm;
    [SerializeField] private Button freeGamesStartButtonLandscape;
    [SerializeField] private Button freeGamesStartButtonPortrait;
    [SerializeField] private Button freeGamesTakeButtonLandscape;
    [SerializeField] private Button freeGamesTakeButtonPortrait;
    [SerializeField] private TMP_Text freeGamesRemainingSpriteText;
    [SerializeField] private TMP_Text freeGamesGameWinSpriteText;
    [SerializeField] private TMP_Text freeGamesFeatureWinSpriteText;
    [SerializeField] private TMP_Text freeGamesTotalWinSpriteText;
    [SerializeField] private float freeGamesAmountCountDuration = 1.5f;
    [SerializeField] private float freeGamesStartDisplayDuration = 3f;

    [Header("Ping Display")]
    [SerializeField] private TMP_Text pingText;
    [SerializeField] private TMP_Text pingTextPortrait;

    [Header("Platform Jackpot")]
    [SerializeField] private TMP_Text grandJackpotText;
    [SerializeField] private TMP_Text majorJackpotText;
    [SerializeField] private TMP_Text minorJackpotText;
    [SerializeField] private TMP_Text miniJackpotText;

    [Header("Platform Jackpot - Portrait")]
    [SerializeField] private TMP_Text grandJackpotTextPortrait;
    [SerializeField] private TMP_Text majorJackpotTextPortrait;
    [SerializeField] private TMP_Text minorJackpotTextPortrait;
    [SerializeField] private TMP_Text miniJackpotTextPortrait;

    [Header("Platform Jackpot - Portrait Floating Icons")]
    [SerializeField] private RectTransform grandJackpotIconPortrait;
    [SerializeField] private RectTransform majorJackpotIconPortrait;
    [SerializeField] private RectTransform minorJackpotIconPortrait;
    [SerializeField] private RectTransform miniJackpotIconPortrait;

    private const float PortraitJackpotFloatDistance = 12f;
    private const float PortraitJackpotFloatDuration = 2f;
    private readonly List<Tween> portraitJackpotIconTweens = new List<Tween>();
    private readonly Dictionary<RectTransform, Vector2> portraitJackpotIconStartPositions =
        new Dictionary<RectTransform, Vector2>();

    [Header("Expand-Shrink Controls")]
    [SerializeField] private Button expandButton;
    [SerializeField] private Button shrinkButton;
    [Header("Expand-Shrink Controls - Portrait")]
    [SerializeField] private Button expandButtonPortrait;
    [SerializeField] private Button shrinkButtonPortrait;

    private bool isExpanded = false;
    private bool isSettingsPanelOpen = false;

    private Tween balanceTween;
    private Tween winTween;
    private double totalFreeSpinWin = 0;
    private int totalFreeSpinsAwarded = 0;

    private int initialFreeSpins = 0;

    private Tween freeGamesAmountTween;
    private Coroutine freeGamesStartAnnouncementCoroutine;
    private int pendingFreeSpins;
    private double freeGamesTriggerWin;
    private double freeGamesFeatureWin;
    private double freeGamesHeldBalance;
    private bool isFreeGamesFeatureActive;
    private bool isFreeGamesEndScreenActive;
    private bool isFreeGamesAmountAnimationComplete;
    private bool isFreeGamesRetriggerStart;
    private bool isFreeGamesAwaitingStartButton;

    // Optimistic balance: the locally-deducted balance shown while the spin is in flight
    private double optimisticBalance = 0;
    private bool hasOptimisticBalance = false;

    [Header("Rapid Stop Cooldown")]
    [Tooltip("Seconds the player must wait before pressing Stop again after an immediate stop.")]
    [SerializeField] private float rapidStopCooldown = 1f;
    private float lastRapidStopTime = -99f;

    private int currentRulesPage = 0;
    private bool isPageAnimating;
    [Header("UI State")]
    private double currentWinDisplayValue = 0;
    private int currentWinDecimalPlaces = 0;
    private bool isSpecialWinActive = false;
    public bool IsSpecialWinActive => isSpecialWinActive;
    public System.Action OnSpecialWinComplete;
    private ImageAnimation activeBigWinAnimation;
    private System.Action<int> bigWinLoopCompleteHandler;
    private System.Action bigWinCompletionCallback;

    // Universal Win Popup state
    private System.Action universalWinPopupCallback;
    private Coroutine uwpAutoCloseCoroutine;
    private Tween uwpWinTween;
    [SerializeField] private float uwpAutoCloseDelay = 5f;

    private void Awake()
    {
        if (jsFunctCalls != null)
        {
            jsFunctCalls.RegisterVisibilityListener(gameObject.name);
        }
    }



    public void OnFocusChanged(string value)
    {
        bool focused = value == "1";
        Debug.Log("UNITY FOCUS CHANGED: " + value + " (focused: " + focused + ")");
        AudioManager.Instance?.SetMuteAll(!focused);
        if (gameManager != null && gameManager.socketManager != null)
        {
            gameManager.socketManager.HandleFocusChange(focused);
        }
    }

    private void Start()
    {
        InitializeFreeGamesFeaturePanel();
        SetupButtons();
        SetupAutoPlayPanel();
        SetupSettingsPanel();
        SetupGameRulesPanel();
        SetupGuidePanel();

        InitializeExpandShrink();

        if (gameScreen) gameScreen.SetActive(true);
        InitializeUI();
        StartPortraitJackpotIconAnimations();
        StartCoroutine(WaitForInitialization());
        RegisterFullscreenListener();
    }

    private void OnEnable()
    {
        OrientationChange.OnOrientationChanged += HandleOrientationChangedForWheelButtons;
    }

    private void OnDisable()
    {
        OrientationChange.OnOrientationChanged -= HandleOrientationChangedForWheelButtons;
    }

    private void HandleOrientationChangedForWheelButtons(OrientationChange.OrientationMode mode, int width, int height)
    {
        UpdateNewWheelSpinButtonsVisibility();
        UpdateFreeGamesActionButtonsVisibility();
    }

    private void UpdateNewWheelSpinButtonsVisibility()
    {
        if (wheelScreen == null || !wheelScreen.activeInHierarchy || wheelSpinTriggered) return;

        var oc = Object.FindFirstObjectByType<OrientationChange>();
        bool isPortraitMode = (oc != null && oc.CurrentMode == OrientationChange.OrientationMode.MobilePortrait);

        if (wheelSpinButton2) wheelSpinButton2.gameObject.SetActive(!isPortraitMode);
        if (wheelSpinButtonPortrait2) wheelSpinButtonPortrait2.gameObject.SetActive(isPortraitMode);
    }

    private bool IsPortraitMode()
    {
        var orientationController = Object.FindFirstObjectByType<OrientationChange>();
        return orientationController != null &&
            orientationController.CurrentMode == OrientationChange.OrientationMode.MobilePortrait;
    }

    private void UpdateFreeGamesActionButtonsVisibility()
    {
        if (!isFreeGamesFeatureActive) return;

        bool isPortraitMode = IsPortraitMode();
        bool showStart =
            !isFreeGamesEndScreenActive &&
            isFreeGamesAwaitingStartButton &&
            pendingFreeSpins > 0;
        bool showTake = isFreeGamesEndScreenActive;

        if (freeGamesStartButtonLandscape) freeGamesStartButtonLandscape.gameObject.SetActive(showStart && !isPortraitMode);
        if (freeGamesStartButtonPortrait) freeGamesStartButtonPortrait.gameObject.SetActive(showStart && isPortraitMode);
        SetButtonInteractable(freeGamesStartButtonLandscape, freeGamesStartButtonPortrait, showStart);

        if (freeGamesTakeButtonLandscape) freeGamesTakeButtonLandscape.gameObject.SetActive(showTake && !isPortraitMode);
        if (freeGamesTakeButtonPortrait) freeGamesTakeButtonPortrait.gameObject.SetActive(showTake && isPortraitMode);
        SetButtonInteractable(
            freeGamesTakeButtonLandscape,
            freeGamesTakeButtonPortrait,
            showTake && isFreeGamesAmountAnimationComplete);
    }

    private void InitializeUI()
    {
        if (soundPanel) soundPanel.SetActive(false);
        SetGameObjectActive(autoPlayPanel, autoPlayPanelPortrait, false);
        if (autoPlayPanelRect) autoPlayPanelRect.anchoredPosition = new Vector2(autoPlayPanelRect.anchoredPosition.x, -600f);
        if (autoPlayPanelRectPortrait) autoPlayPanelRectPortrait.anchoredPosition = new Vector2(autoPlayPanelRectPortrait.anchoredPosition.x, -600f);
        SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, false);

        SetSpinStopButtonStates(isSpinningState: false, isInteractable: true);
        UpdateSpeedButtonsVisibility(gameManager.currentSpinSpeed);

        isSettingsPanelOpen = false;
        SetGameObjectActive(settingsPanel, settingsPanelPortrait, false);
        if (gameRulesPanel) gameRulesPanel.SetActive(false);
        if (guidePanel) guidePanel.SetActive(false);
        if (uwpWinTween != null) { uwpWinTween.Kill(); uwpWinTween = null; }
        if (starFountain != null) starFountain.StopStarBurst();
        if (bigWinObject) bigWinObject.SetActive(false);
        if (universalWinPopup) universalWinPopup.SetActive(false);

        SetFreeGamesPanelState(false, false, false, false);
        StopWheelBonusEffects();
        if (wheelScreen) wheelScreen.SetActive(false);
        SetButtonActive(wheelSpinButton, wheelSpinButtonPortrait, false);
        SetButtonActive(wheelSpinButton2, wheelSpinButtonPortrait2, false);
        if (transitionBackFilm) transitionBackFilm.gameObject.SetActive(false);
        UpdatePingDisplay("-- ms");
    }

    private void StartPortraitJackpotIconAnimations()
    {
        StopPortraitJackpotIconAnimations(true);

        RectTransform[] jackpotIcons =
        {
            grandJackpotIconPortrait,
            majorJackpotIconPortrait,
            minorJackpotIconPortrait,
            miniJackpotIconPortrait
        };

        for (int i = 0; i < jackpotIcons.Length; i++)
        {
            RectTransform icon = jackpotIcons[i];
            if (icon == null || portraitJackpotIconStartPositions.ContainsKey(icon)) continue;

            Vector2 startPosition = icon.anchoredPosition;
            portraitJackpotIconStartPositions.Add(icon, startPosition);

            Tween floatTween = icon
                .DOAnchorPosY(startPosition.y + PortraitJackpotFloatDistance, PortraitJackpotFloatDuration)
                .SetEase(Ease.InOutSine)
                .SetDelay(i * 0.3f)
                .SetLoops(-1, LoopType.Yoyo);

            portraitJackpotIconTweens.Add(floatTween);
        }
    }

    private void StopPortraitJackpotIconAnimations(bool restorePositions)
    {
        foreach (Tween tween in portraitJackpotIconTweens)
        {
            if (tween != null && tween.IsActive()) tween.Kill();
        }
        portraitJackpotIconTweens.Clear();

        if (!restorePositions) return;

        foreach (KeyValuePair<RectTransform, Vector2> entry in portraitJackpotIconStartPositions)
        {
            if (entry.Key != null) entry.Key.anchoredPosition = entry.Value;
        }
        portraitJackpotIconStartPositions.Clear();
    }

    #region Loading & Intro Sequence

    private IEnumerator WaitForInitialization()
    {
        float initializationTimeout = 20f;
        float timer = 0f;
        while (!gameManager.isInitialized && !gameManager.initializationFailed && timer < initializationTimeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (gameManager.initializationFailed || !gameManager.isInitialized)
        {
            if (gameManager.socketManager != null)
            {
                gameManager.socketManager.SetRaycastBlocker(false);
            }

            if (popupManager != null)
            {
                string errorMsg = gameManager.initializationFailed ? "Game failed to initialize." : "Initialization timed out. Please check your connection.";
                popupManager.ShowErrorPopup("Connection Error", errorMsg, true);
            }
        }
        else
        {
            AudioManager.Instance?.PlayBgMusic();
        }
    }

    #endregion

    #region UI Synchronization Helpers

    private void SetTMPText(TMP_Text text1, TMP_Text text2, string content)
    {
        if (text1) text1.text = content;
        if (text2) text2.text = content;
    }

    private void SetGameObjectActive(GameObject obj1, GameObject obj2, bool active)
    {
        if (obj1) obj1.SetActive(active);
        if (obj2) obj2.SetActive(active);
    }

    private void SetButtonInteractable(Button btn1, Button btn2, bool interactable)
    {
        if (btn1) btn1.interactable = interactable;
        if (btn2) btn2.interactable = interactable;
    }

    private void SetButtonActive(Button btn1, Button btn2, bool active)
    {
        if (btn1) btn1.gameObject.SetActive(active);
        if (btn2) btn2.gameObject.SetActive(active);
    }

    #endregion

    #region Button Setup

    private void SetupButtons()
    {
        // Bet buttons
        if (betPlusButton)  betPlusButton.onClick.AddListener(() => gameManager.IncreaseBet());
        if (betMinusButton) betMinusButton.onClick.AddListener(() => gameManager.DecreaseBet());
        if (betPlusButtonPortrait)  betPlusButtonPortrait.onClick.AddListener(() => gameManager.IncreaseBet());
        if (betMinusButtonPortrait) betMinusButtonPortrait.onClick.AddListener(() => gameManager.DecreaseBet());

        // Spin button
        if (spinButton)
        {
            var holdHandler = spinButton.GetComponent<SpinButtonHoldHandler>();
            if (holdHandler != null)
            {
                holdHandler.OnClick.AddListener(OnSpinButtonPressed);
                holdHandler.OnHoldThreeSeconds.AddListener(OnSpinButtonHeld);
            }
            else
            {
                spinButton.onClick.AddListener(OnSpinButtonPressed);
            }
        }
        if (spinButtonPortrait)
        {
            var holdHandler = spinButtonPortrait.GetComponent<SpinButtonHoldHandler>();
            if (holdHandler != null)
            {
                holdHandler.OnClick.AddListener(OnSpinButtonPressed);
                holdHandler.OnHoldThreeSeconds.AddListener(OnSpinButtonHeld);
            }
            else
            {
                spinButtonPortrait.onClick.AddListener(OnSpinButtonPressed);
            }
        }

        // Stop button
        if (stopButton) stopButton.onClick.AddListener(OnStopButtonPressed);
        if (stopButtonPortrait) stopButtonPortrait.onClick.AddListener(OnStopButtonPressed);

        // Auto spin stop button
        if (autoSpinStopButton)
        {
            autoSpinStopButton.onClick.AddListener(() =>
            {
                AudioManager.Instance?.PlayAutoplayStop();
                gameManager.StopAutoPlay();
            });
        }
        if (autoSpinStopButtonPortrait)
        {
            autoSpinStopButtonPortrait.onClick.AddListener(() =>
            {
                AudioManager.Instance?.PlayAutoplayStop();
                gameManager.StopAutoPlay();
            });
        }

        if (autoPlayCloseButton) autoPlayCloseButton.onClick.AddListener(CloseAutoPlayPanel);
        if (autoPlayCloseButtonPortrait) autoPlayCloseButtonPortrait.onClick.AddListener(CloseAutoPlayPanel);

        if (gameQuitButton) gameQuitButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); OnExitButtonPressed(); });
        if (gameQuitButtonPortrait) gameQuitButtonPortrait.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); OnExitButtonPressed(); });

        if (expandButton) expandButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); OnExpand(); });
        if (shrinkButton) shrinkButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); OnShrink(); });
        if (expandButtonPortrait) expandButtonPortrait.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); OnExpand(); });
        if (shrinkButtonPortrait) shrinkButtonPortrait.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); OnShrink(); });

        // Take button for universal win popup
        if (uwpTakeButton) uwpTakeButton.onClick.AddListener(OnUniversalWinTakeButtonClicked);
        if (uwpTakeButtonPortrait) uwpTakeButtonPortrait.onClick.AddListener(OnUniversalWinTakeButtonClicked);

        // These existing wheel Start buttons are also the orientation-specific
        // Start controls for the Free Games feature.
        if (freeGamesStartButtonLandscape) freeGamesStartButtonLandscape.onClick.AddListener(OnWheelOrFreeGamesStartButtonClicked);
        if (freeGamesStartButtonPortrait) freeGamesStartButtonPortrait.onClick.AddListener(OnWheelOrFreeGamesStartButtonClicked);
        if (wheelSpinButton2) wheelSpinButton2.onClick.AddListener(OnWheelSpinClicked);
        if (wheelSpinButtonPortrait2) wheelSpinButtonPortrait2.onClick.AddListener(OnWheelSpinClicked);

        // Speed buttons setup (Three-layer Toggle)
        if (normalSpeedButton) normalSpeedButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); SetSpeedMode(SpinSpeed.Turbo); });
        if (turboSpeedButton)  turboSpeedButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); SetSpeedMode(SpinSpeed.QuickSpin); });
        if (quickSpeedButton)  quickSpeedButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); SetSpeedMode(SpinSpeed.Normal); });
        if (normalSpeedButtonPortrait) normalSpeedButtonPortrait.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); SetSpeedMode(SpinSpeed.Turbo); });
        if (turboSpeedButtonPortrait)  turboSpeedButtonPortrait.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); SetSpeedMode(SpinSpeed.QuickSpin); });
        if (quickSpeedButtonPortrait)  quickSpeedButtonPortrait.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); SetSpeedMode(SpinSpeed.Normal); });
    }

    private void SetupAutoPlayPanel()
    {
        if (autoPlay10Button)       autoPlay10Button.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(10); });
        if (autoPlay50Button)       autoPlay50Button.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(50); });
        if (autoPlay100Button)      autoPlay100Button.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(100); });
        if (autoPlay200Button)      autoPlay200Button.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(200); });
        if (autoPlay500Button)      autoPlay500Button.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(500); });
        if (autoPlayInfiniteButton) autoPlayInfiniteButton.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(-1); });

        if (autoPlay10ButtonPortrait)       autoPlay10ButtonPortrait.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(10); });
        if (autoPlay50ButtonPortrait)       autoPlay50ButtonPortrait.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(50); });
        if (autoPlay100ButtonPortrait)      autoPlay100ButtonPortrait.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(100); });
        if (autoPlay200ButtonPortrait)      autoPlay200ButtonPortrait.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(200); });
        if (autoPlay500ButtonPortrait)      autoPlay500ButtonPortrait.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(500); });
        if (autoPlayInfiniteButtonPortrait) autoPlayInfiniteButtonPortrait.onClick.AddListener(() => { AudioManager.Instance?.PlayButton(); StartAutoplayWithRounds(-1); });
    }

    private void SetupSettingsPanel()
    {
        if (settingsOpenButton) settingsOpenButton.onClick.AddListener(() => { 
            AudioManager.Instance?.PlayButton(); 
            if (isSettingsPanelOpen)
                CloseSettingsPanel();
            else
                OpenSettingsPanel();
        });
        if (settingsOpenButtonPortrait) settingsOpenButtonPortrait.onClick.AddListener(() => { 
            AudioManager.Instance?.PlayButton(); 
            if (isSettingsPanelOpen)
                CloseSettingsPanel();
            else
                OpenSettingsPanel();
        });

        if (settingsCloseButton) settingsCloseButton.onClick.AddListener(() => { 
            AudioManager.Instance?.PlayButton(); 
            CloseSettingsPanel(); 
        });
        if (settingsCloseButtonPortrait) settingsCloseButtonPortrait.onClick.AddListener(() => { 
            AudioManager.Instance?.PlayButton(); 
            CloseSettingsPanel(); 
        });
        if (settingsBgCloseButton) settingsBgCloseButton.onClick.AddListener(() => { 
            AudioManager.Instance?.PlayButton(); 
            CloseSettingsPanel(); 
        });
        if (settingsBgCloseButtonPortrait) settingsBgCloseButtonPortrait.onClick.AddListener(() => { 
            AudioManager.Instance?.PlayButton(); 
            CloseSettingsPanel(); 
        });

        SetButtonActive(settingsOpenButton, settingsOpenButtonPortrait, true);
        SetButtonActive(settingsCloseButton, settingsCloseButtonPortrait, false);
        SetButtonActive(settingsBgCloseButton, settingsBgCloseButtonPortrait, false);

        // Sound panel buttons & sliders
        if (soundPanelOpenButton) soundPanelOpenButton.onClick.AddListener(OpenSoundPanel);
        if (soundPanelOpenButtonPortrait) soundPanelOpenButtonPortrait.onClick.AddListener(OpenSoundPanel);
        if (soundPanelCloseButton) soundPanelCloseButton.onClick.AddListener(CloseSoundPanel);

        if (musicSlider)
        {
            if (AudioManager.Instance != null) musicSlider.value = AudioManager.Instance.MusicVolume;
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        }
        if (sfxSlider)
        {
            if (AudioManager.Instance != null) sfxSlider.value = AudioManager.Instance.SfxVolume;
            sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
        }
    }

    private void SetupGameRulesPanel()
    {
        if (gameRulesOpenButton) gameRulesOpenButton.onClick.AddListener(OpenGameRulesPanel);
        if (gameRulesOpenButtonPortrait) gameRulesOpenButtonPortrait.onClick.AddListener(OpenGameRulesPanel);

        if (gameRulesBackButton) gameRulesBackButton.onClick.AddListener(() => { AudioManager.Instance?.PlayPopupClose(); CloseGameRulesPanel(); });
    }

    private void SetupGuidePanel()
    {
        if (guideOpenButton) guideOpenButton.onClick.AddListener(OpenGuidePanel);
        if (guideOpenButtonPortrait) guideOpenButtonPortrait.onClick.AddListener(OpenGuidePanel);

        if (guideBackButton) guideBackButton.onClick.AddListener(() => { AudioManager.Instance?.PlayPopupClose(); CloseGuidePanel(); });
    }

    #endregion

    #region Game Events

    internal void OnGameInitialized()
    {
        currentWinDisplayValue = 0;
        UpdateBetDisplay();
        UpdateBalanceDisplay();
        UpdateWinDisplay(0);
    }

    internal void OnSpinStarted()
    {
        AudioManager.Instance?.PlaySpinStart();

        if (gameManager.isInFreeSpins)
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: false);
        }
        else
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: true);
            SetBetControlsEnabled(false);
            SetButtonInteractable(settingsOpenButton, settingsOpenButtonPortrait, true);
            UpdateWinDisplay(0);
        }

        UpdateBalanceDisplay();

        CloseAutoPlayPanelImmediate();
    }

    internal void OnSpinResultReceived()
    {
        if (gameManager.isAutoPlaying)
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: true);
        }
        else if (gameManager.isInFreeSpins)
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: false);
        }
        else
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: true);
        }
    }

    internal void OnSpinStopping(SpinResult result = null)
    {
        UpdateBalanceDisplay();
        if (result != null)
        {
            if (gameManager != null && gameManager.isInFreeSpins)
            {
                // Free Games WIN is cumulative. A losing spin keeps the existing
                // amount, while a win counts up from the previous total.
                if (result.winAmount > 0)
                {
                    AnimateWinUpdate(GetRunningFreeGamesWin());
                }
            }
            else
            {
                AnimateWinUpdate(result.winAmount);
            }
        }
    }

    internal void OnSpinCompleted(SpinResult result = null)
    {
        // OnSpinStopping owns the bottom WIN count-up for every spin. Do not
        // snap that tween to its final value when this event follows it.
        UpdateBalanceDisplay();

        if (gameManager.isAutoPlaying)
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: true);
        }
        else if (gameManager.isInFreeSpins)
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: false);
        }
        else
        {
            SetSpinStopButtonStates(isSpinningState: false, isInteractable: true);

            SetBetControlsEnabled(true);
            SetButtonInteractable(settingsOpenButton, settingsOpenButtonPortrait, true);
        }
    }

    internal void TriggerBigWinPopup(SpinResult result, System.Action onComplete = null)
    {
        ClearMidSpinBigWinState(false);

        if (bigWinObject == null)
        {
            onComplete?.Invoke();
            return;
        }

        activeBigWinAnimation = bigWinObject.GetComponent<ImageAnimation>();
        if (activeBigWinAnimation == null || !activeBigWinAnimation.HasAnimationFrames())
        {
            onComplete?.Invoke();
            return;
        }

        isSpecialWinActive = true;
        bigWinCompletionCallback = onComplete;
        bigWinLoopCompleteHandler = completedLoops =>
        {
            if (completedLoops >= 2)
            {
                ClearMidSpinBigWinState(true);
            }
        };
        activeBigWinAnimation.onLoopComplete += bigWinLoopCompleteHandler;

        // Toggling restarts the sprite animation from frame zero. The game flow
        // remains paused until ImageAnimation reports that two loops completed.
        bigWinObject.SetActive(false);
        bigWinObject.SetActive(true);
    }

    private void ClearMidSpinBigWinState(bool invokeCompletion)
    {
        if (activeBigWinAnimation != null && bigWinLoopCompleteHandler != null)
        {
            activeBigWinAnimation.onLoopComplete -= bigWinLoopCompleteHandler;
        }

        activeBigWinAnimation = null;
        bigWinLoopCompleteHandler = null;

        System.Action callback = bigWinCompletionCallback;
        bigWinCompletionCallback = null;

        if (bigWinObject != null) bigWinObject.SetActive(false);
        isSpecialWinActive = false;

        if (invokeCompletion)
        {
            EnableControlsAfterWinAnimation();
            callback?.Invoke();
        }
    }

    internal void DisableControlsDuringWinAnimation()
    {
        SetBetControlsEnabled(false);
        SetSpinStopButtonStates(isSpinningState: false, isInteractable: false);
    }

    internal void EnableControlsAfterWinAnimation()
    {
        if (isSpecialWinActive) return;

        if (gameManager.isAutoPlaying)
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: true);
        }
        else if (gameManager.isInFreeSpins)
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: false);
        }
        else
        {
            SetBetControlsEnabled(true);
            SetButtonInteractable(settingsOpenButton, settingsOpenButtonPortrait, true);
            SetSpinStopButtonStates(isSpinningState: false, isInteractable: true);
        }
    }

    #endregion

    #region Spin Button

    public void OnSpinButtonPressed()
    {
        if (gameManager.isAutoPlaying)
        {
            AudioManager.Instance?.PlayAutoplayStop();
            gameManager.StopAutoPlay();
            return;
        }

        if (!gameManager.IsSpinning())
        {
            gameManager.RequestSpin();
        }
    }

    private void OnStopButtonPressed()
    {
        if (gameManager.isAutoPlaying)
        {
            AudioManager.Instance?.PlayAutoplayStop();
            gameManager.StopAutoPlay();
            return;
        }

        if (gameManager.IsSpinning())
        {
            // Rapid-stop cooldown: prevent the player from spamming the stop button
            if (Time.unscaledTime - lastRapidStopTime < rapidStopCooldown)
                return;

            lastRapidStopTime = Time.unscaledTime;
            AudioManager.Instance?.PlaySpinStop();
            gameManager.RequestStop();
        }
    }

    internal void DisableSpinButtonDuringStop()
    {
        if (gameManager != null && gameManager.isInFreeSpins)
        {
            SetSpinStopButtonStates(isSpinningState: false, isInteractable: false);
            return;
        }

        if (gameManager.isAutoPlaying)
        {
            SetButtonActive(spinButton, spinButtonPortrait, false);
            SetButtonActive(stopButton, stopButtonPortrait, false);
            SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, true);
            SetButtonInteractable(autoSpinStopButton, autoSpinStopButtonPortrait, false);
        }
        else
        {
            SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, false);
            SetButtonActive(spinButton, spinButtonPortrait, false);
            SetButtonActive(stopButton, stopButtonPortrait, true);
            SetButtonInteractable(stopButton, stopButtonPortrait, false);
        }
    }

    internal void SetSpinStopButtonStates(bool isSpinningState, bool isInteractable)
    {
        // Free games are automatic. Keep the familiar Spin button visible as a
        // status indicator, but never allow the player to start/stop a free spin.
        if (gameManager != null && gameManager.isInFreeSpins)
        {
            SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, false);
            SetButtonActive(stopButton, stopButtonPortrait, false);
            SetButtonActive(spinButton, spinButtonPortrait, true);
            SetButtonInteractable(spinButton, spinButtonPortrait, false);
            return;
        }

        if (gameManager.isAutoPlaying)
        {
            SetButtonActive(spinButton, spinButtonPortrait, false);
            SetButtonActive(stopButton, stopButtonPortrait, false);
            SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, true);
            SetButtonInteractable(autoSpinStopButton, autoSpinStopButtonPortrait, isInteractable);
        }
        else
        {
            SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, false);
            
            if (isSpinningState)
            {
                SetButtonActive(spinButton, spinButtonPortrait, false);
                SetButtonActive(stopButton, stopButtonPortrait, true);
                SetButtonInteractable(stopButton, stopButtonPortrait, isInteractable);
            }
            else
            {
                SetButtonActive(stopButton, stopButtonPortrait, false);
                SetButtonActive(spinButton, spinButtonPortrait, true);
                SetButtonInteractable(spinButton, spinButtonPortrait, isInteractable);
            }
        }
    }

    #endregion

    #region Bet Controls

    internal void UpdateBetDisplay()
    {
        if (gameManager.gameConfig == null) return;

        double totalPay = gameManager.GetTotalPay();

        if (betAmountText) betAmountText.text = FormatAmount(totalPay);
        if (betAmountTextPortrait) betAmountTextPortrait.text = "TOTAL PAY : " + FormatAmount(totalPay);

        UpdateBetButtonStates();
        UpdateGameRulesDynamicTexts();
    }

    private void UpdateBetButtonStates()
    {
        SetButtonInteractable(betMinusButton, betMinusButtonPortrait, true);
        SetButtonInteractable(betPlusButton, betPlusButtonPortrait, true);
    }

    #endregion

    #region Auto Play Panel

    public void OnSpinButtonHeld()
    {
        if (isFreeGamesFeatureActive) return;

        if (gameManager.currentState == GameState.Idle && !gameManager.isAutoPlaying)
        {
            AudioManager.Instance?.PlayButton();
            OpenAutoPlayPanel();
        }
    }

    private void OpenAutoPlayPanel()
    {
        AudioManager.Instance?.PlayAutoplayPanelOpen();
        if (isSettingsPanelOpen)
            CloseSettingsPanelImmediate();

        SetGameObjectActive(autoPlayPanel, autoPlayPanelPortrait, true);
        if (autoPlayPanelRect)
        {
            autoPlayPanelRect.anchoredPosition = new Vector2(autoPlayPanelRect.anchoredPosition.x, -600f);
            autoPlayPanelRect.DOAnchorPosY(0f, 0.35f).SetEase(Ease.OutCubic);
        }
        if (autoPlayPanelRectPortrait)
        {
            autoPlayPanelRectPortrait.anchoredPosition = new Vector2(autoPlayPanelRectPortrait.anchoredPosition.x, -600f);
            autoPlayPanelRectPortrait.DOAnchorPosY(0f, 0.35f).SetEase(Ease.OutCubic);
        }
    }

    private void CloseAutoPlayPanel()
    {
        AudioManager.Instance?.PlayPopupClose();

        if (autoPlayPanelRect)
        {
            autoPlayPanelRect.DOAnchorPosY(-600f, 0.35f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                if (autoPlayPanel) autoPlayPanel.SetActive(false);
            });
        }
        else
        {
            if (autoPlayPanel) autoPlayPanel.SetActive(false);
        }

        if (autoPlayPanelRectPortrait)
        {
            autoPlayPanelRectPortrait.DOAnchorPosY(-600f, 0.35f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                if (autoPlayPanelPortrait) autoPlayPanelPortrait.SetActive(false);
            });
        }
        else
        {
            if (autoPlayPanelPortrait) autoPlayPanelPortrait.SetActive(false);
        }
    }

    private void StartAutoplayWithRounds(int rounds)
    {
        CloseAutoPlayPanel();
        gameManager.StartAutoPlay(rounds);
    }

    internal void OnAutoPlayStarted()
    {
        UpdateAutoPlayCount();
        SetSpinStopButtonStates(isSpinningState: true, isInteractable: true);
        SetBetControlsEnabled(false);
    }

    internal void OnAutoPlayStopped()
    {
        SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, false);

        bool isRoundActive = gameManager.IsSpinning() || gameManager.lastResult != null;

        if (!isRoundActive && !gameManager.isInFreeSpins)
        {
            SetSpinStopButtonStates(isSpinningState: false, isInteractable: true);
            SetBetControlsEnabled(true);
            SetButtonInteractable(settingsOpenButton, settingsOpenButtonPortrait, true);
        }
        else if (isRoundActive)
        {
            SetButtonActive(spinButton, spinButtonPortrait, false);
            SetButtonActive(stopButton, stopButtonPortrait, false);
            SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, true);
            SetButtonInteractable(autoSpinStopButton, autoSpinStopButtonPortrait, false);
        }
    }

    internal void UpdateAutoPlayCount()
    {
        string displayStr = "";
        if (gameManager.autoPlayTotalRounds == -1 || gameManager.autoPlayRemainingRounds < 0)
        {
            displayStr = "∞";
        }
        else
        {
            displayStr = $"{gameManager.autoPlayRemainingRounds}";
        }

        SetTMPText(autoSpinRemainingText, autoSpinRemainingTextPortrait, displayStr);
    }

    #endregion

    #region Spin Speed Universal Toggle Logic

    public void SetSpeedMode(SpinSpeed speed)
    {
        gameManager.SetSpinSpeed(speed);
        UpdateSpeedButtonsVisibility(speed);
    }

    private void UpdateSpeedButtonsVisibility(SpinSpeed speed)
    {
        SetButtonActive(normalSpeedButton, normalSpeedButtonPortrait, speed == SpinSpeed.Normal);
        SetButtonActive(turboSpeedButton, turboSpeedButtonPortrait, speed == SpinSpeed.Turbo);
        SetButtonActive(quickSpeedButton, quickSpeedButtonPortrait, speed == SpinSpeed.QuickSpin);
    }

    #endregion

    #region Sound Panel

    private void OpenSoundPanel()
    {
        AudioManager.Instance?.PlayButton();
        if (soundPanel == null) return;
        soundPanel.SetActive(true);
        if (soundPanelRect != null)
        {
            AnimatePopupOpen(soundPanelRect);
        }
        if (musicSlider && AudioManager.Instance != null)
        {
            musicSlider.value = AudioManager.Instance.MusicVolume;
        }
        if (sfxSlider && AudioManager.Instance != null)
        {
            sfxSlider.value = AudioManager.Instance.SfxVolume;
        }
    }

    private void CloseSoundPanel()
    {
        if (soundPanel == null || !soundPanel.activeSelf) return;
        if (soundPanelRect != null)
        {
            AnimatePopupClose(soundPanelRect, () =>
            {
                soundPanel.SetActive(false);
            });
        }
        else
        {
            soundPanel.SetActive(false);
        }
    }

    private void OnMusicSliderChanged(float val)
    {
        AudioManager.Instance?.SetMusicVolume(val);
    }

    private void OnSfxSliderChanged(float val)
    {
        AudioManager.Instance?.SetSfxVolume(val);
    }

    #endregion

    #region Settings Panel

    private void OpenSettingsPanel()
    {
        if ((autoPlayPanel && autoPlayPanel.activeSelf) || (autoPlayPanelPortrait && autoPlayPanelPortrait.activeSelf))
            CloseAutoPlayPanelImmediate();

        isSettingsPanelOpen = true;

        SetButtonActive(settingsOpenButton, settingsOpenButtonPortrait, false);
        SetButtonActive(settingsCloseButton, settingsCloseButtonPortrait, true);
        SetButtonActive(settingsBgCloseButton, settingsBgCloseButtonPortrait, true);

        if (settingsPanel)
        {
            settingsPanel.SetActive(true);
            CanvasGroup cg = settingsPanel.GetComponent<CanvasGroup>();
            if (cg == null) cg = settingsPanel.AddComponent<CanvasGroup>();
            cg.DOKill();
            cg.DOFade(1f, 0.35f);
        }

        if (settingsPanelPortrait)
        {
            settingsPanelPortrait.SetActive(true);
            CanvasGroup cg = settingsPanelPortrait.GetComponent<CanvasGroup>();
            if (cg == null) cg = settingsPanelPortrait.AddComponent<CanvasGroup>();
            cg.DOKill();
            cg.DOFade(1f, 0.35f);
        }
    }

    private void CloseSettingsPanel()
    {
        isSettingsPanelOpen = false;

        SetButtonActive(settingsOpenButton, settingsOpenButtonPortrait, true);
        SetButtonActive(settingsCloseButton, settingsCloseButtonPortrait, false);
        SetButtonActive(settingsBgCloseButton, settingsBgCloseButtonPortrait, false);

        if (settingsPanel)
        {
            CanvasGroup cg = settingsPanel.GetComponent<CanvasGroup>();
            if (cg == null) cg = settingsPanel.AddComponent<CanvasGroup>();
            cg.DOKill();
            cg.DOFade(0f, 0.35f).OnComplete(() =>
            {
                settingsPanel.SetActive(false);
            });
        }

        if (settingsPanelPortrait)
        {
            CanvasGroup cg = settingsPanelPortrait.GetComponent<CanvasGroup>();
            if (cg == null) cg = settingsPanelPortrait.AddComponent<CanvasGroup>();
            cg.DOKill();
            cg.DOFade(0f, 0.35f).OnComplete(() =>
            {
                settingsPanelPortrait.SetActive(false);
            });
        }
    }

    private void CloseSettingsPanelImmediate()
    {
        isSettingsPanelOpen = false;

        SetButtonActive(settingsOpenButton, settingsOpenButtonPortrait, true);
        SetButtonActive(settingsCloseButton, settingsCloseButtonPortrait, false);
        SetButtonActive(settingsBgCloseButton, settingsBgCloseButtonPortrait, false);

        if (settingsPanel)
        {
            CanvasGroup cg = settingsPanel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.DOKill();
                cg.alpha = 0f;
            }
            settingsPanel.SetActive(false);
        }

        if (settingsPanelPortrait)
        {
            CanvasGroup cg = settingsPanelPortrait.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.DOKill();
                cg.alpha = 0f;
            }
            settingsPanelPortrait.SetActive(false);
        }
    }

    private void CloseAutoPlayPanelImmediate()
    {
        if (autoPlayPanelRect) autoPlayPanelRect.localScale = Vector3.one;
        if (autoPlayPanelRectPortrait) autoPlayPanelRectPortrait.localScale = Vector3.one;
        SetGameObjectActive(autoPlayPanel, autoPlayPanelPortrait, false);
    }

    #endregion

    #region Game Rules Panel

    private void OpenGameRulesPanel()
    {
        if (isSettingsPanelOpen)
        {
            CloseSettingsPanelImmediate();
        }
        ShowGameRulesPanel();
    }

    private void ShowGameRulesPanel()
    {
        if (gameRulesPanel == null) return;
        gameRulesPanel.SetActive(true);
    }

    private void CloseGameRulesPanel()
    {
        if (gameRulesPanel == null) return;
        gameRulesPanel.SetActive(false);
    }

    #endregion

    #region Guide Panel

    private void OpenGuidePanel()
    {
        if (isSettingsPanelOpen)
        {
            CloseSettingsPanelImmediate();
        }
        ShowGuidePanel();
    }

    private void ShowGuidePanel()
    {
        if (guidePanel == null) return;
        guidePanel.SetActive(true);
    }

    private void CloseGuidePanel()
    {
        if (guidePanel == null) return;
        guidePanel.SetActive(false);
    }

    #endregion

    #region Free Spins Flow

    private void InitializeFreeGamesFeaturePanel()
    {
        bool hasAllReferences =
            freeGamesFeaturePanel != null &&
            freeGamesStartPanel != null &&
            freeGamesMidPanel != null &&
            freeGamesEndPanel != null &&
            landscapeGreyFilm != null &&
            freeGamesStartButtonLandscape != null &&
            freeGamesStartButtonPortrait != null &&
            freeGamesTakeButtonLandscape != null &&
            freeGamesTakeButtonPortrait != null &&
            freeGamesRemainingSpriteText != null &&
            freeGamesGameWinSpriteText != null &&
            freeGamesFeatureWinSpriteText != null &&
            freeGamesTotalWinSpriteText != null;

        if (!hasAllReferences)
        {
            Debug.LogError("One or more explicit Free Games UI references are missing. Falling back to the old free-spin UI.");
            freeGamesFeaturePanel = null;
        }
    }

    private void SetFreeGamesPanelState(bool rootActive, bool startActive, bool midActive, bool endActive)
    {
        if (freeGamesFeaturePanel) freeGamesFeaturePanel.SetActive(rootActive);
        if (freeGamesStartPanel) freeGamesStartPanel.SetActive(startActive);
        if (freeGamesMidPanel) freeGamesMidPanel.SetActive(midActive);
        if (freeGamesEndPanel) freeGamesEndPanel.SetActive(endActive);
        if (landscapeGreyFilm) landscapeGreyFilm.SetActive(rootActive && (startActive || endActive));
    }

    internal void OnFreeSpinsStarted(int spins, double triggeringGameWin)
    {
        OnFreeSpinsTriggered(spins, triggeringGameWin);
    }

    internal void OnFreeSpinsTriggered(int spinsAwarded, double triggeringGameWin = 0)
    {
        if (freeGamesFeaturePanel == null)
        {
            ShowUniversalWinPopup(WinPopupType.FreeSpinTrigger, 0, spinsAwarded, () =>
            {
                StartFreeSpinsSequence(spinsAwarded);
            });
            return;
        }

        pendingFreeSpins = spinsAwarded;
        totalFreeSpinWin = 0;
        freeGamesTriggerWin = System.Math.Max(0, triggeringGameWin);
        freeGamesFeatureWin = 0;
        freeGamesHeldBalance = gameManager != null && gameManager.playerData != null
            ? gameManager.playerData.balance
            : 0;
        isFreeGamesFeatureActive = true;
        isFreeGamesEndScreenActive = false;
        isFreeGamesAmountAnimationComplete = false;
        isFreeGamesRetriggerStart = false;
        isFreeGamesAwaitingStartButton = false;

        if (bigWinObject) bigWinObject.SetActive(false);

        if (freeGamesAmountTween != null)
        {
            freeGamesAmountTween.Kill();
            freeGamesAmountTween = null;
        }

        SetFreeGamesPanelState(true, true, false, false);
        Debug.Log($"[FreeGames] Showing Start panel with {spinsAwarded} awarded spins.");

        // During the three-second announcement, keep the familiar Spin visible
        // as a disabled status button. It is replaced by Start when Mid appears.
        SetSpinStopButtonStates(isSpinningState: false, isInteractable: false);
        SetButtonActive(wheelSpinButton2, wheelSpinButtonPortrait2, false);
        UpdateFreeGamesActionButtonsVisibility();

        UpdateFreeGamesRemainingSprite(spinsAwarded);
        UpdateBalanceDisplay();
        BeginFreeGamesStartAnnouncement();
    }

    internal void OnFreeSpinsRetriggered(int spinsAwarded)
    {
        if (freeGamesFeaturePanel == null)
        {
            ShowUniversalWinPopup(WinPopupType.FreeSpinTrigger, 0, spinsAwarded, () =>
            {
                gameManager.ContinueFreeSpinsAfterRetrigger();
            });
            return;
        }

        pendingFreeSpins = spinsAwarded;
        totalFreeSpinsAwarded += spinsAwarded;
        isFreeGamesRetriggerStart = true;
        isFreeGamesEndScreenActive = false;
        isFreeGamesAwaitingStartButton = false;

        // Preserve the held balance, cumulative WIN, feature awards, and all
        // previously played free-game totals. Only switch the visible state.
        SetFreeGamesPanelState(true, true, false, false);
        Debug.Log($"[FreeGames] Retriggered {spinsAwarded} more games. Showing Start panel again.");

        // Retriggers use the same disabled Spin during their announcement.
        SetSpinStopButtonStates(isSpinningState: false, isInteractable: false);
        SetButtonActive(wheelSpinButton2, wheelSpinButtonPortrait2, false);
        UpdateFreeGamesActionButtonsVisibility();

        // Do not reveal the server's increased remaining count yet. It is
        // applied by ContinueFreeSpinsSequence after the player presses Start.
        UpdateBalanceDisplay();
        BeginFreeGamesStartAnnouncement();
    }

    private void BeginFreeGamesStartAnnouncement()
    {
        if (freeGamesStartAnnouncementCoroutine != null)
        {
            StopCoroutine(freeGamesStartAnnouncementCoroutine);
        }

        freeGamesStartAnnouncementCoroutine = StartCoroutine(FreeGamesStartAnnouncementSequence());
    }

    private IEnumerator FreeGamesStartAnnouncementSequence()
    {
        yield return new WaitForSeconds(Mathf.Max(0f, freeGamesStartDisplayDuration));
        freeGamesStartAnnouncementCoroutine = null;

        if (!isFreeGamesFeatureActive || isFreeGamesEndScreenActive || pendingFreeSpins <= 0)
        {
            yield break;
        }

        // Start is a timed announcement. Mid is then revealed while the bottom
        // Spin control is replaced by the orientation-specific Start button.
        SetFreeGamesPanelState(true, false, true, false);
        isFreeGamesAwaitingStartButton = true;
        SetButtonActive(spinButton, spinButtonPortrait, false);
        SetButtonActive(stopButton, stopButtonPortrait, false);
        SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, false);
        UpdateFreeGamesActionButtonsVisibility();
        Debug.Log("[FreeGames] Start announcement complete. Showing Mid and waiting for Start button.");
    }

    private void OnFreeGamesStartButtonClicked()
    {
        if (!isFreeGamesFeatureActive ||
            isFreeGamesEndScreenActive ||
            !isFreeGamesAwaitingStartButton ||
            pendingFreeSpins <= 0) return;

        AudioManager.Instance?.PlayButton();
        int spinsToStart = pendingFreeSpins;
        bool resumeAfterRetrigger = isFreeGamesRetriggerStart;
        pendingFreeSpins = 0;
        isFreeGamesRetriggerStart = false;
        isFreeGamesAwaitingStartButton = false;
        SetButtonActive(freeGamesStartButtonLandscape, freeGamesStartButtonPortrait, false);
        SetButtonActive(wheelSpinButton2, wheelSpinButtonPortrait2, false);

        if (resumeAfterRetrigger)
        {
            ContinueFreeSpinsSequence();
        }
        else
        {
            StartFreeSpinsSequence(spinsToStart);
        }
    }

    private void StartFreeSpinsSequence(int spinsAwarded)
    {
        totalFreeSpinWin = 0;
        initialFreeSpins = spinsAwarded;
        totalFreeSpinsAwarded = spinsAwarded;
        
        SetFreeGamesPanelState(true, false, true, false);
        Debug.Log("[FreeGames] Start pressed. Showing Mid panel and hiding LandscapeGreyFilm.");
        UpdateFreeSpinCount(0, spinsAwarded);
        // Keep the triggering spin in the running bottom WIN total. Each free
        // game will count upward from this amount rather than resetting it.
        UpdateWinDisplay(freeGamesTriggerWin, GetWinDecimalPlaces(freeGamesTriggerWin));

        // Show Spin as a disabled status indicator while free games run.
        SetSpinStopButtonStates(isSpinningState: false, isInteractable: false);
        gameManager.StartFirstFreeSpin();
    }

    private void ContinueFreeSpinsSequence()
    {
        SetFreeGamesPanelState(true, false, true, false);
        Debug.Log("[FreeGames] Retrigger Start pressed. Returning to Mid without resetting the accumulated WIN.");
        UpdateFreeSpinCount(gameManager.freeSpinsUsed, totalFreeSpinsAwarded);

        // Keep the cumulative bottom-bar WIN exactly where the previous spin
        // finished and resume automatic games with the normal disabled Spin.
        SetSpinStopButtonStates(isSpinningState: false, isInteractable: false);
        gameManager.ContinueFreeSpinsAfterRetrigger();
    }

    internal void OnFreeSpinsEnded(double serverTotalRoundWin, int serverTotalSpinsUsed)
    {
        initialFreeSpins = 0;
        totalFreeSpinsAwarded = 0;

        if (freeGamesFeaturePanel == null)
        {
            ShowUniversalWinPopup(WinPopupType.FreeSpinComplete, serverTotalRoundWin, 0, () =>
            {
                StartCoroutine(EndFreeSpinsTransitionSequence());
            });
            return;
        }

        ShowFreeGamesEndScreen(serverTotalRoundWin);
    }

    private void ShowFreeGamesEndScreen(double freeSpinWin)
    {
        freeSpinWin = System.Math.Max(0, freeSpinWin);
        isFreeGamesEndScreenActive = true;
        isFreeGamesAmountAnimationComplete = false;
        isFreeGamesAwaitingStartButton = false;
        isSpecialWinActive = true;

        if (freeGamesStartAnnouncementCoroutine != null)
        {
            StopCoroutine(freeGamesStartAnnouncementCoroutine);
            freeGamesStartAnnouncementCoroutine = null;
        }
        if (bigWinObject) bigWinObject.SetActive(false);
        SetFreeGamesPanelState(true, false, false, true);
        Debug.Log("[FreeGames] Showing End panel with LandscapeGreyFilm.");

        SetButtonActive(spinButton, spinButtonPortrait, false);
        SetButtonActive(stopButton, stopButtonPortrait, false);
        SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, false);
        SetButtonActive(wheelSpinButton2, wheelSpinButtonPortrait2, false);

        // Take is visible immediately so the state is obvious, but it cannot be
        // pressed until every sprite amount has reached its final value.
        UpdateFreeGamesActionButtonsVisibility();

        // Game Win belongs to the base-game spin that triggered Free Games.
        // Feature Win contains everything won during the free-game session.
        double gameWin = System.Math.Max(0, freeGamesTriggerWin);
        double featureWin = System.Math.Max(0, freeSpinWin + freeGamesFeatureWin);
        double totalWin = System.Math.Max(0, gameWin + featureWin);
        SetFreeGamesEndAmounts(0, 0, 0);

        AudioManager.Instance?.PlayWinObjectBg();

        if (freeGamesAmountTween != null) freeGamesAmountTween.Kill();
        float amountDuration = Mathf.Max(0.01f, freeGamesAmountCountDuration);
        Sequence amountSequence = DOTween.Sequence();

        // Count each value separately: Game Win, then Feature Win, then Total.
        amountSequence.Append(DOVirtual.Float(0f, 1f, amountDuration, progress =>
        {
            SetFreeGamesEndAmounts(gameWin * progress, 0, 0);
        }).SetEase(Ease.OutCubic));
        amountSequence.Append(DOVirtual.Float(0f, 1f, amountDuration, progress =>
        {
            SetFreeGamesEndAmounts(gameWin, featureWin * progress, 0);
        }).SetEase(Ease.OutCubic));
        amountSequence.Append(DOVirtual.Float(0f, 1f, amountDuration, progress =>
        {
            SetFreeGamesEndAmounts(gameWin, featureWin, totalWin * progress);
        }).SetEase(Ease.OutCubic));
        amountSequence.OnComplete(() =>
        {
            SetFreeGamesEndAmounts(gameWin, featureWin, totalWin);
            freeGamesAmountTween = null;
            isFreeGamesAmountAnimationComplete = true;
            ShowFreeGamesBigWinTakeStage();
        });
        freeGamesAmountTween = amountSequence;
    }

    private void ShowFreeGamesBigWinTakeStage()
    {
        // The totals have finished. Remove the End panel and its grey film,
        // then always show BigWin while Take waits for the player.
        SetFreeGamesPanelState(false, false, false, false);

        if (bigWinObject != null)
        {
            bigWinObject.SetActive(false);
            bigWinObject.SetActive(true);
        }

        UpdateFreeGamesActionButtonsVisibility();
        Debug.Log("[FreeGames] End totals complete. Showing BigWin and enabling Take.");
    }

    private void SetFreeGamesEndAmounts(double gameWin, double featureWin, double totalWin)
    {
        if (freeGamesGameWinSpriteText) freeGamesGameWinSpriteText.text = FormatSpriteAmount(gameWin);
        if (freeGamesFeatureWinSpriteText) freeGamesFeatureWinSpriteText.text = FormatSpriteAmount(featureWin);
        if (freeGamesTotalWinSpriteText) freeGamesTotalWinSpriteText.text = FormatSpriteAmount(totalWin);
    }

    private void OnFreeGamesTakeButtonClicked()
    {
        if (!isFreeGamesEndScreenActive) return;

        AudioManager.Instance?.StopWinObjectBg();
        AudioManager.Instance?.PlayTakeButton();
        if (freeGamesAmountTween != null)
        {
            freeGamesAmountTween.Kill();
            freeGamesAmountTween = null;
        }
        SetButtonInteractable(freeGamesTakeButtonLandscape, freeGamesTakeButtonPortrait, false);
        SetButtonActive(freeGamesTakeButtonLandscape, freeGamesTakeButtonPortrait, false);

        isFreeGamesEndScreenActive = false;
        isFreeGamesFeatureActive = false;
        isFreeGamesAmountAnimationComplete = false;
        isSpecialWinActive = false;
        isFreeGamesRetriggerStart = false;
        isFreeGamesAwaitingStartButton = false;
        freeGamesTriggerWin = 0;
        pendingFreeSpins = 0;
        SetFreeGamesPanelState(false, false, false, false);
        if (bigWinObject) bigWinObject.SetActive(false);

        // The server has already credited every free-game result. Reveal that
        // authoritative final balance now instead of adding the win a second time.
        if (gameManager != null && gameManager.playerData != null)
        {
            AnimateBalanceUpdate(gameManager.playerData.balance, freeGamesHeldBalance);
        }

        FinishFreeSpinsImmediately();
    }

    private void FinishFreeSpinsImmediately()
    {
        // Take closes the feature directly. No black-film transition is needed.
        if (transitionBackFilm != null)
        {
            transitionBackFilm.DOKill();
            transitionBackFilm.alpha = 0f;
            transitionBackFilm.gameObject.SetActive(false);
        }

        UpdateWinDisplay(0);
        SetSpinStopButtonStates(isSpinningState: false, isInteractable: true);
        SetButtonInteractable(settingsOpenButton, settingsOpenButtonPortrait, true);
        SetBetControlsEnabled(true);

        if (gameManager != null && gameManager.ShouldResumeAutoPlay())
        {
            gameManager.ResumeAutoPlay();
        }
    }

    private IEnumerator EndFreeSpinsTransitionSequence()
    {
        // 1. Fade in back film
        if (transitionBackFilm != null)
        {
            transitionBackFilm.gameObject.SetActive(true);
            transitionBackFilm.alpha = 0f;
            yield return transitionBackFilm.DOFade(1f, 0.5f).WaitForCompletion();
            yield return new WaitForSeconds(0.2f);
        }

        // 2. Setup main slot UI state behind back film
        // Reset win display for base game after free spins end
        UpdateWinDisplay(0);

        SetSpinStopButtonStates(isSpinningState: false, isInteractable: true);
        SetButtonInteractable(settingsOpenButton, settingsOpenButtonPortrait, true);
        SetBetControlsEnabled(true);

        // 3. Fade out back film
        if (transitionBackFilm != null)
        {
            yield return transitionBackFilm.DOFade(0f, 0.5f).WaitForCompletion();
            transitionBackFilm.gameObject.SetActive(false);
        }

        // 4. Resume autoplay if it was active before free spins and has leftover rounds
        if (gameManager != null && gameManager.ShouldResumeAutoPlay())
        {
            gameManager.ResumeAutoPlay();
        }
    }

    internal void UpdateFreeSpinCount(int playedSpins, int totalSpins = -1)
    {
        if (totalSpins > 0)
        {
            totalFreeSpinsAwarded = totalSpins;
        }

        int remaining = gameManager != null && gameManager.isInFreeSpins
            ? gameManager.freeSpinsRemaining
            : Mathf.Max(0, totalFreeSpinsAwarded - playedSpins);
        UpdateFreeGamesRemainingSprite(remaining);
    }

    private void UpdateFreeGamesRemainingSprite(int remaining)
    {
        if (freeGamesRemainingSpriteText)
        {
            freeGamesRemainingSpriteText.text = FormatSpriteInteger(Mathf.Max(0, remaining));
        }
    }

    private string FormatSpriteInteger(int value)
    {
        return FormatSpriteCharacters(Mathf.Max(0, value).ToString(System.Globalization.CultureInfo.InvariantCulture));
    }

    private string FormatSpriteAmount(double amount)
    {
        string fixedAmount = System.Math.Max(0, amount).ToString(
            "0.00",
            System.Globalization.CultureInfo.InvariantCulture);
        return FormatSpriteCharacters(fixedAmount);
    }

    private string FormatSpriteCharacters(string value)
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder(value.Length * 10);
        foreach (char character in value)
        {
            if (character >= '0' && character <= '9')
            {
                builder.Append("<sprite=").Append(character - '0').Append('>');
            }
            else if (character == '.')
            {
                // NUMBERS 2 uses sprite index 10 for the decimal point.
                builder.Append("<sprite=10>");
            }
        }
        return builder.ToString();
    }

    #endregion

    #region Expand / Shrink

    private void InitializeExpandShrink()
    {
        SetExpandShrinkButtons(isExpanded: false);
    }

    private void OnExpand()
    {
        isExpanded = true;
        jsFunctCalls?.RequestExpandGame();
        SetExpandShrinkButtons(isExpanded: true);
    }

    private void OnShrink()
    {
        isExpanded = false;
        jsFunctCalls?.RequestShrinkGame();
        SetExpandShrinkButtons(isExpanded: false);
    }

    private void SetExpandShrinkButtons(bool isExpanded)
    {
        SetButtonActive(expandButton, expandButtonPortrait, !isExpanded);
        SetButtonActive(shrinkButton, shrinkButtonPortrait, isExpanded);
    }

    private void RegisterFullscreenListener()
    {
        jsFunctCalls?.RegisterFullscreenListener(gameObject.name);
    }

    internal void OnFullscreenChanged(string isFullscreen)
    {
        bool newExpandedState = isFullscreen == "1";
        Debug.Log($"[UI] OnFullscreenChanged callback: isFullscreen={isFullscreen}, newState={newExpandedState}");

        if (isExpanded != newExpandedState)
        {
            isExpanded = newExpandedState;
            SetExpandShrinkButtons(isExpanded);
            Debug.Log($"[UI] Button states synced to fullscreen: {(isExpanded ? "EXPANDED" : "SHRINK")}");
        }
    }
    
    #endregion

    #region Popup Animations (Generic)

    private void AnimatePopupOpen(RectTransform popupRect)
    {
        if (!popupRect) return;
        popupRect.localScale = Vector3.zero;
        popupRect.DOScale(1.4f, 0.3f).SetEase(Ease.OutBack);
    }

    private void AnimatePopupClose(RectTransform popupRect, System.Action onComplete)
    {
        if (!popupRect) return;

        AudioManager.Instance?.PlayPopupClose();

        Sequence closeSeq = DOTween.Sequence();
        closeSeq.Append(popupRect.DOScale(1.1f, 0.1f));
        closeSeq.Append(popupRect.DOScale(0f, 0.2f).SetEase(Ease.InBack));
        closeSeq.OnComplete(() =>
        {
            popupRect.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            onComplete?.Invoke();
        });
    }

    #endregion

    #region Display Updates

    internal void UpdatePingDisplay(int pingMs)
    {
        SetTMPText(pingText, pingTextPortrait, $"{pingMs} ms");
    }

    internal void UpdatePingDisplay(string content)
    {
        SetTMPText(pingText, pingTextPortrait, content);
    }

    internal void UpdateJackpotDisplay(JackpotValues values)
    {
        if (values == null) return;

        SetTMPText(grandJackpotText, grandJackpotTextPortrait, FormatJackpotValue(values.grandJackpot));
        SetTMPText(majorJackpotText, majorJackpotTextPortrait, FormatJackpotValue(values.majorJackpot));
        SetTMPText(minorJackpotText, minorJackpotTextPortrait, FormatJackpotValue(values.minorJackpot));
        SetTMPText(miniJackpotText, miniJackpotTextPortrait, FormatJackpotValue(values.miniJackpot));
    }

    private string FormatJackpotValue(string val)
    {
        if (string.IsNullOrEmpty(val)) return "$0.00";
        return val.StartsWith("$") ? val : "$" + val;
    }

    internal void UpdateBalanceDisplay()
    {
        double balanceToDisplay = isFreeGamesFeatureActive
            ? freeGamesHeldBalance
            : gameManager.playerData.balance;
        SetTMPText(balanceText, balanceTextPortrait, "BALANCE : " + FormatAmount(balanceToDisplay));
    }

    private void UpdateWinDisplay(double amount, int? decimalPlaces = null)
    {
        if (decimalPlaces.HasValue)
        {
            currentWinDecimalPlaces = Mathf.Clamp(decimalPlaces.Value, 0, 3);
        }

        currentWinDisplayValue = amount;
        if (winAmountText) winAmountText.text = FormatWinAmount(amount, currentWinDecimalPlaces);
        if (winAmountTextPortrait) winAmountTextPortrait.text = "WIN " + FormatWinAmount(amount, currentWinDecimalPlaces);

        bool showWinText = amount > 0 || (gameManager != null && gameManager.isInFreeSpins);

        if (showWinText)
        {
            SetGameObjectActive(goodLuckObject, goodLuckObjectPortrait, false);
            SetGameObjectActive(winTextObject, winTextObjectPortrait, true);
        }
        else
        {
            SetGameObjectActive(goodLuckObject, goodLuckObjectPortrait, true);
            SetGameObjectActive(winTextObject, winTextObjectPortrait, false);
        }
    }

    private void AnimateBalanceUpdate(double newBalance, double startBalance = -1f, float durationOverride = -1f)
    {
        if (balanceTween != null) balanceTween.Kill();
        hasOptimisticBalance = false;
        double balanceToDisplay = isFreeGamesFeatureActive ? freeGamesHeldBalance : newBalance;
        SetTMPText(balanceText, balanceTextPortrait, "BALANCE : " + FormatAmount(balanceToDisplay));
    }

    private void AnimateWinUpdate(double targetWin, float duration = 0.8f)
    {
        if (winTween != null) winTween.Kill();

        targetWin = System.Math.Max(0, targetWin);
        int targetDecimalPlaces = GetWinDecimalPlaces(targetWin);
        currentWinDecimalPlaces = targetDecimalPlaces;
        double startWin = currentWinDisplayValue;
        if (duration <= 0f || System.Math.Abs(targetWin - startWin) < 0.000001d)
        {
            UpdateWinDisplay(targetWin, targetDecimalPlaces);
            winTween = null;
            return;
        }

        winTween = DOTween.To(
                () => currentWinDisplayValue,
                value => UpdateWinDisplay(value, targetDecimalPlaces),
                targetWin,
                duration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                UpdateWinDisplay(targetWin, targetDecimalPlaces);
                winTween = null;
            });
    }

    #endregion

    #region Helper Methods

    private string FormatAmount(double amount)
    {
        return amount.ToString("0.###");
    }

    private string FormatWinAmount(double amount, int decimalPlaces)
    {
        string format = decimalPlaces > 0
            ? "0." + new string('0', decimalPlaces)
            : "0";
        return amount.ToString(format, System.Globalization.CultureInfo.InvariantCulture);
    }

    private int GetWinDecimalPlaces(double amount)
    {
        double rounded = System.Math.Round(System.Math.Abs(amount), 3);
        string value = rounded.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
        int decimalPoint = value.IndexOf('.');
        return decimalPoint >= 0 ? value.Length - decimalPoint - 1 : 0;
    }

    private double GetRunningFreeGamesWin()
    {
        double freeGamesWin = gameManager != null ? gameManager.CurrentFreeSpinWin : 0;
        return System.Math.Max(0, freeGamesTriggerWin + freeGamesWin + freeGamesFeatureWin);
    }

    private void SetBetControlsEnabled(bool enabled)
    {
        SetButtonInteractable(betPlusButton, betPlusButtonPortrait, enabled);
        SetButtonInteractable(betMinusButton, betMinusButtonPortrait, enabled);
    }

    #endregion

    #region Dynamic Game Rules Updates

    private void UpdateGameRulesDynamicTexts()
    {
        if (gameManager.gameConfig == null) return;

        if (totalLineCountText != null)
        {
            totalLineCountText.text = gameManager.gameConfig.paylineCount.ToString();
        }

        TMP_Text[] multiplierTexts = {
            ruleSymbol0Text, ruleSymbol1Text, ruleSymbol2Text, ruleSymbol3Text,
            ruleSymbol4Text, ruleSymbol5Text, ruleSymbol6Text, ruleSymbol7Text,
            ruleSymbol8Text, ruleSymbol9Text
        };

        // Use explicitly assigned fields when available. The current Info-page
        // layout keeps its formatted blocks under objects named "Multiplier",
        // so unassigned fields are resolved without altering the scene layout.
        if (gameRulesPanel != null)
        {
            List<TMP_Text> discoveredTexts = new List<TMP_Text>();
            foreach (TMP_Text text in gameRulesPanel.GetComponentsInChildren<TMP_Text>(true))
            {
                if (text != null && text.gameObject.name == "Multiplier")
                    discoveredTexts.Add(text);
            }

            discoveredTexts.Sort((left, right) =>
                left.transform.parent.GetSiblingIndex().CompareTo(right.transform.parent.GetSiblingIndex()));

            for (int i = 0; i < multiplierTexts.Length && i < discoveredTexts.Count; i++)
            {
                if (multiplierTexts[i] == null)
                    multiplierTexts[i] = discoveredTexts[i];
            }
        }

        if (gameManager.gameConfig.symbols == null) return;

        int multiplierBlockCount = Mathf.Min(InfoPageMultiplierSymbolIds.Length, multiplierTexts.Length);
        for (int i = 0; i < multiplierBlockCount; i++)
        {
            TMP_Text multiplierText = multiplierTexts[i];
            if (multiplierText == null) continue;

            SymbolInfo symbol = gameManager.gameConfig.symbols.Find(
                candidate => candidate.id == InfoPageMultiplierSymbolIds[i]);

            if (symbol == null || symbol.multipliers == null || symbol.multipliers.Count == 0)
                continue;

            multiplierText.text = ReplaceInfoPageMultiplierValues(
                multiplierText.text,
                symbol.multipliers);
        }
    }

    private static string ReplaceInfoPageMultiplierValues(string formattedText, List<double> multipliers)
    {
        if (string.IsNullOrEmpty(formattedText) || multipliers == null || multipliers.Count == 0)
            return formattedText;

        int multiplierIndex = 0;
        return Regex.Replace(
            formattedText,
            @"(?i)(\bx\s*)([-+]?(?:\d+(?:\.\d*)?|\.\d+))",
            match =>
            {
                if (multiplierIndex >= multipliers.Count) return match.Value;

                string value = multipliers[multiplierIndex]
                    .ToString("0.###", CultureInfo.InvariantCulture);
                multiplierIndex++;
                return match.Groups[1].Value + value;
            });
    }

    #endregion

    #region Cleanup

    private void OnDestroy()
    {
        StopPortraitJackpotIconAnimations(true);
        ClearMidSpinBigWinState(false);
        if (balanceTween != null) balanceTween.Kill();
        if (winTween != null) winTween.Kill();
        if (freeGamesAmountTween != null) freeGamesAmountTween.Kill();
        DOTween.KillAll();
    }

    #endregion

    #region Connection Popup Management

    private void OnExitButtonPressed()
    {
        if (popupManager != null)
        {
            popupManager.ShowExitGamePopup();
        }
        else if (gameManager != null)
        {
            gameManager.ExitGame();
        }
    }

    #endregion

    #region Bonus Game

    internal void TriggerUSpinBonus(USpinResultData resultData, System.Action onComplete)
    {
        StartCoroutine(USpinBonusSequence(resultData, onComplete));
    }

    private bool wheelSpinTriggered = false;

    private void OnWheelOrFreeGamesStartButtonClicked()
    {
        if (isFreeGamesFeatureActive && !isFreeGamesEndScreenActive && pendingFreeSpins > 0)
        {
            OnFreeGamesStartButtonClicked();
            return;
        }

        OnWheelSpinClicked();
    }

    private void OnWheelSpinClicked()
    {
        if (wheelSpinTriggered) return;
        wheelSpinTriggered = true;
        SetButtonInteractable(wheelSpinButton, wheelSpinButtonPortrait, false);
        SetButtonInteractable(wheelSpinButton2, wheelSpinButtonPortrait2, false);
        SetButtonActive(wheelSpinButton2, wheelSpinButtonPortrait2, false);
    }

    private IEnumerator USpinBonusSequence(USpinResultData resultData, System.Action onComplete)
    {
        // 1. Fade in back film
        if (transitionBackFilm != null)
        {
            transitionBackFilm.gameObject.SetActive(true);
            transitionBackFilm.alpha = 0f;
            yield return transitionBackFilm.DOFade(1f, 0.5f).WaitForCompletion();
            yield return new WaitForSeconds(0.5f);
        }
        
        // 2. Open spin wheel screen
        wheelSpinTriggered = false;
        if (wheelScreen) wheelScreen.SetActive(true);
        StartWheelBonusEffects();
        
        // Hide normal spin/stop buttons, show wheel spin button
        SetSpinStopButtonStates(isSpinningState: false, isInteractable: false);
        SetButtonActive(spinButton, spinButtonPortrait, false);
        SetButtonActive(autoSpinStopButton, autoSpinStopButtonPortrait, false);
        SetButtonActive(wheelSpinButton, wheelSpinButtonPortrait, true);
        SetButtonInteractable(wheelSpinButton, wheelSpinButtonPortrait, true);
        UpdateNewWheelSpinButtonsVisibility();
        SetButtonInteractable(wheelSpinButton2, wheelSpinButtonPortrait2, true);

        // 3. Fade out back film
        if (transitionBackFilm != null)
        {
            yield return transitionBackFilm.DOFade(0f, 0.5f).WaitForCompletion();
            transitionBackFilm.gameObject.SetActive(false);
        }

        // 4. Wait for user to click wheel spin
        yield return new WaitUntil(() => wheelSpinTriggered);

        // No controller-driven wheel animation is used here.
        yield return new WaitForSeconds(0.5f);

        // 6. Handle the two possibilities
        if (resultData.type == "FREE_GAMES")
        {
            // Show freespin trigger popup BEFORE backfilm transition
            bool takePressed = false;
            ShowUniversalWinPopup(WinPopupType.FreeSpinTrigger, 0, resultData.freeGamesAwarded, () =>
            {
                takePressed = true;
            });
            yield return new WaitUntil(() => takePressed);

            // Blackfilm transition fade in to cover the screen
            if (transitionBackFilm != null)
            {
                transitionBackFilm.gameObject.SetActive(true);
                transitionBackFilm.alpha = 0f;
                yield return transitionBackFilm.DOFade(1f, 0.5f).WaitForCompletion();
                yield return new WaitForSeconds(0.5f);
            }
            
            // Turn off wheel screen behind blackfilm
            StopWheelBonusEffects();
            if (wheelScreen) wheelScreen.SetActive(false);
            
            // Setup free spin mode/UI behind blackfilm
            if (gameManager != null)
            {
                if (gameManager.isInFreeSpins)
                {
                    gameManager.freeSpinsRemaining += resultData.freeGamesAwarded;
                    int updatedTotalSpins = totalFreeSpinsAwarded + resultData.freeGamesAwarded;
                    UpdateFreeSpinCount(gameManager.freeSpinsUsed, updatedTotalSpins);
                }
                else
                {
                    if (gameManager.isAutoPlaying)
                    {
                        int prevTotal = gameManager.autoPlayTotalRounds;
                        int prevRemaining = gameManager.autoPlayRemainingRounds;
                        gameManager.StopAutoPlay();
                        gameManager.wasAutoPlayingBeforeFreeSpins = true;
                        gameManager.savedAutoPlayTotalRounds = prevTotal;
                        gameManager.savedAutoPlayRemainingRounds = (prevTotal != -1) ? (prevRemaining - 1) : -1;
                    }

                    gameManager.isInFreeSpins = true;
                    gameManager.freeSpinsRemaining = resultData.freeGamesAwarded;
                    gameManager.freeSpinsUsed = 0;
                    initialFreeSpins = resultData.freeGamesAwarded;
                    totalFreeSpinsAwarded = resultData.freeGamesAwarded;
                    
                    UpdateFreeSpinCount(0, resultData.freeGamesAwarded);
                    UpdateWinDisplay(0);
                    
                    SetSpinStopButtonStates(isSpinningState: true, isInteractable: false);
                    SetBetControlsEnabled(false);
                    SetButtonInteractable(settingsOpenButton, settingsOpenButtonPortrait, true);

                    if (gameManager.lastResult != null && gameManager.lastResult.freeSpinData != null)
                    {
                        gameManager.lastResult.freeSpinData.isTriggered = false;
                    }
                }
            }

            // Blackfilm transition fade out revealing free spin setup
            if (transitionBackFilm != null)
            {
                yield return transitionBackFilm.DOFade(0f, 0.5f).WaitForCompletion();
                transitionBackFilm.gameObject.SetActive(false);
            }
        }
        else // MULTIPLIER / CREDITS
        {
            if (gameManager != null && gameManager.isInFreeSpins)
            {
                freeGamesFeatureWin += System.Math.Max(0, resultData.winInCash);
            }

            bool takePressed = false;
            ShowUniversalWinPopup(WinPopupType.RegularWin, resultData.winInCash, 0, () =>
            {
                takePressed = true;
            });
            yield return new WaitUntil(() => takePressed);

            if (gameManager != null && gameManager.lastResult != null)
            {
                double prevBalance = gameManager.playerData.balance;
                gameManager.playerData.balance += resultData.winInCash;

                double targetWin = gameManager.isInFreeSpins
                    ? GetRunningFreeGamesWin()
                    : gameManager.lastResult.winAmount + resultData.winInCash;
                AnimateWinUpdate(targetWin);
                AnimateBalanceUpdate(gameManager.playerData.balance, prevBalance);
            }
            
            if (transitionBackFilm != null)
            {
                transitionBackFilm.gameObject.SetActive(true);
                transitionBackFilm.alpha = 0f;
                yield return transitionBackFilm.DOFade(1f, 0.5f).WaitForCompletion();
                yield return new WaitForSeconds(0.5f);
            }
            
            StopWheelBonusEffects();
            if (wheelScreen) wheelScreen.SetActive(false);
            
            if (transitionBackFilm != null)
            {
                yield return transitionBackFilm.DOFade(0f, 0.5f).WaitForCompletion();
                transitionBackFilm.gameObject.SetActive(false);
            }
        }

        SetButtonActive(wheelSpinButton, wheelSpinButtonPortrait, false);
        SetButtonActive(wheelSpinButton2, wheelSpinButtonPortrait2, false);
        if (gameManager == null || !gameManager.isInFreeSpins)
        {
            if (gameManager != null && gameManager.isAutoPlaying)
            {
                OnAutoPlayStarted();
            }
            else
            {
                SetSpinStopButtonStates(isSpinningState: false, isInteractable: true);
            }
        }
        else
        {
            SetSpinStopButtonStates(isSpinningState: true, isInteractable: false);
        }

        onComplete?.Invoke();
    }

    #endregion

    #region Universal Win Popup

    internal void ShowUniversalWinPopup(WinPopupType type, double winAmount, int freeSpinCount = 0, System.Action onTakePressed = null)
    {
        if (universalWinPopup == null) return;

        AudioManager.Instance?.PlayWinObjectBg();
        isSpecialWinActive = true;
        universalWinPopupCallback = onTakePressed;

        if (uwpWinTween != null)
        {
            uwpWinTween.Kill();
            uwpWinTween = null;
        }

        if (uwpCongratulationsTitle) uwpCongratulationsTitle.SetActive(false);
        if (uwpYouWonSubtitle) uwpYouWonSubtitle.SetActive(false);
        if (uwpBigWinTitle) uwpBigWinTitle.SetActive(false);
        if (uwpWinAmountText) uwpWinAmountText.gameObject.SetActive(false);
        if (uwpFreeSpinCountText) uwpFreeSpinCountText.gameObject.SetActive(false);
        if (uwpFreeSpinObject) uwpFreeSpinObject.SetActive(false);

        switch (type)
        {
            case WinPopupType.FreeSpinTrigger:
                if (uwpCongratulationsTitle) uwpCongratulationsTitle.SetActive(true);
                if (uwpYouWonSubtitle) uwpYouWonSubtitle.SetActive(true);
                if (uwpFreeSpinCountText)
                {
                    uwpFreeSpinCountText.gameObject.SetActive(true);
                    uwpFreeSpinCountText.text = freeSpinCount.ToString();
                }
                if (uwpFreeSpinObject) uwpFreeSpinObject.SetActive(true);
                break;

            case WinPopupType.RegularWin:
                if (uwpCongratulationsTitle) uwpCongratulationsTitle.SetActive(true);
                if (uwpYouWonSubtitle) uwpYouWonSubtitle.SetActive(true);
                if (uwpWinAmountText)
                {
                    uwpWinAmountText.gameObject.SetActive(true);
                    uwpWinAmountText.text = FormatAmount(winAmount);
                }
                break;

            case WinPopupType.BigWin:
                if (uwpBigWinTitle) uwpBigWinTitle.SetActive(true);
                if (uwpWinAmountText)
                {
                    uwpWinAmountText.gameObject.SetActive(true);
                    uwpWinAmountText.text = FormatAmount(winAmount);
                    RectTransform bigWinAmountRect = uwpWinAmountText.GetComponent<RectTransform>();
                    if (bigWinAmountRect != null)
                    {
                        Vector2 pos = bigWinAmountRect.anchoredPosition;
                        pos.y = 0f;
                        bigWinAmountRect.anchoredPosition = pos;
                    }
                }
                break;

            case WinPopupType.MoneyBagCollect:
                if (uwpCongratulationsTitle) uwpCongratulationsTitle.SetActive(true);
                if (uwpYouWonSubtitle) uwpYouWonSubtitle.SetActive(true);
                if (uwpWinAmountText)
                {
                    uwpWinAmountText.gameObject.SetActive(true);
                    uwpWinAmountText.text = FormatAmount(winAmount);
                }
                break;

            case WinPopupType.FreeSpinComplete:
                if (uwpCongratulationsTitle) uwpCongratulationsTitle.SetActive(true);
                if (uwpYouWonSubtitle) uwpYouWonSubtitle.SetActive(true);
                if (uwpWinAmountText)
                {
                    uwpWinAmountText.gameObject.SetActive(true);
                    uwpWinAmountText.text = FormatAmount(winAmount);
                }
                break;
        }

        if (type != WinPopupType.BigWin && uwpWinAmountText)
        {
            RectTransform winAmountRect = uwpWinAmountText.GetComponent<RectTransform>();
            if (winAmountRect != null)
            {
                Vector2 pos = winAmountRect.anchoredPosition;
                pos.y = -90f;
                winAmountRect.anchoredPosition = pos;
            }
        }

        SetSpinStopButtonStates(isSpinningState: false, isInteractable: false);

        bool showTakeButton = (type != WinPopupType.BigWin);
        SetButtonActive(uwpTakeButton, uwpTakeButtonPortrait, showTakeButton);
        SetButtonInteractable(uwpTakeButton, uwpTakeButtonPortrait, showTakeButton);

        universalWinPopup.SetActive(true);
        if (universalWinPopupRect)
        {
            universalWinPopupRect.localScale = Vector3.zero;
            Sequence openSeq = DOTween.Sequence();
            openSeq.Append(universalWinPopupRect.DOScale(1.2f, 0.5f).SetEase(Ease.OutCubic));
            openSeq.Append(universalWinPopupRect.DOScale(1f, 0.3f).SetEase(Ease.InOutSine));
        }

        if (starFountain != null) starFountain.PlayStarBurst();

        if (uwpWinAmountText != null && uwpWinAmountText.gameObject.activeSelf && winAmount > 0)
        {
            int decimals = GetDecimalPlaces(winAmount);
            string formatStr = decimals > 0 ? "0." + new string('0', decimals) : "0";

            uwpWinAmountText.text = (0.0).ToString(formatStr);

            float countUpDuration = (type == WinPopupType.BigWin) ? 1.5f : 1.0f;

            uwpWinTween = DOVirtual.Float(0f, (float)winAmount, countUpDuration, (val) =>
            {
                if (uwpWinAmountText != null)
                {
                    uwpWinAmountText.text = val.ToString(formatStr);
                }
            }).OnComplete(() =>
            {
                if (uwpWinAmountText != null)
                {
                    uwpWinAmountText.text = FormatAmount(winAmount);
                }
                uwpWinTween = null;
            });
        }

        if (uwpAutoCloseCoroutine != null) StopCoroutine(uwpAutoCloseCoroutine);
        uwpAutoCloseCoroutine = StartCoroutine(AutoCloseUniversalWinPopup());
    }

    private int GetDecimalPlaces(double amount)
    {
        double rounded = System.Math.Round(amount, 4);
        string str = rounded.ToString(System.Globalization.CultureInfo.InvariantCulture);
        int dotIndex = str.IndexOf('.');
        if (dotIndex < 0) return 0;
        return str.Length - dotIndex - 1;
    }

    private IEnumerator AutoCloseUniversalWinPopup()
    {
        yield return new WaitForSeconds(uwpAutoCloseDelay);
        uwpAutoCloseCoroutine = null;
        CloseUniversalWinPopup();
    }

    private void OnUniversalWinTakeButtonClicked()
    {
        if (isFreeGamesEndScreenActive)
        {
            OnFreeGamesTakeButtonClicked();
            return;
        }

        AudioManager.Instance?.StopWinObjectBg();
        AudioManager.Instance?.PlayTakeButton();
        CloseUniversalWinPopup();
    }

    private void CloseUniversalWinPopup()
    {
        if (universalWinPopup == null || !universalWinPopup.activeSelf) return;

        AudioManager.Instance?.StopWinObjectBg();

        if (uwpWinTween != null)
        {
            uwpWinTween.Kill();
            uwpWinTween = null;
        }

        if (starFountain != null) starFountain.StopStarBurst();

        if (uwpAutoCloseCoroutine != null)
        {
            StopCoroutine(uwpAutoCloseCoroutine);
            uwpAutoCloseCoroutine = null;
        }

        System.Action callback = universalWinPopupCallback;
        universalWinPopupCallback = null;

        SetButtonInteractable(uwpTakeButton, uwpTakeButtonPortrait, false);

        if (universalWinPopupRect)
        {
            Sequence closeSeq = DOTween.Sequence();
            closeSeq.Append(universalWinPopupRect.DOScale(1.1f, 0.1f));
            closeSeq.Append(universalWinPopupRect.DOScale(0f, 0.2f).SetEase(Ease.InBack));
            closeSeq.OnComplete(() =>
            {
                universalWinPopupRect.localScale = Vector3.one;
                universalWinPopup.SetActive(false);

                SetButtonActive(uwpTakeButton, uwpTakeButtonPortrait, false);
                isSpecialWinActive = false;
                EnableControlsAfterWinAnimation();

                callback?.Invoke();
            });
        }
        else
        {
            universalWinPopup.SetActive(false);
            SetButtonActive(uwpTakeButton, uwpTakeButtonPortrait, false);
            isSpecialWinActive = false;
            EnableControlsAfterWinAnimation();

            callback?.Invoke();
        }
    }

    #endregion

    #region Bonus Wheel Effects

    internal void StartWheelBonusEffects()
    {
        var oc = Object.FindFirstObjectByType<OrientationChange>();
        
        if (wheelCoinFountain != null)
        {
            wheelCoinFountain.PlayStarBurst();
        }

        if (wheelAnticlockwiseRotatingObject != null)
        {
            if (wheelAnticlockwiseRotationTween != null)
            {
                wheelAnticlockwiseRotationTween.Kill();
                wheelAnticlockwiseRotationTween = null;
            }

            // Continuous anticlockwise rotation (positive Z rotation 0 to 360 degrees)
            wheelAnticlockwiseRotationTween = wheelAnticlockwiseRotatingObject
                .DORotate(new Vector3(0f, 0f, 360f), wheelRotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }

        // Title object popping loop animation (1 -> 1.2 -> 1 scale in loop)
        List<Transform> titlesToAnimate = new List<Transform>();
        if (wheelTitleTransform != null) titlesToAnimate.Add(wheelTitleTransform);
        if (wheelTitleTransformPortrait != null) titlesToAnimate.Add(wheelTitleTransformPortrait);
        if (wheelTitleObjects != null)
        {
            foreach (var t in wheelTitleObjects)
            {
                if (t != null && !titlesToAnimate.Contains(t)) titlesToAnimate.Add(t);
            }
        }

        if (wheelTitleTweens == null) wheelTitleTweens = new List<Tween>();
        else wheelTitleTweens.Clear();

        if (wheelTitleInitialScales == null) wheelTitleInitialScales = new Dictionary<Transform, Vector3>();

        foreach (var t in titlesToAnimate)
        {
            if (t == null) continue;
            t.DOKill();
            if (!wheelTitleInitialScales.ContainsKey(t))
            {
                wheelTitleInitialScales[t] = t.localScale;
            }
            Vector3 initScale = wheelTitleInitialScales[t];
            t.localScale = initScale;

            Tween titleTween = t.DOScale(initScale * 1.2f, 0.6f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            wheelTitleTweens.Add(titleTween);
        }
    }

    internal void StopWheelBonusEffects()
    {
        if (wheelCoinFountain != null)
        {
            wheelCoinFountain.StopStarBurst();
        }

        if (wheelAnticlockwiseRotationTween != null)
        {
            wheelAnticlockwiseRotationTween.Kill();
            wheelAnticlockwiseRotationTween = null;
        }

        if (wheelTitleTweens != null)
        {
            foreach (var tween in wheelTitleTweens)
            {
                if (tween != null && tween.IsActive())
                {
                    tween.Kill();
                }
            }
            wheelTitleTweens.Clear();
        }

        if (wheelTitleInitialScales != null)
        {
            foreach (var kvp in wheelTitleInitialScales)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.DOKill();
                    kvp.Key.localScale = kvp.Value;
                }
            }
        }
    }

    #endregion
}
