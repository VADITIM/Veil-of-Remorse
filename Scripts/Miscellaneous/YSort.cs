using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float offset = 0f; 
    private int baseSortingOrder = 1000;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        spriteRenderer.sortingOrder = baseSortingOrder - Mathf.RoundToInt((transform.position.y + offset) * 100);
    }
}
