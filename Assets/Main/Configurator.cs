using UnityEngine;

public sealed class Configurator : MonoBehaviour
{
    #region Editable fields

    [SerializeField] float _lifetime = 60 * 60;
    [SerializeField] string[] _prompts = null;
    [SerializeField] Color _overlayColor = Color.white;

    #endregion

    #region Private fields

    Shuffler _target;

    #endregion

    #region Randomizers

    async void RandomizePromptAsync()
    {
        while (true)
        {
            await Awaitable.WaitForSecondsAsync(Random.Range(10, 30));
            _target.Prompt = _prompts[Random.Range(0, _prompts.Length)];
        }
    }

    async void RandomizePrefilter()
    {
        while (true)
        {
            await Awaitable.WaitForSecondsAsync(Random.Range(10, 30));
            _target.Prefilter = Mathf.Max(0, Random.Range(-2, 4));
        }
    }

    async void RandomizeNoiseLevel()
    {
        while (true)
        {
            await Awaitable.NextFrameAsync();
            var n = Klak.Math.Noise.Fractal(Time.time * 0.8f, 3, 3245);
            _target.NoiseLevel = Mathf.Clamp01(n * 0.5f);
        }
    }

    async void RandomizeCameraMotion()
    {
        var target = FindFirstObjectByType<CameraController>();
        while (true)
        {
            await Awaitable.WaitForSecondsAsync(Random.Range(10, 30));
            target.Strength = Mathf.Clamp01(Random.value * 1.5f);
        }
    }

    async void RandomizeOverlay()
    {
        while (true)
        {
            await Awaitable.WaitForSecondsAsync(Random.Range(10, 30));
            var color = _overlayColor;
            color.a *= Mathf.Clamp01(Random.Range(-3.0f, 1.0f));
            _target.OverlayColor = color;
        }
    }

    #endregion

    #region MonoBehaviour implementation

    async void Start()
    {
        _target = FindFirstObjectByType<Shuffler>();

        RandomizePromptAsync();
        RandomizePrefilter();
        RandomizeNoiseLevel();
        RandomizeCameraMotion();
        RandomizeOverlay();

        await Awaitable.WaitForSecondsAsync(_lifetime);
        Application.Quit(1);
    }

    #endregion
}
