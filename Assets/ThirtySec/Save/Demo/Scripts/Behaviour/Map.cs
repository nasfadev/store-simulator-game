using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ThirtySec
{
    public class Map : MonoBehaviour
    {
        public List<Cell> cells;

        private void Start()
        {
            if (MapData.instance.cellDatas.Count == 0)
            {
                CreateMap();
            }
            else
            {
                LoadMap();
            }
        }
        void CreateMap()
        {
            MapData.instance.cellDatas.Clear();
            for (int x = 0; x < cells.Count; x++)
            {
                var cd = new CellData();
                MapData.instance.cellDatas.Add(cd);
                cells[x].LoadData(cd);
            }

        }

        void LoadMap()
        {
            for (int x = 0; x < cells.Count; x++)
            {
                cells[x].LoadData(MapData.instance.cellDatas[x]);
            }
        }
    }
}