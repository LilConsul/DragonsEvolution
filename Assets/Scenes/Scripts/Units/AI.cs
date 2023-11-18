using Scenes.Scripts.Field;
using UnityEngine;
using System;

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
            /*_dragons = container.GetUnitsField<BotDragon>(x, y, size);
            _chickens = container.GetUnitsField<Chicken>(x, y, size);*/
            _dragons = container.GetUnitsField<BotDragon>();
            _chickens = container.GetUnitsField<Chicken>();

            return true;
        }

        public void SetWeight() {
            int size = _dragons.GetLength(0);
            var dragonCoords = _dragon.Cords();

            for (var i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    if ((i, j) == dragonCoords) {
                        weights[i, j] = 0;
                        continue;
                    }

                    if (_chickens[i, j] != null) {
                        weights[i, j] = _chickens[i, j].GetCalories();
                        continue;
                    }

                    if (_dragons[i, j] != null) {
                        var myDrag = _dragons[i, j];
                        weights[i, j] = myDrag.Color == _dragon.Color ? -100 : 50;
                        continue;
                    }
                }
            }
        }

        public double[,] TakeWeight() {
            return weights;
        }
        public (int x, int y) GetNextMove() {
            int size = weights.GetLength(0);
            double maxWeight = double.MinValue;
            (int nextX, int nextY) = (_dragon.Cords().x, _dragon.Cords().y);

            for (int i = Math.Max(0, _dragon.Cords().x - 1); i <= Math.Min(size - 1, _dragon.Cords().x + 1); i++) {
                for (int j = Math.Max(0, _dragon.Cords().y - 1); j <= Math.Min(size - 1, _dragon.Cords().y + 1); j++) {
                    if ((i != _dragon.Cords().x || j != _dragon.Cords().y)
                        && Math.Abs(i - _dragon.Cords().x) + Math.Abs(j - _dragon.Cords().y) == 1
                        && FieldContainer.Instance.ValidCords(i, j)) {
                        if (weights[i, j] > maxWeight) {
                            maxWeight = weights[i, j];
                            (nextX, nextY) = (i, j);
                        }
                    }
                }
            }

            return (nextX, nextY);
        }
    }
}