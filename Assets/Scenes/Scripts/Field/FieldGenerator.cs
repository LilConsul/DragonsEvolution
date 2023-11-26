using Scenes.Scripts.Units;
using UnityEngine;

namespace Scenes.Scripts.Field {
    public class FieldGenerator : MonoBehaviour {
        public static FieldGenerator Instance;
        
        private void Awake() {
            Instance = this;
        }

        public void GeneratePreset(int x, int y) {
            var container = FieldContainer.Instance;
            var dragon = gameObject.AddComponent<BotDragon>();
            dragon.Initialization(2, 2);
            container.Add(dragon);

            var chicken = gameObject.AddComponent<Food>();
            chicken.Initialization(2 + x, 2 + y, 50);
            container.Add(chicken);

        }

        public void CustomGenerator(int numDragons, int numFood) {
            DragonFactory.Instance.SpawnDragons(numDragons);
            FoodFactory.Instance.SpawnFood(numFood);
        }
    }
}