using Scenes.Scripts.Enums;
using Scenes.Scripts.Field;
using UnityEngine;

namespace Scenes.Scripts.Units {
    public class AI : MonoBehaviour {
        private double[,] weights;
        private BotDragon _dragon;
        private BotDragon[,] _dragons;
        private Chicken[,] _chickens;

        public bool SetInitialization(BotDragon dragon) {
            if (dragon == null) {
                Debug.Log("No dragon on field!");
                return false;
            }
            var container = FieldContainer.Instance;

            weights = new double[container.Size(), container.Size()];
            
            var size = dragon.GetIntelligence();
            (int x, int y) = dragon.Cords();
            _dragon = dragon;
            _dragons = container.GetUnitsField<BotDragon>(x, y, size);
            _chickens = container.GetUnitsField<Chicken>(x, y, size);
            
            return true;
        }

        public void SetWeight() {
            int size = _dragons.GetLength(0);
            var dragonCoords = _dragon.Cords();

            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    // Initialize weight for each cell
                    weights[i, j] = 0;

                    // Calculate the health cost to reach this cell
                    int healthCost = Mathf.Abs(dragonCoords.x - i) + Mathf.Abs(dragonCoords.y - j);

                    // Calculate weight based on food (chickens) and health cost
                    if (_chickens[i, j] != null) {
                        int foodCalories = _chickens[i, j].GetCalories();
                        int weight = foodCalories - healthCost;

                        // Ensure weight doesn't become negative
                        weights[i, j] = Mathf.Max(weight, 0);
                    }
                    else {
                        weights[i, j] = -healthCost; // Penalize empty cells based on health cost
                    }
                }
            }

            ShiftWeightsTowardsFood();
        }

        private void ShiftWeightsTowardsFood() {
            int size = weights.GetLength(0);

            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    int maxFoodAround = GetMaxFoodAround(i, j);
                    weights[i, j] += maxFoodAround;
                }
            }
        }

        private int GetMaxFoodAround(int x, int y) {
            int size = weights.GetLength(0);
            int maxFood = 0;

            for (int i = Mathf.Max(0, x - 1); i <= Mathf.Min(size - 1, x + 1); i++) {
                for (int j = Mathf.Max(0, y - 1); j <= Mathf.Min(size - 1, y + 1); j++) {
                    if (_chickens[i, j] != null) {
                        maxFood = Mathf.Max(maxFood, _chickens[i, j].GetCalories());
                    }
                }
            }

            return maxFood;
        }

        public double[,] TakeWeight() {
            return weights;
        }
        public (int x, int y) GetNextMove() {
            return (0, 0);
        }
    }
}