using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator : MonoBehaviour
{
    public uint BlindZone;
    public uint ForestWidth;
    public uint TreeCanBeScaledBy;
    [SerializeField] public GameObject[] TreePrefab;

    void Start()
    {
        var blindZone = (int) BlindZone;
        var forestWidth = (int) ForestWidth;
        var treeSize = (int)TreeCanBeScaledBy + 1;
        var endOfPlane = blindZone + forestWidth;
        var currentSpawnPosition = new Vector3(-endOfPlane , 0 , -endOfPlane);

        while (true)
        {
            if (currentSpawnPosition.x >= endOfPlane)
            {
                currentSpawnPosition.x = -endOfPlane;
                currentSpawnPosition.z++;
            }
            if (currentSpawnPosition.z >= endOfPlane)
            {
                break;
            }
            if (Mathf.Abs(currentSpawnPosition.x) <= blindZone && Mathf.Abs(currentSpawnPosition.z)<=blindZone)
            {
                currentSpawnPosition.x = blindZone;
            }

            var randomObj = Random.Range(0, TreePrefab.Length);
            var randomSize = Random.Range(1, treeSize);
            var objMesh = TreePrefab[randomObj].GetComponent<Renderer>().bounds;
            while (true)
            {
                var objExtendsScaledBySize = objMesh.extents*randomSize;
                var objectSpawnPositionScaledBysize = currentSpawnPosition +
                                             new Vector3(objExtendsScaledBySize.x, 0, objExtendsScaledBySize.z);
                if (randomSize <= 0)
                {
                    break;
                }
                if (objectSpawnPositionScaledBysize.x >= endOfPlane ||
                    objectSpawnPositionScaledBysize.z >= endOfPlane ||
                    (Mathf.Abs(objectSpawnPositionScaledBysize.x) <= blindZone &&
                     Mathf.Abs(objectSpawnPositionScaledBysize.z) <= blindZone))
                {
                    randomSize--;
                    continue;
                }
                if (Physics.CheckBox(objectSpawnPositionScaledBysize, objExtendsScaledBySize))
                {
                    randomSize--;
                    continue;
                }

                break;
            }
            if (randomSize == 0)
            {
                currentSpawnPosition.x++;
                continue;
            }
            var spawnedObject = Instantiate(TreePrefab[randomObj], transform, false);
            spawnedObject.transform.localScale *= randomSize; 
            spawnedObject.transform.localPosition = currentSpawnPosition + new Vector3(
                spawnedObject.GetComponent<Renderer>().bounds.extents.x , 0 ,
                spawnedObject.GetComponent<Renderer>().bounds.extents.z);
            currentSpawnPosition.x += spawnedObject.GetComponent<Renderer>().bounds.size.x;
        }

    }
}

