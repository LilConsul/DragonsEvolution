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
        
        private void Awake() {
            Instance = this;
        }
    }
}
