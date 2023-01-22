using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle
{
    public class PuzzlePiecesHolder : MonoBehaviour
    {
        private List<Drag> _children;

        private void Start()
        {
            _children = GetComponentsInChildren<Drag>().ToList();
            var rand = new List<int>();

            for (var i = 0; i < _children.Count; i++)
            {
                rand.Add(i);
            }

            foreach (var t in _children)
            {
                var currentRand = Random.Range(0, rand.Count);

                t.transform.SetSiblingIndex(rand[currentRand]);
                rand.RemoveAt(currentRand);
            }
        }
    }
}