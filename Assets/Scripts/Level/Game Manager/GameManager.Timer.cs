using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Level
{
    public partial class GameManager
    {
        [Header("Timer Settings")]
        [SerializeField] private float _remainingTime = 0;

        private CancellationTokenSource _timerCancellationTokenSource;

        public float remainingTime
        {
            get => _remainingTime;
            private set
            {
                _remainingTime = value;
                onTimerChanged?.Invoke(_remainingTime);
            }
        }


        private void InitTimer(float initialTime)
        {
            Debug.Assert(initialTime > 0, "Initial time must be greater than 0");

            StopTimer();
            _timerCancellationTokenSource = new CancellationTokenSource();

            remainingTime = initialTime;
            TimerLoop().Forget();
        }

        private async UniTaskVoid TimerLoop()
        {
            

            while (_remainingTime > 0 && !_timerCancellationTokenSource.IsCancellationRequested)
            {
                await UniTask.Delay(1000, cancellationToken: _timerCancellationTokenSource.Token);

                remainingTime--;

                if (remainingTime > 0 && remainingTime < 1)
                {
                    await UniTask.WaitForSeconds(remainingTime, cancellationToken: _timerCancellationTokenSource.Token);
                    remainingTime = 0;
                }

                if (remainingTime <= 0) onLevelFailed?.Invoke();
            }

        }

        private void StopTimer()
        {
            if (_timerCancellationTokenSource != null)
            {
                _timerCancellationTokenSource.Cancel();
                _timerCancellationTokenSource.Dispose();
                _timerCancellationTokenSource = null;
            }
        }

    }

}