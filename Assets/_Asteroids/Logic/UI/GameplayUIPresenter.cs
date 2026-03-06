using System;
using UnityEngine;

namespace Assets._Asteroids.Logic.UI
{
    public class GameplayUIPresenter : IDisposable
    {
        private GameplayUIView _view;
        private GameplayUIModel _model;
        
        
        public GameplayUIPresenter(GameplayUIView view, GameplayUIModel model)
        {
            _view = view;
            _model = model;
            
            _model.OnPlayerMoved += OnMove;
            _model.OnPlayerRotated += OnRotate;
            _model.OnScoreChanged += OnScoreChanged;
            _model.OnSecondaryCountChanged += WeaponOnOnCountChanged;
            _model.OnSecondaryCooldownChanged += WeaponOnOnReloadTimeChanged;
        }

        private void OnRotate(float rotate)
        {
            _view.ChangePlayerRotation(rotate);
        }

        private void OnScoreChanged(int score)
        {
            _view.ChangeScore(score);
        }

        private void WeaponOnOnReloadTimeChanged(float currentCooldown, float cooldown)
        {
            _view.ChangeSecondaryCooldown(currentCooldown, cooldown);
        }

        private void WeaponOnOnCountChanged(int Count)
        {
            _view.ChangeSecondaryCount(Count);
        }

        private void OnMove(Vector2 coords, float speed)
        {
            _view.ChangePlayerCoords(coords, speed);
        }

        public void Dispose()
        {
            _model.OnPlayerMoved -= OnMove;
            _model.OnPlayerRotated -= OnRotate;
            _model.OnScoreChanged -= OnScoreChanged;
            _model.OnSecondaryCountChanged -= WeaponOnOnCountChanged;
            _model.OnSecondaryCooldownChanged -= WeaponOnOnReloadTimeChanged;
        }
    }
}