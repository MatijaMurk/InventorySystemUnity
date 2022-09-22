using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
  

    public void ToggleMenu(GameObject menu)
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }
    }
}
