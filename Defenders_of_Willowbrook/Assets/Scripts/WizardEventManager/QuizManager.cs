using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;

    [Header("Quiz Elements")]
    public GameObject questionPanel;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;  // ✅ Hiển thị số câu đúng
    public List<Button> answerButtons;

    private List<QuizQuestion> questions = new List<QuizQuestion>();
    private int currentQuestionIndex = 0;
    private int correctAnswers = 0;
    private const int totalQuestions = 3;

    private ILevelManager currentLevelManager;

    public event Action<bool> OnQuizComplete;

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
        correctAnswers = 0;  // ✅ Reset số câu đúng
        UpdateScoreUI();  // ✅ Cập nhật UI khi bắt đầu quiz
        Debug.Log("Starting Quiz...");
        StartCoroutine(FetchQuestionsFromAPI());
    }

    private IEnumerator FetchQuestionsFromAPI()
    {
        string apiUrl = "https://localhost:7168/api/Quiz/random?count=3";
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to fetch quiz questions: {request.error}");
                yield break;
            }

            try
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log($"API Response: {jsonResponse}");

                questions = JsonConvert.DeserializeObject<List<QuizQuestion>>(jsonResponse) ?? new List<QuizQuestion>();

                if (questions.Count == 0)
                {
                    Debug.LogError("No questions received from API!");
                    yield break;
                }

                Debug.Log($"Total Questions Loaded: {questions.Count}");
                currentQuestionIndex = 0;
                ShowQuestion();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing JSON: {e.Message}");
            }
        }
    }

    private void ShowQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            Debug.LogError($"Invalid Question Index: {currentQuestionIndex}/{questions.Count}");
            return;
        }

        QuizQuestion questionData = questions[currentQuestionIndex];

        if (string.IsNullOrEmpty(questionData.Question) || questionData.Answers == null || questionData.Answers.Count < 2)
        {
            Debug.LogError($"Invalid question data at index {currentQuestionIndex}");
            return;
        }

        questionText.text = questionData.Question;

        for (int i = 0; i < answerButtons.Count; i++)
        {
            if (i < questionData.Answers.Count)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = questionData.Answers[i];
                int answerIndex = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(answerIndex));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void CheckAnswer(int selectedAnswerIndex)
    {
        if (selectedAnswerIndex == questions[currentQuestionIndex].CorrectAnswerIndex)
        {
            correctAnswers++;
            UpdateScoreUI();  // ✅ Cập nhật số câu đúng
        }

        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Count)
        {
            ShowQuestion();
        }
        else
        {
            EndQuiz();
        }
    }

    private void EndQuiz()
    {
        Debug.Log($"Quiz Ended - Correct Answers: {correctAnswers}/{totalQuestions}");
        questionPanel.SetActive(false);

        currentLevelManager?.PauseGame(false);

        bool passed = correctAnswers >= 2;
        OnQuizComplete?.Invoke(passed);

        if (passed)
            ApplyBuff();
        else
            ApplyDebuff();
    }

    private void ApplyBuff()
    {
        Debug.Log("Buff applied.");
        currentLevelManager?.IncreaseMoney(50);
    }

    private void ApplyDebuff()
    {
        Debug.Log("Debuff applied.");
        currentLevelManager?.SpendMoney(20);
    }

    // ✅ Hàm cập nhật UI số câu đúng
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Correct: {correctAnswers}/{totalQuestions}";
        }
    }
}

[System.Serializable]
public class QuizQuestion
{
    [JsonProperty("question")]
    public string Question { get; set; }

    [JsonProperty("answers")]
    public List<string> Answers { get; set; }

    [JsonProperty("correctAnswerIndex")]
    public int CorrectAnswerIndex { get; set; }
}
