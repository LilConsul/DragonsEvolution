using Scenes.Scripts.Field;
using Scenes.Scripts.Units;
using Unity.VisualScripting;
using UnityEngine;

namespace Scenes.Scripts.Globals {
    public class GameUI : MonoBehaviour {
        private void Start() {
            FieldContainer.Instance.SetSize(15);
            FieldGenerator.Instance.GenerateEasy();
            FieldDrawer.Instance.DrawField();
            FieldDrawer.Instance.RenderUnits<BotDragon>();
            FieldDrawer.Instance.RenderUnits<Chicken>();
            FieldContainer.Instance.StartGame();
            InvokeRepeating("BotUpdate", 0.1f, 1.0f);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void BotUpdate() {
            var dragon = FieldContainer.Instance.GetNextDragon();
            dragon.Move(dragon.Cords().x + 1, dragon.Cords().y + 1);
            if (!FieldContainer.Instance.Add(dragon)){
                FieldContainer.Instance.ReturnMove(dragon);
            }
            
            Debug.Log("Updating every second...");
        }
    }
}