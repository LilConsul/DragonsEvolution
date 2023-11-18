using System;
using Scenes.Scripts.Units;
using UnityEngine;
using Random = System.Random;

namespace Scenes.Scripts.Field {
    [Serializable]
    public class FoodFactory : MonoBehaviour{
        private Random _random;
        public static FoodFactory Instance;

        [SerializeField] private int minCalories;
        [SerializeField] private int maxCalories;
        private void Awake() {
            Instance = this;
            _random = new Random();
        }
        
        public void SpawnFood(int amount = 1) {
            var container = FieldContainer.Instance;
            var size = container.Size();
            for (var i = 0; i < amount; i++) {
                var x = RandomCoordinate(size);
                var y = RandomCoordinate(size);
                var calories = RandomCalories();
                var chicken = gameObject.AddComponent<Chicken>();
                chicken.Initialization(x, y, calories);
                if (!container.Add(chicken))
                    i--;
            }
        }
        
        private int RandomCoordinate(int size) {
            return _random.Next(size);
        }
        
        private int RandomCalories() {
            return _random.Next(minCalories, maxCalories);
        }
    }
}