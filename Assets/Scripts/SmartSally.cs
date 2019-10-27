﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartSally : MonoBehaviour
{
    bool ringIsLit;

    [SerializeField] GameObject smartSally;

    [SerializeField] Sprite litRingInside;
    [SerializeField] Sprite unlitRingInside;

    [SerializeField] Sprite litRingOutside;
    [SerializeField] Sprite unlitRingOutside;
    SpriteRenderer cubeRenderer;

    // Start is called before the first frame update
    void Start()
    {
        ringIsLit = false;

        cubeRenderer = smartSally.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.name);

        if (collision.name == "Elf")
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log(collision.name + " clicked");
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
