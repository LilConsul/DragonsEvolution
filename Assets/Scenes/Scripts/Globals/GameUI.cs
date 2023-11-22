using System;
using Scenes.Scripts.CameraScripts;
using Scenes.Scripts.Enums;
using Scenes.Scripts.Field;
using Scenes.Scripts.Units;
using UnityEngine;

namespace Scenes.Scripts.Globals {
    public class GameUI : MonoBehaviour {
        public static GameUI Instance;
        private float botUpdateDelay;
        private AI _ai;
        private int _turns;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            _ai = gameObject.AddComponent<AI>();
            StartGame();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void BotUpdate() {
            try {
                if (!GlobalSettings.Instance.gameIsOnline) return;
                var dragon = FieldContainer.Instance.GetNextDragon();
                PlaceHolderController.Instance.ShowInfo(dragon);

                if (dragon.State == EntityState.Dead) {
                    dragon.Move(0, 0);
                    Invoke(nameof(BotUpdate), 0f);
                    if (GlobalSettings.Instance.gameIsOnline)
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

                    if (GlobalSettings.Instance.gameIsOnline)
                        Invoke(nameof(BotUpdate), botUpdateDelay / 3);
                    return;
                }

                _turns = 0;
                if (!FieldContainer.Instance.Add(dragon)) {
                    Debug.LogWarning($"Dragon on {dragon.Cords()} not moved!");
                    FieldContainer.Instance.ReturnMove(dragon);
                }

                if (GlobalSettings.Instance.gameIsOnline)
                    Invoke(nameof(BotUpdate), botUpdateDelay);
            }
            catch (Exception) { }
        }

        public void RestartGame() {
            GlobalSettings.Instance.gameIsOnline = false;
            FieldDrawer.Instance.DestroyField();
            Invoke(nameof(StartGame), 1.0f);
        }

        private void StartGame() {
            botUpdateDelay = GlobalSettings.Instance.delayTime;
            GlobalSettings.Instance.gameIsOnline = true;
            
            _turns = 0;
            uint size = GlobalSettings.Instance.fieldSize;
            FieldContainer.Instance.SetSize(size);
            FieldGenerator.Instance.CustomGenerator(5, 15);

            FieldDrawer.Instance.DrawField();
            FieldDrawer.Instance.RenderUnits<BotDragon>();
            FieldDrawer.Instance.RenderUnits<Food>();

            FieldContainer.Instance.StartGame();
            CameraMove();
            
            Invoke(nameof(BotUpdate), botUpdateDelay);
        }

        private void CameraMove() {
            var cords = FieldDrawer.Instance.GetCentralPosition();
            MoveCamera.Instance.MoveTo(cords.x, cords.y);
        }
    }
}