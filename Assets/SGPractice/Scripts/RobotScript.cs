using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotScript : MonoBehaviour
{
    public Transform[] goal = null;
    
    Animator anim;
    CharacterController characterController;

    bool moving = false;
    //　목적지에 도착했다고 보는 정지 거리.
    const float StoppingDistance = 1.0f;
    // 도착했는가(도착했다 true / 도착하지 않았다 false).
    public bool arrived = false;
    // 이동 속도.
    public float walkSpeed = 2.0f;
    // 현재 이동 속도.
    Vector3 velocity = Vector3.zero;

    public Vector3 destination;
    Vector3 starting;

    /* sound effects */
    public AudioClip sound_alert;
    public AudioClip sound_open;
    AudioSource audioSource;
    
    public float openTime = 0f;
    float time;
    bool openByTime= false;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        starting = transform.position;
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        /* Time Check */
        if (openTime != 0f)
        {
            if (time >= openTime && !openByTime)
            {
                if (!IsOpen())
                {
                    Open();
                    openByTime = true;
                }
            }
            else
            {
                time += Time.deltaTime;
            }

        }
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            if (arrived)
            {
                if(goal.Length > 1)
                {
                    if (destination == goal[0].position)
                    {
                        destination = goal[1].position;

                    }
                    else
                    {
                        destination = goal[0].position;
                    }
                }
                else
                {
                    if (destination == goal[0].position)
                    {
                        destination = starting;

                    }
                    else
                    {
                        destination = goal[0].position;
                    }
                }
                

                transform.LookAt(destination);
                arrived = false;
            }

            // 목적지까지 거리와 방향을 구한다.
            Vector3 direction = (destination - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, destination);

            // 현재 속도를 보관한다.
            Vector3 currentVelocity = velocity;
            

            // 이동 속도를 구한다.

            velocity = direction * walkSpeed;

            // 부드럽게 보간 처리.
            velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            velocity.y = 0;

            // CharacterController를 사용해서 움직인다.
            characterController.Move(velocity * Time.deltaTime);

            if (characterController.velocity.magnitude < 0.1f)
            {
                arrived = true;
            }
        }
    }

    public void Stop()
    {
        audioSource.PlayOneShot(sound_open);
        anim.SetBool("Walk_Anim", false);
        anim.SetBool("Open_Anim", false);
        moving = false;
    }

    public void Open()
    {
        audioSource.PlayOneShot(sound_open);
        anim.SetBool("Open_Anim", true);

        GameManager.gm.ClearCheck();

        Invoke("Move", 3.0f);
    }

    public bool IsOpen()
    {
        return anim.GetBool("Open_Anim");
    }
    
    public void Move()
    {
        destination = goal[0].position;
        moving = true;
        anim.SetBool("Walk_Anim", true);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && moving)
        {
            audioSource.PlayOneShot(sound_alert);
            moving = false;
            anim.SetBool("Walk_Anim", false);
            Vector3 temp = Camera.main.transform.position;
            temp.y = 0f;
            Debug.Log(temp);
            transform.LookAt(temp);
            GameManager.gm.Alert();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && moving)
        {
            audioSource.PlayOneShot(sound_alert);

            moving = false;
            anim.SetBool("Walk_Anim", false);
            Vector3 temp = Camera.main.transform.position;
            temp.y = 0f;
            Debug.Log(temp);
            transform.LookAt(temp);
            GameManager.gm.Alert();
            
        }
    }
}
