using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldAnswerManager : MonoBehaviour
{
    [System.Serializable]
    public class InputFieldAnswer
    {
        public TMP_InputField inputField;
        public string correctAnswer;
        public string questionId; // Optional identifier for the question
    }

    [Header("Input Fields Configuration")]
    public List<InputFieldAnswer> inputFieldAnswers = new List<InputFieldAnswer>();

    [Header("UI Elements")]
    public Button checkAnswersButton;
    public TextMeshProUGUI feedbackText;
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;
    public Color defaultColor = Color.white;

    private Dictionary<TMP_InputField, string> answerDictionary;

    void Start()
    {
        InitializeDictionary();

        if (checkAnswersButton != null)
        {
            checkAnswersButton.onClick.AddListener(CheckAllAnswers);
        }
    }

    void InitializeDictionary()
    {
        answerDictionary = new Dictionary<TMP_InputField, string>();

        foreach (var item in inputFieldAnswers)
        {
            if (item.inputField != null)
            {
                answerDictionary[item.inputField] = item.correctAnswer;

                // Add listener to each input field for real-time checking
                item.inputField.onValueChanged.AddListener(delegate { OnInputChanged(item.inputField); });
            }
        }
    }

    public void AddInputFieldAnswer(TMP_InputField inputField, string answer)
    {
        if (answerDictionary == null)
            answerDictionary = new Dictionary<TMP_InputField, string>();

        answerDictionary[inputField] = answer;

        // Add to the serialized list as well
        inputFieldAnswers.Add(new InputFieldAnswer
        {
            inputField = inputField,
            correctAnswer = answer
        });

        // Add listener for real-time checking
        inputField.onValueChanged.AddListener(delegate { OnInputChanged(inputField); });
    }

    public void RemoveInputFieldAnswer(TMP_InputField inputField)
    {
        if (answerDictionary != null && answerDictionary.ContainsKey(inputField))
        {
            answerDictionary.Remove(inputField);
            inputFieldAnswers.RemoveAll(x => x.inputField == inputField);
        }
    }

    public bool CheckAnswer(TMP_InputField inputField)
    {
        if (answerDictionary == null || !answerDictionary.ContainsKey(inputField))
            return false;

        string userAnswer = inputField.text.Trim().ToLower();
        string correctAnswer = answerDictionary[inputField].Trim().ToLower();

        return userAnswer == correctAnswer;
    }

    public void CheckAllAnswers()
    {
        int correctCount = 0;
        int totalCount = answerDictionary.Count;

        foreach (var kvp in answerDictionary)
        {
            TMP_InputField inputField = kvp.Key;
            bool isCorrect = CheckAnswer(inputField);

            if (isCorrect)
            {
                correctCount++;
                SetInputFieldColor(inputField, correctColor);
            }
            else
            {
                SetInputFieldColor(inputField, incorrectColor);
            }
        }

        // Update feedback text
        if (feedbackText != null)
        {
            feedbackText.text = $"Score: {correctCount}/{totalCount}";

            if (correctCount == totalCount)
            {
                feedbackText.text += " - Perfect!";
                feedbackText.color = correctColor;
            }
            else
            {
                feedbackText.color = incorrectColor;
            }
        }
    }

    private void OnInputChanged(TMP_InputField inputField)
    {
        // Reset color when user starts typing
        SetInputFieldColor(inputField, defaultColor);

        if(AllAnswersCorrect())
        {
            gameObject.SetActive(false);
            //evetn here.
        }

        // Optional: Clear feedback text when user makes changes
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    private void SetInputFieldColor(TMP_InputField inputField, Color color)
    {
        if (inputField != null)
        {
            var colors = inputField.colors;
            colors.normalColor = color;
            colors.selectedColor = color;
            inputField.colors = colors;
        }
    }

    public void ResetAllFields()
    {
        foreach (var kvp in answerDictionary)
        {
            kvp.Key.text = "";
            SetInputFieldColor(kvp.Key, defaultColor);
        }

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    public void ShowAnswers()
    {
        foreach (var kvp in answerDictionary)
        {
            kvp.Key.text = kvp.Value;
            SetInputFieldColor(kvp.Key, correctColor);
        }
    }

    // Method to get answer for a specific input field
    public string GetCorrectAnswer(TMP_InputField inputField)
    {
        if (answerDictionary != null && answerDictionary.ContainsKey(inputField))
        {
            return answerDictionary[inputField];
        }
        return "";
    }

    // Method to check if all answers are correct
    public bool AllAnswersCorrect()
    {
        foreach (var kvp in answerDictionary)
        {
            if (!CheckAnswer(kvp.Key))
                return false;
        }
        return true;
    }
}