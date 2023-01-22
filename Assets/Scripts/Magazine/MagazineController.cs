using System;
using UnityEngine;

namespace Magazine
{
    public class MagazineController : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;

        private void Start()
        {
            mainMenu.SetActive(true);
        }
    }
}
