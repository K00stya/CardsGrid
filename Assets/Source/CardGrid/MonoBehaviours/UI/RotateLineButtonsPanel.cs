using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateLineButtonsPanel : MonoBehaviour
{
    public GameObject Button;

    public void LoadButtons(List<Image> buttons, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            buttons.Add(Instantiate(Button, transform).GetComponent<Image>());
        }
    }
}
