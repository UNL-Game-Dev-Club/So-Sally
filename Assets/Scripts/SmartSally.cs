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

    bool sallyIsSpeaking;

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
        sallyIsSpeaking = false;
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
        string displayText = "";

        foreach (string line in parts)
        {
            ShowSpeechBubbles("", false, false, false);

            if (string.IsNullOrEmpty(line))
            {
                ShowSpeechBubbles("", false, false, false);
                yield return new WaitForSeconds(3f);

                continue;
            }

            displayText = line.Substring(1).Trim();

            if (line[0] == 'S')
            {
                yield return new WaitForSeconds(1.5f);
                ShowSpeechBubbles(displayText, false, false, true);
                LightUpRing(true);
            }
            else
            {
                if (line[0] == 'L')
                {
                    ShowSpeechBubbles(displayText, true, false, false);
                }
                else
                {
                    ShowSpeechBubbles(displayText, false, true, false);
                }
            }

            penalize = false;
            yield return new WaitForSeconds(1.5f);

            if (!sallyIsSpeaking)
                penalize = true;
            yield return new WaitForSeconds(3.5f);

            if (sallyIsSpeaking)
            {
                sallyIsSpeaking = false;
                StopListening();
            }
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
            sallyIsSpeaking = true;
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
        LightUpRing(true);
    }

    void StopListening()
    {
        isRecording = false;
        if (!sallyIsSpeaking)
            LightUpRing(false);
    }

    void LightUpRing(bool tf)
    {
        if (tf)
        {
            cubeRenderer.sprite = litRingInside;
        }
        else
        {
            cubeRenderer.sprite = unlitRingInside;
        }
    }
}
