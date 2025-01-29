using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreTextBlue;
    [SerializeField] private TMP_Text scoreTextRed;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text periodText;

    [SerializeField] private MatchScript matchScript;

    private void Update()
    {
        scoreTextBlue.text = matchScript.BlueScore.ToString();
        scoreTextRed.text = matchScript.RedScore.ToString();
        timeText.text = "0:" + ((int)matchScript.CurrentTime).ToString();
        periodText.text = matchScript.PeriodCount.ToString();
    }
}