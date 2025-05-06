using UnityEngine;
using TMPro;
using DG.Tweening;

public class XPText : MonoBehaviour
{
    public GameObject xpPrefab;
    public TextMeshProUGUI xpText;
    public CanvasGroup canvasGroup;
    public Transform player;

    public void Initialize(int xpAmount, Transform playerTransform)
    {
        xpText = GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        player = playerTransform;

        xpText.text = $"+{xpAmount} XP";

        Animate();
    }

    private void Animate()
    {

        Vector3 targetPosition = player.position + new Vector3(0, 1.5f, 0); 

        transform.DOMove(targetPosition, 1f).SetEase(Ease.OutQuad);
        canvasGroup.DOFade(0, 1f).SetEase(Ease.OutQuad).OnComplete(() => Destroy(gameObject));
    }
}
