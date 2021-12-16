using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tmpSetSortOrder : MonoBehaviour
{
    public TextMeshPro tmp;
    void Start()
    {
        tmp.sortingOrder = 2;
    }

}
