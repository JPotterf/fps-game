using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.Potterf.FpsGame
{
    public class Motion : MonoBehaviour
    {
        #region Variables
        public float speed;
        public float sprintModifier;
        public float jumpForce;
        public Camera normalCam;
        public Transform groundDetector;
        public LayerMask ground;

        private Rigidbody rig;
        private float baseFOV;
        private float sprintFOVModifier = 1.15f;

        #endregion

        #region MonoBehaviour Callbacks
        void Start()
        {
            baseFOV = normalCam.fieldOfView;
            Camera.main.enabled = false;

            rig = GetComponent<Rigidbody>();
        }


        private void Update()
        {
            //TODO: dry this up, add functions for this repetition
            //axis input
            float t_hmove = Input.GetAxisRaw("Horizontal");
            float t_vmove = Input.GetAxisRaw("Vertical");

            //controls
            bool t_sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool t_jump = Input.GetKey(KeyCode.Space);

            //state
            bool isGrounded = Physics.Raycast(groundDetector.position.normalized, Vector3.down, 0.1f, ground);
            bool isJumping = t_jump && isGrounded;
            bool isSprinting = t_sprint && t_vmove > 0 && !isJumping && isGrounded;

            //jumping
            if (isJumping)
            {
                rig.AddForce(Vector3.up * jumpForce);
            }
        }

        void FixedUpdate()
        {
            //axis input
            float t_hmove = Input.GetAxisRaw("Horizontal");
            float t_vmove = Input.GetAxisRaw("Vertical");

            //controls
            bool t_sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool t_jump = Input.GetKey(KeyCode.Space);

            //state
            bool isGrounded = Physics.Raycast(groundDetector.position.normalized, Vector3.down, 0.1f, ground);
            bool isJumping = t_jump && isGrounded;
            bool isSprinting = t_sprint && t_vmove > 0 && !isJumping && isGrounded;


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
        #endregion

    }
}