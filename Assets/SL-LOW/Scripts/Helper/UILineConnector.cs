using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("UI/UI Line Connector")]
public class UILineConnector : MonoBehaviour
{
    [Header("Connection Points")]
    [SerializeField] private RectTransform startPoint;
    [SerializeField] private RectTransform midPoint;
    [SerializeField] private RectTransform endPoint;

    [Tooltip("Optional list to connect multiple points. If populated, it overrides the individual Start/Mid/End points.")]
    [SerializeField] private List<RectTransform> customPoints = new List<RectTransform>();

    [Header("Line Styling")]
    [Range(1f, 100f)]
    [SerializeField] private float thickness = 5f;
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField] private Sprite lineSprite;
    [SerializeField] private Image.Type imageType = Image.Type.Simple;
    [SerializeField] private Material lineMaterial;

    [Header("Rounded Edges and Corners")]
    [Tooltip("Rounds both ends of every line and all corners where segments meet.")]
    [SerializeField] private bool roundEdgesAndCorners = false;

    [Header("Border")]
    [SerializeField] private bool useBorder = false;
    [SerializeField] private Color borderColor = Color.black;
    [Range(0f, 50f)]
    [SerializeField] private float borderWidth = 2f;

    [Header("Blending Settings")]
    [Range(0f, 1f)]
    [Tooltip("Extends the segment ends slightly beyond the points to overlap and close sharp angle gaps.")]
    [SerializeField] private float overlapExtension = 0.15f;

    [Header("Smooth Anti-Aliasing (Procedural UI Image)")]
    [Tooltip("Use the Procedural UI Image component already in your project to draw perfectly smooth, anti-aliased lines.")]
    [SerializeField] private bool useProceduralImage = true;

    [Header("Update Options")]
    [SerializeField] private bool updateInEditor = true;
    [SerializeField] private bool updateAtRuntime = true;
    [Tooltip("When checked in the Editor Inspector, this component will be removed from the GameObject while leaving spawned child segments intact.")]
    [SerializeField] private bool isDone = false;

    private List<Image> activeSegments = new List<Image>();
    private List<Image> activeBorderSegments = new List<Image>();
    private RectTransform rectTransform;

    public RectTransform StartPoint { get => startPoint; set { startPoint = value; RebuildLine(); } }
    public RectTransform MidPoint { get => midPoint; set { midPoint = value; RebuildLine(); } }
    public RectTransform EndPoint { get => endPoint; set { endPoint = value; RebuildLine(); } }
    public float Thickness { get => thickness; set { thickness = value; RebuildLine(); } }
    public Color LineColor { get => lineColor; set { lineColor = value; UpdateSegmentStyles(); } }
    public Sprite LineSprite { get => lineSprite; set { lineSprite = value; UpdateSegmentStyles(); } }
    public Image.Type ImageType { get => imageType; set { imageType = value; UpdateSegmentStyles(); } }
    public Material LineMaterial { get => lineMaterial; set { lineMaterial = value; UpdateSegmentStyles(); } }
    public bool RoundEdgesAndCorners { get => roundEdgesAndCorners; set { roundEdgesAndCorners = value; RebuildLine(); } }
    public bool UseBorder { get => useBorder; set { useBorder = value; RebuildLine(); } }
    public Color BorderColor { get => borderColor; set { borderColor = value; UpdateSegmentStyles(); } }
    public float BorderWidth { get => borderWidth; set { borderWidth = Mathf.Max(0f, value); RebuildLine(); } }
    public float OverlapExtension { get => overlapExtension; set { overlapExtension = value; RebuildLine(); } }
    public bool UseProceduralImage { get => useProceduralImage; set { useProceduralImage = value; RebuildLine(); } }
    public bool IsDone { get => isDone; set => isDone = value; }

    private void OnEnable()
    {
        if (isDone) return;
        rectTransform = GetComponent<RectTransform>();
        RebuildLine();
    }

    private void Update()
    {
        if (isDone) return;
        if (!Application.isPlaying && !updateInEditor) return;
        if (Application.isPlaying && !updateAtRuntime) return;

        RebuildLine();
    }

    private void OnValidate()
    {
        if (isDone)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this != null)
                {
                    UnityEditor.Undo.DestroyObjectImmediate(this);
                }
            };
