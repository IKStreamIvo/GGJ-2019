using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {
    [SerializeField] private float _maxEnergy;
    private static float currentEnergy;
    private static float maxEnergy;
    [SerializeField] private Image _fillImage;
	[SerializeField] private Text _fillText;

    private static Image fillImage;
	private static Text fillText;
    private static bool continuousDrain;
    private static float drainValue;

    private void Start() {
        fillImage = _fillImage;
        maxEnergy = _maxEnergy;
		fillText = _fillText;

        currentEnergy = maxEnergy;
    }

    public static void Drain(float value){
        currentEnergy -= value;
        UpdateUI();
    }
    public static void Drain(float value, bool continuous){
        drainValue = value;
        continuousDrain = continuous;
    }

    private static void UpdateUI(){
        float value = currentEnergy / maxEnergy;
        fillImage.fillAmount = value;
		fillText.text = "" + value;
    }

    private void Update() {
        if(continuousDrain){
            Drain(drainValue * Time.deltaTime);
        }   
    }

	public static bool HasEnergy(float cost) {
		return currentEnergy >= cost;
	}

	public static bool EnergyFull() {
		if(currentEnergy >= maxEnergy) {
			return true;
		}
		return false;
	}
}
