using Scenes.Scripts.Field;
using Scenes.Scripts.Units;
using UnityEngine;

namespace Scenes.Scripts.Globals {
    public class GameUI : MonoBehaviour {
        private AI _ai;
        private void Start() {
            FieldContainer.Instance.SetSize(15);
            FieldGenerator.Instance.GenerateEasy();
            FieldDrawer.Instance.DrawField();
            FieldDrawer.Instance.RenderUnits<BotDragon>();
            FieldDrawer.Instance.RenderUnits<Chicken>();
            FieldContainer.Instance.StartGame();
            _ai = gameObject.AddComponent<AI>();
            InvokeRepeating("BotUpdate", 0.1f, 1.0f);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void BotUpdate() {
            var dragon = FieldContainer.Instance.GetNextDragon();
            _ai.SetInitialization(dragon);
            _ai.SetWeight();
            var weights = _ai.TakeWeight();
            Print(dragon, weights);
            
            dragon.Move(dragon.Cords().x + 1, dragon.Cords().y + 1);
            if (!FieldContainer.Instance.Add(dragon)){
                FieldContainer.Instance.ReturnMove(dragon);
            }

            Debug.Log("Updating every second...");
        }

        private void Print(BotDragon who,double[,] input) {
            var rows = input.GetLength(0);
            var cols = input.GetLength(1);

            (var x, var y) = who.Cords();
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