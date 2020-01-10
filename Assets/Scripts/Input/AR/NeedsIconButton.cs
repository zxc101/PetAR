using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Helpers;

public class NeedsIconButton : MonoBehaviour
{
    [SerializeField] private Color offColor;
    [SerializeField] private Color onColor;

    private Sprite icon;
    private Image image;

    public Transform Need;
    public Sprite Icon { private get { return icon; } set { SetInoc(value); } }

    public bool IsChoice {
        set
        {
            if (value)
            {
                image.color = onColor;
                Helper.ChoicesNeeds = Need;
            }
            else
            {
                image.color = offColor;
            }
        }
    }

    private void SetInoc(Sprite sprite)
    {
        icon = sprite;
        GetComponent<Image>().sprite = sprite;
    }

    private void Start()
    {
        image = GetComponent<Image>();
        foreach (Transform child in transform.parent)
        {
            child.GetComponent<NeedsIconButton>().IsChoice = false;
        }
    }

    public void Choise()
    {
        foreach(Transform child in transform.parent)
        {
            child.GetComponent<NeedsIconButton>().IsChoice = false;
        }
        IsChoice = true;
    }
}
