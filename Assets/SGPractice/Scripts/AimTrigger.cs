using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTrigger : MonoBehaviour
{
    public Camera camera;

    FillCube prevSeen;
    GameObject nowSeen;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * GameManager의 Alert, 즉 isTriggered 상태면 에임트릭 작동X
         */
        if (GameManager.gm.isTriggered)
        {
            Clean();
            return;
        }

        Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<FillCube>() != null && !hit.transform.CompareTag("Plane"))
            {
                if (hit.transform.GetComponent<FillCube>() != prevSeen)
                {
                    Clean();
                    prevSeen = hit.transform.GetComponent<FillCube>();
                    nowSeen = prevSeen.gameObject;
                    prevSeen.SetTrigger(true);
                    Debug.Log(prevSeen.gameObject.name);
                }

            }else
            {
                Clean();
            }
        }


        /* Fire 1 */
        if (Input.GetButtonUp("Fire1") && nowSeen != null)
        {
            if (nowSeen.GetComponent<TextControl>() != null)
            {
                nowSeen.GetComponent<TextControl>().Action();
            }
        }
    }

    void Clean()
    {
        if (prevSeen != null)
        {
            prevSeen.SetTrigger(false);
            prevSeen = null;
        }

        if(nowSeen != null)
        {
            nowSeen = null;
        }
    }
}
