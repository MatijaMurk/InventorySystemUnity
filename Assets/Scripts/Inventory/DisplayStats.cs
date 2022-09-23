using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour
{
    [SerializeField] private PlayerModifications player;
    [SerializeField] private GameObject statsText;
    [SerializeField] RectTransform contentPanel;
    private HorizontalLayoutGroup[] children;
    private void Start()
    {
        CreateStats();
    }

    public void CreateStats()
    {
        for (int i = 0; i < player.attributes.Length; i++)
        {
            var obj = Instantiate(statsText, Vector3.zero, Quaternion.identity, transform);
            obj.transform.SetParent(contentPanel);
            obj.transform.GetChild(0).GetComponent<Text>().text = player.attributes[i].type.ToString();
            obj.transform.GetChild(1).GetComponent<Text>().text = (player.attributes[i].value.BaseValue + player.attributes[i].value.ModifiedValue).ToString();

        }
    }
    public void UpdateStatsText()
    {
            int i = 0;
            children = contentPanel.GetComponentsInChildren<HorizontalLayoutGroup>();
            foreach(var child in children)
            {
                child.transform.GetChild(1).GetComponent<Text>().text = (player.attributes[i].value.BaseValue + player.attributes[i].value.ModifiedValue).ToString();
                i++;
            }
        
    }
}
