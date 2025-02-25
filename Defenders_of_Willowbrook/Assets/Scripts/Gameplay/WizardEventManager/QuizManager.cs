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
    public List<Button> answerButtons; // Thêm các nút trong Inspector
    private int correctAnswerIndex = 0;

    private ILevelManager currentLevelManager;

    // S? ki?n báo hoàn thành quiz
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
        ShowQuestion();
    }

    private void ShowQuestion()
    {
        currentLevelManager.PauseGame(true);
        wizardPanel.SetActive(true);

        questionText.text = "What weakens fire enemies?";
        string[] answers = { "Water", "Wind", "Earth", "Fire" };
        correctAnswerIndex = 0; // ?áp án ?úng là Water

        for (int i = 0; i < answerButtons.Count; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
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
        OnQuizComplete.Invoke(); // Thông báo hoàn thành quiz
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
