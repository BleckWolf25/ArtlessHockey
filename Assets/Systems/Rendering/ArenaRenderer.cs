using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ArenaRenderer : MonoBehaviour
{
    public float width = 16f;
    public float height = 9f;
    public int resolution = 64;

    private void Start()
    {
        var line = GetComponent<LineRenderer>();
        line.positionCount = resolution + 1;
        line.loop = true;
        line.widthMultiplier = 0.1f;

        for (int i = 0; i <= resolution; i++)
        {
            float angle = i * Mathf.PI * 2f / resolution;
            float x = Mathf.Cos(angle) * width / 2f;
            float y = Mathf.Sin(angle) * height / 2f;
            line.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}