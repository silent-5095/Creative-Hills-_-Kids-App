using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Kelerjiman
{
    public class AssetManager : MonoBehaviour
    {
        [SerializeField] private List<AssetReference> bundles;
        [SerializeField] private AssetReference mainAssetHandler;
        [SerializeField] private Slider slider;

        [SerializeField] private GameObject errorPanel;

        //[SerializeField] private ReconnectPanel reconnectPanel;
        [SerializeField] private Button reconnectBtn, skipButton, exitButton;
        [SerializeField] private TextMeshProUGUI text, percent;
        private AsyncOperationHandle _handle;
        private bool _isFirstTime = true, _connected;
        private int _bundleIndex = 0;


        // ReSharper disable once PossibleLossOfFraction
        private void Update()
        {
            slider.value = _handle.IsValid() ? _handle.PercentComplete : 0;
            // percent.text = Mathf.RoundToInt(_handle.IsValid() ? _handle.PercentComplete * 100 : 0).ToString();
            if (_handle.IsValid())
                percent.text = Mathf.RoundToInt(_handle.PercentComplete * 100).ToString();
        }

        private void OnDestroy()
        {
            // Profile.Instance.CheckConnectionEvent -= OnCheckConnectionEvent;
        }

        private void Awake()
        {
            // Profile.Instance.CheckConnectionEvent += OnCheckConnectionEvent;

            reconnectBtn.onClick.AddListener(() => StartCoroutine(DownloadDependency(GetScene)));
        }

        private void Start()
        {
            _isFirstTime = !PlayerPrefs.HasKey(nameof(_isFirstTime));
            exitButton.gameObject.SetActive(_isFirstTime);
            skipButton.gameObject.SetActive(!_isFirstTime);
            skipButton.onClick.AddListener(OnSkipButton);
            exitButton.onClick.AddListener(Application.Quit);
            StartCoroutine(DownloadDependency(GetScene));
        }

        // private void GetBundle()
        // {
        //     _handle = mainAssetHandler.InstantiateAsync();
        //     _handle.Completed += handle =>
        //     {
        //         text.text = $"downloading dataPack_{_bundleIndex} is Completed !";
        //         PlayerPrefs.SetInt(nameof(_isFirstTime), 1);
        //         Invoke(nameof(GetScene), 1f);
        //     };
        // }

        private void GetScene()
        {
            ForDemo.Instance.LoadScene("FirstScene");
        }

        private IEnumerator DownloadDependency(Action act)
        {
            yield return Addressables.InitializeAsync();

            text.text = $"Initializing {bundles[_bundleIndex].SubObjectName}";
            if (_isFirstTime && !_connected)
            {
                errorPanel.SetActive(true);
                yield break;
            }

            _handle = new AsyncOperationHandle();
            _handle = Addressables.GetDownloadSizeAsync(bundles[_bundleIndex]);
            yield return _handle;
            if (int.TryParse(_handle.Result?.ToString(), out var rTemp) && rTemp > 0)
            {
                _handle = Addressables.DownloadDependenciesAsync(bundles[_bundleIndex]);
                text.text = $"downloading _ {bundles[_bundleIndex].SubObjectName}";
                while (_handle.Status == AsyncOperationStatus.None)
                {
                    yield return null;
                }

                if (_handle.Status == AsyncOperationStatus.Failed)
                {
                    text.text = "Error On Download";
                    errorPanel.SetActive(true);
                    yield break;
                }
                else
                {
                    _bundleIndex++;
                }
            }
            else
            {
                _bundleIndex++;
            }

            if (_bundleIndex >= bundles.Count - 1)
                act?.Invoke();
            else
            {
                StartCoroutine(DownloadDependency(act));
            }
        }

        private void OnSkipButton()
        {
            GetScene();
        }
    }
}