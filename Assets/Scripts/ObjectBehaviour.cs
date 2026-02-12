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

    public static event Action<IRiskSource> OnRiskIdentified; // Event pour notifier le GameManager

    void Start()
    {
        GameManager.Instance.RegisterRisk(this);
    }
    public void SetRiskFound()
    {
        if (dangerFound) return;

        dangerFound = true;

        OnRiskIdentified?.Invoke(this); // Notifie le GameManager que ce risque a été identifié
        
        // feedback visuel
        SelectableObject selectable = GetComponent<SelectableObject>();
        if (selectable != null){
            selectable.SetResult(true);
        }
    }
}
