using System;
using NUnit.Framework;
using UnityEngine;

public class ObjectBehaviour : MonoBehaviour, IRiskSource
{
    [Header("Risk Data")]
    public RiskObjectData riskData;

    private bool dangerFound = false;

    // ===== IRiskSource =====
    public RiskObjectData RiskData => riskData;
    public bool IsDangerIdentified => dangerFound;

   /* [Header("Holograms")]
    public GameObject hologramCorrectPrefab;
    public GameObject hologramIncorrectPrefab;
    public Transform hologramAnchor;
    private GameObject spawnedHologram;*/

   /* void Awake()
    {
        if(objectRenderer == null)
            objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }
*/
    //object state a faire

    void Start()
    {
        GameManager.Instance.RegisterRisk(this);
    }
    public void SetRiskFound()
    {
        if (dangerFound) return;

        dangerFound = true;

        // feedback visuel
        SelectableObject selectable = GetComponent<SelectableObject>();
        if (selectable != null){
            selectable.SetResult(true);
            //  ShowHologram(isCorrect);
            GameManager.Instance.PlayFeedbackSound(true);
     
          GameManager.Instance.RegisterAnswer(riskData, true);
          GameManager.Instance.DisplayFeedbackUI(riskData);
         // GameManager.Instance.ShowHologram(transform, true); 
        }
    }

   /* private void ShowHologram(bool isCorrect)
    {
        if(spawnedHologram != null)
            Destroy(spawnedHologram);
        
        GameObject prefabToSpawn = isCorrect ? hologramCorrectPrefab : hologramIncorrectPrefab;
        spawnedHologram = Instantiate(prefabToSpawn, hologramAnchor.position, Quaternion.identity);
        
        spawnedHologram.transform.SetParent(hologramAnchor);
    }*/
}
