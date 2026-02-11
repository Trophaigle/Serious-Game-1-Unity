using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Objects/ObjectData")]
public class RiskObjectData : ScriptableObject
{
    public string riskName;
    public bool isDangerous;

    [TextArea]
    public string explanation;
}
