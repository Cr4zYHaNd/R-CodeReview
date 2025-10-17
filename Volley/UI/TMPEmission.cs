using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class TMPEmission : MonoBehaviour
{
    private TextMeshPro TMP;
    public Action<TMPEmission> expire;
    public Action risen;
    public void Init()
    {
        var renderer = gameObject.AddComponent<MeshRenderer>();
        TMP = gameObject.AddComponent<TextMeshPro>();
        TMP.rectTransform.sizeDelta = new Vector2(4, 1.5f);
        TMP.fontSize = 5;
        TMP.font = Resources.Load<TMP_FontAsset>("UI/Fonts/TMP Fonts/04B_30__ SDF");
        TMP.alpha = 0;
        TMP.alignment = TextAlignmentOptions.Center;
        renderer.sortingLayerName = "FX";
    }

    public void SetTMPText(string textToSet)
    {
        if (TMP != null)
        {
            TMP.text = textToSet;
        }
    }

    public IEnumerator LerpColor(Color target, float duration)
    {
        Color original = TMP.faceColor;
        float t = duration;
        while (t > 0)
        {
            yield return null;
            t -= Time.deltaTime;
            TMP.faceColor = Color.Lerp(target, original, (t / duration));
        }
        TMP.faceColor = original;
    }


    public IEnumerator LerpAlpha(float target, float duration)
    {
        float original = TMP.alpha;
        float t = duration;
        while (t > 0)
        {
            yield return null;
            t -= Time.deltaTime;
            TMP.alpha = Mathf.Lerp(target, original, (t / duration));
        }
        TMP.alpha = original;
    }

    private IEnumerator Rise(Vector3 origin, float distance, float duration, float waitTime)
    {
        float progress = 0;
        Vector3 u = origin;
        Vector3 v = origin + distance * Vector3.up;

        while (progress < duration)
        {
            progress += Time.deltaTime;
            progress = Mathf.Min(progress, duration);
            transform.position = Vector3.Lerp(u, v, progress / duration);
            yield return null;
        }
        yield return new WaitForSeconds(waitTime);

        risen?.Invoke();
    }

    private void Expire()
    {
        risen = null;
        expire?.Invoke(this);
        LerpAlpha(0, .2f);
        expire = null;
    }

    public void StartEmitter(Vector3 from)
    {
        StartCoroutine(LerpColor(Color.red, 0.5f));
        StartCoroutine(LerpAlpha(.6f, .4f));
        risen += Expire;

        StartCoroutine(Rise(from, 0.75f, 0.5f, 1f));

    }

}
