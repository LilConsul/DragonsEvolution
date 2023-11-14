using Scenes.Scripts.Field;
using Scenes.Scripts.Units;
using UnityEngine;

namespace Scenes.Scripts.Globals {
    public class GameUI : MonoBehaviour {
        private void Start() {
            FieldContainer.Instance.SetSize(15);
            FieldGenerator.Instance.GenerateEasy();
            FieldDrawer.Instance.DrawField();
            FieldDrawer.Instance.RenderUnits<BotDragon>();
            FieldDrawer.Instance.RenderUnits<Chicken>();
            InvokeRepeating("BotUpdate", 1.0f, 1.0f);
        }

        private void BotUpdate() {
            Debug.Log("Updating every second...");
        }
    }
}