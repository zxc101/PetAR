using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pets;

public class InitNeedsUI : MonoBehaviour
{
    [SerializeField] private Pet pet;
    [SerializeField] private RectTransform iconButton;
    [SerializeField] private RectTransform iconsContent;
    [SerializeField] private RectTransform statSlider;
    [SerializeField] private RectTransform statsContent;
    
    private List<RectTransform> statsList = new List<RectTransform>();

    void Start()
    {
        for (int i = 0; i < pet.Needs.Length; i++)
        {
            InitIcon(pet.Needs[i].icon, pet.Needs[i].prefab);
            InitStat(pet.Needs[i].maxValue, pet.Needs[i].icon);
        }
        InitIcon(pet.PointsIcon, pet.Point);
    }

    private void InitIcon(Sprite sprite, Transform prefab)
    {
        RectTransform icon = Instantiate(iconButton, iconsContent);
        icon.GetComponent<NeedsIconButton>().Icon = sprite;
        icon.GetComponent<NeedsIconButton>().Need = prefab;
    }

    private void InitStat(float max, Sprite icon)
    {
        RectTransform stat = Instantiate(statSlider, statsContent);
        stat.GetComponent<StatSlider>().Icon = icon;
        stat.GetComponent<StatSlider>().MaxVal = max;
        statsList.Add(stat);
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < pet.Needs.Length; i++)
        {
            statsList[i].GetComponent<StatSlider>().Value = pet.Needs[i].value;
        }
    }
}
