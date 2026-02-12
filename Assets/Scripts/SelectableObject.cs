using UnityEngine;

public class SelectableObject : MonoBehaviour, IClickable, IHoverable
{
    private Outline outline;
    private bool locked = false;

    [Header("Outline Colors")]
    public Color hoverColor = Color.yellow;
    public Color successColor = Color.green;
    public Color failColor = Color.red;

    void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        outline.OutlineWidth = 9f;
    }

    public void OnHoverEnter()
    {
        if (locked) return;

        outline.OutlineColor = hoverColor;
        outline.enabled = true;
    }

    public void OnHoverExit()
    {
        if (locked) return;

        outline.enabled = false;
    }

    public void SetResult(bool isCorrect)
    {
        locked = true;
        outline.enabled = true;
        outline.OutlineColor = isCorrect ? successColor : failColor;
    }

    public void OnClicked()
    {
        if(locked) return;

        IRiskSource riskSource = GetComponent<IRiskSource>(); // Vérifie si l'objet est un danger
        

        if (riskSource != null) // Si c'est un objet avec un comportement de danger
            riskSource.SetRiskFound();

        else
            HandleWrongAnswer(); // Si ce n'est pas un danger, c'est une erreur

        
    }

    private void HandleWrongAnswer()
    {
        GameManager.Instance.RegisterAnswer(null, false);
        SetResult(false);
        GameManager.Instance.PlayFeedbackSound(false);
        GameManager.Instance.DisplayFeedbackUI(null);
        //GameManager.Instance.ShowHologram(transform, false);
    }
}
