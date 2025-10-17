using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Autoreader component - this should be a bool on the Flipbook Reader
// does express how the system can be extended to have scripted reading
// of different flipbooks, whether its a component based workflow or by
// some other means.

[RequireComponent(typeof(FlipbookReader))]
public class Autoread : MonoBehaviour
{
    [SerializeField] Flipbook toRead;
    [SerializeField] bool loop;
    private void Awake()
    {
        GetComponent<FlipbookReader>().StartRead(toRead, loop);
    }
}
