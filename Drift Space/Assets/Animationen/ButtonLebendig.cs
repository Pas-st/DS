using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Button))]
public class ButtonLebendig : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 startScale;
    private Quaternion startRotation;

    [Header("Wackeln")]
    public float wackelStaerke = 2f;
    public float wackelDauer = 0.25f;

    [Header("Atmen")]
    public float atemStaerke = 0.05f;
    public float atemGeschwindigkeit = 3f;

    private bool mausDrueber = false;
    private bool wackelt = false;

    void Start()
    {
        startScale = transform.localScale;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (mausDrueber)
        {
            float scale = 1 + Mathf.Sin(Time.time * atemGeschwindigkeit) * atemStaerke;
            transform.localScale = startScale * scale;
        }
        else
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                startScale,
                Time.deltaTime * 10f
            );
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mausDrueber = true;

        if (!wackelt)
        {
            StartCoroutine(WackelKurz());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mausDrueber = false;
    }

    IEnumerator WackelKurz()
    {
        wackelt = true;

        float timer = 0f;

        while (timer < wackelDauer)
        {
            float winkel = Mathf.Sin(timer * 40f) * wackelStaerke;
            transform.rotation = startRotation * Quaternion.Euler(0, 0, winkel);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.rotation = startRotation;
        wackelt = false;
    }
}