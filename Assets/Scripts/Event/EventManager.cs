using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void TriggerEventAtPosition(Vector3 position, int distanceFromMainGroup)
    {
        // 根据位置和距离大部队的距离决定触发的事件类型
        EventType eventType = DetermineEventType(distanceFromMainGroup);
        GameEvent gameEvent = GetEvent(eventType);

        // 处理事件效果
        ProcessEvent(gameEvent);
    }

    EventType DetermineEventType(int distanceFromMainGroup)
    {
        int diceRoll = Random.Range(1, 21); // 模拟一个20面的骰子

        if (distanceFromMainGroup == 1)
        {
            if (diceRoll <= 5) return EventType.RewardEvent;
            else if (diceRoll <= 16) return EventType.NeutralEvent;
            else return EventType.DangerEvent;
        }
        else if (distanceFromMainGroup == 2)
        {
            if (diceRoll <= 7) return EventType.RewardEvent;
            else if (diceRoll <= 14) return EventType.NeutralEvent;
            else return EventType.DangerEvent;
        }
        else if (distanceFromMainGroup == 3)
        {
            if (diceRoll <= 9) return EventType.RewardEvent;
            else if (diceRoll <= 13) return EventType.NeutralEvent;
            else return EventType.DangerEvent;
        }
        else
        {
            if (diceRoll <= 9) return EventType.RewardEvent;
            else if (diceRoll <= 11) return EventType.NeutralEvent;
            else return EventType.DangerEvent;
        }
    }

    GameEvent GetEvent(EventType eventType)
    {
        // 根据事件类型从事件库中获取事件
        return new GameEvent(); // 这里应该有实际的事件实例
    }

    void ProcessEvent(GameEvent gameEvent)
    {
        // 根据事件类型和效果处理
        // 例如，调整资源，修改小队状态等
    }
}

public enum EventType
{
    RewardEvent,
    NeutralEvent,
    DangerEvent
}

public class GameEvent
{
    // 事件的具体定义
}
