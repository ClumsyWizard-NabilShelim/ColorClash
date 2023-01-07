using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClumsyWizard.Utilities
{
    public class UIButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        private AudioManager audioManager;
        public bool audioOverride;
        public bool dontMoveChildren;
        private Dictionary<Transform, Vector3> children = new Dictionary<Transform, Vector3>();

        private void Awake()
        {
            if(!audioOverride)
                audioManager = GetComponent<AudioManager>();

            if (dontMoveChildren)
                return;

            for (int i = 0; i < transform.childCount; i++)
            {
                children.Add(transform.GetChild(i), transform.GetChild(i).localPosition);
            }
        }

        public void OnClick()
        {
            audioManager.Play("Click");

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (dontMoveChildren)
                return;

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localPosition = children[transform.GetChild(i)];
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (dontMoveChildren)
                return;

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localPosition -= new Vector3(0, 5f, 0);
            }
        }
    }
}
