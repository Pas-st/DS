using UnityEngine;

public class BlackHoleDiagram : MonoBehaviour
{
    [Header("Radii")]
    public float eventHorizon = 1f;
    public float photonSphere = 2f;
    public float accretionDisk = 4f;

    [Header("Colors")]
    public Color eventHorizonColor = Color.black;
    public Color photonColor = new Color(1f, 0.7f, 0.2f);
    public Color diskColor = new Color(0.2f, 0.6f, 1f);

    void Start()
    {
        CreateBlackHole();
    }

    void CreateBlackHole()
    {
        // =========================
        // SINGULARITY (optional visual point)
        // =========================
        CreateDot("Singularity", 0.05f, Color.white);

        // =========================
        // EVENT HORIZON (BLACK CORE)
        // =========================
        CreateRing("EventHorizon", eventHorizon, eventHorizonColor, 0);

        // =========================
        // PHOTON SPHERE (bright ring)
        // =========================
        CreateRing("PhotonSphere", photonSphere, photonColor, 1);

        // =========================
        // ACCRETION DISK (outer glow)
        // =========================
        CreateRing("AccretionDisk", accretionDisk, diskColor, 2);
    }

    void CreateRing(string name, float radius, Color color, int order)
    {
        GameObject ring = new GameObject(name);
        ring.transform.SetParent(transform);
        ring.transform.localPosition = Vector3.zero;

        SpriteRenderer sr = ring.AddComponent<SpriteRenderer>();
        sr.sprite = CreateRingSprite();
        sr.color = color;
        sr.sortingOrder = order;

        ring.transform.localScale = Vector3.one * radius * 2f;
    }

    void CreateDot(string name, float size, Color color)
    {
        GameObject dot = new GameObject(name);
        dot.transform.SetParent(transform);
        dot.transform.localPosition = Vector3.zero;

        SpriteRenderer sr = dot.AddComponent<SpriteRenderer>();
        sr.sprite = CreateCircleSprite();
        sr.color = color;
        sr.sortingOrder = 10;

        dot.transform.localScale = Vector3.one * size;
    }

    Sprite CreateCircleSprite()
    {
        Texture2D tex = new Texture2D(64, 64);

        Vector2 center = new Vector2(32, 32);

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                float dist = Vector2.Distance(center, new Vector2(x, y));
                float alpha = Mathf.Clamp01(1f - dist / 32f);

                tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
            }
        }

        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
    }

    Sprite CreateRingSprite()
    {
        Texture2D tex = new Texture2D(64, 64);

        Vector2 center = new Vector2(32, 32);

        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                float dist = Vector2.Distance(center, new Vector2(x, y));

                float inner = 12f;
                float outer = 28f;

                float alpha =
                    (dist > inner && dist < outer)
                    ? 1f - Mathf.Abs((dist - (inner + outer) / 2f)) / 10f
                    : 0f;

                tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
            }
        }

        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
    }
}