using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ThirtySec
{
    [System.Serializable]
    public class CellData
    {
        public int value;

    }

    public class MapData : ThirtySec.Serializable<MapData>
    {
        public List<CellData> cellDatas = new List<CellData>();



    }
}