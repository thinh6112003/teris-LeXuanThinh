using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fill : MonoBehaviour
{
    public int maxValue;
    public Image fill;
    private int currentValue;
    // Start is called before the first frame update
    void Start()
    {
        currentValue = maxValue;
        fill.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //fill.fillMethod = Image.FillMethod.Horizontal;
        if (Input.GetKeyDown(KeyCode.A))
            Add(10);
        if (Input.GetKeyDown(KeyCode.B))
            Deduct(10);
    }
    public void Add(int i)
    {
        currentValue += i;
        if (currentValue > maxValue)
            currentValue = maxValue;
        fill.fillAmount = (float)currentValue / maxValue;
    }
    public void Deduct(int i)
    {
        currentValue -= i;
        if (currentValue < 0)
            currentValue = 0;
        fill.fillAmount = (float)currentValue / maxValue;
    }
}
