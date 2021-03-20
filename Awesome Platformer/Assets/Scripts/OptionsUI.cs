using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] GameObject container;

    public void ToggleVisibility()
    {
        container.SetActive(!container.activeSelf);
    }
}
