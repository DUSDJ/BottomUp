using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CubeSpace
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerController : MonoBehaviour
    {
        Rigidbody rigid;
        public Menu choice;
        public Image fadeImage;

        private bool isEntered = false;

        private bool trigger = false;

        /* Effect Source */
        AudioSource audio_start;

        public enum Menu
        {
            Tutorial,
            Exit,
        }


        // Start is called before the first frame update
        void Start()
        {
            audio_start = GetComponent<AudioSource>();
            

            Cursor.visible = false;
            rigid = GetComponent<Rigidbody>();
            choice = Menu.Tutorial;
        }
        
        // Update is called once per frame
        void Update()
        {
            /* VR Mode */
            float yInput = Input.GetAxis("Vertical");

            if(yInput > 0.8)
            {
                Debug.Log("Up");
                choice = Menu.Tutorial;

            }else if(yInput < -0.8) {
                Debug.Log("Down");
                choice = Menu.Exit;
            }

            /* 종료 */
            if (Input.GetButtonDown("Cancel") || Input.GetButtonUp("Fire2"))
            {
                Application.Quit();
            }

            if (Input.GetButtonDown("Fire1") && !isEntered)
            {
                isEntered = true;

                switch (choice)
                {
                    case Menu.Tutorial:
                        audio_start.Play(0);
                        StartCoroutine(LoadScene(1));
                        break;

                    case Menu.Exit:
                        audio_start.Play(0);
                        StartCoroutine(LoadScene(1));
                        break;
                    default:
                        break;
                }
            }

            /* Non VR Mode */
            
            transform.rotation *= Quaternion.Euler(0, 0, 7f * Time.deltaTime);
            
            if(Input.GetMouseButton(0))
            {
                if (Input.mousePosition.x > Screen.width / 3
                        && Input.mousePosition.x < Screen.width / 3 * 2)
                {
                    if (Input.mousePosition.y > Screen.height / 2)
                    {
                        rigid.velocity += transform.rotation * Vector3.up * 10f * Time.deltaTime;

                    }
                    else
                    {
                        rigid.velocity += transform.rotation *Vector3.down * 10f * Time.deltaTime;
                    }

                }
                else if (Input.mousePosition.x < Screen.width / 3)
                {
                    rigid.velocity += transform.rotation * Vector3.left * 10f * Time.deltaTime;
                }
                else if (Input.mousePosition.x > Screen.width / 3 * 2)
                {
                    rigid.velocity += transform.rotation * Vector3.right * 10f * Time.deltaTime;
                }
            }

        }

        IEnumerator FadeOut()
        {
            Color color = fadeImage.color;
            float start = 0f;
            float end = 1f;
            float animTime = 3f;
            float time = 0f;
            color.a = Mathf.Lerp(start, end, time);

            while (color.a < 1f)
            {
                // 경과 시간 계산.  
                // animTime동안 재생될 수 있도록 animTime으로 나누기.  
                time += Time.deltaTime / animTime;

                // 알파 값 계산.  
                color.a = Mathf.Lerp(start, end, time);
                // 계산한 알파 값 다시 설정.  
                fadeImage.color = color;

                yield return null;
            }
        }

        IEnumerator Cinematic()
        {
            while(Time.timeScale < 6.0f)
            {
                Time.timeScale += 1.0f * Time.fixedDeltaTime;
                Debug.Log(Time.timeScale);
                yield return null;
            }
            
            trigger = true;
            
        }

        IEnumerator LoadScene(int sceneNumber)
        {
            StartCoroutine(Cinematic());
            StartCoroutine(FadeOut());

            AsyncOperation asyncOper = SceneManager.LoadSceneAsync(sceneNumber);
            asyncOper.allowSceneActivation = false;

            while (asyncOper.progress < 0.9f)
            {
                yield return null;
                Debug.Log(asyncOper.progress);
            }
            
            
            //Trigger
            while (!trigger)
            {
                yield return null;
            }

            asyncOper.allowSceneActivation = true;
            Time.timeScale = 1.0f;


        }
    }
}

