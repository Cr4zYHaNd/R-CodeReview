using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

// The preview pane - A little viewport in the Editor Window that shows the visual output of
// the focused flipbook. I learned alot about Unity's preview render utility pipeline in the
// days I spent on this.
public class FlipbookEditorPreviewPane : VisualElement, IPopulatable, IUpdatable
{
    private Image previewPort;
    private FlipbookPreviewer preview;
    public FlipbookPreviewer PreviewObject { get => preview; }

    private PreviewRenderUtility _previewRenderUtility;
    private FloatField currentTimeField, fullTimeField;
    bool playing;
    float playbackRate = 1;
    public Action onPopulated, onPaused, onPlayed;
    public Action<float> onAdjust;

    public void OnDisable()
    {
        onPaused = null;
        onPlayed = null;
        onPopulated = null;
        onAdjust = null;
        if (_previewRenderUtility == null)
        {
            return;
        }
        _previewRenderUtility.Cleanup();
        _previewRenderUtility = null;
    }

    public FlipbookEditorPreviewPane()
    {
        style.width = 700;

        previewPort = new();
        previewPort.style.alignSelf = Align.Center;
        previewPort.style.width = 600;
        previewPort.style.height = 400;

        // Playback Control bar is created and setup here
        VisualElement PlaybackControlBar = new();

        // First create all buttons
        Button toStartButton = new()
        {
            text = "|<"
        };

        toStartButton.clicked += () => preview.SetProgressRaw(0);

        Button rewindButton = new()
        {
            text = "<<"
        };

        Button pauseButton = new()
        {
            text = "Pause"
        };
        Button playButton = new()
        {
            text = "Play"
        };

        Button fastFwdButton = new()
        {
            text = ">>"
        };

        Button toEndButton = new()
        {
            text = ">|"
        };

        onPaused = () =>
        {
            playing = false;
            playButton.SetEnabled(true);
            pauseButton.SetEnabled(false);
        };

        toEndButton.clicked += () =>
            {
                preview.SetProgressRaw(preview.Length);
                currentTimeField.value = preview.ProgressRaw;
                onPaused?.Invoke();
            };

        pauseButton.clicked += onPaused;

        onPlayed = () =>
        {
            playing = true;
            playButton.SetEnabled(false);
            pauseButton.SetEnabled(true);
        };

        rewindButton.clicked += () =>
        {
            if (!playing)
            {
                playbackRate = -1;
                onPlayed?.Invoke();
                return;
            }

            playbackRate -= .5f;
            playbackRate = Mathf.Max(-3, playbackRate);

            rewindButton.SetEnabled(playbackRate > -3);
            fastFwdButton.SetEnabled(playbackRate < 3);

        };

        fastFwdButton.clicked += () =>
        {
            if (!playing)
            {
                playbackRate = 1;
                onPlayed.Invoke();
                return;
            }

            playbackRate += .5f;
            playbackRate = Mathf.Min(3, playbackRate);

            rewindButton.SetEnabled(playbackRate > -3);
            fastFwdButton.SetEnabled(playbackRate < 3);
        };

        playButton.clicked += onPlayed;

        PlaybackControlBar.Add(toStartButton);
        PlaybackControlBar.Add(rewindButton);
        PlaybackControlBar.Add(playButton);
        PlaybackControlBar.Add(pauseButton);
        PlaybackControlBar.Add(fastFwdButton);
        PlaybackControlBar.Add(toEndButton);

        PlaybackControlBar.style.flexDirection = FlexDirection.Row;
        PlaybackControlBar.style.alignSelf = Align.Center;
        PlaybackControlBar.style.height = 50;
        PlaybackControlBar.style.top = 15;
        PlaybackControlBar.style.bottom = 10;


        VisualElement PlaybackScrubber = new();
        PlaybackScrubber.style.alignSelf = Align.Center;
        PlaybackScrubber.style.width = 700;
        PlaybackScrubber.style.height = 20;
        PlaybackScrubber.style.top = 5;
        PlaybackScrubber.style.flexDirection = FlexDirection.Row;

        currentTimeField = new();
        currentTimeField.SetEnabled(false);

        currentTimeField.style.width = 40;

        fullTimeField = new();
        fullTimeField.SetEnabled(false);

        fullTimeField.style.width = 40;

        Slider scrubControl = new()
        {
            lowValue = 0,
            highValue = 1
        };
        scrubControl.style.width = 620;

        scrubControl.RegisterValueChangedCallback((change) =>
        {
            onPaused?.Invoke();
            currentTimeField.SetValueWithoutNotify(preview.Length * change.newValue);
            preview.Preview(currentTimeField.value);
            onAdjust?.Invoke(currentTimeField.value);
            _previewRenderUtility.BeginPreview(previewPort.contentRect, GUIStyle.none);
            _previewRenderUtility.Render();
            previewPort.image = _previewRenderUtility.EndPreview();
        });

        currentTimeField.RegisterValueChangedCallback((change) =>
        {
            scrubControl.SetValueWithoutNotify(currentTimeField.value / fullTimeField.value);
        });

        PlaybackScrubber.Add(currentTimeField);
        PlaybackScrubber.Add(fullTimeField);
        PlaybackScrubber.Add(scrubControl);

        Add(previewPort);
        Add(PlaybackScrubber);
        Add(PlaybackControlBar);

        onPopulated += () =>
        {
            toStartButton.SetEnabled(true);
            rewindButton.SetEnabled(true);
            playButton.SetEnabled(!playing);
            pauseButton.SetEnabled(playing);
            fastFwdButton.SetEnabled(true);
            toEndButton.SetEnabled(true);
            fullTimeField.value = preview.Length;
            currentTimeField.value = 0;
            scrubControl.SetEnabled(true);
        };
    }

    public void Populate(Flipbook sourceClip)
    {
        if (_previewRenderUtility != null)
        {
            _previewRenderUtility.Cleanup();
            _previewRenderUtility = null;
        }

        _previewRenderUtility = new();
        preview = new GameObject("Preview Object").AddComponent<FlipbookPreviewer>();
        preview.Init(sourceClip);
        preview.Preview(0);
        _previewRenderUtility.AddSingleGO(preview.gameObject);

        _previewRenderUtility.camera.transform.position = Vector3.back * 10;

        _previewRenderUtility.BeginPreview(previewPort.contentRect, GUIStyle.none);
        _previewRenderUtility.Render();
        previewPort.image = _previewRenderUtility.EndPreview();

        onPopulated?.Invoke();
    }
    public void Update()
    {

        if (_previewRenderUtility == null)
        {
            return;
        }

        if (playing)
        {
            preview.ForwardPreview(playbackRate);
            currentTimeField.value = preview.Progress;
            onAdjust?.Invoke(currentTimeField.value);
        }

        _previewRenderUtility.BeginPreview(previewPort.contentRect, GUIStyle.none);
        _previewRenderUtility.Render();
        previewPort.image = _previewRenderUtility.EndPreview();

        if (playing)
        {
            return;
        }

        preview.UpdateCallTime();
    }

    public void ChangePreviewTime(float deltaTime)
    {
        float newVal = currentTimeField.value + deltaTime;
        newVal = Mathf.Clamp(newVal, 0, fullTimeField.value);
        currentTimeField.value = newVal;
        onPaused?.Invoke();
        preview.Preview(currentTimeField.value);
        _previewRenderUtility.BeginPreview(previewPort.contentRect, GUIStyle.none);
        _previewRenderUtility.Render();
        previewPort.image = _previewRenderUtility.EndPreview();
    }

}
