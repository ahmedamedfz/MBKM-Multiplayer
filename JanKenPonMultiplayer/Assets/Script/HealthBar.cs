using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
public class HealthBar : MonoBehaviour
    {
    public Image image;
    public void UpdateBar(float fillAmount)
    {
        image.DOFillAmount(fillAmount,0.5f);
        if(fillAmount > 0.5)
        {
            image.DOColor(Color.green,0.5f);
        }
        else if(fillAmount > 0.4)
        {
            image.DOColor(Color.cyan,0.5f);
        }
        else if(fillAmount > 0.2)
        {
            image.DOColor(Color.blue,0.5f);
        }
        else
        {
            image.DOColor(Color.yellow,0.5f);
        }
    }
    }
}
