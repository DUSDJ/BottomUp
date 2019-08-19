using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BottomUp { 
    public class CameraController : MonoBehaviour
    {
        public GameObject camBody;
        public Camera cam;
        public Transform cameraPos;

        public float forwardSpeed = 5.0f;
        public float backwardSpeed = 2.0f;
        public float sideSpeed = 3.0f;
        public float rotateSpeed = 3.0f;

        public bool cinematic = false;

        private Rigidbody rigid;
        private CapsuleCollider m_Capsule;
        
        // Start is called before the first frame update
        void Start()
        {
            rigid = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            RotCtrl();
            if (cinematic)
            {
                return;
            }
            rigidCtrl();
            
        }

        void rigidCtrl()
        {
            Vector3 v = new Vector3(0,0,0);

            float horizon = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            if (vertical > 0)
            {
               v = transform.forward * vertical * forwardSpeed;
              
            }

            if (vertical < 0)
            {
               v = transform.forward * vertical * backwardSpeed;
               
            }

            if(vertical == 0 && horizon != 0)
            {
               v = transform.right * horizon * sideSpeed;
               
            }

            if (vertical != 0 && horizon != 0)
            {
                v = transform.right * horizon + transform.forward * vertical;
               
                v = v.normalized;
                v = v * forwardSpeed;
                
            }

            rigid.velocity = v;
        }

        void RotCtrl()
        {
            camBody.transform.position = cameraPos.position;

            /* VR Mode */
            if (GameManager.VRMode)
            {
                transform.localRotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
                //transform.localRotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
            }
            /* Non-VR Mode */
            else
            {
                float rotX = Input.GetAxis("Mouse Y") * rotateSpeed;
                float rotY = Input.GetAxis("Mouse X") * rotateSpeed;
                rigid.angularVelocity = new Vector3(0, 0, 0);

                camBody.transform.localRotation *= Quaternion.Euler(-rotX, rotY, 0);
                camBody.transform.localRotation = Quaternion.Euler(camBody.transform.eulerAngles.x, camBody.transform.eulerAngles.y, 0);
                transform.localRotation = Quaternion.Euler(0, camBody.transform.eulerAngles.y, 0);
            }

            
        }


        void Skill()
        {
            // RayCast하여 Aim에 해당하는 오브젝트의 Fade 브로드캐스트.
        }
        
    }
}