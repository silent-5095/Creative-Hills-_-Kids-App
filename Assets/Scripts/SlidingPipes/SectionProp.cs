using System;
using UnityEngine;

namespace SlidingPipes
{
    public class SectionProp :MonoBehaviour
    {
        [SerializeField] private new string name;
        public GameObject gfx;
        public SectionType type;
        public bool isActive;
        public void SetActive() => gfx.SetActive(isActive);
    }
}