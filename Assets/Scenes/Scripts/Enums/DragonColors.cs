using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.Scripts.Enums {
    [Serializable]
    public struct DragonColors {
        [SerializeField] private Sprite orange;
        [SerializeField] private Sprite pink;
        [SerializeField] private Sprite purple;
        [SerializeField] private Sprite green;
        [SerializeField] private Sprite blue;
        [SerializeField] private Sprite red;
        [SerializeField] private Sprite dead;
        
        private Dictionary<Colors, Sprite> _colorMap;
        
        public Sprite Get(Colors color) {
            _colorMap = new Dictionary<Colors, Sprite> {
                { Colors.Orange, orange },
                { Colors.Pink, pink },
                { Colors.Purple, purple },
                { Colors.Green, green },
                { Colors.Blue, blue },
                { Colors.Red, red },
                { Colors.Dead, dead}
            };
            return _colorMap[color];
        }

        public Colors RandomColor() {
            var randomColor = (Colors)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Colors)).Length - 1);
            if (randomColor == Colors.Dead)
                return RandomColor();
            return randomColor;
        }
    }
}