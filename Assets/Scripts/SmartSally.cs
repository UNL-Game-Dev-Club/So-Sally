using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartSally : MonoBehaviour
{
    bool ringIsLit;

    [SerializeField] Sprite litRingInside;
    [SerializeField] Sprite unlitRingInside;

    [SerializeField] Sprite litRingOutside;
    [SerializeField] Sprite unlitRingOutside;
    SpriteRenderer cubeRenderer;

    // Start is called before the first frame update
    void Start()
    {
        ringIsLit = false;

        cubeRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (ringIsLit)
            {
                StopListening();
            }
            else
            {
                Listen();
            }
        }
    }

    void Listen()
    {
        ringIsLit = true;
        cubeRenderer.sprite = litRingInside;
    }

    void StopListening()
    {
        ringIsLit = false;
        cubeRenderer.sprite = unlitRingInside;
    }
}
