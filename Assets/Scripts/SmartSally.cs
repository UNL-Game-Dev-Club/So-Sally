using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmartSally : MonoBehaviour
{
    bool isRecording;

    [SerializeField] GameObject smartSally;

    [SerializeField] Sprite litRingInside;
    [SerializeField] Sprite unlitRingInside;

    [SerializeField] Sprite litRingOutside;
    [SerializeField] Sprite unlitRingOutside;
    SpriteRenderer cubeRenderer;

    [SerializeField] Text leftText;
    [SerializeField] Text rightText;
    [SerializeField] Text sallyText;
    [SerializeField] Text pointsText;

    [SerializeField] GameObject speechBubbleLeft;
    [SerializeField] GameObject speechBubbleRight;
    [SerializeField] GameObject speechBubbleSally;

    int score;
    bool scoreChanged;

    bool penalize;

    [SerializeField] TextAsset scriptedText;

    // Start is called before the first frame update
    void Start()
    {
        isRecording = false;
        penalize = true;

        cubeRenderer = smartSally.GetComponent<SpriteRenderer>();
        StartCoroutine(WatchForPoints());
        StartCoroutine(ManageText());

        Debug.Log(scriptedText.text);

        pointsText.text = "Points: 0";
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ManageText()
    {
        ShowSpeechBubbles("", false, false, false);

        // Wait 5 seconds to start
        yield return new WaitForSeconds(5);

        string[] parts = scriptedText.text.Split('\n');
        // int randomSide = -1;
        bool side = true;

        foreach (string line in parts)
        {
            ShowSpeechBubbles("", false, false, false);

            if (string.IsNullOrEmpty(line))
            {
                yield return new WaitForSeconds(3f);

                continue;
            }
            
            if (side)
            {
                ShowSpeechBubbles(line, true, false, false);
            }
            else
            {
                ShowSpeechBubbles(line, false, true, false);
            }

            penalize = false;
            yield return new WaitForSeconds(1.5f);

            penalize = true;
            yield return new WaitForSeconds(3.5f);

            side = !side;
        }
    }

    void ShowSpeechBubbles(string displayText, bool left, bool right)
    {
        ShowSpeechBubbles(displayText, left, right, false);
    }

    void ShowSpeechBubbles(string displayText, bool left, bool right, bool sally)
    {
        leftText.text = "";
        rightText.text = "";
        sallyText.text = "";

        speechBubbleLeft.SetActive(false);
        speechBubbleRight.SetActive(false);
        speechBubbleSally.SetActive(false);

        if (left)
        {
            leftText.text = displayText;
            speechBubbleLeft.SetActive(true);
        }
        else if (right)
        {
            rightText.text = displayText;
            speechBubbleRight.SetActive(true);
        }
        else if (sally)
        {
            sallyText.text = displayText;
            speechBubbleSally.SetActive(true);
        }
    }

    IEnumerator WatchForPoints()
    {
        while (true)
        {
            if (isRecording)
            {
                if (leftText.text.Contains("So Sally"))
                {
                    score++;
                }
                else if (rightText.text.Contains("So Sally"))
                {
                    score++;
                }
                else
                {
                    if (penalize)
                        score--;
                }

                scoreChanged = true;
            }
            else
            {
                if (leftText.text.Contains("So Sally"))
                {
                    if (penalize)
                    {
                        score--;
                        scoreChanged = true;
                    }
                }
                else if (rightText.text.Contains("So Sally"))
                {
                    if (penalize)
                    {
                        score--;
                        scoreChanged = true;
                    }
                }
                else
                {
                    // Do nothing
                }
            }

            if (scoreChanged)
            {
                pointsText.text = "Points: " + score;
                scoreChanged = false;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Debug.Log(collision.name);

        if (collision.name == "Elf")
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log(collision.name + " clicked");
                if (isRecording)
                {
                    StopListening();
                }
                else
                {
                    Listen();
                }
            }
        }
    }

    void Listen()
    {
        isRecording = true;
        cubeRenderer.sprite = litRingInside;
    }

    void StopListening()
    {
        isRecording = false;
        cubeRenderer.sprite = unlitRingInside;
    }
}
