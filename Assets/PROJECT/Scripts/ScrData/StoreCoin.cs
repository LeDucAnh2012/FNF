using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.Data
{
    public class StoreCoin : MonoBehaviour
    {
        [SerializeField] private Text txtCoin;

        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float timeInit;

        private float timeCurrent;
        private void Start()
        {
            InitCoin();
        }
        public void InitCoin()
        {
            txtCoin.text = VariableSystem.StoreCoin.ToString();
        }
        public void SetCoin(int amountCoin, int isIncress)
        {
            StartCoroutine(IE_SetCoin(amountCoin, isIncress));
        }
        IEnumerator IE_SetCoin(int amountCoin, int isIncress)
        {
            timeCurrent = 0;
            while (timeCurrent < timeInit)
            {
                yield return new WaitForEndOfFrame();
                timeCurrent += Time.deltaTime;

                float valueWant = amountCoin * curve.Evaluate(timeCurrent / timeInit);

                txtCoin.text = VariableSystem.StoreCoin + (int)valueWant * isIncress + "";
            }
            VariableSystem.StoreCoin += amountCoin * isIncress;
            InitCoin();
        }
    }
}
