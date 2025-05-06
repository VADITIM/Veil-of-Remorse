using UnityEngine;
using TMPro;
using DG.Tweening;

public class XPPopupManager : MonoBehaviour
{
    [SerializeField] private GameObject xpPopupPrefab; 
    [SerializeField] private Canvas uiCanvas; 
    [SerializeField] private Transform playerTransform;
    private Camera mainCamera;

    private void Awake()
    {
        if (uiCanvas == null)
        {
            uiCanvas = FindObjectOfType<Canvas>();
        }
        
        mainCamera = Camera.main;

        if (playerTransform == null)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public void ShowXPPopup(Vector3 worldPosition, int xpAmount)
    {
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
        
        GameObject popup = Instantiate(xpPopupPrefab, uiCanvas.transform);
        
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.position = screenPosition;

        TextMeshProUGUI textComponent = popup.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = $"+{xpAmount} XP";
        }

        AnimatePopup(rectTransform, textComponent);
    }

    private void AnimatePopup(RectTransform rectTransform, TextMeshProUGUI text)
    {
        Tweener movementTween = rectTransform.DOAnchorPos(Vector2.zero, .3f)
            .SetEase(Ease.OutCubic)
            .SetUpdate(UpdateType.Normal, true);

        movementTween.OnUpdate(() =>
        {
            Vector2 targetPos = uiCanvas.transform.InverseTransformPoint(
                mainCamera.WorldToScreenPoint(playerTransform.position));
            movementTween.ChangeEndValue(targetPos, true);
        });

        Sequence sequence = DOTween.Sequence();

        sequence.Append(rectTransform.DOScale(1f, 0.05f)
            .SetEase(Ease.OutBack));
        sequence.Append(rectTransform.DOScale(.4f, 0.15f)
            .SetEase(Ease.InBack));

        sequence.Insert(0f, text.DOFade(0f, .5f)
            .SetEase(Ease.InQuad));

        movementTween.OnComplete(() => Destroy(rectTransform.gameObject));
    }
}