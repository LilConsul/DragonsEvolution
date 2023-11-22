using UnityEngine;

namespace Scenes.Scripts.Globals {
    public class GlobalSettings : MonoBehaviour {
        public static GlobalSettings Instance;

        public bool spawnNewDragons;
        public bool spawnNewChickens;
        
        public bool gameIsOnline;
        public float delayTime;
        
        public int basicHealth;
        public int basicSpeed;
        public int basicIntellect;

        public int minChicken;
        public int maxChicken;

        public uint fieldSize;
        
        private void Awake() {
            Instance = this;
        }
    }
}
