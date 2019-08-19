using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearTrigger : MonoBehaviour
{
    /* sound effects */
    public AudioClip sound_clear;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //audioSource.PlayOneShot(sound_clear);
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Clear!");
        Debug.Log("Next Level : " + (GameManager.gm.setStage + 1).ToString());
        SceneManager.LoadScene((GameManager.gm.setStage + 1).ToString());
       
    }
}
