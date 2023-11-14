using Scenes.Scripts.Field;
using UnityEngine;

namespace Scenes.Scripts.Globals {
    public class GameUI : MonoBehaviour {
        [SerializeField]private FieldDrawer fieldDrawerPrefab;
        [SerializeField]private FieldGenerator fieldGeneratorPrefab;
        
        private FieldDrawer _drawer;
        private FieldGenerator _generator;
        private FieldContainer _container;

        private void Awake() {
            _drawer = Instantiate(fieldDrawerPrefab);
            _generator = Instantiate(fieldGeneratorPrefab);
            _container = new FieldContainer(15);
        }

        private void Start() {
            _generator.GenerateEasy(ref _container);
            _drawer.DrawField(_container);
            _drawer.RenderDragons(_container);
            _drawer.RenderFood(_container);
            InvokeRepeating("BotUpdate", 1.0f, 1.0f);
        }

        private void BotUpdate() {
            Debug.Log("Updating every second...");
        }
    }
}