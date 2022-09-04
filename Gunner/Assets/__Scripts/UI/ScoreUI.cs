using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private TMP_Text scoreText;

    private void Awake()
    {
        scoreText = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        StaticEventHandler.OnScoreChanged += StaticEventHandler_OnScoreChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnScoreChanged -= StaticEventHandler_OnScoreChanged;
    }

    private void StaticEventHandler_OnScoreChanged(ScoreChangedArgs scoreChangedArgs)
    {
        scoreText.text = "SCORE: " + scoreChangedArgs.score.ToString("###,###0") + "\nMULTIPLIER: x" + scoreChangedArgs.multiplier;
    }
}
