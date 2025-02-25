using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;

    [Header("Quiz Elements")]
    public GameObject wizardPanel;
    public TextMeshProUGUI questionText;
    public List<Button> answerButtons;
    private int correctAnswerIndex = 0;

    private ILevelManager currentLevelManager;

    // S? ki?n b�o ho�n th�nh quiz
    public UnityEvent OnQuizComplete = new UnityEvent();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void StartQuiz(ILevelManager levelManager)
    {
        currentLevelManager = levelManager;
        Debug.Log("Starting Quiz...");

        ShowQuestion();
    }

    private void ShowQuestion()
    {
        Debug.Log("Displaying Question UI...");
        currentLevelManager.PauseGame(true);
        wizardPanel.SetActive(true);

        questionText.text = "What weakens fire enemies?";
        Debug.Log("Question set: " + questionText.text);
        string[] answers = { "Water", "Wind", "Earth", "Fire" };
        correctAnswerIndex = 0; // ?�p �n ?�ng l� Water

        for (int i = 0; i < answerButtons.Count; i++)
        {
            if (answerButtons[i] != null)
            {
                int index = i;
                var buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText != null)
                {
                    buttonText.text = answers[i];
                    Debug.Log($"Button {i} assigned with text: {answers[i]}");
                }
                else
                {
                    Debug.LogError($"Button {i} is missing a TextMeshProUGUI component!");
                }

                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
                Debug.Log($"Listener added for button {i}");
            }
            else
            {
                Debug.LogError($"Answer button at index {i} is not assigned in the Inspector!");
            }
        }
    }

    private void CheckAnswer(int selectedAnswerIndex)
    {
        if (selectedAnswerIndex == correctAnswerIndex)
        {
            ApplyBuff();
        }
        else
        {
            ApplyDebuff();
        }

        wizardPanel.SetActive(false);
        currentLevelManager.PauseGame(false);
        OnQuizComplete.Invoke(); // Th�ng b�o ho�n th�nh quiz
    }

    private void ApplyBuff()
    {
        Debug.Log("Correct Answer! Buff applied.");
        currentLevelManager.IncreaseMoney(50);
    }

    private void ApplyDebuff()
    {
        Debug.Log("Wrong Answer! Debuff applied.");
        currentLevelManager.SpendMoney(20);
    }
}