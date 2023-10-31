using UnityEngine;
using UnityEngine.UI;

public class UIDisplayRockAmount : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private void Update()
    {
        text.text = SpawnRockSystem.RockAmount.ToString("F2");
    }
}