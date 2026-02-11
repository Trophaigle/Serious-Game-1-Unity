using System;
using NUnit.Framework;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
{
    [Header("Risk Data")]
    public RiskObjectData riskData;

    private bool dangerFound = false;

    [Header("Visual")]
    public Renderer objectRenderer;
    public Color highlightColor = Color.yellow;
    private Color originalColor; //couleur d'origine

    [Header("Holograms")]
    public GameObject hologramCorrectPrefab;
    public GameObject hologramIncorrectPrefab;
    public Transform hologramAnchor;
    private GameObject spawnedHologram;

    void Awake()
    {
        if(objectRenderer == null)
            objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }

    //object state a faire

    void Start()
    {
        GameManager.Instance.RegisterRisk(riskData);
    }

    public void Highlight(bool highlight)
    {
        if(dangerFound) return;

        objectRenderer.material.color = highlight ? highlightColor : originalColor;
    }

    public void ValidateAnswer(bool playerSaysDanger)
    {
        if (dangerFound) return;

        dangerFound = true;

        bool isCorrect = (playerSaysDanger == riskData.isDangerous);

        objectRenderer.material.color = isCorrect ? Color.green : Color.red;

        ShowHologram(isCorrect);
        PlayFeedbackSound(isCorrect);
     
        GameManager.Instance.RegisterAnswer(riskData, isCorrect);
    }

    private void PlayFeedbackSound(bool isCorrect)
    {
        var audio = GameManager.Instance.audioSource;
        audio.PlayOneShot(isCorrect 
        ? GameManager.Instance.correctSound 
        : GameManager.Instance.wrongSound);
    }

    private void ShowHologram(bool isCorrect)
    {
        if(spawnedHologram != null)
            Destroy(spawnedHologram);
        
        GameObject prefabToSpawn = isCorrect ? hologramCorrectPrefab : hologramIncorrectPrefab;
        spawnedHologram = Instantiate(prefabToSpawn, hologramAnchor.position, Quaternion.identity);
        
        spawnedHologram.transform.SetParent(hologramAnchor);
    }
}
