using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace SlidingPipes
{
    public class Section : MonoBehaviour, ITouchable
    {
        public static event Action MoveEvent;
        [SerializeField] private Section top, right, bottom, left;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private List<SectionProp> sectionList;
        [SerializeField] private SectionProp sectionProp;
        private bool _canTouch = true;
        public SectionProp SectionProp => sectionProp;

        private void Awake()
        {
            foreach (var section in sectionList.Where(gfx => gfx.gameObject.activeSelf))
            {
                sectionProp = section;
                break;
            }
        }

        private void Start()
        {
            Matrix.WinEvent += MatrixOnWinEvent;
        }

        private void MatrixOnWinEvent(bool con)
        {
            _canTouch = !con;
        }

        private void OnMove()
        {
            Debug.Log($"{sectionProp.type.ToString()}");
            if (sectionProp.type == SectionType.End || sectionProp.type == SectionType.Start || sectionProp.type== SectionType.Movable)
                return;
            if (top != null && top.SectionProp.type == SectionType.Movable)
            {
                sectionProp.isActive = false;
                top.SectionProp.isActive = false;
                background.enabled = false;
                top.background.enabled = true;
                var changeType = top.SectionProp.type;
                top.SectionProp.gameObject.SetActive(false);
                top.sectionProp = top.sectionList.Find(p => p.type == sectionProp.type);
                top.SectionProp.gameObject.SetActive(true);
                sectionProp.gameObject.SetActive(false);
                sectionProp = sectionList.Find(p => p.type == changeType);
                sectionProp.gameObject.SetActive(true);
                // sectionProp.isActive=true;
                top.SectionProp.isActive = true;
                MoveEvent?.Invoke();
            }
            else if (right != null && right.SectionProp.type == SectionType.Movable)
            {
                sectionProp.isActive = false;
                right.SectionProp.isActive = false;
                background.enabled = false;
                right.background.enabled = true;
                var changeType = right.SectionProp.type;
                right.SectionProp.gameObject.SetActive(false);
                right.sectionProp = right.sectionList.Find(p => p.type == sectionProp.type);
                right.SectionProp.gameObject.SetActive(true);
                sectionProp.gameObject.SetActive(false);
                sectionProp = sectionList.Find(p => p.type == changeType);
                sectionProp.gameObject.SetActive(true);
                // sectionProp.isActive=true;
                right.SectionProp.isActive = true;
                MoveEvent?.Invoke();
            }
            else if (bottom != null && bottom.SectionProp.type == SectionType.Movable)
            {
                sectionProp.isActive = false;
                bottom.SectionProp.isActive = false;
                background.enabled = false;
                bottom.background.enabled = true;
                var changeType = bottom.SectionProp.type;
                bottom.SectionProp.gameObject.SetActive(false);
                bottom.sectionProp = bottom.sectionList.Find(p => p.type == sectionProp.type);
                bottom.SectionProp.gameObject.SetActive(true);
                sectionProp.gameObject.SetActive(false);
                sectionProp = sectionList.Find(p => p.type == changeType);
                sectionProp.gameObject.SetActive(true);
                // sectionProp.isActive=true;
                bottom.SectionProp.isActive = true;
                MoveEvent?.Invoke();
            }
            else if (left != null && left.SectionProp.type == SectionType.Movable)
            {
                sectionProp.isActive = false;
                left.SectionProp.isActive = false;
                background.enabled = false;
                left.background.enabled = true;
                var changeType = left.SectionProp.type;
                left.SectionProp.gameObject.SetActive(false);
                left.sectionProp = left.sectionList.Find(p => p.type == sectionProp.type);
                left.SectionProp.gameObject.SetActive(true);
                sectionProp.gameObject.SetActive(false);
                sectionProp = sectionList.Find(p => p.type == changeType);
                sectionProp.gameObject.SetActive(true);
                // sectionProp.isActive=true;
                left.SectionProp.isActive = true;
                MoveEvent?.Invoke();
            }
        }

        public void OnBeganTouchHandler()
        {
            if (!_canTouch)
                return;
            Debug.Log("OnBeganTouch");
            OnMove();
        }

        public void OnMoveTouchHandler(Vector3 position)
        {
        }

        public void OnEndTouchHandler()
        {
        }
    }
}