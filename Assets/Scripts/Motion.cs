using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Com.Potterf.FpsGame
{
    public class Motion : MonoBehaviourPunCallbacks
    {
        #region Variables
        public float speed;
        public float sprintModifier;
        public float jumpForce;
        public Camera normalCam;
        public Transform groundDetector;
        public Transform weaponParent;
        public LayerMask ground;

        private Rigidbody rig;
        private float baseFOV;
        private Vector3 weaponParentOrigin;
        private Vector3 targetWeaponBobPosition;
        private float sprintFOVModifier = 1.15f;
        private float movementCounter;
        private float idleCounter;

        #endregion

        #region MonoBehaviour Callbacks
        void Start()
        {
            baseFOV = normalCam.fieldOfView;
            Camera.main.enabled = false;

            rig = GetComponent<Rigidbody>();
            weaponParentOrigin = weaponParent.localPosition;
        }


        private void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            } 

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

            //headbob
            //idle headbob
            if (t_hmove == 0 && t_vmove == 0)
            {
                HeadBob(idleCounter, .01f,.01f);
                idleCounter += Time.deltaTime;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);
            }
            else if (!isSprinting) //not sprinting movement headbob/breathing 
            {
                HeadBob(movementCounter, .05f, .05f);
                movementCounter += Time.deltaTime * 2f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 6f);
            }
            else // sprinting movement headbob/breathing 
            {
                HeadBob(movementCounter, .09f, .075f);
                movementCounter += Time.deltaTime * 7f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 10f);
            }

        }

        void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }
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

        #region Private Methods
        void HeadBob(float p_z, float p_x_intensity, float p_y_intensity)
        {
            //p_z is location on sin wave
            targetWeaponBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z * 2) * p_y_intensity, 0); 
        }
        #endregion
    }
}