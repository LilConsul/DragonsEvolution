using System;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Globals;
using Scenes.Scripts.Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes.Scripts.Field {
    [Serializable]
    public class DragonFactory : MonoBehaviour{
        public static DragonFactory Instance;

        [SerializeField] private float evolutionChance;
        [SerializeField] private float degradationChance;
        [SerializeField] private float evolutionRange;
        public delegate void DragonAction(BotDragon sender);
        public event DragonAction OnDragonAdded;

        private bool _gameStarted;
        private void Awake() {
            Instance = this;
        }

        public void SpawnDragons(int numDragons) {
            var container = FieldContainer.Instance;
            var size = container.Size();
            
            for (var i = 0; i < numDragons; i++) {
                var x = RandomCoordinate(size);
                var y = RandomCoordinate(size);
                var dragon = gameObject.AddComponent<BotDragon>();
                dragon.Initialization(x, y);

                var settings = GlobalSettings.Instance;
                dragon.Intellect = settings.basicIntellect;
                dragon.Speed = settings.basicSpeed;
                dragon.Health = settings.basicHealth;
                
                if (!container.Add(dragon)) i--;
                else if(_gameStarted) OnDragonAdded?.Invoke(dragon);
            }
        }

        public void SpawnSpecialDragon((int x, int y) cords, double food, double speed,  double intellect, Colors color) {
            if (Random.Range(0f, 100f) > evolutionChance) {
                food += Random.Range(0f, evolutionRange);
                speed += Random.Range(0f, evolutionRange);
                intellect += Random.Range(0f, evolutionRange);
            }
            else if(Random.Range(0f, 100f) > degradationChance){
                food -= Random.Range(0f, evolutionRange);
                speed -= Random.Range(0f, evolutionRange);
                intellect -= Random.Range(0f, evolutionRange);
            }

            var dragon = gameObject.AddComponent<BotDragon>();
            dragon.Initialization(cords.x, cords.y);
            dragon.Color = color;
            dragon.Health = food;
            dragon.Speed = speed;
            dragon.Intellect = intellect;
            
            FieldContainer.Instance.Add(dragon);
            if(_gameStarted) OnDragonAdded?.Invoke(dragon);
        }
        
        public void StartGame() => _gameStarted = true;
        
        private int RandomCoordinate(int size) {
            return Random.Range(0, size);
        }
    }
}