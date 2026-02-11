using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int numberTotalDangers;  
    private int wrongAnswers = 0;
    private int numberDangerFound = 0;
    private bool gameEnded = false;

    [Header("UI End Game")]
    public GameObject endGamePanel;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI dangersText;
    public TMPro.TextMeshProUGUI errorsText;
    public TMPro.TextMeshProUGUI feedbackText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip endGameSound;

    private HashSet<RiskObjectData> registeredRisks = new HashSet<RiskObjectData>();

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterRisk(RiskObjectData riskData)
    {
        if(riskData.isDangerous)
         numberTotalDangers++;
    }

    public void RegisterAnswer(RiskObjectData riskData, bool isCorrect)
    {
        if(gameEnded) return;

        if(isCorrect && riskData.isDangerous)
            numberDangerFound++;

        if(!isCorrect)
            wrongAnswers++;

        CheckEndGame();
    }

    private void CheckEndGame()
    {
        if(numberDangerFound >= numberTotalDangers)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameEnded = true;

        int scoreBrut = Mathf.Max(0, (numberDangerFound * 10) - (wrongAnswers * 10));
        int scoreMax = numberTotalDangers * 10;

        int note = 0;
        if (scoreMax > 0)
            note = Mathf.Clamp(Mathf.RoundToInt((float)scoreBrut / scoreMax * 100f), 0, 100);

        Debug.Log("=== FIN DE PARTIE ===");
        Debug.Log($"Score : {note} / 100");
        Debug.Log($"Dangers correctly identified : {numberDangerFound} / {numberTotalDangers}");
        Debug.Log($"False alerts : {wrongAnswers}");
        Debug.Log("The score values correct identification of dangers while penalizing false alerts, to encourage thoughtful analysis rather than random behavior.");

        // Active le panel
        if (endGamePanel != null)
            endGamePanel.SetActive(true);

        // Remplir les textes UI
        if (scoreText != null)
            scoreText.text = $"Score : {note} / 100";

        if (dangersText != null)
            dangersText.text = $"Dangers correclty identified : {numberDangerFound} / {numberTotalDangers}";

        if (errorsText != null)
            errorsText.text = $"False alerts : {wrongAnswers}";

        if (feedbackText != null)
            feedbackText.text = "The score values correct identification of dangers while penalizing false alerts, to encourage thoughtful analysis rather than random behavior.";
            
        audioSource.PlayOneShot(endGameSound);
    }
}