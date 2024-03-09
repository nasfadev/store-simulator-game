using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public struct CashierData
{
    public int level;
    public int prefabID;
    public float rotation;
    public SerializableVector3 position;
    [System.Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
    public CashierData(int newLevel, int newPrefab, float rot, Vector3 pos)
    {
        level = newLevel;
        prefabID = newPrefab;
        rotation = rot;
        position = new SerializableVector3(pos);
    }
}
