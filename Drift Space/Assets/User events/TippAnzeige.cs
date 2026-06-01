using UnityEngine;
using TMPro;

public class TippAnzeige : MonoBehaviour
{
    public TMP_Text tippText;

    private string[] tipps =
    {
        "W = Boost",
        "A = Nach links drehen",
        "D = Nach rechts drehen",
        "Folge dem grünen Pfeil",
        "Berühre nicht die blaue Wand"
    };

    private int aktuellerTipp = 0;

    void Start()
    {
        InvokeRepeating(nameof(WechsleTipp), 0f, 3f);
    }

    void WechsleTipp()
    {
        tippText.text = tipps[aktuellerTipp];

        aktuellerTipp++;

        if (aktuellerTipp >= tipps.Length)
        {
            aktuellerTipp = 0;
        }
    }
}