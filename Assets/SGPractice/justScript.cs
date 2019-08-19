using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class justScript : MonoBehaviour
{
    public Canvas canvas;
    CanvasGroup cg;

    public Material mate;
    Material matt;
    float da = 0.3f;



    // Start is called before the first frame update
    void Start()
    {
        //mate = GetComponent<Material>();
        matt = gameObject.GetComponent<Renderer>().material;
        cg = canvas.GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(GameManager.gm.isTriggered)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }

        /*
        if(Input.GetKey(KeyCode.Space))
        {
            FadeIn();
        }
        else if (da > 0.2)
        {
            FadeOut();

        }
        */
    }

    public void Action()
    {

    }

    void FadeIn()
    {
        if(da < 1)
        {
            da += 0.3f * Time.deltaTime;
            if(da > 1)
            {
                da = 1;
            }
            matt.SetFloat("Vector1_31655727", da);           
        }
        
        if (cg.alpha < 1)
        {
            cg.alpha += 0.3f * Time.deltaTime;

            if (cg.alpha > 0)
            {
                cg.alpha = 1;
            }
        }
    }

    void FadeOut()
    {
        if(da > 0.3)
        {
            da -= 0.3f * Time.deltaTime;
            matt.SetFloat("Vector1_31655727", da);

            if (da < 0.3f)
            {
                da = 0.3f;
            }
        }
        
        if (cg.alpha > 0)
        {
            cg.alpha -= 0.3f * Time.deltaTime;

            if(cg.alpha < 0)
            {
                cg.alpha = 0;
            }
        }
    }
}
