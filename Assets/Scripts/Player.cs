using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D thisRigidbody;
    const float speed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();

        if (thisRigidbody == null)
        {
            thisRigidbody = this.gameObject.AddComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(horizontal) > 0.5f)
        {
            thisRigidbody.transform.Translate(Vector2.right * horizontal * speed);
        }
    }
}
