using UnityEngine;
using UnityEngine.UI;

// AI GENERATED

public class MinimapController : MonoBehaviour
{
    [SerializeField] private float mapSize = 30f; // Size of the minimap in world units
    [SerializeField] private Vector2 displaySize = new Vector2(200, 200); // Size in pixels
    [SerializeField] private Vector2 displayPosition = new Vector2(20, 20); // Position in pixels
    [SerializeField] private float borderSize = 2f;
    
    [SerializeField] private Color backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
    [SerializeField] private Color borderColor = Color.black;
    [SerializeField] private Color wallColor = Color.white;
    [SerializeField] private Color playerColor = Color.green;
    [SerializeField] private Color keyColor = Color.yellow;
    
    [SerializeField] private LayerMask wallLayer; // Layer containing walls
    
    private RawImage minimapDisplay;
    private Transform playerTransform;
    private Texture2D minimapTexture;
    
    void Start()
    {
        // Find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            playerTransform = player.transform;
        }
        
        // Create the minimap texture
        minimapTexture = new Texture2D((int)displaySize.x, (int)displaySize.y);
        minimapTexture.filterMode = FilterMode.Point; // Pixelated look
        
        // Fill with background color
        Color[] colors = new Color[minimapTexture.width * minimapTexture.height];
        for (int i = 0; i < colors.Length; i++) {
            colors[i] = backgroundColor;
        }
        minimapTexture.SetPixels(colors);
        minimapTexture.Apply();
        
        // Create UI elements
        CreateMinimapUI();
    }
    
    void LateUpdate()
    {
        if (playerTransform == null || minimapTexture == null) return;
        
        // Clear the texture with background color
        Color[] clearColors = new Color[minimapTexture.width * minimapTexture.height];
        for (int i = 0; i < clearColors.Length; i++) {
            clearColors[i] = backgroundColor;
        }
        minimapTexture.SetPixels(clearColors);
        
        // Draw walls as outlines
        DrawWalls();
        
        // Draw keys
        DrawKeys();
        
        // Draw player last so it's always visible
        DrawPlayer();
        
        // Apply all changes to the texture
        minimapTexture.Apply();
    }
    
    private void CreateMinimapUI()
    {
        // Get or create canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        // Create border
        GameObject borderObj = new GameObject("Minimap Border");
        borderObj.transform.SetParent(canvas.transform, false);
        Image border = borderObj.AddComponent<Image>();
        border.color = borderColor;
        
        RectTransform borderRect = border.GetComponent<RectTransform>();
        borderRect.anchorMin = new Vector2(0, 1);
        borderRect.anchorMax = new Vector2(0, 1);
        borderRect.pivot = new Vector2(0, 1);
        borderRect.sizeDelta = displaySize + new Vector2(borderSize * 2, borderSize * 2);
        borderRect.anchoredPosition = new Vector2(displayPosition.x - borderSize, -displayPosition.y + borderSize);
        
        // Create minimap display
        GameObject minimapObj = new GameObject("Minimap Display");
        minimapObj.transform.SetParent(canvas.transform, false);
        minimapDisplay = minimapObj.AddComponent<RawImage>();
        minimapDisplay.texture = minimapTexture;
        
        RectTransform minimapRect = minimapDisplay.GetComponent<RectTransform>();
        minimapRect.anchorMin = new Vector2(0, 1);
        minimapRect.anchorMax = new Vector2(0, 1);
        minimapRect.pivot = new Vector2(0, 1);
        minimapRect.sizeDelta = displaySize;
        minimapRect.anchoredPosition = displayPosition * -1;
    }
    
    private void DrawPlayer()
    {
        // Draw player as a green point at the center
        int centerX = minimapTexture.width / 2;
        int centerY = minimapTexture.height / 2;
        
        // Draw a 3x3 square for the player
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                SetPixelSafe(centerX + x, centerY + y, playerColor);
            }
        }
    }
    
    private void DrawKeys()
    {
        // Find all keys in the scene
        GameObject[] keys = GameObject.FindGameObjectsWithTag("Key");
        
        foreach (GameObject key in keys) {
            // Calculate relative position to player
            Vector2 relativePos = key.transform.position - playerTransform.position;
            
            // Skip if outside of minimap range
            if (Mathf.Abs(relativePos.x) > mapSize || Mathf.Abs(relativePos.y) > mapSize) 
                continue;
                
            // Convert world position to minimap coordinates
            int keyX = minimapTexture.width / 2 + (int)(relativePos.x * minimapTexture.width / (mapSize * 2));
            int keyY = minimapTexture.height / 2 + (int)(relativePos.y * minimapTexture.height / (mapSize * 2));
            
            // Draw a 3x3 square for the key
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    SetPixelSafe(keyX + x, keyY + y, keyColor);
                }
            }
        }
    }
    
    private void DrawWalls()
    {
        // Get the player's position
        Vector3 playerPos = playerTransform.position;
        
        // Calculate the world bounds of our minimap view
        float halfMapSize = mapSize;
        Bounds mapBounds = new Bounds(playerPos, new Vector3(halfMapSize * 2, halfMapSize * 2, 10));
        
        // Find all walls near the player
        Collider2D[] walls = Physics2D.OverlapBoxAll(
            playerPos, 
            new Vector2(halfMapSize * 2, halfMapSize * 2),
            0, 
            wallLayer
        );
        
        foreach (Collider2D wall in walls) {
            // Get the bounds of the wall
            Bounds wallBounds = wall.bounds;
            
            // Convert world bounds to minimap coordinates
            int minX = WorldToMinimapX(wallBounds.min.x, playerPos.x);
            int maxX = WorldToMinimapX(wallBounds.max.x, playerPos.x);
            int minY = WorldToMinimapY(wallBounds.min.y, playerPos.y);
            int maxY = WorldToMinimapY(wallBounds.max.y, playerPos.y);
            
            // Draw wall outline
            DrawLine(minX, minY, maxX, minY, wallColor); // Bottom edge
            DrawLine(maxX, minY, maxX, maxY, wallColor); // Right edge
            DrawLine(maxX, maxY, minX, maxY, wallColor); // Top edge
            DrawLine(minX, maxY, minX, minY, wallColor); // Left edge
        }
    }
    
    private int WorldToMinimapX(float worldX, float playerX)
    {
        float relativeX = worldX - playerX;
        return minimapTexture.width / 2 + (int)(relativeX * minimapTexture.width / (mapSize * 2));
    }
    
    private int WorldToMinimapY(float worldY, float playerY)
    {
        float relativeY = worldY - playerY;
        return minimapTexture.height / 2 + (int)(relativeY * minimapTexture.height / (mapSize * 2));
    }
    
    // Bresenham line algorithm for drawing wall outlines
    private void DrawLine(int x0, int y0, int x1, int y1, Color color)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;
        
        while (true) {
            SetPixelSafe(x0, y0, color);
            
            if (x0 == x1 && y0 == y1) break;
            
            int e2 = 2 * err;
            if (e2 > -dy) {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx) {
                err += dx;
                y0 += sy;
            }
        }
    }
    
    private void SetPixelSafe(int x, int y, Color color)
    {
        // Only set pixels that are within the texture bounds
        if (x >= 0 && x < minimapTexture.width && y >= 0 && y < minimapTexture.height) {
            minimapTexture.SetPixel(x, y, color);
        }
    }
}
