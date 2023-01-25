using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Puzzle
{
    public class InWorldDrag : MonoBehaviour, ITouchable
    {
        public  event Action DropEvent;
        [SerializeField] private bool is3D;
        [SerializeField] private new SpriteRenderer renderer;
        [SerializeField] private new PolygonCollider2D collider2D;
        private List<Vector2>_physicsShape= new List<Vector2>();

        public void SetSprite(SpriteRenderer sp)
        {
            renderer.sprite = sp.sprite;
            renderer.sortingOrder = sp.sortingOrder+1;
            // renderer.sortingLayerID = sp.sortingLayerID;
            renderer.sprite.GetPhysicsShape(0, _physicsShape);
            collider2D.SetPath(0, _physicsShape);
        }

        public bool IsPlaced { get; set; }

        public void OnBeganTouchHandler()
        {
            
        }

        public void OnMoveTouchHandler(Vector3 position)
        {
            if (IsPlaced)
                return;
            
            var tempPos = position;
                
            var objectTransform = transform;
            
            tempPos.z = !is3D ? objectTransform.position.z : tempPos.z;
            objectTransform.position = tempPos;
        }

        public void OnStationaryTouchHandler(Vector3 position)
        {
            if (IsPlaced)
                return;
            
            var tempPos = position;
                
            var objectTransform = transform;
            
            tempPos.z = !is3D ? objectTransform.position.z : tempPos.z;
            objectTransform.position = tempPos;
        }

        public void OnEndTouchHandler()
        {
            if (IsPlaced)
                return;
            DropEvent?.Invoke();
        }
    }
}