using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransParam : MonoBehaviour
{
    public static SceneTransParam instance;
    public float fDifficulty = 20f;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
           
        }
    }
}
