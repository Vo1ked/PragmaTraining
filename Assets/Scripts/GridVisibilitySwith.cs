using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisibilitySwith : MonoBehaviour {
    private float _moveDistance = 0.2f;


    public void GridVisibilitySwitch()
    {
        _moveDistance *= -1;
        transform.position += new Vector3(0,_moveDistance,0);
    }
}
