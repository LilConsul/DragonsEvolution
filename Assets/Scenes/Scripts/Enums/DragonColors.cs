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
        [SerializeField] private Sprite red;
        
        [SerializeField] private Sprite orangeEgg;
        [SerializeField] private Sprite pinkEgg;
        [SerializeField] private Sprite purpleEgg;
        [SerializeField] private Sprite greenEgg;
        [SerializeField] private Sprite blueEgg;
        [SerializeField] private Sprite redEgg;

        
        [SerializeField] private Sprite dead;
        
        
        public Sprite Get(Colors color) {
            var colorMap = new Dictionary<Colors, Sprite> {
                { Colors.Orange, orange },
                { Colors.Pink, pink },
                { Colors.Purple, purple },
                { Colors.Green, green },
                { Colors.Blue, blue },
                { Colors.Red, red },
                { Colors.Dead, dead}
            };
            return colorMap[color];
        }

        public Sprite GetEgg(Colors color) {
            var eggMap = new Dictionary<Colors, Sprite>() {
                { Colors.Orange, orangeEgg },
                { Colors.Pink, pinkEgg },
                { Colors.Purple, purpleEgg },
                { Colors.Green, greenEgg },
                { Colors.Blue, blueEgg },
                { Colors.Red, redEgg }
            };
            return eggMap[color];
        }

        public Colors RandomColor() {
            var randomColor = (Colors)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Colors)).Length - 1);
            if (randomColor == Colors.Dead)
                return RandomColor();
            return randomColor;
        }
    }
}