using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Potterf.FpsGame
{
    [CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]

    public class Gun : ScriptableObject
    {
        public new string name;
        public GameObject prefab;
        public float firerate;
        //other gun attributes here
    }
}