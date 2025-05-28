using System;
using UnityEngine;

namespace SaveSystem.Data
{
    [Serializable]
    public struct UnitData
    {
        [SerializeField]
        public string _name;
        
        [SerializeField]
        public string _type;

        [SerializeField]
        public int _hitPoints;
        
        [SerializeField]
        public float _xPosition;
        [SerializeField]
        public float _yPosition;
        [SerializeField]
        public float _zPosition;
        [SerializeField]
        public float _xRotation;
        [SerializeField]
        public float _yRotation;
        [SerializeField]
        public float _zRotation;

        public UnitData(string name, string type, int hitPoints, Vector3 position, Vector3 rotation)
        {
            _name = name;
            _type = type;
            
            _hitPoints = hitPoints;
            
            _xPosition = position.x;
            _yPosition = position.y;
            _zPosition = position.z;
            
            _xRotation = rotation.x;
            _yRotation = rotation.y;
            _zRotation = rotation.z;
        }
    }
}