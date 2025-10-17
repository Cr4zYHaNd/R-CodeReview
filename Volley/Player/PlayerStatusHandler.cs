using System;
using System.Collections;
using UnityEngine;


public class PlayerStatusHandler : MonoBehaviour
{
    private bool[] statusFlags;
    public Action SpeedBoost, SpeedDrop, INSTAKO, NoJump, ExtraJump, Confused, Featherfallen, Hasted, Blessed;
    public void Init()
    {
        statusFlags = new bool[9] { false, false, false, false, false, false, false, false, false };
    }

    public void ApplyStatus(Status.StatusType status)
    {
        StartCoroutine(StatusEffect(status));

    }

    private IEnumerator StatusEffect(Status.StatusType status)
    {

        float time = StartStatus(status);
        if (time > 0)
        {
            yield return new WaitForSeconds(time);

            EndStatus(status);
        }

        statusFlags[(int)status] = false;
    }

    private float StartStatus(Status.StatusType status)
    {
        if (statusFlags[(int)status])
        {

            Debug.Log(status);
            return 0;
        }

        statusFlags[(int)status] = true;

        switch (status)
        {
            case Status.StatusType.SpeedBoost:
                //Speed Boost
                SpeedBoost?.Invoke();
                return 5;
            case Status.StatusType.SpeedDrop:
                //Speed decrease
                SpeedDrop?.Invoke();
                return 5;
            case Status.StatusType.InstaKO:
                INSTAKO?.Invoke();
                return 0;
            case Status.StatusType.NoJump:
                NoJump?.Invoke();
                return 5;
            case Status.StatusType.ExtraJump:
                ExtraJump?.Invoke();
                return 8;
            case Status.StatusType.FeatherFall:
                Featherfallen?.Invoke();
                return 8;
            case Status.StatusType.Confused:
                Confused?.Invoke();
                return 5;
            case Status.StatusType.Hasted:
                Hasted?.Invoke();
                return 6;
            case Status.StatusType.Blessed:
                Blessed?.Invoke();
                return 6;
            default:
                return 0;
        }

    }
    private void EndStatus(Status.StatusType status)
    {
        if (statusFlags[(int)status] == true)
        {

            statusFlags[(int)status] = false;

            switch (status)
            {
                case Status.StatusType.SpeedBoost:
                    //Speed Boost
                    SpeedBoost?.Invoke();
                    break;
                case Status.StatusType.SpeedDrop:
                    //Speed decrease
                    SpeedDrop?.Invoke();
                    break;
                case Status.StatusType.InstaKO:
                    INSTAKO?.Invoke();
                    break;
                case Status.StatusType.NoJump:
                    NoJump?.Invoke();
                    break;
                case Status.StatusType.ExtraJump:
                    ExtraJump?.Invoke();
                    break;
                case Status.StatusType.FeatherFall:
                    Featherfallen?.Invoke();
                    break;
                case Status.StatusType.Confused:
                    Confused?.Invoke();
                    break;
                case Status.StatusType.Hasted:
                    Hasted?.Invoke();
                    break;
                case Status.StatusType.Blessed:
                    Blessed?.Invoke();
                    break;
            }

        }

    }

    public void Consume(Ball_PowerUp powerUp)
    {

        powerUp.ConsumeMe(this);

    }



    public bool CheckStatus(int flagNumber)
    {

        return statusFlags[flagNumber];

    }
}
