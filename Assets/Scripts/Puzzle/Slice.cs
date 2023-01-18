using System;
using UnityEngine;

namespace Puzzle
{
    public class Slice : MonoBehaviour
    {
        [SerializeField] private InWorldDrop destination;
        [SerializeField] private InWorldDrag movingPart;
        [SerializeField] private SpriteRenderer renderer;

        private void Start()
        {
            SetData(renderer);
            renderer.enabled = false;
        }

        public void SetData(SpriteRenderer spRenderer)
        {
            destination.SetSprite(spRenderer);
            movingPart.SetSprite(spRenderer);
        }
    }
}