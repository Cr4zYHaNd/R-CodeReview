using System.Collections;
using UnityEngine;

public interface IKOPotential
{
    public void ApplyKOP(PlayerKOHandler player);

    public void StartIgnoring(PlayerKOHandler playerToIgnore);

    public IEnumerator IgnorePlayer(PlayerKOHandler playerToIgnore);


}
