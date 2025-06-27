using UnityEngine;

/// <summary>
/// Procedural sprite renderer with pooling and modern Unity practices
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public abstract class ProceduralRenderer : MonoBehaviour
{
    [Header("Rendering")]
    [SerializeField] protected Color color = Color.white;
    [SerializeField] protected float scale = 1f;
    [SerializeField] protected string spriteName = "CircleSprite";

    protected SpriteRenderer spriteRenderer;
    protected static readonly System.Collections.Generic.Dictionary<string, Sprite> spriteCache = new();

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitializeRenderer();
    }

    protected virtual void InitializeRenderer()
    {
        Sprite sprite = GetOrCreateSprite();
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        transform.localScale = Vector3.one * scale;
    }

    protected Sprite GetOrCreateSprite()
    {
        if (spriteCache.TryGetValue(spriteName, out Sprite cachedSprite))
        {
            return cachedSprite;
        }

        Sprite sprite = Resources.Load<Sprite>(spriteName);
        if (sprite == null)
        {
            sprite = CreateDefaultSprite();
            Debug.LogWarning($"[{GetType().Name}] {spriteName} not found, using default");
        }

        spriteCache[spriteName] = sprite;
        return sprite;
    }

    protected virtual Sprite CreateDefaultSprite()
    {
        return Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);
    }

    public void SetColor(Color newColor)
    {
        color = newColor;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    public void SetScale(float newScale)
    {
        scale = newScale;
        transform.localScale = Vector3.one * scale;
    }
}

