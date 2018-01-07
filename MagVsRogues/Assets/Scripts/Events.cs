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
    public PowerUpType Type;

    public OnPowerUpCreated(PowerUpType type)
    {
        Type = type;
    }
}

public class OnPowerUpUsed : IGameEvent
{
    public PowerUpType Type;

    public OnPowerUpUsed(PowerUpType type)
    {
        Type = type;
    }
}

public class OnPowerUpReset : IGameEvent
{
}