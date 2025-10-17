using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

// Component on a UI element that will expand/shrink
// as the pointer enters/exits its space respectively.
[RequireComponent(typeof(GraphicRaycaster))]
public class HandDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] Vector2 NeutralPosition = new(-30, 0);
    [SerializeField] Vector2 HoveredPosition = new(0, 0);
    [SerializeField] Vector2 NeutralScale = new(600, 50);
    [SerializeField] Vector2 HoveredScale = new(700, 100);
    [SerializeField] float mousePanSpeed = 2f;

    float mousePanTargetAlpha, mousePanAlpha;
    private IEnumerator cScaler, cPanner;
    private float scaleSpeed = 4;
    private float neutralDelayPeriod = .25f;


    private void OnValidate()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = NeutralScale;
        rectTransform.anchoredPosition = NeutralPosition;
    }

    private IEnumerator Rescale(bool neutral)
    {
        float alpha = 0;
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (neutral)
        {
            while (alpha < 1)
            {
                yield return null;
                alpha += Time.deltaTime / neutralDelayPeriod;
            }
            alpha = 0;
        }
        while (alpha < 1)
        {
            yield return null;
            alpha += Time.deltaTime * scaleSpeed * (neutral ? 1f : 1.5f);
            alpha = Mathf.Min(alpha, 1);
            rectTransform.sizeDelta = neutral ? Vector2.Lerp(HoveredScale, NeutralScale, alpha) : Vector2.Lerp(NeutralScale, HoveredScale, alpha);
            rectTransform.anchoredPosition = neutral ? Vector2.Lerp(HoveredPosition, NeutralPosition, alpha) : Vector2.Lerp(NeutralPosition, HoveredPosition, alpha);
        }

    }

    private IEnumerator MousePan()
    {
        while (mousePanAlpha != mousePanTargetAlpha)
        {
            yield return null;
            bool lower = mousePanAlpha < mousePanTargetAlpha;
            mousePanAlpha += Time.deltaTime * (mousePanTargetAlpha - mousePanAlpha) * mousePanSpeed;
            mousePanAlpha = lower ? Mathf.Min(mousePanAlpha, mousePanTargetAlpha) : Mathf.Max(mousePanAlpha, mousePanTargetAlpha);
            Debug.Log(mousePanAlpha);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cScaler != null)
        {
            StopCoroutine(cScaler);
        }
        cScaler = Rescale(false);
        StartCoroutine(cScaler);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (cScaler != null)
        {
            StopCoroutine(cScaler);
        }
        if (cPanner != null)
        {
            StopCoroutine(cPanner);
        }
        cScaler = Rescale(true);
        StartCoroutine(cScaler);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        RectTransform rt = GetComponent<RectTransform>();

        mousePanTargetAlpha = ((eventData.position - (Vector2)rt.position) / rt.sizeDelta).x;
        if (cPanner == null)
        {
            cPanner = MousePan();
            StartCoroutine(cPanner);
        }
    }
}
