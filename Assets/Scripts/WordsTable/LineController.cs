using System;
using UnityEditor;
using UnityEngine;

namespace WordsTable
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineController : MonoBehaviour
    {
        [SerializeField] private LineRenderer line;

        public void AddStartingNode(Vector3 sPos)
        {
            sPos.z = 1;
            line.transform.position = sPos;
            sPos.x = 0;
            sPos.y = 0;
            line.positionCount = 2;
            line.SetPositions(new[] {sPos, sPos});
        }

        public void SetColor(Color color)
        {
            line.startColor = color;
            line.endColor = color;
        }

        public void MoveLastPoint(Vector3 newPos)
        {
            var defPos = transform.position;
            newPos -= defPos;
            newPos.z = 1;
             // newPos.x -= defPos.x;
             // newPos.y -= defPos.y;
            line.SetPosition(line.positionCount - 1, newPos);
        }

        public Vector3 GetLastPos()
        {
            return line.GetPosition(line.positionCount - 1);
        }
    }
}