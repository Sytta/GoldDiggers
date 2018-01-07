using UnityEngine;
using System.Collections;

public class IGameEvent
{
}

public class OnRoundStarted : IGameEvent
{
}

public class OnGoldModified : IGameEvent
{
    public int goldAmount;

    public OnGoldModified(int gold)
    {
        goldAmount = gold;
    }

    public OnGoldModified()
    {
        goldAmount = 0;
    }
}