using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.Potterf.FpsGame
{
    public class Motion : MonoBehaviour
    {
        public float speed;
        public float sprintModifier;
        public float jumpForce;
        public Camera normalCam;

        private Rigidbody rig;
        private float baseFOV;
        private float sprintFOVModifier = 1.15f;
        

        // Start is called before the first frame update
        void Start()
        {
            baseFOV = normalCam.fieldOfView;
            Camera.main.enabled = false;

            rig = GetComponent<Rigidbody>();
        }

        
        void FixedUpdate()
        {
            //axis input
            float t_hmove = Input.GetAxisRaw("Horizontal");
            float t_vmove = Input.GetAxisRaw("Vertical");

            //controls
            bool t_sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool t_jump = Input.GetKeyDown(KeyCode.Space);

            //state
            bool isJumping = t_jump;
            bool isSprinting = t_sprint && t_vmove > 0 && !isJumping;
            

            //jumping
            if(isJumping)
            {
                rig.AddForce(Vector3.up * jumpForce); 
            }

            //Movement 
            Vector3 t_direction = new Vector3(t_hmove, 0, t_vmove);
            t_direction.Normalize();


            float t_adjustedSpeed = speed;
            if (isSprinting)
            {
                t_adjustedSpeed *= sprintModifier;
                //field of view increase while sprinting
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f);
            }
            else
            {
                //field of view narrows while walking
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
            }

            Vector3 t_targetVelocity = transform.TransformDirection(t_direction) * t_adjustedSpeed * Time.deltaTime;
            t_targetVelocity.y = rig.velocity.y;
            rig.velocity = t_targetVelocity;
            
        }
    }
}