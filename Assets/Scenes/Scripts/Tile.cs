using UnityEngine;

namespace Scenes.Scripts {
    public class Tile : MonoBehaviour {
        [SerializeField] private Color _base, _additional;
        [SerializeField] private SpriteRenderer _renderer;
        public bool IsOccupied { get; set; }
        protected bool IsPlayer;

        public void Init(bool isBase) {
            _renderer.color = isBase ? _base : _additional;
        }
    
    }
}