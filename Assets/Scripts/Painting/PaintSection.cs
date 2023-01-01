using System;
using Interfaces;
using UnityEngine;

namespace Painting
{
    public class PaintSection : MonoBehaviour, ITouchable
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private new SpriteRenderer renderer;

        public void OnBeganTouchHandler()
        {
            source.Play();
            renderer.color = PaintController.GetColor();
        }

        public void OnMoveTouchHandler(Vector3 position)
        {
            
        }

        public void OnMoveTouchHandler()
        {
            
        }

        public void OnEndTouchHandler()
        {
            
        }
    }
}