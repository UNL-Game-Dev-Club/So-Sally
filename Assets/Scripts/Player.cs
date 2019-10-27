using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D thisRigidbody;
    const float speed = 0.1f;

    bool ringIsLit;

    [SerializeField] GameObject sallyCube;
    SpriteRenderer cubeRenderer;

    [SerializeField] Sprite litRingInside;
    [SerializeField] Sprite unlitRingInside;

    [SerializeField] Sprite litRingOutside;
    [SerializeField] Sprite unlitRingOutside;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();

        if (thisRigidbody == null)
        {
            thisRigidbody = this.gameObject.AddComponent<Rigidbody2D>();
        }

        ringIsLit = false;
        cubeRenderer = sallyCube.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(horizontal) > 0.5f)
        {
            thisRigidbody.transform.Translate(Vector2.right * horizontal * speed);
        }

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
