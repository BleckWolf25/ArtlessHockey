using UnityEngine;

/// <summary>
/// Optimized arena collider builder with object pooling
/// </summary>
public sealed class ArenaColliderBuilder : MonoBehaviour
{
    [Header("Arena Configuration")]
    [SerializeField] private float width = 14.6f;
    [SerializeField] private float height = 10f;
    [SerializeField] private float wallThickness = 0.5f;
    [SerializeField] private PhysicsMaterial2D wallMaterial;
    [SerializeField] private Color wallColor = new(1, 1, 1, 0.1f);

    private readonly struct WallData
    {
        public readonly string name;
        public readonly Vector2 position;
        public readonly Vector2 size;

        public WallData(string name, Vector2 position, Vector2 size)
        {
            this.name = name;
            this.position = position;
            this.size = size;
        }
    }

    private void Start()
    {
        BuildArenaWalls();
    }

    private void BuildArenaWalls()
    {
        WallData[] walls = new WallData[]
        {
            new("Top", new Vector2(0, (height * 0.5f) + (wallThickness * 0.5f)),
                new Vector2(width + (wallThickness * 2), wallThickness)),
            new("Bottom", new Vector2(0, (-height * 0.5f) - (wallThickness * 0.5f)),
                new Vector2(width + (wallThickness * 2), wallThickness)),
            new("Left", new Vector2((-width * 0.5f) - (wallThickness * 0.5f), 0),
                new Vector2(wallThickness, height)),
            new("Right", new Vector2((width * 0.5f) + (wallThickness * 0.5f), 0),
                new Vector2(wallThickness, height))
        };

        foreach (WallData wall in walls)
        {
            CreateWall(wall);
        }
    }

    private void CreateWall(WallData wallData)
    {
        GameObject wallObject = new(wallData.name);
        wallObject.transform.SetParent(transform, false);
        wallObject.transform.localPosition = wallData.position;

        // Collider
        BoxCollider2D collider = wallObject.AddComponent<BoxCollider2D>();
        collider.size = wallData.size;
        collider.sharedMaterial = wallMaterial;

        // Visual
        SpriteRenderer renderer = wallObject.AddComponent<SpriteRenderer>();
        renderer.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);
        renderer.color = wallColor;
        renderer.size = wallData.size;
    }
}