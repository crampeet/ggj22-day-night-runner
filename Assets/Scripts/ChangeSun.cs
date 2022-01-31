using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSun : MonoBehaviour
{
    public bool isSunHot;
    private double GlobalPosition;
    int ranNum;
    public SpriteRenderer spriteRenderer;
    public float rotationSpeed = 0.2f;
    

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed);
        GlobalPosition = transform.rotation.z;
        if (GlobalPosition > -0.02 && GlobalPosition < 0.02)
        {
            ranNum = Random.Range(0, 10);
            isSunHot = ranNum > 5;
            //Debug.Log("Sun State: " + isSunHot);
            if (isSunHot)
            {
                //Debug.Log("Its red");
                spriteRenderer.color = new Color(255, 0, 0, 1);
            }
            else
            { 
                //Debug.Log("Its blue");
                spriteRenderer.color = new Color(0, 0, 255, 1);
            }
        }
    }
}

