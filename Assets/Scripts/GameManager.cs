using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int numberTotalDangers;  
    private int wrongAnswers = 0;
    private int numberDangerFound = 0;

    [Header("UI End Game")]
    public GameObject endGamePanel;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI dangersText;
    public TMPro.TextMeshProUGUI errorsText;
    public TMPro.TextMeshProUGUI feedbackText;

    [SerializeField] private MonoBehaviour feedbackProviderComponent; //Drag FeedbackUI
    private IFeedbackProvider feedbackProvider;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip endGameSound;

    [Header("Hologram Prefabs")] //au cas ou le outline se voit pas trop
    public GameObject hologramCorrectPrefab;
    public GameObject hologramIncorrectPrefab;

    public GameState CurrentState { get; private set; } = GameState.Intro;

    private HashSet<IRiskSource> registeredRisks = new HashSet<IRiskSource>();

    void Awake()
    {
        feedbackProvider = feedbackProviderComponent as IFeedbackProvider;
        if (feedbackProvider == null)
             Debug.LogError("Feedback provider must implement IFeedbackProvider");

        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    void OnEnable()
    {
        ObjectBehaviour.OnRiskIdentified += HandleRiskFound;
        SelectableObject.OnWrongClick += HandleWrongAnswer;
    }

    void OnDisable()
    {
        ObjectBehaviour.OnRiskIdentified -= HandleRiskFound;
        SelectableObject.OnWrongClick -= HandleWrongAnswer;
    }

    private void HandleWrongAnswer(SelectableObject selectableObject)
    {
        RegisterAnswer(null, false);
        PlayFeedbackSound(false); 
        DisplayFeedbackUI(null); 
        // ShowHologram(selectableObject.transform, false);
    }

    private void HandleRiskFound(IRiskSource riskSource)
    {
        RegisterAnswer(riskSource.RiskData, true);
        DisplayFeedbackUI(riskSource.RiskData); 
        PlayFeedbackSound(true); 
        // ShowHologram(riskSource as MonoBehaviour, true);
    }
       
    public void SetState(GameState newState)
    {
        if(CurrentState == newState)
            return;

        CurrentState = newState;
        Debug.Log("Game State changed to: " + newState);
    }

    public bool IsState(GameState state)
    {
        return CurrentState == state;
    }

    public void RegisterRisk(IRiskSource riskSource)
    {
        if (riskSource.RiskData != null && riskSource.RiskData.isDangerous)
            numberTotalDangers++;
    }

  public void RegisterAnswer(RiskObjectData riskData, bool isCorrect)
    {
        if (IsState(GameState.EndGame))
            return;

        // Bonne réponse ET vrai danger
        if (isCorrect && riskData != null && riskData.isDangerous)
            numberDangerFound++;

        // Toute mauvaise réponse est une erreur
        if (!isCorrect)
            wrongAnswers++;

        CheckEndGame();
    }

    public void DisplayFeedbackUI(RiskObjectData riskData)
    {
        if(riskData != null) 
        {
            Debug.Log($"Feedback: {riskData.explanation}");
            feedbackProvider?.ShowFeedback(riskData.explanation, Color.green);
        } else
        {
            Debug.Log("Feedback: This object does not present a safety risk in this context.");
            feedbackProvider?.ShowFeedback("This object does not present a safety risk in this context.", Color.red);
        }
    }

    public void PlayFeedbackSound(bool isCorrect)
    {
        var audio = audioSource;
        audio.PlayOneShot(isCorrect 
        ? correctSound 
        : wrongSound);
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
          SetState(GameState.EndGame);

        // Base points
        int pointsPerDanger = 25;  // 4 dangers → 100 max
        int penaltyPerFalseAlert = 5;

        int scoreBrut = numberDangerFound * pointsPerDanger - wrongAnswers * penaltyPerFalseAlert;
        int scoreMax = numberTotalDangers * pointsPerDanger;

        // Bonus prudence : pas de fausse alerte
        if (wrongAnswers == 0)
            scoreBrut += 10;

        int note = Mathf.Clamp(Mathf.RoundToInt((float)scoreBrut / scoreMax * 100f), 0, 100);

        Debug.Log("=== END OF GAME ===");
        Debug.Log($"Score : {note} / 100");
        Debug.Log($"Dangers correctly identified : {numberDangerFound} / {numberTotalDangers}");
        Debug.Log($"False alerts : {wrongAnswers}");
        Debug.Log("Score rewards correct identification while also encouraging careful decisions.");

        // Active le panel
        if (endGamePanel != null)
            endGamePanel.SetActive(true);

        // Remplir les textes UI
        if (scoreText != null)
            scoreText.text = $"Score : {note} / 100";

        if (dangersText != null)
            dangersText.text = $"Risks correctly identified : {numberDangerFound} / {numberTotalDangers}";

        if (errorsText != null)
            errorsText.text = $"False alerts : {wrongAnswers}";

        if (feedbackText != null)
            feedbackText.text = "The score values correct identification of dangers while penalizing false alerts, to encourage thoughtful analysis rather than random behavior.";
            
        audioSource.PlayOneShot(endGameSound);
    }

    public void ShowHologram(Transform t, bool isCorrect)
    {
        GameObject prefabToSpawn = isCorrect ? hologramCorrectPrefab : hologramIncorrectPrefab;
        // Récupérer le Renderer du mesh pour connaître ses dimensions
    Renderer rend = t.GetComponent<Renderer>();
    if (rend == null)
    {
        Debug.LogWarning("No Renderer found on object for hologram positioning.");
        return;
    }

    // Calculer la position juste au-dessus du sommet
    float topY = rend.bounds.max.y; // Y maximal du mesh
    Vector3 spawnPosition = new Vector3(rend.bounds.center.x, topY, rend.bounds.center.z);

    // Décaler légèrement au-dessus pour que l'hologram ne chevauche pas le mesh
    spawnPosition += Vector3.up * 0.1f; // 10 cm au-dessus, ajustable

    GameObject hologram = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }
}