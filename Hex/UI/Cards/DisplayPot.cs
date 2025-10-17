using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Monobehaviours that use a Graphic of type U to represent
// a piece of data of type T. Using generics here allows for
// extension of the types usable for this in future. It may
// however, be better to seperate the graphic component from
// the presentation of data itself, as doing it this way
// means that additional combinations create multiplicative
// work when this could and should scale linearly.

public abstract class DisplayPot<T, U> : MonoBehaviour where U : MaskableGraphic
{
    [SerializeField] protected U displayOfValue;
    protected T value;
    public T Value => value;

    public virtual void Init(T valueToDisplay, Transform parent)
    {
        transform.SetParent(parent);
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        if (displayOfValue == null)
        {
            Init(valueToDisplay, gameObject.AddComponent<U>());
        }
        Init(valueToDisplay, displayOfValue);
    }
    public virtual void Init(T valueToDisplay, U mediumToDisplayWith)
    {
        displayOfValue = mediumToDisplayWith;
        Assign(valueToDisplay);
    }

    public virtual void Assign(T newVal)
    {
        value = newVal;
    }


}

public class DP_ICD : DisplayPot<ICustomDisplayable, TextMeshProUGUI>
{
    public override void Assign(ICustomDisplayable newVal)
    {
        base.Assign(newVal);

        if (displayOfValue == null)
        {
            return;
        }

        displayOfValue.text = value.GetCustomDisplay();

    }


}

public class DP_Integer : DisplayPot<int, TextMeshProUGUI>
{
    public override void Init(int valueToDisplay, TextMeshProUGUI mediumToDisplayWith)
    {
        base.Init(valueToDisplay, mediumToDisplayWith);
        displayOfValue.autoSizeTextContainer = true;
        displayOfValue.alignment = TextAlignmentOptions.Center;
        displayOfValue.fontStyle |= TMPro.FontStyles.Bold;
    }
    public override void Assign(int newVal)
    {
        base.Assign(newVal);

        if (displayOfValue == null)
        {
            return;
        }

        displayOfValue.text = value.ToString();
    }
}