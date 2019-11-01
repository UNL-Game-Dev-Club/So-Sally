using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    [SerializeField] Text instructionsText;
    [SerializeField] TextAsset instructionsFile;

    [SerializeField] Text shortcutsText;

    float delaySpeed;

    bool readyToContinue;

    // Start is called before the first frame update
    void Start()
    {
        delaySpeed = 0.1f;
        readyToContinue = false;

        shortcutsText.text = "[ Press Space to Speed Up  ]\n[      Press Esc to Skip       ]";

        StartCoroutine(DisplayInstructions());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (readyToContinue)
            {
                NextScene();
            }
            else
            {
                delaySpeed = 0.01f;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            NextScene();
        }
    }

    IEnumerator DisplayInstructions()
    {
        instructionsText.text = "";
        string instructions = instructionsFile.text;
        foreach (char letter in instructions)
        {
            yield return new WaitForSeconds(delaySpeed);
            instructionsText.text += letter;
        }

        readyToContinue = true;
        shortcutsText.text = "[ Press Space to Continue  ]\n[      Press Esc to Skip       ]";
    }

    void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
