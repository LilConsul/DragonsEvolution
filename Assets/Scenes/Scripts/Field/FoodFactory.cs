using System;
using Scenes.Scripts.Units;
using UnityEngine;
using Random = System.Random;

namespace Scenes.Scripts.Field {
    [Serializable]
    public class FoodFactory : MonoBehaviour{
        public static FoodFactory Instance;

        public delegate void FoodAction(Chicken sender);

        public event FoodAction OnFoodAdded;
        
        private Random _random;
        private bool _gameStarted;
        [SerializeField] private int minCalories;
        [SerializeField] private int maxCalories;
        private void Awake() {
            Instance = this;
            _random = new Random();
        }
        
        public void SpawnFood(int amount = 1) {
            var size = FieldContainer.Instance.Size();
            for (int i = 0; i < amount; i++) {
                var x = RandomCoordinate(size);
                var y = RandomCoordinate(size);
                var calories = RandomCalories();
                var chicken = gameObject.AddComponent<Chicken>();
                chicken.Initialization(x, y, calories);
                if (!FieldContainer.Instance.Add(chicken)) i--;
                else if(_gameStarted) OnFoodAdded?.Invoke(chicken);
            }
        }

        public void StartGame() => _gameStarted = true;
        
        private int RandomCoordinate(int size) {
            return _random.Next(size);
        }
        
        private int RandomCalories() {
            return _random.Next(minCalories, maxCalories);
        }
    }
}