using Scenes.Scripts.Field;
using UnityEngine;

namespace Scenes.Scripts.Globals {
    public class GameUI : MonoBehaviour {
        [SerializeField]private FieldDrawer fieldDrawerPrefab;
        [SerializeField]private FieldGenerator fieldGeneratorPrefab;
        
        private FieldDrawer _drawer;
        private FieldGenerator _generator;

        private void Awake() {
            _drawer = Instantiate(fieldDrawerPrefab);
            _generator = Instantiate(fieldGeneratorPrefab);
            FieldContainer.Instance.SetSize(15);

        }

        private void Start() {
            _generator.GenerateEasy();
            _drawer.DrawField();
            _drawer.RenderDragons();
            _drawer.RenderFood();
            InvokeRepeating("BotUpdate", 1.0f, 1.0f);
        }

        private void BotUpdate() {
            Debug.Log("Updating every second...");
        }
    }
}