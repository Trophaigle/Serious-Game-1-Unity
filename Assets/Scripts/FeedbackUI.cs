using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FeedbackUI : MonoBehaviour, IFeedbackProvider
{
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float displayTime = 3.5f;

    private Coroutine currentCoroutine;

    void Awake()
    {
        feedbackText.gameObject.SetActive(false);
    }

    public void ShowFeedback(string message, Color color)
    {
        if(currentCoroutine != null) StopCoroutine(currentCoroutine); 

        currentCoroutine = StartCoroutine(ShowRoutine(message, color));
    }

    IEnumerator ShowRoutine(string message, Color color)
    {
        feedbackText.text = message;
        feedbackText.color = color;
        feedbackText.gameObject.SetActive(true); 

        yield return new WaitForSeconds(displayTime);

        feedbackText.gameObject.SetActive(false);
        currentCoroutine = null;
    }
}
