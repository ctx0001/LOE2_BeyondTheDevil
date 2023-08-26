using System;
using UnityEngine;

namespace Managers
{
    public class BalanceManager : MonoBehaviour
    {
        public static BalanceManager Instance;
        private int _balance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                _balance = PlayerPrefs.GetInt("Balance");
            }
        }

        private void Start()
        {
            CanvasManager.Instance.UpdateBalance(_balance);
        }

        public void AddBalance(int balanceToAdd)
        {
            _balance += balanceToAdd;
            CanvasManager.Instance.UpdateBalance(_balance);
            PlayerPrefs.SetInt("Balance", _balance);
            // save data
        }
    }
}