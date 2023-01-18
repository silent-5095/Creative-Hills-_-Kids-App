using System;
using UnityEngine;

namespace SlidingPipes
{
    public class SlidingController : MonoBehaviour
    {
        [SerializeField] private GameObject matrix3In3, matrix4In4, matrix5In5;
        [SerializeField] private MatrixType matrixType;


        private void OnDestroy()
        {
            Path.EndAnimationEvent -= OnWinEvent;
        }

        private void Start()
        {
            Path.EndAnimationEvent+= OnWinEvent;
            //for test
            switch (matrixType)
            {
                case MatrixType.ThreeMultiply:

                    matrix3In3.SetActive(true);
                    break;
                case MatrixType.ForMultiply:

                    matrix4In4.SetActive(true);
                    break;
                case MatrixType.FiveMultilpy:

                    matrix5In5.SetActive(true);
                    break;
            }

            ;
        }

        private void OnWinEvent()
        {
            InGameController.Instance.Win();
        }
    }

    public enum MatrixType
    {
        ThreeMultiply,
        ForMultiply,
        FiveMultilpy
    }
}