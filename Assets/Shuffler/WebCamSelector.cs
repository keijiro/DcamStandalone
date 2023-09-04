using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public sealed class WebCamSelector : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public RenderTexture Destination { get; set; }

    #endregion

    #region Private objects

    WebCamTexture _webcam;

    #endregion

    #region UI callbacks

    async void SelectDevice(string name)
    {
        if (_webcam != null) Destroy(_webcam);

        _webcam = new WebCamTexture(name);
        _webcam.Play();

        while (_webcam.width < 32) await Awaitable.NextFrameAsync();
    }

    #endregion

    #region MonoBehaviour implementation

    async Awaitable Start()
    {
        await Application.RequestUserAuthorization(UserAuthorization.WebCam);

        var doc = GetComponent<UIDocument>();
        var selector = (DropdownField)doc.rootVisualElement.Q("Selector");

        var c_devs = WebCamTexture.devices.Select(dev => dev.name);
        var d_devs = WebCamTexture.devices.Select(dev => dev.depthCameraName);
        selector.choices = c_devs.Concat(d_devs.Where(s => s != null)).ToList();
        selector.RegisterValueChangedCallback(e => SelectDevice(e.newValue));
    }

    void Update()
    {
        if (_webcam == null) return;
        var vflip = _webcam.videoVerticallyMirrored;
        var scale = new Vector2(1, vflip ? -1 : 1);
        var offset = new Vector2(0, vflip ? 1 : 0);
        Graphics.Blit(_webcam, Destination, scale, offset);
    }

    #endregion
}
