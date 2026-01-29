using System;
using NUnit.Framework;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour
{
    [Header("Danger Info")]
    [SerializeField] public bool isDangerous;

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

    public void Highlight(bool highlight)
    {
        if(dangerFound) return;

        if(highlight)
            objectRenderer.material.color = highlightColor;
        else
            objectRenderer.material.color = originalColor;
    }

    public void ValidateAnswer(bool playerSaysDanger)
    {
        if (dangerFound)
            return;

        dangerFound = true;

        bool isCorrect = (playerSaysDanger == isDangerous);
        
        if(isCorrect)
            objectRenderer.material.color = Color.green;
        else
            objectRenderer.material.color = Color.red;
        ShowHologram(isCorrect);

        GameManager.Instance.RegisterAnswer(isCorrect, isDangerous);
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
