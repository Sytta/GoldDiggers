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
    public float Cooldown;

    public OnPowerUpCreated(PowerUpType type, float cooldown)
    {
        Type = type;
        Cooldown = cooldown;
    }

    public OnPowerUpCreated(PowerUpType type)
    {
        Type = type;
        Cooldown = 0.0f;
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