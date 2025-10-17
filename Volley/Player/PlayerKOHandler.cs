using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKOHandler : MonoBehaviour
{

    private int KOP;
    public Action dizzied;
    public Action fainted;
    public Action snapOut;
    public Action woken;
    public Action knocked;
    private Coroutine dizzyTimer, inactionTimer;
    private bool impossibleRecovery;
    private bool hasted;

    private bool dizzy;

    // Start is called before the first frame update
    public void Init()
    {
        KOP = 0;
    }

    public void HasteToggle()
    {
        hasted = !hasted;
    }

    public void AddKOP()
    {
        if (hasted || dizzy)
        {
            return;
        }



        KOP++;
        knocked?.Invoke();

        if (KOP == 3)
        {
            dizzied?.Invoke();
            dizzy = true;
            OnDizzy();
            return;
        }

        dizzy = false;

    }

    public void AddKOP(int pointsToAdd)
    {
        for (int i = 0; i < pointsToAdd; i++)
        {
            AddKOP();
            if (dizzy)
            {
                i = pointsToAdd;
            }
        }
    }

    public void OnRecovery()
    {
        KOP = 0;
    }

    public void OnDizzy()
    {

        dizzyTimer = StartCoroutine(DizzyTimer());

    }

    private IEnumerator DizzyTimer()
    {
        StartCoroutine(RecoveryTimer());
        inactionTimer = StartCoroutine(InactionTimer());

        yield return new WaitForSeconds(4);

        if (dizzy)
        {
            StartFaint();
        }

    }

    private IEnumerator RecoveryTimer()
    {
        impossibleRecovery = false;

        yield return new WaitForSeconds(2 + Mathf.Epsilon);

        impossibleRecovery = true;

    }

    public void InstaKO()
    {

        StartFaint();
        for (int i = 3 - KOP; i > 0; i--)
        {
            knocked?.Invoke();
        }
    }

    private IEnumerator InactionTimer()
    {
        yield return new WaitForSeconds(2);

        dizzy = false;
        StopCoroutine(dizzyTimer);
        snapOut?.Invoke();
        OnRecovery();
    }

    public void StartFaint()
    {
        StartCoroutine(Faint());
    }

    private IEnumerator Faint()
    {
        fainted?.Invoke();

        yield return new WaitForSeconds(4);

        WakeUp();
    }

    private void WakeUp()
    {
        woken?.Invoke();
        OnRecovery();
        impossibleRecovery = false;
    }

    public void ActEarly()
    {
        StopCoroutine(inactionTimer);

        if (!impossibleRecovery)
        {
            inactionTimer = StartCoroutine(InactionTimer());
        }

    }


}
