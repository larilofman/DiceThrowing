using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject scorePanel;
    public GameObject throwPanel;
    public GameObject rethrowPanel;
    public GameObject setupPanel;
    public TextMeshProUGUI scoreTextPrefab;
    public RectTransform scoreTextParent;
    public TextMeshProUGUI totalScoreText;
    int totalScore;
    // Start is called before the first frame update
    void Start()
    {
        ShowThrow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void HideUI()
    {
        throwPanel.gameObject.SetActive(false);
        scorePanel.gameObject.SetActive(false);
        setupPanel.gameObject.SetActive(false);
        rethrowPanel.gameObject.SetActive(false);
    }

    private void ClearScore()
    {
        totalScore = 0;

        foreach (Transform child in scoreTextParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void InsertScore(List<DiceResult> diceResults)
    {
        ClearScore();
        foreach (DiceResult diceResult in diceResults)
        {
            totalScore += diceResult.result;
            string result = diceResult.result.ToString();
            string paddedResult = result.PadRight(5 - result.Length);
            TextMeshProUGUI scoreText = Instantiate(scoreTextPrefab, scoreTextParent);
            scoreText.text = $"<b>{paddedResult}</b> ({diceResult.type})";
            //scoreString += $"<b>{paddedResult}</b> ({diceResult.type})\n";
        }
        totalScoreText.text = $"Total: <b>{totalScore}</b>";
        //scoreText.text = scoreString;
    }
    public void ShowScore(bool hideOthers=false)
    {
        if (hideOthers)
        {
            HideUI();
        }
        scorePanel.gameObject.SetActive(true);       
    }

    public void ShowRethrow()
    {
        rethrowPanel.gameObject.SetActive(true);
    }

    public void ShowSetup()
    {
        HideUI();
        setupPanel.gameObject.SetActive(true);
    }

    public void ShowThrow()
    {
        HideUI();
        throwPanel.gameObject.SetActive(true);
    }
}
