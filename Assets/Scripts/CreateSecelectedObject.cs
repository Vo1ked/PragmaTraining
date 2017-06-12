using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSecelectedObject : MonoBehaviour
{
    public ObjectInfo ItemType;
    public ObjectManager _objectManager;
	// Use this for initialization
	void Start ()
	{
	    _objectManager = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<ObjectManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateObject()
    {
        _objectManager.CreateNewItemByType(ItemType);
        _objectManager.Status.text = "Secect Cell For Your new Object\nYou Choose " + ItemType.ObjectType + " by Cell Size " + ItemType.Size +
                                "X" + ItemType.Size;
        transform.GetComponentInParent<SwicthActivity>().SwicthActivityInHierarhy();
    }
}