#endif
            return;
        }

        RebuildLine();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (isDone) return;
        if (!Application.isPlaying && updateInEditor)
        {
            RebuildLine();
        }
    }
#endif

    public void RebuildLine()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        // Collect active points
        List<RectTransform> pts = GetActivePoints();
        if (pts.Count < 2)
        {
            ClearSegments();
            return;
        }

        int neededSegments = pts.Count - 1;
        EnsureSegmentCount(neededSegments);

        // Position and scale line segments
        for (int i = 0; i < neededSegments; i++)
        {
            RectTransform pA = pts[i];
            RectTransform pB = pts[i + 1];

            if (pA == null || pB == null) continue;

            Vector3 localA = rectTransform.InverseTransformPoint(pA.position);
            Vector3 localB = rectTransform.InverseTransformPoint(pB.position);

            Vector2 direction = localB - localA;
            float distance = direction.magnitude;

            // Rounded segments extend by half their thickness so their end-cap
            // centres remain exactly on the configured connection points.
            float extensionLength = roundEdgesAndCorners
                ? thickness * 0.5f
                : thickness * overlapExtension;

            if (extensionLength > 0f && distance > 0.001f)
            {
                Vector2 dirNormal = direction.normalized;
                localA -= (Vector3)(dirNormal * extensionLength);
                localB += (Vector3)(dirNormal * extensionLength);
                direction = localB - localA;
                distance = direction.magnitude;
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Image img = activeSegments[i];
            RectTransform segRect = img.rectTransform;

            segRect.pivot = new Vector2(0f, 0.5f);
            segRect.anchorMin = new Vector2(0.5f, 0.5f);
            segRect.anchorMax = new Vector2(0.5f, 0.5f);

            segRect.localPosition = localA;
            segRect.localRotation = Quaternion.Euler(0f, 0f, angle);
            segRect.sizeDelta = new Vector2(distance, thickness);
            segRect.localScale = Vector3.one;

            if (useBorder && i < activeBorderSegments.Count && activeBorderSegments[i] != null)
            {
                Image borderImage = activeBorderSegments[i];
                RectTransform borderRect = borderImage.rectTransform;
                Vector2 directionNormal = distance > 0.001f ? direction.normalized : Vector2.right;

                borderRect.pivot = new Vector2(0f, 0.5f);
                borderRect.anchorMin = new Vector2(0.5f, 0.5f);
                borderRect.anchorMax = new Vector2(0.5f, 0.5f);
                borderRect.localPosition = localA - (Vector3)(directionNormal * borderWidth);
                borderRect.localRotation = Quaternion.Euler(0f, 0f, angle);
                borderRect.sizeDelta = new Vector2(
                    distance + borderWidth * 2f,
                    thickness + borderWidth * 2f);
                borderRect.localScale = Vector3.one;
            }
        }

        UpdateSegmentStyles();
    }

    private List<RectTransform> GetActivePoints()
    {
        List<RectTransform> pts = new List<RectTransform>();
        if (customPoints != null && customPoints.Count > 0)
        {
            foreach (var p in customPoints)
            {
                if (p != null) pts.Add(p);
            }
        }
        else
        {
            if (startPoint != null) pts.Add(startPoint);
            if (midPoint != null) pts.Add(midPoint);
            if (endPoint != null) pts.Add(endPoint);
        }
        return pts;
    }

    private void EnsureSegmentCount(int count)
    {
        // Rounded corners require ProceduralImage even when smooth rendering was
        // otherwise disabled, because a standard Image cannot generate capsules.
        bool createProceduralSegments = useProceduralImage || roundEdgesAndCorners;
        System.Type imageComponentType = createProceduralSegments
            ? typeof(UnityEngine.UI.ProceduralImage.ProceduralImage)
            : typeof(Image);

        List<Image> existingSegments = FindExistingSegments("Segment_");
        List<Image> existingBorderSegments = FindExistingSegments("BorderSegment_");

        if (HasComponentTypeMismatch(existingSegments, createProceduralSegments))
        {
            DestroySegments(existingSegments);
            existingSegments.Clear();
        }

        if (HasComponentTypeMismatch(existingBorderSegments, createProceduralSegments))
        {
            DestroySegments(existingBorderSegments);
            existingBorderSegments.Clear();
        }

        ResizeSegmentList(existingSegments, count, "Segment_", imageComponentType);

        int neededBorderSegments = useBorder && borderWidth > 0f ? count : 0;
        ResizeSegmentList(existingBorderSegments, neededBorderSegments, "BorderSegment_", imageComponentType);

        activeSegments = existingSegments;
        activeBorderSegments = existingBorderSegments;

        // Every border must render behind every fill segment. Pair-by-pair
        // ordering would leave visible border seams where two lines intersect.
        for (int i = 0; i < activeBorderSegments.Count; i++)
            activeBorderSegments[i].transform.SetSiblingIndex(i);

        for (int i = 0; i < activeSegments.Count; i++)
            activeSegments[i].transform.SetSiblingIndex(activeBorderSegments.Count + i);
    }

    private List<Image> FindExistingSegments(string namePrefix)
    {
        List<Image> segments = new List<Image>();
        foreach (Transform child in transform)
        {
            Image image = child.GetComponent<Image>();
            if (image != null && child.name.StartsWith(namePrefix))
                segments.Add(image);
        }
        return segments;
    }

    private static bool HasComponentTypeMismatch(List<Image> segments, bool shouldBeProcedural)
    {
        foreach (Image image in segments)
        {
            bool isProcedural = image is UnityEngine.UI.ProceduralImage.ProceduralImage;
            if (isProcedural != shouldBeProcedural) return true;
        }
        return false;
    }

    private void ResizeSegmentList(
        List<Image> segments,
        int targetCount,
        string namePrefix,
        System.Type imageComponentType)
    {
        while (segments.Count > targetCount)
        {
            Image extra = segments[segments.Count - 1];
            segments.RemoveAt(segments.Count - 1);
            DestroySegment(extra);
        }

        while (segments.Count < targetCount)
        {
            GameObject segmentObject = new GameObject(
                $"{namePrefix}{segments.Count}",
                typeof(RectTransform),
                imageComponentType);
            segmentObject.transform.SetParent(transform, false);

            Image image = segmentObject.GetComponent<Image>();
            if (image is UnityEngine.UI.ProceduralImage.ProceduralImage proceduralImage)
            {
                proceduralImage.ModifierType = typeof(UniformModifier);
            }

            segments.Add(image);
        }
    }

    private void DestroySegments(List<Image> segments)
    {
        foreach (Image segment in segments)
            DestroySegment(segment);
    }

    private void DestroySegment(Image segment)
    {
        if (segment == null) return;
        if (Application.isPlaying) Destroy(segment.gameObject);
        else DestroyImmediate(segment.gameObject);
    }

    private void UpdateSegmentStyles()
    {
        if (activeBorderSegments != null)
        {
            foreach (Image borderSegment in activeBorderSegments)
            {
                ApplySegmentStyle(borderSegment, borderColor);
            }
        }

        if (activeSegments != null)
        {
            foreach (Image segment in activeSegments)
                ApplySegmentStyle(segment, lineColor);
        }
    }

    private void ApplySegmentStyle(Image image, Color color)
    {
        if (image == null) return;

        image.color = color;
        image.sprite = lineSprite;
        image.type = imageType;
        image.material = lineMaterial;
        image.raycastTarget = false;

        if (image is UnityEngine.UI.ProceduralImage.ProceduralImage proceduralImage)
        {
            proceduralImage.ModifierType = typeof(UniformModifier);
            UniformModifier uniform = proceduralImage.GetComponent<UniformModifier>();
            if (uniform != null)
            {
                uniform.Radius = roundEdgesAndCorners
                    ? image.rectTransform.sizeDelta.y * 0.5f
                    : 0f;
            }
        }
    }

    private void ClearSegments()
    {
        if (activeSegments != null)
        {
            DestroySegments(activeSegments);
            activeSegments.Clear();
        }

        if (activeBorderSegments != null)
        {
            DestroySegments(activeBorderSegments);
            activeBorderSegments.Clear();
        }
    }

    private void OnDestroy()
    {
        if (!Application.isPlaying && !isDone)
        {
            ClearSegments();
        }
    }
}

