using System.Collections;
using UnityEngine;
using UnityEditor;

// A component for GameObjects that will read Flipbooks. It takes a flipbook and reads all its
// sentences by running a coroutine per sentence. This in turn animates the property
// corresponding to each sentence. There is an alternative reading method for editor previewing
public class FlipbookReader : MonoBehaviour
{
    private Flipbook current;
    private Coroutine[] sightLines;
    [Range(0, 15)]
    [SerializeField] private float localPlaybackSpeed = 1f;

    //Method for Starting the process of reading through flipbook
    public void StartRead(Flipbook toRead, bool loop)
    {
        current = toRead;

        sightLines = new Coroutine[current.Sentences.Count];

        int n = 0;

        //Iterate through each sentence in the flipbook
        foreach (FlipbookSentence sentence in current.Sentences)
        {
            //EACH SENTENCE NEEDS TO HAVE A COPY INSTANTIATED, OTHERWISE MULTIPLE INSTANCES OF THE SAME ANIMATION CANNOT CO-EXIST
            sightLines[n] = StartCoroutine(ReadSentence(Instantiate(sentence), loop));
            n++;
        }
    }

    [ExecuteInEditMode]
    //Method for Starting the process of reading through flipbook
    public IEnumerator[] StartReadInEditor(Flipbook toRead, bool loop)
    {
        current = toRead;

        IEnumerator[] processes = new IEnumerator[current.Sentences.Count];

        int n = 0;

        //Iterate through each sentence in the flipbook
        foreach (FlipbookSentence sentence in current.Sentences)
        {
            //EACH SENTENCE NEEDS TO HAVE A COPY INSTANTIATED, OTHERWISE MULTIPLE INSTANCES OF THE SAME ANIMATION CANNOT CO-EXIST
            processes[n] = ReadSentence(Instantiate(sentence), loop);
            n++;
        }

        return processes;
    }



    //Read sentence coroutine uses elapse time to calculate new position in playback
    private IEnumerator ReadSentence(FlipbookSentence sentence, bool loop)
    {
        sentence.Init(gameObject);
        float t = sentence.StartSentence();
        while (t > 0)
        {

            yield return null;

            t -= Time.deltaTime * localPlaybackSpeed;

            if (t > 0)
            {
                continue;
            }

            //loop to catch skipped frames
            float newT;

            do
            {
                do
                {
                    //returns time left in next event
                    newT = sentence.OnCurrentTimeout(loop);

                    //if event is meant to be immediate (0-time), loop through until the next non-immediate frame
                } while (newT == 0);

                t += newT;
            } while (t <= 0);
        }
    }

    public void StopReading()
    {
        current = null;
        foreach (Coroutine sightline in sightLines)
        {
            StopCoroutine(sightline);
        }
    }


}
