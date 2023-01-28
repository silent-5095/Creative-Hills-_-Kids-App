using System;
using UnityEngine;
using UnityEngine.UI;

namespace Magazine
{
    public class MagazineController : MonoBehaviour
    {
        [SerializeField] private GameObject landScape, lSBackButton, portrait, pBackButton;
        [SerializeField] private Image landScapePrefab, portraitPrefab;
        [SerializeField] private Sprite[] pages;
        [SerializeField] private Transform landScapeCHolder, portraitCHolder;

        private void Update()
        {
            
            if (Screen.orientation == ScreenOrientation.Portrait ||
                Screen.orientation == ScreenOrientation.PortraitUpsideDown && !portrait.activeSelf)
            {
                landScape.SetActive(false);
                lSBackButton.SetActive(false);
                portrait.SetActive(true);
                pBackButton.SetActive(true);
            }
            else if (Screen.orientation == ScreenOrientation.LandscapeLeft ||
                     Screen.orientation == ScreenOrientation.LandscapeRight && !landScape.activeSelf)
            {
                landScape.SetActive(true);
                lSBackButton.SetActive(true);
                portrait.SetActive(false);
                pBackButton.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            // Screen.orientation = ScreenOrientation.LandscapeLeft;
            // Screen.autorotateToPortrait = false;
        }

        private void Start()
        {
            ForDemo.Instance.ChangeOrientation(ScreenOrientation.AutoRotation);
            Screen.autorotateToPortrait = true;
            foreach (var page in pages)
            {
                var img = Instantiate(landScapePrefab, landScapeCHolder);
                img.sprite = page;
                img = Instantiate(portraitPrefab, portraitCHolder);
                img.sprite = page;
            }
        }
        //
        // private void FixedUpdate()
        // {
        //     if (Screen.orientation != ScreenOrientation.AutoRotation)
        //         Screen.orientation = ScreenOrientation.AutoRotation;
        // }
    }
}