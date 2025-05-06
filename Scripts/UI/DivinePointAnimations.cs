using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class DivinePointAnimations : MonoBehaviour
{
    [Header("Horizontal Slide Buttons")]
    [SerializeField] private GameObject OfferEssenceButton;
    [SerializeField] private GameObject KindlePowerButton;
    [SerializeField] private GameObject LeaveButton;
    
    [Header("Vertical Slide Elements")]
    [SerializeField] private GameObject VerticalElement1;
    [SerializeField] private GameObject VerticalElement2;
    [SerializeField] private TextMeshProUGUI VerticalTMPText;
    
    [Header("Animation Settings - Horizontal")]
    [SerializeField] private float initialOffscreenX = -1913f;
    [SerializeField] private float animationDuration = .4f;
    [SerializeField] private float delayBetweenButtons = .2f;
    [SerializeField] private int bounces = 3;
    [SerializeField] private float bounceStrength = 1.2f;
    
    [Header("Animation Settings - Vertical")]
    [SerializeField] private float initialOffscreenYBottom = 150f;
    [SerializeField] private float initialOffscreenYTop = 150f;
    [SerializeField] private float verticalAnimDuration = 0.5f;
    [SerializeField] private float verticalDelayBetween = 0.15f;
    
    private Vector3 offerEssenceOriginalPos;
    private Vector3 kindlePowerOriginalPos;
    private Vector3 leaveOriginalPos;
    
    private Vector3 vertElement1OriginalPos;
    private Vector3 vertElement2OriginalPos;
    private float tmpTextOriginalY;
    
    void Awake()
    {
        offerEssenceOriginalPos = OfferEssenceButton.transform.localPosition;
        kindlePowerOriginalPos = KindlePowerButton.transform.localPosition;
        leaveOriginalPos = LeaveButton.transform.localPosition;
        
        // Store vertical element positions
        if (VerticalElement1 != null)
            vertElement1OriginalPos = VerticalElement1.transform.localPosition;
        
        if (VerticalElement2 != null)
            vertElement2OriginalPos = VerticalElement2.transform.localPosition;
        
        if (VerticalTMPText != null)
            tmpTextOriginalY = VerticalTMPText.rectTransform.anchoredPosition.y;
    }
    
    public void SetInitialPositions()
    {
        // Set horizontal elements to offscreen position
        OfferEssenceButton.transform.localPosition = new Vector3(initialOffscreenX, offerEssenceOriginalPos.y, offerEssenceOriginalPos.z);
        KindlePowerButton.transform.localPosition = new Vector3(initialOffscreenX, kindlePowerOriginalPos.y, kindlePowerOriginalPos.z);
        LeaveButton.transform.localPosition = new Vector3(initialOffscreenX, leaveOriginalPos.y, leaveOriginalPos.z);
        
        // Set vertical elements to offscreen position
        if (VerticalElement1 != null)
            VerticalElement1.transform.localPosition = new Vector3(vertElement1OriginalPos.x, initialOffscreenYBottom, vertElement1OriginalPos.z);
        
        if (VerticalElement2 != null)
            VerticalElement2.transform.localPosition = new Vector3(vertElement2OriginalPos.x, initialOffscreenYBottom, vertElement2OriginalPos.z);
        
        if (VerticalTMPText != null)
        {
            Vector2 currentPos = VerticalTMPText.rectTransform.anchoredPosition;
            VerticalTMPText.rectTransform.anchoredPosition = new Vector2(currentPos.x, initialOffscreenYTop);
        }
    }

    public void AnimateButtonsIn()
    {
        // Reset to initial positions
        SetInitialPositions();

        // Animate horizontal elements
        OfferEssenceButton.transform.DOLocalMoveX(offerEssenceOriginalPos.x, animationDuration)
            .SetDelay(0)
            .SetEase(Ease.OutBack, bounceStrength)
            .SetUpdate(true);
        
        KindlePowerButton.transform.DOLocalMoveX(kindlePowerOriginalPos.x, animationDuration)
            .SetDelay(delayBetweenButtons)
            .SetEase(Ease.OutBack, bounceStrength)
            .SetUpdate(true);
        
        LeaveButton.transform.DOLocalMoveX(leaveOriginalPos.x, animationDuration)
            .SetDelay(delayBetweenButtons * 2)
            .SetEase(Ease.OutBack, bounceStrength)
            .SetUpdate(true);
            
        if (VerticalElement1 != null)
        {
            VerticalElement1.transform.DOLocalMoveY(vertElement1OriginalPos.y, verticalAnimDuration)
                .SetDelay(0)
                .SetEase(Ease.OutBack, bounceStrength)
                .SetUpdate(true);
        }
        
        if (VerticalElement2 != null)
        {
            VerticalElement2.transform.DOLocalMoveY(vertElement2OriginalPos.y, verticalAnimDuration)
                .SetDelay(verticalDelayBetween)
                .SetEase(Ease.OutBack, bounceStrength)
                .SetUpdate(true);
        }
        
        if (VerticalTMPText != null)
        {
            VerticalTMPText.rectTransform.DOAnchorPosY(tmpTextOriginalY, verticalAnimDuration)
                .SetDelay(verticalDelayBetween * 2)
                .SetEase(Ease.OutBack, bounceStrength)
                .SetUpdate(true);
        }
    }
    
    public void AnimateButtonsOut()
    {
        // Animate horizontal elements out
        OfferEssenceButton.transform.DOLocalMoveX(initialOffscreenX, animationDuration * 0.6f)
            .SetDelay(delayBetweenButtons * 2)
            .SetEase(Ease.InBack)
            .SetUpdate(true);
        
        KindlePowerButton.transform.DOLocalMoveX(initialOffscreenX, animationDuration * 0.6f)
            .SetDelay(delayBetweenButtons)
            .SetEase(Ease.InBack)
            .SetUpdate(true);
        
        LeaveButton.transform.DOLocalMoveX(initialOffscreenX, animationDuration * 0.6f)
            .SetDelay(0)
            .SetEase(Ease.InBack)
            .SetUpdate(true);
            
        // Animate vertical elements out
        if (VerticalElement1 != null)
        {
            VerticalElement1.transform.DOLocalMoveY(initialOffscreenYBottom, verticalAnimDuration * 0.6f)
                .SetDelay(verticalDelayBetween * 2)
                .SetEase(Ease.InBack)
                .SetUpdate(true);
        }
        
        if (VerticalElement2 != null)
        {
            VerticalElement2.transform.DOLocalMoveY(initialOffscreenYBottom, verticalAnimDuration * 0.6f)
                .SetDelay(verticalDelayBetween)
                .SetEase(Ease.InBack)
                .SetUpdate(true);
        }
        
        if (VerticalTMPText != null)
        {
            VerticalTMPText.rectTransform.DOAnchorPosY(initialOffscreenYTop, verticalAnimDuration * 0.6f)
                .SetDelay(0)
                .SetEase(Ease.InBack)
                .SetUpdate(true);
        }
    }
}