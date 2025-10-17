using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;

// This was one of my first attempts at UI/UX design and styling. I'd have beneifited from
// taking a closer look at how Unity implements its own editor UI to make a more informed
// set of decisions about how I made mine. What I ended up with thankfully became a lot
// less monolithic than earlier in the project. Alot of the tabs in the window are now
// represented as tabs.
//
// Another thing that would have been useful would be to establish what work is being done
// in what parts of the UI and then derive a class structure from that information.

public class FBEditorWindow : EditorWindow, IPopulatable
{

    private Flipbook TargetClip;
    private VisualElement TopHalf;

    [MenuItem("Window/FB Editor")]
    static void Init()
    {
        FBEditorWindow window = GetWindow<FBEditorWindow>();
        window.minSize = new(1600, 900);
        window.maxSize = new(1600, 900);
        window.Show();
    }

    private void OnDisable()
    {
        Repaint();
    }

    private void CreateGUI()
    {
        // Top Half Contains Details Panels and Preview Panel and Bottom Half is simply the Track Tab

        TopHalf = new();
        TopHalf.style.height = 500;
        TopHalf.style.flexDirection = FlexDirection.Row;

        rootVisualElement.Add(TopHalf);

        // Flipbook Details Pane is setup and event is bound to retarget focussed clip when the selection is changed

        FlipbookDetailsPane FBDPane = new();

        FBDPane.onSelectionChanged += Populate;

        TopHalf.Add(FBDPane);

        // Flipbook Preview Pane is setup and added to window

        FlipbookEditorPreviewPane FBPreview = new();

        TopHalf.Add(FBPreview);

        // Next, add the details pane for the focussed word

        FlipbookWordPane FBWordPane = new();
        TopHalf.Add(FBWordPane);

        // Finally, Sentence Track Tab is added
        SentenceTrackTab STT = new();

        STT.onSelect += (s) =>
        {
            FBWordPane.FocusSentence(s);
        };
        FBWordPane.onChangeMade += () =>
        {
            TargetClip.AlignAllSentences();
            Populate(TargetClip);
        };
        STT.onModification += () =>
        {
            TargetClip.AlignAllSentences();
            Populate(TargetClip);
        };
        STT.onScrub += (deltaTime) =>
        {
            FBPreview.onPaused?.Invoke();
            FBPreview.ChangePreviewTime(deltaTime);
            FBWordPane.UpdateTime(FBPreview.PreviewObject.ProgressRaw);
        };
        FBPreview.onAdjust += (delta) =>
        {
            STT.OnPreviewAdjusted(delta);
            FBWordPane.UpdateTime(FBPreview.PreviewObject.ProgressRaw);
        };

        rootVisualElement.Add(STT);


    }
    private void Update()
    {

        foreach (VisualElement child in rootVisualElement.Children())
        {
            if (child as IUpdatable == null)
            {
                continue;
            }

            ((IUpdatable)child).Update();
        }

        foreach (VisualElement child in TopHalf.Children())
        {
            if (child as IUpdatable == null)
            {
                continue;
            }

            ((IUpdatable)child).Update();
        }

    }
    public void Populate(Flipbook sourceClip)
    {
        TargetClip = sourceClip;
        foreach (VisualElement child in rootVisualElement.Children())
        {
            if (child as IPopulatable == null)
            {
                continue;
            }

            ((IPopulatable)child).Populate(sourceClip);
        }
        foreach (VisualElement child in TopHalf.Children())
        {
            if (child as IPopulatable == null)
            {
                continue;
            }

            ((IPopulatable)child).Populate(sourceClip);
        }

    }
}
