using Scenes.Scripts.Field;
using Scenes.Scripts.Units;
using UnityEngine;

namespace Scenes.Scripts.Globals {
    public class GameUI : MonoBehaviour {
        private AI _ai;

        private void Start() {
            FieldContainer.Instance.SetSize(10);
            
            //FieldGenerator.Instance.GeneratePreset();
            FieldGenerator.Instance.CustomGenerator(5, 15);
            
            FieldDrawer.Instance.DrawField();
            FieldDrawer.Instance.RenderUnits<BotDragon>();
            FieldDrawer.Instance.RenderUnits<Chicken>();
            
            FieldContainer.Instance.StartGame();
            _ai = gameObject.AddComponent<AI>();
            InvokeRepeating("BotUpdate", 1.0f, 1.0f);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void BotUpdate() {
            var dragon = FieldContainer.Instance.GetNextDragon();
            var nextMove = _ai.GetNextMove(dragon);
            dragon.Move(nextMove.x, nextMove.y);
            Print(dragon, _ai.TakeWeight());

            
            /*(int x, int y) = _ai.GetNextMove();
            dragon.Move(x, y);*/
            if (!FieldContainer.Instance.Add(dragon)) {
                Debug.LogWarning($"Dragon on {dragon.Cords()} not moved!");
                FieldContainer.Instance.ReturnMove(dragon);
            }

            Debug.Log("Updating every second...");
        }

        private void Print(BotDragon who, int[,] input) {
            var rows = input.GetLength(0);
            var cols = input.GetLength(1);

            (var x, var y) = who != null ? who.Cords() : (-100, -100);
            var matrixString = "Matrix:\n";

            for (var i = 0; i < rows; i++) {
                for (int j = 0; j < cols; j++) {
                    if (i == x && j == y) matrixString += "Me!" + "\t";
                    else matrixString += input[i, j].ToString("F1") + "\t";
                }

                matrixString += "\n";
            }

            Debug.Log(matrixString);
        }
    }
}