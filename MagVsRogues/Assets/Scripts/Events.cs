using UnityEngine;
using System.Collections;

public class IGameEvent
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

public class OnPowerUpCreated : IGameEvent
{
    public int ID;
    public PowerUpType Type;

    public OnPowerUpCreated(int id, PowerUpType type)
    {
        ID = id;
        Type = type;
    }
}

public class OnPowerUpUsed : IGameEvent
{
    public int ID;

    public OnPowerUpUsed(int powerUpId)
    {
        ID = powerUpId;
    }
}