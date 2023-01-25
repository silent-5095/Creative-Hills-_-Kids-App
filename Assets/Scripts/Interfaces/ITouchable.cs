using System;
using UnityEngine;

namespace Interfaces
{
    public interface ITouchable
    {
        void OnBeganTouchHandler();
        void OnMoveTouchHandler(Vector3 position);
        void OnStationaryTouchHandler(Vector3 position);
        void OnEndTouchHandler();
    }
}