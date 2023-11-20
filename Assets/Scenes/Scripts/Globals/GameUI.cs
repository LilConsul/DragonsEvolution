using Scenes.Scripts.Enums;
using Scenes.Scripts.Field;
using Scenes.Scripts.Units;
using UnityEngine;

namespace Scenes.Scripts.Globals {
    public class GameUI : MonoBehaviour {
        [SerializeField] private float botUpdateDelay;
        private AI _ai;
        private int _turns;

        private void Start() {
            _turns = 0;
            FieldContainer.Instance.SetSize(15);

            //FieldGenerator.Instance.GeneratePreset();
            FieldGenerator.Instance.CustomGenerator(5, 15);

            FieldDrawer.Instance.DrawField();
            FieldDrawer.Instance.RenderUnits<BotDragon>();
            FieldDrawer.Instance.RenderUnits<Food>();

            FieldContainer.Instance.StartGame();
            _ai = gameObject.AddComponent<AI>();
            Invoke(nameof(BotUpdate), botUpdateDelay);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void BotUpdate() {
            var dragon = FieldContainer.Instance.GetNextDragon();

            if (dragon.State == EntityState.Dead) {
                dragon.Move(0, 0);
                Invoke(nameof(BotUpdate), 0f);
                FieldContainer.Instance.Add(dragon);
                return;
            }

            var nextMove = _ai.GetNextMove(dragon);
            if (nextMove.mate == false) {
                dragon.Move(nextMove.x, nextMove.y);
            }
            else {
                dragon.Mate(nextMove.x, nextMove.y);
            }

            if (_turns < dragon.Speed) {
                _turns++;
                if (!FieldContainer.Instance.AddFirst(dragon)) {
                    Debug.LogWarning($"Dragon on {dragon.Cords()} not moved!");
                    FieldContainer.Instance.ReturnMove(dragon);
                }

                Invoke(nameof(BotUpdate), botUpdateDelay / 3);
                return;
            }

            _turns = 0;
            if (!FieldContainer.Instance.Add(dragon)) {
                Debug.LogWarning($"Dragon on {dragon.Cords()} not moved!");
                FieldContainer.Instance.ReturnMove(dragon);
            }

            Invoke(nameof(BotUpdate), botUpdateDelay);
        }
    }
}