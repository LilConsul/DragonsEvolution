using System;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Units;
using UnityEngine;
using UnityEngine.Serialization;
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
                
                if (!container.Add(dragon)) i--;
                else if(_gameStarted) OnDragonAdded?.Invoke(dragon);
            }
        }

        public void SpawnSpecialDragon((int x, int y) cords, double newFood, double newSpeed,  double newIntelect, Colors color) {
            if (Random.Range(0f, 100f) > evolutionChance) {
                newFood += Random.Range(0f, evolutionRange);
                newSpeed += Random.Range(0f, evolutionRange);
                newIntelect += Random.Range(0f, evolutionRange);
            }
            else if(Random.Range(0f, 100f) > degradationChance){
                newFood -= Random.Range(0f, evolutionRange);
                newSpeed -= Random.Range(0f, evolutionRange);
                newIntelect -= Random.Range(0f, evolutionRange);
            }

            var dragon = gameObject.AddComponent<BotDragon>();
            dragon.Initialization(cords.x, cords.y);
            dragon.Color = color;
            dragon.Health = newFood;
            dragon.Speed = newSpeed;
            dragon.Intelect = newIntelect;
            
            FieldContainer.Instance.Add(dragon);
            if(_gameStarted) OnDragonAdded?.Invoke(dragon);
        }
        
        public void StartGame() => _gameStarted = true;
        
        private int RandomCoordinate(int size) {
            return Random.Range(0, size);
        }
    }
}