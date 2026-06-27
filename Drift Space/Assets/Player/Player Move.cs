using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Move : MonoBehaviour
{
    public bool levelComplete = false;
    public GameObject finish;
    public GameObject finishCanvas;
    public bool goAfterFinishToMenu = true;


    [Header("Movement")]
    private Rigidbody2D rb;
    public float thrust = 2f;
    public float maxSpeed = 15f;
    public float currentSpeed = 0f;
    public float actualSpeed = 0f;
    public float rotationSpeed = 200f;


    [Header("Particle Trail Bézier Positions")]
    public Vector2 bezierStartOffset = new Vector2(0.3f, -0.65f);
    public Vector2 bezierControlOffset = new Vector2(0, -0.23f);
    public Vector2 bezierEndOffset = new Vector2(-0.3f, -0.65f);
    private float trailTimer = 0f;
    private Sprite squareSprite;


    [Header("Debug")]
    public bool drawLine = false;
    private List<Vector2> hitPoints = new List<Vector2>();
    private LineRenderer lineRenderer;

    public int currentHealth = 100;

    public bool useKeyInput = true;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject lineObj = new GameObject("HitTrace");
        lineRenderer = lineObj.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 0;

        CreateSquareSprite();
    }

    void CreateSquareSprite()
    {
        int size = 16;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color32[] pixels = new Color32[size * size];
        Color32 color = new Color32(255, 255, 255, 255);
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;
        texture.SetPixels32(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        squareSprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
    }

    Vector2 GetQuadraticBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float u = 1f - t;
        return u * u * p0 + 2f * u * t * p1 + t * t * p2;
    }

    void SpawnBezierSquares()
    {
        Vector2 start =
            (Vector2)transform.position +
            (Vector2)(transform.rotation * bezierStartOffset);

        Vector2 control =
            (Vector2)transform.position +
            (Vector2)(transform.rotation * bezierControlOffset);

        Vector2 end =
            (Vector2)transform.position +
            (Vector2)(transform.rotation * bezierEndOffset);

        int particleCount = 2;

        for (int i = 0; i < particleCount; i++)
        {
            float t = Random.value;

            // Punkt auf der Bézier-Kurve
            Vector2 curvePoint = GetQuadraticBezierPoint(t, start, control, end);

            // Zufälliger Punkt auf der geraden Verbindung zwischen start und end
            Vector2 linePoint = Vector2.Lerp(start, end, Random.value);

            // Zufälliger Punkt INNERHALB der Form
            Vector2 spawnPos = Vector2.Lerp(linePoint, curvePoint, Random.value);

            GameObject square = new GameObject("TrailParticle");

            square.transform.position = spawnPos;

            square.transform.rotation =
                Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

            // Zufällige Größe
            float randomSize = Random.Range(0.1f, 0.5f);
            square.transform.localScale = Vector3.one * randomSize;

            SpriteRenderer renderer = square.AddComponent<SpriteRenderer>();
            renderer.sprite = squareSprite;

            // Zufällige Rot-Töne
            float red = Random.Range(0.7f, 1f);
            float green = Random.Range(0f, 0.25f);
            float blue = Random.Range(0f, 0.1f);

            renderer.color = new Color(red, green, blue, 1f);
            renderer.sortingOrder = 1;

            StartCoroutine(FadeToBlackAndDestroy(renderer, square));
        }
    }

    System.Collections.IEnumerator FadeToBlackAndDestroy(
    SpriteRenderer renderer,
    GameObject obj)
    {
        float duration = 1f;

        Color startColor = renderer.color;
        Color targetColor = Color.black;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            renderer.color = Color.Lerp(
                startColor,
                targetColor,
                time / duration);

            yield return null;
        }

        Destroy(obj);
    }

    void Update()
    {
        UpdateLine();

        if (levelComplete)
        {
            finishCanvas.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (goAfterFinishToMenu)
                {
                    SceneManager.LoadScene(0);
                }
                else
                {
                    SceneManager.LoadScene(1);
                }
            }
            return;
        }
        if (useKeyInput)
            HandleInput();
        
        actualSpeed = rb.linearVelocity.magnitude;
    }

    void HandleInput()
    {
        float rotateInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            rotateInput = 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotateInput = -1f;
        }

        rb.angularVelocity = rotateInput * rotationSpeed;

        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed += thrust * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

            rb.AddForce(transform.up * thrust);

            // Speed hart begrenzen
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);

            trailTimer += Time.deltaTime;
            if (trailTimer >= 0.01f)
            {
                trailTimer = 0f;
                SpawnBezierSquares();
            }
        }
        else
        {
            currentSpeed = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contactCount > 0)
        {
            Vector2 hitPoint = collision.GetContact(0).point;

            hitPoints.Add(hitPoint);
            if (!levelComplete)
            {
                currentHealth -= actualSpeed > 0 ? Mathf.RoundToInt(actualSpeed) : 1;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == finish)
        {
            levelComplete = true;
        }
    }

    void UpdateLine()
    {
        if (!drawLine || hitPoints.Count < 2)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = hitPoints.Count;

        for (int i = 0; i < hitPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, hitPoints[i]);
        }
    }

    void OnDrawGizmos()
    {
        Vector3 start =
            transform.position +
            transform.rotation * (Vector3)bezierStartOffset;

        Vector3 control =
            transform.position +
            transform.rotation * (Vector3)bezierControlOffset;

        Vector3 end =
            transform.position +
            transform.rotation * (Vector3)bezierEndOffset;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(start, 0.08f);
        Gizmos.DrawSphere(control, 0.08f);
        Gizmos.DrawSphere(end, 0.08f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(start, control);
        Gizmos.DrawLine(control, end);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(start, end);

        Vector3 previousPoint = start;
        int resolution = 20;
        for (int i = 1; i <= resolution; i++)
        {
            float t = (float)i / resolution;
            Vector3 currentPoint = GetQuadraticBezierPoint(t, start, control, end);
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }
}