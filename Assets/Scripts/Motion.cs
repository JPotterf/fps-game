﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.Potterf.FpsGame
{
    public class Motion : MonoBehaviour
    {
        public float speed;
        private Rigidbody rig;
        // Start is called before the first frame update
        void Start()
        {
            Camera.main.enabled = false;

            rig = GetComponent<Rigidbody>();
        }

        
        void FixedUpdate()
        {
            float t_hmove = Input.GetAxisRaw("Horizontal");
            float t_vmove = Input.GetAxisRaw("Vertical");

            Vector3 t_direction = new Vector3(t_hmove, 0, t_vmove);
            t_direction.Normalize();

            rig.velocity = transform.TransformDirection(t_direction) * speed * Time.deltaTime;    
        }
    }
}