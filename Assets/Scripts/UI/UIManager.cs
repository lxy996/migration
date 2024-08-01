using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text foodText;
    public Text waterText;
    public Text staminaText;

    public void UpdateTeamStatus(int food, int water, int stamina)
    {
        foodText.text = "Food: " + food;
        waterText.text = "Water: " + water;
        staminaText.text = "Stamina: " + stamina;
    }

    public void ShowEventOptions(Event gameEvent)
    {
        // ��ʾ�¼�ѡ������ѡ��
    }
}