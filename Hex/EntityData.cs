using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    [SerializeField] protected string mName;
    public string Name { get => mName; set => mName = value; }
}
