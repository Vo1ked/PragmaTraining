using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGeneratorWithRandomSizeCells : MonoBehaviour
{
    [SerializeField] private GameObject Cell;

    public uint PlaneSize;
    public uint MaxCellSize;

    private int _cellSize;

    private Vector3 _spawnPosition;

    private bool _gridIsCreating;

    void Awake ()
    {
        var planesize = (int)PlaneSize;
        var maxCellsize = (int) MaxCellSize +1;
       _spawnPosition = new Vector3(-planesize, 0f , -planesize);
        _gridIsCreating = true;
        while (_gridIsCreating)
        {
            if (_spawnPosition.x >= planesize)
            {
                _spawnPosition = new Vector3(-planesize, 0f, _spawnPosition.z + 1f);
                if (_spawnPosition.z >= planesize)
                {
                    _gridIsCreating = false;
                }
            }
            _cellSize = Random.Range(1, maxCellsize);
            CheckSpaceForCellReduceSizeIfFalse();
            if (_cellSize > 0)
            {
                var cellPosition = _spawnPosition + new Vector3(_cellSize/2f, 0,
                                       _cellSize/2f);
              var newCell = Instantiate(Cell, transform,false);
                newCell.transform.localScale = new Vector3(_cellSize,0.2f,_cellSize);
                newCell.transform.localPosition = cellPosition;
                newCell.name ="Cell " + _cellSize + "x" + _cellSize + " P[" + cellPosition.x + ":" + cellPosition.z + "]";
                _spawnPosition += new Vector3(_cellSize, 0, 0);

            }
        }
    }

    private void CheckSpaceForCellReduceSizeIfFalse()
    {
        var retry = true;
        while (retry)
        {
            var sizeChanged = false;
            var rayCastPosition = _spawnPosition + new Vector3(_cellSize/2f , 0, 0.5f);
            var rightUpCornerCell = _spawnPosition + new Vector3(_cellSize - 0.5f ,0,_cellSize - 0.4f);
            if (_cellSize == 0)
            {
                _spawnPosition += new Vector3(1, 0, 0);
                break;
            }

            if (rightUpCornerCell.x > PlaneSize || rightUpCornerCell.z > PlaneSize)
            {
                _cellSize--;
                continue;
            }

            if (Physics.CheckBox(rayCastPosition, new Vector3(_cellSize /2f - 0.1f, 0.1f, 0.4f)))
            {
                sizeChanged = true;
                _cellSize--;
            }


            if (sizeChanged == false)
            {
                retry = false;
            }
        } ;

    }
}
