using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanInfoPanel : MonoBehaviour {

    public void CleanPanelInfo()
    {
        gameObject.GetComponent<Text>().text = "";
    }
}
