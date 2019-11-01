using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField] TextAsset goodEndingText;
    [SerializeField] TextAsset goodEndingTextOverlay;
    [SerializeField] TextAsset badEndingText;
    [SerializeField] TextAsset badEndingTextOverlay;

    [SerializeField] Canvas overlayCanvas;
    [SerializeField] Text overlayCanvasEndingText;

    int score;
    bool scoreChanged;

    bool penalize;

    bool sallyIsSpeaking;

    [SerializeField] TextAsset scriptedText;

    const string ERROR_RESPONSE = "Why isn't this thing working?";
    bool didRecord;

    bool playAgainTime = false;

    // Start is called before the first frame update
    void Start()
    {
        isRecording = false;
        penalize = true;

        cubeRenderer = smartSally.GetComponent<SpriteRenderer>();
        StartCoroutine(WatchForPoints());
        StartCoroutine(ManageText());

        // Debug.Log(scriptedText.text);

        pointsText.text = "Points: 0";
        sallyIsSpeaking = false;

        didRecord = false;

        overlayCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!playAgainTime)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    IEnumerator ManageText()
    {
        string displayText = "";
        bool previousSideWasLeft = true;
        bool isLeft = false;

        string line = "";
        string prevLine = "";
        char indicator = '?';

        ShowSpeechBubbles("", false, false, false);

        // Wait 5 seconds to start
        yield return new WaitForSeconds(5);

        string[] parts = scriptedText.text.Split('\n');
        for (int i = 0; i < parts.Length; i++)
        {
            line = parts[i];

            penalize = false;

            ShowSpeechBubbles("", false, false, false);

            if (isPause(line.Trim()))
            {
                yield return new WaitForSeconds(3f);
                continue;
            }
            indicator = line[0];

            displayText = line.Substring(1).Trim();

            Debug.Log("Indicator: " + indicator + ", Line: " + displayText);

            if (indicator == 'S')
            {
                /* if (didRecord)
                {
                    yield return new WaitForSeconds(1.5f);
                    ShowSpeechBubbles(displayText, false, false, true);
                    LightUpRing(true);
                }
                else
                {
                    yield return new WaitForSeconds(3);
                    ShowSpeechBubbles(ERROR_RESPONSE, previousSideWasLeft, !previousSideWasLeft, false);
                }

                if (isPause(parts[i + 1]))
                {
                    didRecord = false;
                }
                else
                {
                    if (parts[i + 1][0] != 'S')
                    {
                        didRecord = false;
                    }
                }
                */

                yield return new WaitForSeconds(1.5f);
                ShowSpeechBubbles(displayText, false, false, true);
                LightUpRing(true);

                /* if (!didRecord)
                {
                    yield return new WaitForSeconds(3);
                    ShowSpeechBubbles(ERROR_RESPONSE, previousSideWasLeft, !previousSideWasLeft, false);
                } */
            }
            else
            {
                isLeft = line[0] == 'L';
                ShowSpeechBubbles(displayText, isLeft, !isLeft);
                previousSideWasLeft = isLeft;
            }

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

        string overlayText = "";
        // if (score > 20)
        if (score > 0)
        {
            // Good Ending
            parts = goodEndingText.text.Split('\n');
            overlayText = goodEndingTextOverlay.text;
        }
        else
        {
            // Bad ending
            parts = badEndingText.text.Split('\n');
            overlayText = badEndingTextOverlay.text;
        }

        displayText = "";
        isLeft = false;
        foreach (string thisLine in parts)
        {
            displayText = thisLine.Substring(1);
            isLeft = thisLine[0] == 'L';
            ShowSpeechBubbles(displayText, isLeft, !isLeft);

            yield return new WaitForSeconds(5);
        }

        overlayCanvas.gameObject.SetActive(true);
        overlayCanvasEndingText.text = overlayText;
    }

    bool isPause(string line)
    {
        if (string.IsNullOrEmpty(line.Trim()))
        {
            ShowSpeechBubbles("", false, false, false);
            return true;
        }
        else
        {
            return false;
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
                if (leftText.text.Contains("So Sally") || rightText.text.Contains("So Sally"))
                {
                    score++;
                    didRecord = true;
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

            if (score < 0)
            {
                pointsText.color = Color.red;
            }
            else
            {
                pointsText.color = Color.black;
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
