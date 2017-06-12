using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{

    private List<ObjectInfo> _itemTypePull;
    private List<ObjectInfo> _createdItemPull;

    [SerializeField]private RectTransform StoreItemContainer;
    [SerializeField]private RectTransform ItemInShopPrefab;
    [SerializeField]private RectTransform Canvas;

    public Text Status;

    [SerializeField]private Transform ContainerCreatedItemsByPlayer;

    [SerializeField]private GameObject[] PrefabsPull;

    [SerializeField]private Texture[] TexturesPull;

    [SerializeField]private uint ScaleAllObjectby;
    private uint _createdObject = 1;

    private bool _itemIsCreated;

	// Use this for initialization
	void Start () {
        _itemTypePull = new List<ObjectInfo>();
        _createdItemPull = new List<ObjectInfo>();
	    for (uint i = 1; i <= ScaleAllObjectby; i++)
	    {
            AddAllObjectsInListScaledby(i);
        }
        InstantiateItemsInContainer();

    }

    private void AddAllObjectsInListScaledby(uint scale)
    {
        for (int i = 0; i < PrefabsPull.Length; i++)
        {
            var item = new ObjectInfo
            {
                Prefab = PrefabsPull[i],
                Texture = TexturesPull[i],
                Size = scale,
                ObjectType = PrefabsPull[i].name.Substring(0, PrefabsPull[i].name.Length - 3)
            };
            _itemTypePull.Add(item);
        }
    }

    private void InstantiateItemsInContainer()
    {
        const int numberOfRows = 2;
        const int distanceBetweenItem = 10;
        var posX = 10f;
        var posY = -10f;

        var allItemLenght = (ItemInShopPrefab.rect.height + distanceBetweenItem)
                            *_itemTypePull.Count/numberOfRows;
        StoreItemContainer.sizeDelta = new Vector2(allItemLenght + distanceBetweenItem - Canvas.rect.width,
            StoreItemContainer.sizeDelta.y);
        for (int i = 0; i < _itemTypePull.Count; i++)
        {
            if (i == _itemTypePull.Count/numberOfRows)
            {
                posY = -120;
                posX = 10;
            }
            var newItem = Instantiate(ItemInShopPrefab, StoreItemContainer);
            newItem.anchoredPosition3D = new Vector3(posX, posY, 0);
            newItem.GetComponent<RawImage>().texture = _itemTypePull[i].Texture;
            newItem.transform.GetChild(0).GetComponent<Text>().text = "" + _itemTypePull[i].ObjectType +
                                                                      _itemTypePull[i].Size + " X " +
                                                                      _itemTypePull[i].Size;
            newItem.GetComponent<CreateSecelectedObject>().ItemType = new ObjectInfo
            {
                Prefab = _itemTypePull[i].Prefab,
                Texture = _itemTypePull[i].Texture,
                Size = _itemTypePull[i].Size,
                ObjectType = _itemTypePull[i].ObjectType
            };
            posX += newItem.sizeDelta.x + distanceBetweenItem;
        }
    }

    public void CreateNewItemByType(ObjectInfo itemType)
    {

        itemType.ObjectCode = itemType.ObjectType + _createdObject;
        _createdItemPull.Add(itemType);
        _itemIsCreated = true;
    }

    public void ChoosedItemIsCanseled()
    {
        if(_itemIsCreated == false) return;
        _createdItemPull.Remove(_createdItemPull.Last());
        _itemIsCreated = false;
        Status.text = "";
    }

    public void ItemInfo(BaseEventData eventData)
    {
        var eventDataPosition = ((PointerEventData) eventData).pressPosition;
        Ray ray = Camera.main.ScreenPointToRay(eventDataPosition);
        RaycastHit hit;
        if (_itemIsCreated ) return;
        if (!Physics.Raycast(ray, out hit, 100f)) return;
        if (hit.collider.tag != "Cell") return;
        var coordinate = hit.transform.localPosition;
        if (!_createdItemPull.Exists(x => x.Coordinate == coordinate)) return;
        var item = _createdItemPull.Find(x => x.Coordinate == coordinate);
        Status.text = "Object Indentificator: " + item.ObjectCode + "  Type: " + item.ObjectType + "  Size: "
                      + item.Size + "  Coordinate: " + item.Coordinate;

    }

    public void CheckPositionToNewItem(BaseEventData eventData)
    {
        var eventDataPosition = ((PointerEventData) eventData).pressPosition;
        Ray ray = Camera.main.ScreenPointToRay(eventDataPosition);
        RaycastHit hit;


        if (!_itemIsCreated) return;
        if (!Physics.Raycast(ray, out hit, 100f)) return;
        if (hit.collider.tag != "Cell")
        {
            Status.text = "Out of Grid";
            return;
        }
        if (_createdItemPull.Count > 1 && _createdItemPull.Exists(x => x.Coordinate == hit.transform.position))
        {
            Status.text = "Sorry this cell already Contain another Object";
            return;
        }
        if (hit.transform.localScale.x < _createdItemPull.Last().Size)
        {
            Status.text = "Choose Biger Cell For This Object";
            return;
        }
        if (hit.transform.localScale.x > _createdItemPull.Last().Size)
        {
            Status.text = "Choose Smaller Cell For This Object";
            return;
        }

        var newItem = Instantiate(_createdItemPull.Last().Prefab, ContainerCreatedItemsByPlayer);
        newItem.transform.localPosition = hit.transform.position;
        newItem.transform.localScale *= _createdItemPull.Last().Size;
        DeactivateColider(newItem);
        _createdItemPull.Last().ObjectCode = _createdItemPull.Last().ObjectType + _createdObject;
        _createdObject++;
        _createdItemPull.Last().Coordinate = hit.transform.position;
        _itemIsCreated = false;
        Status.text = "";
    }

    private void DeactivateColider(GameObject obj)
    {
       var allColidersInObject = obj.GetComponents<Collider>();
        foreach (var coll in allColidersInObject)
        {
            coll.enabled = false;
        }
    }
}

public class ObjectInfo
{
    [SerializeField] public GameObject Prefab;
    [SerializeField] public Texture Texture;
    [SerializeField] public uint Size;
    [SerializeField] public string ObjectCode="null";
    [SerializeField] public string ObjectType = "null";
    [SerializeField] public Vector3 Coordinate = Vector3.zero;

}
