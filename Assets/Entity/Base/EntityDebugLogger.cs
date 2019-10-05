using System;
using UnityEngine;

public interface IEntityDebugLogger
{
    void DebugString(string message);
   // void DebugAction(Action action);
    void DebugLine(Vector2 origin, Vector2 dest, Color color);
}

public class EntityDebugLogger : IEntityDebugLogger
{
    private BaseEntity _entity;
    public EntityDebugLogger(BaseEntity e)
    {
        _entity = e;
    }
    public void DebugString(string message)
    {
        if (_entity.DebugEntity)
            Debug.Log(message);
    }

    public void DebugAction(Action dAction)
    {
        if (_entity.DebugEntity)
            dAction.Invoke();
    }

    public void DebugLine(Vector2 origin, Vector2 dest, Color color)
    {
        if (!_entity.DebugEntity)
            return;
        Debug.DrawLine(origin, dest, color);

    }
}
