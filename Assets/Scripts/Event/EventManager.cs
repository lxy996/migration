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
        // ����λ�ú;���󲿶ӵľ�������������¼�����
        EventType eventType = DetermineEventType(distanceFromMainGroup);
        GameEvent gameEvent = GetEvent(eventType);

        // �����¼�Ч��
        ProcessEvent(gameEvent);
    }

    EventType DetermineEventType(int distanceFromMainGroup)
    {
        int diceRoll = Random.Range(1, 21); // ģ��һ��20�������

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
        // �����¼����ʹ��¼����л�ȡ�¼�
        return new GameEvent(); // ����Ӧ����ʵ�ʵ��¼�ʵ��
    }

    void ProcessEvent(GameEvent gameEvent)
    {
        // �����¼����ͺ�Ч������
        // ���磬������Դ���޸�С��״̬��
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
    // �¼��ľ��嶨��
}
