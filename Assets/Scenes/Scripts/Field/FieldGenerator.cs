using System;
using Scenes.Scripts.Dragon;
using Scenes.Scripts.Food;

namespace Scenes.Scripts.Field {
    public class FieldGenerator {
        private Random _random = new Random();
        
        public void GenerateEasy(ref FieldContainer container) {
            CustomGenerator(ref container, 5, 15);
        }

        private void CustomGenerator(ref FieldContainer container , int numDragons, int numFood) {
            var size = container.Size();
            for (var i = 0; i < numDragons; i++) {
                var x = RandomCoordinate(size);
                var y = RandomCoordinate(size);
                if (!container.Add(new BotDragon(x, y)))
                    i--;
            }
            
            // Generate Food
            for (var i = 0; i < numFood; i++) {
                var x = RandomCoordinate(size);
                var y = RandomCoordinate(size);
                var calories = RandomCalories();
                if (!container.Add(new Chicken(x, y, calories)))
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