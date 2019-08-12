using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HelpMenu : MonoBehaviour
{
    int instructionIndex = 0;
    const int maxInstructions = 4;

    string textPath = "Help/Help_";

    Text titleText;
    Text instructionText;
    Button homeButton;
    Button backButton;
    Button nextButton;

    Animator animator;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        var canvas = GameObject.Find("Canvas").transform;
        titleText = canvas.Find("How Text").GetComponent<Text>();
        titleText.text = TextAssetLoader.GetCorrectTextAsset("Help/Help").text;
        instructionText = canvas.Find("Instruction Text").GetComponent<Text>();
        homeButton = canvas.Find("Return Button").GetComponent<Button>();
        backButton = canvas.Find("Back Button").GetComponent<Button>();
        nextButton = canvas.Find("Next Button").GetComponent<Button>();

        homeButton.onClick.AddListener(()=>SceneManager.LoadScene("MainMenu"));
        backButton.onClick.AddListener(() =>
        {
            instructionIndex--;
            audioSource.Play();
            UpdateText();
        });
        nextButton.onClick.AddListener(() =>
        {
            instructionIndex++;
            audioSource.Play();
            UpdateText();
        });

        animator = FindObjectOfType<Animator>();
        audioSource = GetComponent<AudioSource>();

        UpdateText();
    }

    void UpdateText()
    {
        instructionText.text = TextAssetLoader.GetCorrectTextAsset($"{textPath}{instructionIndex}").text;
        backButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);

        switch (instructionIndex)
        {
            case 0:
                backButton.gameObject.SetActive(false);
                animator.Play("Idle");
                break;
            case 1:
                animator.Play("Walk");
                break;
            case 2:
                animator.Play("Hit");
                break;
            case 3:
                animator.Play("Panque");
                break;
            case 4:
                animator.Play("Dance");
                nextButton.gameObject.SetActive(false);
                break;
        }
    }
}
