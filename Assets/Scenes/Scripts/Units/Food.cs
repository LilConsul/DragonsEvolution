using UnityEngine;

namespace Scenes.Scripts.Units {
    public class Food : MonoBehaviour {
        private int _x;
        private int _y;
        private int _calories;
        
        public void Initialization(int x, int y, int calories) {
            _x = x;
            _y = y;
            _calories = calories;
        }

        public (int x, int y) Cords() => (_x, _y);
        
        public int GetCalories() {
            return _calories;
        }
    }
}