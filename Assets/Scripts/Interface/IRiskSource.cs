using UnityEngine;

public interface IRiskSource 
{
   RiskObjectData RiskData { get; }
    bool IsDangerIdentified { get; }
    void SetRiskFound();
}
