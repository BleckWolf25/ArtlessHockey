using UnityEngine;

public class ArenaColliderBuilder : MonoBehaviour
{
    public float width = 16f;
    public float height = 9f;
    public float wallThickness = 0.5f;

    private void Start()
    {
        CreateWall("Top", new Vector2(0, height / 2 + wallThickness / 2), new Vector2(width + wallThickness * 2, wallThickness));
        CreateWall("Bottom", new Vector2(0, -height / 2 - wallThickness / 2), new Vector2(width + wallThickness * 2, wallThickness));
        CreateWall("Left", new Vector2(-width / 2 - wallThickness / 2, 0), new Vector2(wallThickness, height));
        CreateWall("Right", new Vector2(width / 2 + wallThickness / 2, 0), new Vector2(wallThickness, height));
    }

    private void CreateWall(string name, Vector2 position, Vector2 size)
    {
        var wall = new GameObject(name);
        wall.transform.parent = transform;
        wall.transform.position = transform.position + (Vector3)position;

        var box = wall.AddComponent<BoxCollider2D>();
        box.size = size;

        var sr = wall.AddComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);
        sr.color = new Color(1, 1, 1, 0.1f); // Transparent white

        wall.layer = LayerMask.NameToLayer("Default");

        // Assign a physics material to the collider
        box.sharedMaterial = Resources.Load<PhysicsMaterial2D>("PuckMaterial");
    }
}