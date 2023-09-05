using UnityEngine;

public sealed class Configurator : MonoBehaviour
{
    #region Editable fields

    [SerializeField] string[] _prompts = null;
    [SerializeField] Color _titleColor = Color.white;
    [SerializeField] Color _overlayColor = Color.white;
    [SerializeField] float _lifetime = 60 * 60;

    #endregion

    #region Public accessors for input handlers

    public void SetPromptByIndex(int index)
      => _target.Prompt = _prompts[index];

    public void SetPrefilter(int index)
      => _target.Prefilter = index;

    public void InsertionLength(float param)
      => _target.InsertionCount = (int)(param * 5 + 1);

    public void SetStrengthAndStepCount(float param)
    {
        _target.Strength = 0.3f + param * 0.4f;
        _target.StepCount = (int)(12 - param * 4);
    }

    public void SetTitleOpacity(float opacity)
    {
        var color = _titleColor;
        color.a *= opacity;
        _target.TitleColor = color;
    }

    public void SetOverlayOpacity(float opacity)
    {
        var color = _overlayColor;
        color.a *= opacity;
        _target.OverlayColor = color;
    }

    public float AudioSensitivity { get; set; }

    #endregion

    #region Audio input to noise level

    float _audioLevel;

    public float AudioLevel { get => _audioLevel; set => SetAudioLevel(value); }

    void SetAudioLevel(float level)
    {
        _audioLevel = level;
        _target.NoiseLevel = level * AudioSensitivity;
    }

    #endregion

    #region MonoBehaviour implementation

    Shuffler _target;

    async void Start()
    {
        _target = FindFirstObjectByType<Shuffler>();

        await Awaitable.WaitForSecondsAsync(_lifetime);
        Application.Quit(1);
    }

    #endregion
}
