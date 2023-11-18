using Scenes.Scripts.Units;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Scenes.Scripts.Field {
    public class FieldGenerator : MonoBehaviour {
        public static FieldGenerator Instance;

        private Random _random;

        private void Awake() {
            Instance = this;
            _random = new Random();
        }

        public void GeneratePreset() {
            var container = FieldContainer.Instance;
            var dragon = gameObject.AddComponent<BotDragon>();
            dragon.Initialization(2, 2);
            container.Add(dragon);
            var chicken = gameObject.AddComponent<Chicken>();
            chicken.Initialization(4, 2, 15);
            container.Add(chicken);
            var chicken2 = gameObject.AddComponent<Chicken>();
            chicken2.Initialization(5, 2, 20);
            container.Add(chicken2);
            var chicken3 = gameObject.AddComponent<Chicken>();
            chicken3.Initialization(5, 3, 20);
            container.Add(chicken3);
        }

        public void CustomGenerator(int numDragons, int numFood) {
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
            FoodFactory.Instance.SpawnFood(numFood);
        }

        private int RandomCoordinate(int size) {
            return _random.Next(size);
        }
    }
}