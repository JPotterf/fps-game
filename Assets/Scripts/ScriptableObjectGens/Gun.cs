using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Potterf.FpsGame
{
    [CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]

    public class Gun : ScriptableObject
    {
        #region Variables
        public new string name;
        public GameObject prefab;
        public float firerate;
        public float aimSpeed;
        //other gun attributes here
        #endregion

    }
}