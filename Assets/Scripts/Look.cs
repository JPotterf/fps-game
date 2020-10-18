﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Potterf.FpsGame
{
    public class Look : MonoBehaviour
    {
        public static bool cursorLocked = true;

        public Transform player;
        public Transform cams;

        public float xSensitivity;
        public float ySensitivity;
        public float maxAngle;

        private Quaternion camCenter;

         void Start()
        {
            camCenter = cams.localRotation;
        }

         void Update()
        {
            SetY();
            SetX();
            UpdateCursorLock();
        }

        void SetY()
        {
            //this affects the camera(player view) up and down, with clamping to limit viewspace
            float t_input = Input.GetAxis("Mouse Y") *  ySensitivity * Time.deltaTime;
            Quaternion t_adj = Quaternion.AngleAxis(t_input, -Vector3.right);
            Quaternion t_delta = cams.localRotation * t_adj;
            
            if(Quaternion.Angle(camCenter, t_delta) < maxAngle)
            {
                cams.localRotation = t_delta;   
            }    
        }

        void SetX()
        {
            //this affects the player view left right
            float t_input = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
            Quaternion t_adj = Quaternion.AngleAxis(t_input, Vector3.up);
            Quaternion t_delta = player.localRotation * t_adj;

            player.localRotation = t_delta;
        }

        void UpdateCursorLock()
        {
            if (cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    cursorLocked = false;
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    cursorLocked = true;
                }

            }
        }
    }
}