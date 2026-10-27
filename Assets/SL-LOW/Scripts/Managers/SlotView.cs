using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using DG.Tweening;

public class SlotView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Symbol Sprites - Assign by Name")]
    [FormerlySerializedAs("spriteLantern"), SerializeField] private Sprite symbolWildSprite;       // ID: 0
    [FormerlySerializedAs("spriteHammer"), SerializeField] private Sprite symbolScatterSprite;    // ID: 1
    [FormerlySerializedAs("spriteMoneyPouch"), SerializeField] private Sprite symbolCarSprite;     // ID: 2
    [FormerlySerializedAs("spriteCoin"), SerializeField] private Sprite symbolYachtSprite;         // ID: 3
    [FormerlySerializedAs("spriteA"), SerializeField] private Sprite symbolMansionSprite;          // ID: 4
    [FormerlySerializedAs("spriteK"), SerializeField] private Sprite symbolWatchSprite;            // ID: 5
    [FormerlySerializedAs("spriteQ"), SerializeField] private Sprite symbolDiamondSprite;          // ID: 6
    [FormerlySerializedAs("spriteJ"), SerializeField] private Sprite symbolASprite;                // ID: 7
    [FormerlySerializedAs("spriteTen"), SerializeField] private Sprite symbolKSprite;              // ID: 8
    [FormerlySerializedAs("spriteNine"), SerializeField] private Sprite symbolQSprite;             // ID: 9
    [FormerlySerializedAs("spriteWild"), SerializeField] private Sprite symbolJSprite;             // ID: 10
    [FormerlySerializedAs("spriteUSpin"), SerializeField] private Sprite symbolTenSprite;          // ID: 11
    [FormerlySerializedAs("spriteMoneyBag"), SerializeField] private Sprite symbolDiceSprite;      // ID: 12

    // Internal array built from named sprites
    private Sprite[] symbolSprites;

    [Header("Win Animation Sprite Arrays")]
    [Tooltip("Animation sprites for the Wild symbol (ID 0)")]
    [SerializeField] private List<Sprite> animSpritesWild;
    [Tooltip("Animation sprites for the Scatter symbol (ID 1)")]
    [FormerlySerializedAs("animSpritesUSpin"), SerializeField] private List<Sprite> animSpritesScatter;
    [Tooltip("Animation sprites for the Car symbol (ID 2)")]
    [SerializeField] private List<Sprite> animSpritesCar;
    [Tooltip("Animation sprites for the Yacht symbol (ID 3)")]
    [SerializeField] private List<Sprite> animSpritesYacht;
    [Tooltip("Animation sprites for the Mansion symbol (ID 4)")]
    [SerializeField] private List<Sprite> animSpritesMansion;
    [Tooltip("Animation sprites for the Watch symbol (ID 5)")]
    [SerializeField] private List<Sprite> animSpritesWatch;
    [Tooltip("Animation sprites for the Diamond symbol (ID 6)")]
    [SerializeField] private List<Sprite> animSpritesDiamond;
    [Tooltip("Animation sprites for the A symbol (ID 7)")]
    [SerializeField] private List<Sprite> animSpritesA;
    [Tooltip("Animation sprites for the K symbol (ID 8)")]
    [SerializeField] private List<Sprite> animSpritesK;
    [Tooltip("Animation sprites for the Q symbol (ID 9)")]
    [SerializeField] private List<Sprite> animSpritesQ;
    [Tooltip("Animation sprites for the J symbol (ID 10)")]
    [SerializeField] private List<Sprite> animSpritesJ;
    [Tooltip("Animation sprites for the 10 symbol (ID 11)")]
    [SerializeField] private List<Sprite> animSpritesTen;
    [Tooltip("Animation sprites for the Dice symbol (ID 12)")]
    [SerializeField] private List<Sprite> animSpritesDice;

    // Internal array of animation sprite lists
    private List<Sprite>[] animationSpriteArrays;

    [Header("Reel Containers")]
    [SerializeField] private Transform[] reelTransforms;

    [Header("Spin Strip Images (fixed symbols; never changed while spinning)")]
    [SerializeField] private List<ReelImages> reelImagesList;

    [Header("Win Animation Layer - SlotAnimHolder")]
    [Tooltip("Optional. When empty, SlotView finds SlotAnimHolder and binds its 5 columns x 3 rows automatically.")]
    [SerializeField] private Transform slotAnimHolder;
    [Tooltip("Animation-only Images for each column. They stay hidden until the matching normal result symbol wins.")]
    [FormerlySerializedAs("resultImagesList"), SerializeField] private List<ReelImages> slotAnimationImages = new List<ReelImages>(5);

    [Header("Spin Settings")]
    [SerializeField] private float symbolHeight = 100f;
    [SerializeField] private float spinSpeed = 2000f;
    [SerializeField] private float spinTravelDistance = 800f;
    [Tooltip("Time for one full top-to-bottom reel pass in Normal mode.")]
    [SerializeField] private float normalSpinCycleDuration = 0.55f;
    [Tooltip("Turbo reel speed relative to Normal mode.")]
    [SerializeField] private float turboSpinSpeedMultiplier = 1.4f;
    [Tooltip("Reel pass duration used only by Quick/Skip Spin mode.")]
    [FormerlySerializedAs("spinCycleDuration")]
    [FormerlySerializedAs("fastSpinCycleDuration")]
    [SerializeField] private float quickSpinCycleDuration = 0.27f;
    [SerializeField] private float reelStartStagger = 0.08f;
    [SerializeField] private float reelStopStagger = 0.12f;
    private const float NormalReelStopStagger = 0.4f;
    private const float TurboReelStopStagger = 0.2f;

    [Header("Win Animation Settings")]
    [SerializeField] private float winPopDuration = 0.4f;
    [SerializeField] private int winPopRepeat = 3;


    [Header("Stop Animation Settings")]
    [Tooltip("Duration of the single continuous reel landing tween to Y = 0.")]
    [SerializeField] private float reelLandingDuration = 0.25f;

    [Header("Quick Spin Settings")]
    [SerializeField] private float quickStopDuration = 0.2f;
    [SerializeField] private int minSpinCyclesBeforeStop = 3;


    [Header("Win Animation Settings")]
    [SerializeField] private float winAnimationDuration = 3.0f; // Total duration each win symbol animation plays
    [SerializeField] private float winSymbolLoopDuration = 1.5f;
    [SerializeField] private int winSymbolLoopCount = 3;
    [Tooltip("Delay between enabling winBox overlay and starting the ImageAnimation - for sync timing")]
    [SerializeField] private float winLineBoxToAnimationDelay = 0.05f;

    [Header("Symbol Info Card")]
    [SerializeField] private SymbolInfoCard symbolInfoCard;


    private float middlePosition = 0f;


    private List<Tween> spinTweens = new List<Tween>();
    private List<Tween> winTweens = new List<Tween>();
    private List<int> reelCycleCount = new List<int>();
    private Coroutine winAnimationCoroutine;


    internal List<List<int>> currentDisplayMatrix;

    private bool isSpinning;

    #region Initialization

    private void Awake()
    {
        BuildSymbolSpriteArray();
        BindSpinStripImages();
        BindResultAndAnimationLayer();
        InitializeReels();
    }

    private void Start()
    {
        if (symbolSprites == null || symbolSprites.Length == 0)
        {
            BuildSymbolSpriteArray();
        }
        DisableAllOverlays();
        SetupSymbolButtons();
    }

    private void DisableAllOverlays()
    {
        StopAnimationOverlays();
        if (symbolInfoCard) symbolInfoCard.HideCard();
    }

    private void SetupSymbolButtons()
    {
        if (reelImagesList == null) return;
        for (int col = 0; col < reelImagesList.Count; col++)
        {
            for (int row = 0; row < 3; row++)
            {
                Image img = GetNormalResultImage(col, row);
                if (img != null)
                {
                    SymbolButtonHandler btnHandler = img.GetComponent<SymbolButtonHandler>();
                    if (btnHandler == null)
                    {
                        btnHandler = img.gameObject.AddComponent<SymbolButtonHandler>();
                    }
                    btnHandler.Init(col, row, this);
                }
            }
        }
    }

    private void BindSpinStripImages()
    {
        if (HasCompleteSpinStrips()) return;

        reelImagesList = new List<ReelImages>(5);
        int columnCount = reelTransforms == null ? 0 : Mathf.Min(5, reelTransforms.Length);

        for (int col = 0; col < columnCount; col++)
        {
            ReelImages strip = new ReelImages();
            strip.images.Clear();
            Transform reelTransform = reelTransforms[col];

            if (reelTransform != null)
            {
                for (int childIndex = 0; childIndex < reelTransform.childCount; childIndex++)
                {
                    Image image = reelTransform.GetChild(childIndex).GetComponent<Image>();
                    if (image != null) strip.images.Add(image);
                }
            }

            reelImagesList.Add(strip);
        }

        if (!HasCompleteSpinStrips())
            Debug.LogError("[SlotView] Each regular reel must contain at least 3 Image children.");
    }

    private bool HasCompleteSpinStrips()
    {
        if (reelImagesList == null || reelImagesList.Count < 5) return false;

        for (int col = 0; col < 5; col++)
        {
            ReelImages strip = reelImagesList[col];
            if (strip == null || strip.images == null || strip.images.Count < 3) return false;
            for (int index = 0; index < strip.images.Count; index++)
                if (strip.images[index] == null) return false;
        }

        return true;
    }

    private void BindResultAndAnimationLayer()
    {
        if (!HasCompleteResultLayer())
        {
            if (slotAnimHolder == null)
            {
                GameObject holderObject = GameObject.Find("SlotAnimHolder");
                if (holderObject != null) slotAnimHolder = holderObject.transform;
            }

            if (slotAnimHolder != null)
            {
                slotAnimationImages = new List<ReelImages>(5);
                int columnCount = Mathf.Min(5, slotAnimHolder.childCount);

                for (int col = 0; col < columnCount; col++)
                {
                    Transform column = slotAnimHolder.GetChild(col);
                    ReelImages resultColumn = new ReelImages();
                    resultColumn.images.Clear();

                    int rowCount = Mathf.Min(3, column.childCount);
                    for (int row = 0; row < rowCount; row++)
                    {
                        resultColumn.images.Add(column.GetChild(row).GetComponent<Image>());
                    }

                    slotAnimationImages.Add(resultColumn);
                }
            }
        }

        if (!HasCompleteResultLayer())
        {
            Debug.LogError("[SlotView] SlotAnimHolder must contain 5 columns with 3 Image children in each column.");
        }
    }

    private bool HasCompleteResultLayer()
    {
        if (slotAnimationImages == null || slotAnimationImages.Count < 5) return false;

        for (int col = 0; col < 5; col++)
        {
            if (slotAnimationImages[col] == null || slotAnimationImages[col].images == null || slotAnimationImages[col].images.Count < 3)
                return false;

            for (int row = 0; row < 3; row++)
                if (slotAnimationImages[col].images[row] == null) return false;
        }

        return true;
    }

    internal void HideSymbolInfoCard()
    {
        if (symbolInfoCard != null) symbolInfoCard.HideCard();
    }

    internal void OnBetChanged()
    {
        if (symbolInfoCard != null && symbolInfoCard.gameObject.activeSelf)
        {
            symbolInfoCard.RefreshCard(gameManager);
        }
    }

    internal void OnSymbolClicked(int col, int row, RectTransform symbolRect)
    {
        if (isSpinning)
        {
            if (symbolInfoCard != null) symbolInfoCard.HideCard();
            return;
        }

        if (currentDisplayMatrix == null || col >= currentDisplayMatrix.Count || row >= currentDisplayMatrix[col].Count)
        {
            return;
        }

        int symbolId = currentDisplayMatrix[col][row];
        if (symbolInfoCard != null)
        {
            symbolInfoCard.ShowCard(symbolId, col, row, symbolRect, gameManager);
        }
    }

    private GameObject GetAnimationObject(int column, int row)
    {
        if (slotAnimationImages == null || column < 0 || column >= slotAnimationImages.Count) return null;

        ReelImages animationColumn = slotAnimationImages[column];
        if (animationColumn == null || animationColumn.images == null || row < 0 || row >= animationColumn.images.Count)
            return null;

        Image animationImage = animationColumn.images[row];
        return animationImage != null ? animationImage.gameObject : null;
    }

    private void SynchronizeWinBoxLoop(GameObject symbolAnimationObject, ImageAnimation symbolAnimation)
    {
        if (symbolAnimationObject == null) return;

        // Each SlotAnimHolder symbol has its WinBox as a child. The symbol
        // completes two loops while the WinBox completes one loop over the
        // same total duration, so both reach a clean boundary together.
        float winBoxLoopDuration = winSymbolLoopDuration * 2f;
        ImageAnimation[] childAnimations = symbolAnimationObject.GetComponentsInChildren<ImageAnimation>(true);

        foreach (ImageAnimation childAnimation in childAnimations)
        {
            if (childAnimation == null || childAnimation == symbolAnimation) continue;

            childAnimation.SetLoopDuration(winBoxLoopDuration);
            childAnimation.doLoopAnimation = true;
            childAnimation.onLoopComplete = null;
        }
    }

    private static List<ImageAnimation> GetWinBoxAnimations(GameObject symbolAnimationObject, ImageAnimation symbolAnimation)
    {
        List<ImageAnimation> winBoxAnimations = new List<ImageAnimation>();
        if (symbolAnimationObject == null) return winBoxAnimations;

        ImageAnimation[] childAnimations = symbolAnimationObject.GetComponentsInChildren<ImageAnimation>(true);
        foreach (ImageAnimation childAnimation in childAnimations)
        {
            if (childAnimation == null || childAnimation == symbolAnimation) continue;
            if (childAnimation.gameObject.activeSelf && childAnimation.HasAnimationFrames())
                winBoxAnimations.Add(childAnimation);
        }

        return winBoxAnimations;
    }

    private List<Sprite> GetLoopingSymbolFrames(int symbolId)
    {
        if (animationSpriteArrays == null || symbolId < 0 || symbolId >= animationSpriteArrays.Length)
            return null;

        List<Sprite> frames = animationSpriteArrays[symbolId];
        if (frames == null || frames.Count == 0) return frames;

        // The serialized animation lists begin with the normal stopped symbol.
        // Showing that duplicate base sprite at every wrap makes a continuous
        // loop look as though it briefly switched off. Exclude it at runtime;
        // the source list and Inspector configuration remain unchanged.
        if (frames.Count > 1 && symbolSprites != null && symbolId < symbolSprites.Length && frames[0] == symbolSprites[symbolId])
            return frames.GetRange(1, frames.Count - 1);

        return frames;
    }

    private void BuildSymbolSpriteArray()
    {
        // Build the symbol sprite array from named sprite fields
        symbolSprites = new Sprite[13];
        symbolSprites[0] = symbolWildSprite;
        symbolSprites[1] = symbolScatterSprite;
        symbolSprites[2] = symbolCarSprite;
        symbolSprites[3] = symbolYachtSprite;
        symbolSprites[4] = symbolMansionSprite;
        symbolSprites[5] = symbolWatchSprite;
        symbolSprites[6] = symbolDiamondSprite;
        symbolSprites[7] = symbolASprite;
        symbolSprites[8] = symbolKSprite;
        symbolSprites[9] = symbolQSprite;
        symbolSprites[10] = symbolJSprite;
        symbolSprites[11] = symbolTenSprite;
        symbolSprites[12] = symbolDiceSprite;

        // Validate
        for (int i = 0; i < symbolSprites.Length; i++)
        {
            if (symbolSprites[i] == null)
            {
                Debug.LogError($"[SlotView] Symbol sprite at index {i} is not assigned in inspector!");
            }
        }

        // Build the animation sprite arrays
        animationSpriteArrays = new List<Sprite>[13];
        animationSpriteArrays[0] = animSpritesWild;
        animationSpriteArrays[1] = animSpritesScatter;
        animationSpriteArrays[2] = animSpritesCar;
        animationSpriteArrays[3] = animSpritesYacht;
        animationSpriteArrays[4] = animSpritesMansion;
        animationSpriteArrays[5] = animSpritesWatch;
        animationSpriteArrays[6] = animSpritesDiamond;
        animationSpriteArrays[7] = animSpritesA;
        animationSpriteArrays[8] = animSpritesK;
        animationSpriteArrays[9] = animSpritesQ;
        animationSpriteArrays[10] = animSpritesJ;
        animationSpriteArrays[11] = animSpritesTen;
        animationSpriteArrays[12] = animSpritesDice;
    }

    private void InitializeReels()
    {
        middlePosition = 0f;

        // Use the real scene spacing so loop boundaries stay aligned with the
        // symbol grid even if the Inspector value is out of date.
        float detectedSymbolPitch = GetSymbolPitch(0);
        if (detectedSymbolPitch > 0f)
            symbolHeight = detectedSymbolPitch;

        int rowCount = (gameManager != null && gameManager.gameConfig != null) ? gameManager.gameConfig.rowCount : 3;

        currentDisplayMatrix = new List<List<int>>();
        for (int col = 0; col < 5; col++)
        {
            var defaultCol = new List<int>();
            for (int r = 0; r < rowCount; r++)
            {
                Image sceneImage = GetNormalResultImage(col, r);
                defaultCol.Add(GetSymbolId(sceneImage != null ? sceneImage.sprite : null));
            }
            currentDisplayMatrix.Add(defaultCol);
            reelCycleCount.Add(0);
        }
    }

    #endregion

    #region Symbol Display

    private void SetResultSymbols(int columnIndex, List<int> visibleSymbolIds)
    {
        if (reelImagesList == null || columnIndex < 0 || columnIndex >= reelImagesList.Count)
        {
            Debug.LogError($"SetResultSymbols: Invalid column index {columnIndex}");
            return;
        }

        int rowCount = (gameManager != null && gameManager.gameConfig != null) ? gameManager.gameConfig.rowCount : 3;

        if (visibleSymbolIds == null || visibleSymbolIds.Count != rowCount)
        {
            Debug.LogError($"SetResultSymbols: Invalid visibleSymbolIds count {visibleSymbolIds?.Count}, expected {rowCount}");
            return;
        }

        for (int row = 0; row < rowCount; row++)
        {
            Image resultImage = GetNormalResultImage(columnIndex, row);
            if (resultImage != null)
            {
                int symbolId = visibleSymbolIds[row];
                resultImage.gameObject.SetActive(true);
                resultImage.sprite = GetSymbolSprite(symbolId);
                resultImage.preserveAspect = true;
                resultImage.transform.localScale = Vector3.one;
                Color color = resultImage.color;
                resultImage.color = new Color(color.r, color.g, color.b, 1f);
            }
        }
    }

    private Image GetNormalResultImage(int column, int row)
    {
        if (reelImagesList == null || column < 0 || column >= reelImagesList.Count) return null;
        ReelImages reel = reelImagesList[column];
        if (reel == null || reel.images == null || row < 0 || row >= 3) return null;

        // The visible result occupies the three cells in the middle of the
        // strip when the reel is stopped at Y = 0. The cells before and after
        // it are only there to keep the strip populated while it is spinning.
        //
        // Using images[row] writes the server result into the first/off-screen
        // cells, so the originally configured middle cells remain visible when
        // the reel lands. Derive the middle section from the strip length so
        // this works for both the old 7-cell strips and the current 13-cell
        // strips.
        const int visibleRowCount = 3;
        int visibleStartIndex = Mathf.Max(0, (reel.images.Count - visibleRowCount) / 2);
        int imageIndex = visibleStartIndex + row;

        return imageIndex < reel.images.Count ? reel.images[imageIndex] : null;
    }

    private void SetAnimationColumnVisible(int column, bool visible)
    {
        if (slotAnimationImages == null || column < 0 || column >= slotAnimationImages.Count) return;
        ReelImages resultColumn = slotAnimationImages[column];
        if (resultColumn == null || resultColumn.images == null) return;

        foreach (Image image in resultColumn.images)
        {
            if (image == null) continue;
            image.DOKill();
            Color color = image.color;
            image.color = new Color(color.r, color.g, color.b, 1f);
            image.gameObject.SetActive(visible);
        }
    }

    private void SetNormalResultVisible(int column, int row, bool visible)
    {
        Image image = GetNormalResultImage(column, row);
        if (image == null) return;

        image.DOKill();
        Color color = image.color;
        image.color = new Color(color.r, color.g, color.b, 1f);
        image.gameObject.SetActive(visible);
    }

    private void StopAnimationOverlays()
    {
        if (slotAnimationImages == null) return;

        for (int col = 0; col < slotAnimationImages.Count; col++)
        {
            ReelImages resultColumn = slotAnimationImages[col];
            if (resultColumn == null || resultColumn.images == null) continue;

            for (int row = 0; row < resultColumn.images.Count; row++)
            {
                Image image = resultColumn.images[row];
                if (image == null) continue;

                ImageAnimation animation = image.GetComponent<ImageAnimation>();
                if (animation != null)
                {
                    animation.onLoopComplete = null;
                    animation.doLoopAnimation = false;
                }

                image.gameObject.SetActive(false);
                animation?.StopAnimation();
                SetNormalResultVisible(col, row, true);
            }
        }
    }

    private Sprite GetSymbolSprite(int symbolId)
    {
        if (symbolSprites == null || symbolSprites.Length == 0)
        {
            BuildSymbolSpriteArray();
        }

        if (symbolSprites == null || symbolSprites.Length == 0)
        {
            Debug.LogError("[SlotView] symbolSprites array is null or empty after BuildSymbolSpriteArray!");
            return null;
        }

        // Validate symbolId range (0-12)
        if (symbolId < 0 || symbolId >= symbolSprites.Length)
        {
            Debug.LogWarning($"[SlotView] Invalid symbolId {symbolId}, using default sprite 0. Total sprites: {symbolSprites.Length}");
            return symbolSprites[0];
        }

        if (symbolSprites[symbolId] == null)
        {
            Debug.LogError($"[SlotView] Symbol sprite for ID {symbolId} is null!");
            return symbolSprites[0];
        }

        return symbolSprites[symbolId];
    }

    private int GetSymbolId(Sprite sprite)
    {
        if (sprite != null && symbolSprites != null)
        {
            for (int symbolId = 0; symbolId < symbolSprites.Length; symbolId++)
            {
                if (symbolSprites[symbolId] == sprite) return symbolId;
            }
        }

        return 0;
    }

    #endregion

    #region Spin Animation

    internal void StartSpin()
    {
        if (isSpinning) return;

        if (symbolInfoCard != null) symbolInfoCard.HideCard();

        isSpinning = true;
        KillAllTweens();

        DisableAllOverlays();

        for (int i = 0; i < reelCycleCount.Count; i++)
        {
            reelCycleCount[i] = 0;
        }

        for (int col = 0; col < 5; col++)
        {
            StartReelCycleWithDelay(col, col * reelStartStagger);
        }
    }

    private void StartReelCycleWithDelay(int columnIndex, float delay)
    {
        if (columnIndex >= reelTransforms.Length) return;

        if (delay <= 0f)
        {
            StartReelCycle(columnIndex);
            return;
        }

        Sequence startSequence = DOTween.Sequence();
        startSequence.AppendInterval(delay);

        startSequence.OnComplete(() => {
            if (isSpinning)
            {
                StartReelCycle(columnIndex);
            }
        });

        startSequence.Play();

        if (spinTweens.Count <= columnIndex)
            spinTweens.Add(startSequence);
        else
            spinTweens[columnIndex] = startSequence;
    }

    private void StartReelCycle(int columnIndex)
    {
        if (columnIndex >= reelTransforms.Length) return;
        if (!isSpinning) return;

        Transform slotTransform = reelTransforms[columnIndex];

        float travel = GetAlignedSpinTravel(columnIndex);
        float configuredCycleDuration = normalSpinCycleDuration;
        if (gameManager != null)
        {
            if (gameManager.currentSpinSpeed == SpinSpeed.Turbo)
                configuredCycleDuration = normalSpinCycleDuration / Mathf.Max(0.01f, turboSpinSpeedMultiplier);
            else if (gameManager.currentSpinSpeed == SpinSpeed.QuickSpin)
                configuredCycleDuration = quickSpinCycleDuration;
        }

        float cycleDuration = configuredCycleDuration > 0f
            ? configuredCycleDuration
            : (travel * 2f) / Mathf.Max(1f, spinSpeed);

        // The strip is presentation-only: move the existing fixed symbols through
        // the mask and restart the transform. No sprite is replaced during a spin.
        slotTransform.localPosition = new Vector3(slotTransform.localPosition.x, middlePosition + travel, 0f);
        Tween cycleTween = slotTransform
            .DOLocalMoveY(middlePosition - travel, cycleDuration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .OnStepComplete(() =>
            {
                if (isSpinning && columnIndex < reelCycleCount.Count)
                    reelCycleCount[columnIndex]++;
            });

        if (spinTweens.Count <= columnIndex)
            spinTweens.Add(cycleTween);
        else
            spinTweens[columnIndex] = cycleTween;
    }

    #endregion

    #region Stop Spin

    internal void StopSpin(List<List<int>> resultMatrix, System.Action onComplete)
    {
        if (!isSpinning)
        {
            currentDisplayMatrix = resultMatrix;
            for (int col = 0; col < 5; col++)
            {
                SetResultSymbols(col, resultMatrix[col]);
                SetAnimationColumnVisible(col, false);
            }
            onComplete?.Invoke();
            return;
        }

        StartCoroutine(StopSpinSequence(resultMatrix, onComplete, false, false));
    }

    internal void StopSpinTogether(List<List<int>> resultMatrix, System.Action onComplete)
    {
        if (!isSpinning)
        {
            currentDisplayMatrix = resultMatrix;
            for (int col = 0; col < 5; col++)
            {
                SetResultSymbols(col, resultMatrix[col]);
                SetAnimationColumnVisible(col, false);
            }
            onComplete?.Invoke();
            return;
        }

        StartCoroutine(StopSpinSequence(resultMatrix, onComplete, false, true));
    }

    private IEnumerator StopSpinSequence(
        List<List<int>> resultMatrix,
        System.Action onComplete,
        bool isQuickStop,
        bool stopTogether)
    {
        currentDisplayMatrix = resultMatrix;
        int requiredSpinCycles = stopTogether ? 0 : (isQuickStop ? 1 : minSpinCyclesBeforeStop);

        while (true)
        {
            bool allReelsReady = true;
            for (int col = 0; col < 5; col++)
            {
                if (reelCycleCount[col] < requiredSpinCycles)
                {
                    allReelsReady = false;
                    break;
                }
            }

            if (allReelsReady) break;
            yield return null;
        }

        // Normal and Turbo use their own fixed reel-stop cadence. Quick/Skip
        // and a manual Stop land all reels together.
        float stagger = (isQuickStop || stopTogether) ? 0f : reelStopStagger;
        bool useFixedNormalStopCadence = !isQuickStop && !stopTogether &&
            gameManager != null &&
            gameManager.currentSpinSpeed == SpinSpeed.Normal;
        bool useFixedTurboStopCadence = !isQuickStop && !stopTogether &&
            gameManager != null &&
            gameManager.currentSpinSpeed == SpinSpeed.Turbo;

        if (useFixedNormalStopCadence)
            stagger = NormalReelStopStagger;
        else if (useFixedTurboStopCadence)
            stagger = TurboReelStopStagger;

        int completedReels = 0;

        for (int col = 0; col < 5; col++)
        {
            float delay = col * stagger;
            bool waitForCycleBoundary = !isQuickStop && !stopTogether &&
                !useFixedNormalStopCadence &&
                !useFixedTurboStopCadence;
            StartCoroutine(StopSingleReel(
                col,
                resultMatrix[col],
                delay,
                isQuickStop,
                waitForCycleBoundary,
                () => completedReels++));
        }

        yield return new WaitUntil(() => completedReels >= 5);

        isSpinning = false;

        onComplete?.Invoke();
    }

    private IEnumerator StopSingleReel(
        int columnIndex,
        List<int> targetSymbols,
        float delay,
        bool isQuickStop,
        bool waitForCycleBoundary,
        System.Action onLandingComplete)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        // Turbo can still finish its current strip pass. Normal mode deliberately
        // skips this variable wait so every reel starts landing at a fixed cadence.
        if (waitForCycleBoundary && columnIndex < reelCycleCount.Count)
        {
            // Capture after the stagger. Capturing before it can make this
            // condition stale and stop the reel in the middle of a pass.
            int cycleCountWhenStopRequested = reelCycleCount[columnIndex];
            yield return new WaitUntil(() => reelCycleCount[columnIndex] > cycleCountWhenStopRequested);
        }

        if (columnIndex < spinTweens.Count && spinTweens[columnIndex] != null)
        {
            spinTweens[columnIndex].Kill();
        }

        Transform slotTransform = reelTransforms[columnIndex];

        // Move the three centered result cells above the visible mask first.
        // Only now is it safe to replace them with the server-authoritative result.
        float offscreenY = middlePosition + GetAlignedSpinTravel(columnIndex);
        slotTransform.localPosition = new Vector3(slotTransform.localPosition.x, offscreenY, 0f);

        SetResultSymbols(columnIndex, targetSymbols);
        SetAnimationColumnVisible(columnIndex, false);

        // ── Play reel-stop sound immediately when symbols lock in ──────────
        // ──────────────────────────────────────────────────────────────────

        if (isQuickStop)
        {
            Tween quickLandingTween = slotTransform
                .DOLocalMoveY(middlePosition, quickStopDuration)
                .SetEase(Ease.OutBack);

            spinTweens[columnIndex] = quickLandingTween;
            yield return quickLandingTween.WaitForCompletion();
        }
        else
        {
            float landingDuration = reelLandingDuration > 0f ? reelLandingDuration : 0.25f;
            Tween landingTween = slotTransform
                .DOLocalMoveY(middlePosition, landingDuration)
                .SetEase(Ease.OutBack);

            spinTweens[columnIndex] = landingTween;
            yield return landingTween.WaitForCompletion();
        }

        // Synchronize the audio impact with the visual landing at Y = 0.
        AudioManager.Instance?.PlayReelStop();

        onLandingComplete?.Invoke();
    }

    private float GetAlignedSpinTravel(int columnIndex)
    {
        float pitch = GetSymbolPitch(columnIndex);
        if (pitch <= 0f)
            pitch = Mathf.Max(1f, symbolHeight);

        float requestedTravel = Mathf.Max(pitch, spinTravelDistance);
        float alignedTravel = Mathf.Round(requestedTravel / pitch) * pitch;
        return Mathf.Max(pitch, alignedTravel);
    }

    private float GetSymbolPitch(int columnIndex)
    {
        if (reelImagesList == null || columnIndex < 0 || columnIndex >= reelImagesList.Count)
            return 0f;

        List<Image> images = reelImagesList[columnIndex]?.images;
        if (images == null || images.Count < 2)
            return 0f;

        RectTransform first = images[0] != null ? images[0].rectTransform : null;
        RectTransform second = images[1] != null ? images[1].rectTransform : null;
        if (first == null || second == null)
            return 0f;

        return Mathf.Abs(first.anchoredPosition.y - second.anchoredPosition.y);
    }

    #endregion

    #region Quick Spin

    internal void QuickStop(List<List<int>> resultMatrix, System.Action onComplete = null)
    {
        if (!isSpinning)
        {
            currentDisplayMatrix = resultMatrix;
            for (int col = 0; col < 5; col++)
            {
                if (col < reelTransforms.Length)
                {
                    SetResultSymbols(col, resultMatrix[col]);
                    SetAnimationColumnVisible(col, false);
                    reelTransforms[col].localPosition = new Vector3(
                        reelTransforms[col].localPosition.x,
                        middlePosition,
                        0
                    );
                }
            }
            
            onComplete?.Invoke();
            return;
        }

        StartCoroutine(StopSpinSequence(resultMatrix, onComplete, true, false));
    }

    #endregion

    #region Stop Symbol Animations

    internal void AnimateAllScatters(int loopCount)
    {
        if (currentDisplayMatrix == null) return;

        // Clear any individual hit animations before starting the collective one
        KillWinTweens();

        int actualScatterId = gameManager?.gameConfig != null ? gameManager.gameConfig.scatterSymbolId : -1;
        if (actualScatterId < 0) return;
        
        for (int col = 0; col < 5; col++)
        {
            for (int row = 0; row < currentDisplayMatrix[col].Count; row++)
            {
                if (currentDisplayMatrix[col][row] == actualScatterId)
                {
                    AnimateSymbolSingleLoop(col, row, loopCount);
                }
            }
        }
    }

    private void AnimateSymbolSingleLoop(int column, int row, int loopCount = 1)
    {
        Image symbolImage = GetNormalResultImage(column, row);
        if (symbolImage == null) return;

        var animGO = GetAnimationObject(column, row);
        if (animGO == null) return;

        ImageAnimation imageAnim = animGO.GetComponent<ImageAnimation>();
        if (imageAnim == null) return;

        int symbolId = currentDisplayMatrix[column][row];
        if (symbolId < 0 || symbolId >= animationSpriteArrays.Length) return;

        List<Sprite> animSprites = animationSpriteArrays[symbolId];
        if (animSprites == null || animSprites.Count == 0) return;

        imageAnim.textureArray = animSprites;
        imageAnim.SetLoopDuration(winSymbolLoopDuration);
        imageAnim.doLoopAnimation = true;

        Sequence seq = DOTween.Sequence();
        
        seq.AppendCallback(() => {
            SetNormalResultVisible(column, row, false);
            animGO.SetActive(true);
            imageAnim.StartAnimation();
        });

        seq.AppendInterval(winSymbolLoopDuration * loopCount);

        seq.AppendCallback(() => {
            if (imageAnim != null)
            {
                imageAnim.doLoopAnimation = false;
                imageAnim.StopAnimation();
            }
            if (animGO != null) animGO.SetActive(false);
            SetNormalResultVisible(column, row, true);
        });

        winTweens.Add(seq);
    }

    #endregion

    #region Win Line Animation

    internal void ShowWinLineAnimation(List<WinLine> winLines, System.Action onComplete)
    {
        if (winLines == null || winLines.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        KillWinTweens();
        winAnimationCoroutine = StartCoroutine(PlayTwoPhaseWinLines(winLines, onComplete));
    }

    private IEnumerator PlayTwoPhaseWinLines(List<WinLine> winLines, System.Action onComplete)
    {
        const int multipleWinLineLoopCount = 2;

        // ==========================================
        // PHASE 1: Show all winning icons at once
        // ==========================================
        HashSet<int> allWinPositions = new HashSet<int>();
        foreach (var winLine in winLines)
        {
            if (winLine.positions != null)
            {
                foreach (int flatIndex in winLine.positions)
                {
                    allWinPositions.Add(flatIndex);
                }
            }
        }

        Debug.Log($"[PlayTwoPhaseWinLines] Phase 1: Showing all {allWinPositions.Count} winning icons at once for {winLines.Count} win lines");

        AudioManager.Instance?.PlayWinLinePhase1Start();

        // A single win line has nothing to transition to. Keep its symbols
        // looping until the next spin stops the win-animation coroutine. The
        // game-completion callback is fired after the first complete loop so
        // autoplay/free-spin progression is not blocked.
        if (winLines.Count == 1)
        {
            yield return AnimateWinPositions(
                allWinPositions,
                1,
                true,
                onComplete);
            yield break;
        }

        List<WinLine> displayWinLines = new List<WinLine>();
        foreach (WinLine winLine in winLines)
        {
            if (winLine.positions != null && winLine.positions.Count > 0)
                displayWinLines.Add(winLine);
        }

        HashSet<int> firstLinePositions = displayWinLines.Count > 0
            ? new HashSet<int>(displayWinLines[0].positions)
            : null;

        // With multiple win lines, first show every winning symbol together
        // for exactly two complete loops. Positions used by the first individual
        // line remain active so they do not restart at the phase transition.
        yield return AnimateWinPositions(
            allWinPositions,
            multipleWinLineLoopCount,
            false,
            null,
            firstLinePositions);

        HideAllWinLineTexts();

        // Invoke onComplete immediately after Phase 1 so game logic (Free Spins / Autoplay / Win complete) can proceed
        onComplete?.Invoke();

        // Skip Phase 2 while the server-authoritative free-spin trigger is being processed.
        bool hasSpecialFeature = gameManager != null && gameManager.lastResult != null &&
            gameManager.lastResult.freeSpinData != null &&
            gameManager.lastResult.freeSpinData.isTriggered;

        bool skipPhase2 = (gameManager != null && (gameManager.isInFreeSpins || gameManager.isAutoPlaying)) || hasSpecialFeature;
        if (skipPhase2)
        {
            KillWinTweens(false);
            yield break;
        }

        if (displayWinLines.Count == 0)
        {
            KillWinTweens(false);
            yield break;
        }

        // ==========================================
        // PHASE 2: Individual Win Line presentation loop
        // ==========================================
        while (true)
        {
            for (int lineIndex = 0; lineIndex < displayWinLines.Count; lineIndex++)
            {
                WinLine winLine = displayWinLines[lineIndex];
                WinLine nextWinLine = displayWinLines[(lineIndex + 1) % displayWinLines.Count];
                HashSet<int> nextLinePositions = new HashSet<int>(nextWinLine.positions);

                HideAllWinLineTexts();

                int firstFlatIndex = winLine.positions[0];
                int firstRow = firstFlatIndex / 5;
                int firstCol = firstFlatIndex % 5;
                ShowWinLineTextOnIcon(firstCol, firstRow, winLine.winAmount);

                // Show each individual win line for exactly two complete loops.
                // Any positions shared with the next line continue uninterrupted.
                yield return AnimateWinPositions(
                    winLine.positions,
                    multipleWinLineLoopCount,
                    false,
                    null,
                    nextLinePositions);
            }
        }
    }

    private IEnumerator AnimateWinPositions(
        IEnumerable<int> flatPositions,
        int loopCountTarget,
        bool continueUntilNextSpin = false,
        System.Action onInitialLoopsComplete = null,
        ISet<int> positionsToKeepActive = null)
    {
        if (flatPositions == null) yield break;

        int rowLimit = (gameManager != null && gameManager.gameConfig != null) ? gameManager.gameConfig.rowCount : 3;
        loopCountTarget = Mathf.Max(1, loopCountTarget);

        List<ImageAnimation> activeAnims = new List<ImageAnimation>();
        List<GameObject> activeAnimationObjects = new List<GameObject>();
        List<Vector2Int> activePositions = new List<Vector2Int>();
        List<int> activeFlatIndices = new List<int>();
        List<List<ImageAnimation>> activeWinBoxAnimations = new List<List<ImageAnimation>>();

        foreach (int flatIndex in flatPositions)
        {
            int row = flatIndex / 5;
            int col = flatIndex % 5;

            if (col < 0 || col >= 5 || row < 0 || row >= rowLimit) continue;

            Image symbolImage = GetNormalResultImage(col, row);
            if (symbolImage == null) continue;

            var animGO = GetAnimationObject(col, row);
            if (animGO == null) continue;

            ImageAnimation imageAnim = animGO.GetComponent<ImageAnimation>();
            if (imageAnim == null) continue;

            SynchronizeWinBoxLoop(animGO, imageAnim);

            if (col >= currentDisplayMatrix.Count || row >= currentDisplayMatrix[col].Count) continue;
            int symbolId = currentDisplayMatrix[col][row];
            if (symbolId < 0 || symbolId >= animationSpriteArrays.Length) continue;

            List<Sprite> animSprites = GetLoopingSymbolFrames(symbolId);
            if (animSprites == null || animSprites.Count == 0) continue;

            imageAnim.textureArray = animSprites;
            imageAnim.SetLoopDuration(winSymbolLoopDuration);
            imageAnim.doLoopAnimation = true;
            imageAnim.onLoopComplete = null;

            activeAnims.Add(imageAnim);
            activeAnimationObjects.Add(animGO);
            activePositions.Add(new Vector2Int(col, row));
            activeFlatIndices.Add(flatIndex);
            activeWinBoxAnimations.Add(GetWinBoxAnimations(animGO, imageAnim));
        }

        // Activate every position before advancing the shared clock so all
        // symbols and WinBoxes begin on the same rendered frame.
        for (int index = 0; index < activeAnims.Count; index++)
        {
            Vector2Int position = activePositions[index];
            SetNormalResultVisible(position.x, position.y, false);
            activeAnimationObjects[index].SetActive(true);
        }

        if (activeAnims.Count > 0)
        {
            for (int index = 0; index < activeAnims.Count; index++)
            {
                activeAnims[index].BeginSynchronizedPlayback();
                foreach (ImageAnimation winBoxAnimation in activeWinBoxAnimations[index])
                {
                    winBoxAnimation.onLoopComplete = null;
                    winBoxAnimation.doLoopAnimation = true;
                    winBoxAnimation.BeginSynchronizedPlayback();
                }
            }

            float phaseDuration = winSymbolLoopDuration * loopCountTarget;
            float winBoxLoopDuration = winSymbolLoopDuration * 2f;
            float elapsedTime = 0f;

            while (elapsedTime < phaseDuration)
            {
                for (int index = 0; index < activeAnims.Count; index++)
                {
                    activeAnims[index].SetSynchronizedTime(elapsedTime, winSymbolLoopDuration);
                    foreach (ImageAnimation winBoxAnimation in activeWinBoxAnimations[index])
                    {
                        winBoxAnimation.SetSynchronizedTime(elapsedTime, winBoxLoopDuration);
                    }
                }

                yield return null;
                elapsedTime += Time.deltaTime;
            }

            // Put every animation on the exact shared boundary in the same frame.
            for (int index = 0; index < activeAnims.Count; index++)
            {
                activeAnims[index].SetSynchronizedTime(phaseDuration, winSymbolLoopDuration);
                foreach (ImageAnimation winBoxAnimation in activeWinBoxAnimations[index])
                {
                    winBoxAnimation.SetSynchronizedTime(phaseDuration, winBoxLoopDuration);
                }
            }
            elapsedTime = phaseDuration;

            onInitialLoopsComplete?.Invoke();

            if (continueUntilNextSpin)
            {
                while (true)
                {
                    yield return null;
                    elapsedTime += Time.deltaTime;

                    for (int index = 0; index < activeAnims.Count; index++)
                    {
                        activeAnims[index].SetSynchronizedTime(elapsedTime, winSymbolLoopDuration);
                        foreach (ImageAnimation winBoxAnimation in activeWinBoxAnimations[index])
                        {
                            winBoxAnimation.SetSynchronizedTime(elapsedTime, winBoxLoopDuration);
                        }
                    }
                }
            }

            // Stop the manual clock for every group together. Shared groups stay
            // visible on frame zero and are picked up by the next line immediately.
            for (int index = 0; index < activeAnims.Count; index++)
            {
                activeAnims[index].EndSynchronizedPlayback();
                foreach (ImageAnimation winBoxAnimation in activeWinBoxAnimations[index])
                {
                    winBoxAnimation.EndSynchronizedPlayback();
                }
            }

            for (int i = 0; i < activeAnims.Count; i++)
            {
                if (positionsToKeepActive != null && positionsToKeepActive.Contains(activeFlatIndices[i]))
                    continue;

                ImageAnimation imageAnim = activeAnims[i];
                imageAnim.onLoopComplete = null;
                imageAnim.doLoopAnimation = false;
                activeAnimationObjects[i].SetActive(false);
                imageAnim.StopAnimation();

                Vector2Int position = activePositions[i];
                SetNormalResultVisible(position.x, position.y, true);
            }
        }
        else
        {
            yield return new WaitForSeconds(winSymbolLoopDuration * loopCountTarget);
            onInitialLoopsComplete?.Invoke();
        }
    }

    private void ShowWinLineTextOnIcon(int col, int row, double winAmount)
    {
        Image symbolImage = GetNormalResultImage(col, row);
        if (symbolImage == null) return;

        Transform textTransform = symbolImage.transform.Find("WinLineText");
        if (textTransform != null)
        {
            var tmpText = textTransform.GetComponent<TMPro.TMP_Text>();
            if (tmpText != null)
            {
                tmpText.text = winAmount.ToString("0.###");
            }
            AnimateTextScaleAppear(textTransform);
        }
    }

    private void HideAllWinLineTexts()
    {
        for (int col = 0; col < 5; col++)
        {
            for (int row = 0; row < 3; row++)
            {
                Image image = GetNormalResultImage(col, row);
                if (image != null)
                {
                    Transform textTransform = image.transform.Find("WinLineText");
                    if (textTransform != null)
                    {
                        textTransform.DOKill();
                        textTransform.localScale = Vector3.one;
                        textTransform.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void AnimateTextScaleAppear(Transform textTransform, float popScale = 1.2f, float durationUp = 0.15f, float durationDown = 0.10f)
    {
        if (textTransform == null) return;
        textTransform.DOKill();
        textTransform.localScale = Vector3.zero;
        textTransform.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(textTransform.DOScale(popScale, durationUp).SetEase(Ease.OutQuad));
        seq.Append(textTransform.DOScale(1.0f, durationDown).SetEase(Ease.InQuad));
        winTweens.Add(seq);
    }
    private void ResetSymbolScale(int col, int row)
    {
        Image resultImage = GetNormalResultImage(col, row);
        if (resultImage != null)
        {
            resultImage.DOKill();
            resultImage.transform.localScale = Vector3.one;
            // Restore alpha to full opacity
            Color c = resultImage.color;
            resultImage.color = new Color(c.r, c.g, c.b, 1f);
        }

        // Also ensure the corresponding animation object is disabled
        var animGO = GetAnimationObject(col, row);
        if (animGO != null)
        {
            ImageAnimation imageAnim = animGO.GetComponent<ImageAnimation>();
            if (imageAnim != null)
            {
                if (imageAnim.rendererDelegate != null) imageAnim.rendererDelegate.DOKill();
                imageAnim.doLoopAnimation = false;
                imageAnim.StopAnimation();
            }
            if (resultImage != null && currentDisplayMatrix != null && col < currentDisplayMatrix.Count && row < currentDisplayMatrix[col].Count)
                resultImage.sprite = GetSymbolSprite(currentDisplayMatrix[col][row]);
            animGO.SetActive(false);
            SetNormalResultVisible(col, row, true);
        }
    }


    private void AnimateWinSymbol(int column, int row)
    {
        Image symbolImage = GetNormalResultImage(column, row);
        if (symbolImage == null)
        {
            Debug.LogError($"[AnimateWinSymbol] Result image is NULL at col: {column}, row: {row}");
            return;
        }



        // Get the animation GameObject for this position
        var animGO = GetAnimationObject(column, row);
        if (animGO == null)
        {
            Debug.LogError($"[AnimateWinSymbol] Animation GameObject is NULL at col: {column}, row: {row}");
            return;
        }

        // Get the ImageAnimation component
        ImageAnimation imageAnim = animGO.GetComponent<ImageAnimation>();
        if (imageAnim == null)
        {
            Debug.LogError($"[AnimateWinSymbol] ImageAnimation component not found on animation object at col: {column}, row: {row}");
            return;
        }

        // Get the current symbol ID at this position
        if (column >= currentDisplayMatrix.Count || row >= currentDisplayMatrix[column].Count)
        {
            Debug.LogError($"[AnimateWinSymbol] Invalid matrix position col: {column}, row: {row}");
            return;
        }

        int symbolId = currentDisplayMatrix[column][row];
        
        // Validate symbolId
        if (symbolId < 0 || symbolId >= animationSpriteArrays.Length)
        {
            Debug.LogError($"[AnimateWinSymbol] Invalid symbolId {symbolId} at col: {column}, row: {row}");
            return;
        }

        // Get the animation sprite array for this symbol
        List<Sprite> animSprites = animationSpriteArrays[symbolId];
        if (animSprites == null || animSprites.Count == 0)
        {
            return;
        }

        // Set the sprite array on the ImageAnimation component
        imageAnim.textureArray = animSprites;
        imageAnim.SetLoopDuration(winSymbolLoopDuration);
        imageAnim.doLoopAnimation = true;

        Sequence seq = DOTween.Sequence();
        
        seq.AppendCallback(() => {
            SetNormalResultVisible(column, row, false);
            animGO.SetActive(true);
        });

        if (winLineBoxToAnimationDelay > 0)
        {
            seq.AppendInterval(winLineBoxToAnimationDelay);
        }

        seq.AppendCallback(() => {
            imageAnim.StartAnimation();
        });

        int loopCount = (gameManager != null && (gameManager.isInFreeSpins || gameManager.isAutoPlaying)) ? 1 : winSymbolLoopCount;
        seq.AppendInterval(winSymbolLoopDuration * loopCount);

        seq.AppendCallback(() => {
            if (imageAnim != null)
            {
                imageAnim.doLoopAnimation = false;
                imageAnim.StopAnimation();
            }
            if (animGO != null) animGO.SetActive(false);
            SetNormalResultVisible(column, row, true);
        });

        winTweens.Add(seq);
    }

    private void KillWinTweens(bool stopCoroutine = true)
    {
        foreach (var tween in winTweens)
        {
            tween?.Kill();
        }
        winTweens.Clear();

        if (stopCoroutine && winAnimationCoroutine != null)
        {
            StopCoroutine(winAnimationCoroutine);
            winAnimationCoroutine = null;
        }

        // Stop all win animations and disable animation GameObjects
        if (slotAnimationImages != null)
        {
            foreach (ReelImages column in slotAnimationImages)
            {
                if (column?.images != null)
                {
                    foreach (Image animationImage in column.images)
                    {
                        GameObject animGO = animationImage != null ? animationImage.gameObject : null;
                        if (animGO != null)
                        {
                            ImageAnimation imageAnim = animGO.GetComponent<ImageAnimation>();
                            if (imageAnim != null)
                            {
                                imageAnim.onLoopComplete = null;
                                imageAnim.doLoopAnimation = false;
                                if (imageAnim.rendererDelegate != null)
                                {
                                    imageAnim.rendererDelegate.DOKill();
                                    Color ac = imageAnim.rendererDelegate.color;
                                    imageAnim.rendererDelegate.color = new Color(ac.r, ac.g, ac.b, 1f);
                                }
                            }
                            animGO.SetActive(false);
                            imageAnim?.StopAnimation();
                        }
                    }
                }
            }
        }

        HideAllWinLineTexts();

        // Restore the normal result cells that may have been hidden while their
        // matching SlotAnimHolder animation was playing.
        for (int col = 0; col < 5; col++)
        {
            for (int row = 0; row < 3; row++)
            {
                Image image = GetNormalResultImage(col, row);
                if (image != null)
                {
                    image.DOKill();
                    image.transform.localScale = Vector3.one;
                    if (currentDisplayMatrix != null && col < currentDisplayMatrix.Count && row < currentDisplayMatrix[col].Count)
                        image.sprite = GetSymbolSprite(currentDisplayMatrix[col][row]);
                    Color c = image.color;
                    image.color = new Color(c.r, c.g, c.b, 1f);
                    image.gameObject.SetActive(true);
                }
            }
        }
    }

    #endregion


    
    internal List<List<int>> GetCurrentDisplayMatrix()
    {
        return currentDisplayMatrix;
    }

    internal bool IsSpinning()
    {
        return isSpinning;
    }

    internal float GetQuickSpinRotationDuration()
    {
        return Mathf.Max(0.01f, quickSpinCycleDuration);
    }


    private void KillAllTweens()
    {
        foreach (var tween in spinTweens)
        {
            tween?.Kill();
        }
        spinTweens.Clear();

        KillWinTweens();
    }

    #region Cleanup

    private void OnDestroy()
    {
        KillAllTweens();
    }

    #endregion
}

[System.Serializable]
public class ReelImages
{
    public List<Image> images = new List<Image>(16);
}
