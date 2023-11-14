using Scenes.Scripts.Units;
using UnityEngine;
using Random = System.Random;

namespace Scenes.Scripts.Field {
    public class FieldGenerator : MonoBehaviour {
        private Random _random;

        private void Awake() {
            _random = new Random();
        }

        public void GenerateEasy() {
            CustomGenerator( 50, 15);
        }

        private void CustomGenerator(int numDragons, int numFood) {
            var container = FieldContainer.Instance;
            var size = container.Size();
            
            // Generate Dragons
            for (var i = 0; i < numDragons; i++) {
                var x = RandomCoordinate(size);
                var y = RandomCoordinate(size);
                var dragon = gameObject.AddComponent<BotDragon>();
                dragon.Initialization(x, y);
                if (!container.Add(dragon))
                    i--;
            }

            // Generate Food
            for (var i = 0; i < numFood; i++) {
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
            return _random.Next(100, 500);
        }
    }
}