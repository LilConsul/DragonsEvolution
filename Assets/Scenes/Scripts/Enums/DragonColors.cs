using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Scripts.Enums {
    [Serializable]
    public struct DragonColors {
        [SerializeField] private Sprite orange;
        [SerializeField] private Sprite pink;
        [SerializeField] private Sprite purple;
        [SerializeField] private Sprite green;
        [SerializeField] private Sprite blue;
        [SerializeField] private Sprite player;

        private Dictionary<Colors, Sprite> colorMap;

        private void Start() {
            colorMap = new Dictionary<Colors, Sprite> {
                { Colors.Orange, orange },
                { Colors.Pink, pink },
                { Colors.Purple, purple },
                { Colors.Green, green },
                { Colors.Blue, blue },
                { Colors.Player, player }
            };
        }

        public Sprite Get(Colors color) {
            return colorMap[color];
        }
    }
}