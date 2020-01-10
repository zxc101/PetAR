using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Helpers;
using Pets;

public class StatSlider : MonoBehaviour
{
    [SerializeField] private Text textValue;
    [SerializeField] private Image icon;
    [SerializeField] private Image fillImage;

    private float val;
    private float maxVal;
    private Vector4 color = new Vector4(255/255, 255/255, 0, 1);

    public Sprite Icon { set { icon.sprite = value; } }
    public float MaxVal { private get => maxVal; set => maxVal = value; }

    public float Value {
        set
        {
            val = value;
            ColorUpdate(val);
        }
    }

    private void ColorUpdate(float val)
    {
        if(val > 50)
        {
            fillImage.color = Helper.VectorCreater(5.1f * (50 - (Helper.ValueToPercent(val, MaxVal) - 50)) / 255, 1, 0, 1);
        }
        else if(val == 50)
        {
            fillImage.color = Helper.VectorCreater(1, 1, 0, 1);
        }
        else
        {
            fillImage.color = Helper.VectorCreater(1, 5.1f * Helper.ValueToPercent(val, MaxVal) / 255, 0, 1);
        }
        textValue.text = val.ToString();
    }
}
