using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSun : MonoBehaviour
{
    public bool SunState;
    private double GlobalPosition;
    int ranNum;
    public SpriteRenderer spriteRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, .025f);
        GlobalPosition = transform.rotation.z;
        if (GlobalPosition > -.0025f && GlobalPosition < .0025f)
        {
            Debug.Log("It worked");
            ranNum = Random.Range(0, 10);
            if(ranNum > 5)
            {
                SunState = true;
            }
            else
            {
                SunState = false;
            }
            Debug.Log(SunState);
            Debug.Log(ranNum);
            if (SunState)
            {
                Debug.Log("Its red");
                spriteRenderer.color = new Color(255, 0, 0, 1);
            }
            else
            { 
                Debug.Log("Its blue");
                spriteRenderer.color = new Color(0, 0, 255, 1);
                
            }
        }
    }
}

