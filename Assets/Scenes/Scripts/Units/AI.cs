using Scenes.Scripts.Field;
using UnityEngine;

namespace Scenes.Scripts.Units {
    public class AI : MonoBehaviour{
        private double[,] weights;
        private BotDragon _dragon;
        private FieldContainer _container;

        public bool SetInitialization(BotDragon dragon, FieldContainer container) {
            var dragons = container.GetUnitsField<BotDragon>();
            (int x, int y) = dragon.Cords();
            if (dragons[x, y] == null){
                Debug.LogWarning("No dragon found on this field!");
                return false;
            }
            _dragon = dragon;
            _container = container;
            return true;
        }

        
        
        public (int x, int y) GetNextMove() {
            return (0, 0);
        }
        
        
    }
}