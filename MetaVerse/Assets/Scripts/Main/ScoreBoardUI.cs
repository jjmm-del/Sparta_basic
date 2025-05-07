using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI bestComboText;

    private void OnEnable()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        int bestCombo = PlayerPrefs.GetInt("BestCombo", 0);
        bestScoreText.text = bestScore.ToString();
        bestComboText.text = bestCombo.ToString();
    }
}
