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

    void SelectDevice(string name)
    {
        if (_webcam != null) Destroy(_webcam);
        _webcam = new WebCamTexture(name);
        _webcam.Play();
    }

    #endregion

    #region MonoBehaviour implementation

    async Awaitable Start()
    {
        await Application.RequestUserAuthorization(UserAuthorization.WebCam);

        var doc = GetComponent<UIDocument>();
        var list = (DropdownField)doc.rootVisualElement.Q("Selector");
        list.choices = WebCamTexture.devices.Select(dev => dev.name).ToList();
        list.RegisterValueChangedCallback(e => SelectDevice(e.newValue));
    }

    void Update()
    {
        if (_webcam == null || _webcam.width < 32) return;

        var aspect1 = (float)_webcam.width / _webcam.height;
        var aspect2 = (float)Destination.width / Destination.height;

        var scale = new Vector2(aspect2 / aspect1, aspect1 / aspect2);
        scale = Vector2.Min(Vector2.one, scale);
        if (_webcam.videoVerticallyMirrored) scale.y *= -1;

        var offset = (Vector2.one - scale) / 2;

        Graphics.Blit(_webcam, Destination, scale, offset);
    }

    #endregion
}
