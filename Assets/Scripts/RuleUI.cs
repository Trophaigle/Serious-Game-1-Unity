using UnityEngine;

public class RuleUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    void Start()
    {
        Show();
    }

    public void Show()
    {
        panel.SetActive(true);
        // Bloque le jeu
        GameManager.Instance.SetState(GameState.Intro); 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Hide()
    {
        panel.SetActive(false);
        // Débloque le jeu
        GameManager.Instance.SetState(GameState.Exploring);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnStartButtonClicked() { Hide(); }
}
