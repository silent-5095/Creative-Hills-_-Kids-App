using System;
using UnityEngine;

namespace SlidingPipes
{
    public class SlidingController : MonoBehaviour
    {
        [SerializeField] private GameObject matrix3In3,matrix4In4,matrix5In5;

        private void Start()
        {
            //for test
            matrix3In3.SetActive(true);
        }
    }
}
