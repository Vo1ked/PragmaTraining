using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwicthActivity : MonoBehaviour
{
    private bool _switch;
    public void SwicthActivityInHierarhy()
    {
        GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
        _switch = !_switch;
        gameObject.SetActive(_switch);
    }
}
