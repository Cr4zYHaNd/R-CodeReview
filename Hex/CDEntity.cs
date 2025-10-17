using UnityEngine;

public abstract class CDEntity<T> : CardData where T : EntityData
{
    [SerializeField] protected T data;

}
