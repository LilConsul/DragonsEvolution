using Scenes.Scripts.Units;
using Unity.VisualScripting;
using UnityEngine;

namespace Scenes.Scripts.Field {
    public class FieldGenerator : MonoBehaviour {
        public static FieldGenerator Instance;
        
        private void Awake() {
            Instance = this;
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
            DragonFactory.Instance.SpawnDragons(numDragons);
            FoodFactory.Instance.SpawnFood(numFood);
        }
    }
}