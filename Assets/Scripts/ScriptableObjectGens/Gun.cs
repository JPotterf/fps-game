﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.Potterf.FpsGame
{
    [CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]

    public class Gun : ScriptableObject
    {
        #region Variables
        public new string name;
        public int damage;
        public GameObject prefab;
        public float firerate;
        public float aimSpeed;

        public float bloom;
        public float recoil;
        public float kickback;
        //other gun attributes here
        #endregion

    }
}