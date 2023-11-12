using Scenes.Scripts.Field;
using UnityEngine;

namespace Scenes.Scripts {
    public class GameUI : MonoBehaviour {
        [SerializeField]private FieldDrawer fieldDrawerPrefab;
        
        private FieldDrawer _drawer;
        private FieldGenerator _generator;
        private FieldContainer _container;

        private void Start() {
            _drawer = Instantiate(fieldDrawerPrefab);
            _generator = new FieldGenerator();
            _container = new FieldContainer(50);

            _generator.GenerateEasy(ref _container);
            _drawer.Draw(_container);
        }
    }
}