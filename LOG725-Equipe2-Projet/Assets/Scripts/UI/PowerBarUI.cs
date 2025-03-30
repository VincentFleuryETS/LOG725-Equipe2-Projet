using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerBarUI : MonoBehaviour
{
    // Références aux icônes et textes dans l'UI
    [SerializeField] private Image waterIcon;
    [SerializeField] private TextMeshProUGUI waterChargesText;
    [SerializeField] private Image airIcon;
    [SerializeField] private TextMeshProUGUI airChargesText;
    [SerializeField] private Image fireIcon;
    [SerializeField] private TextMeshProUGUI fireChargesText;
    [SerializeField] private Image earthIcon;
    [SerializeField] private TextMeshProUGUI earthChargesText;

    // Référence au PlayerPowerController
    [SerializeField] private PlayerPowerController powerController;

    // Couleurs pour les icônes (débloqué = couleur, bloqué = noir)
    private Color unlockedColor = Color.white;
    private Color lockedColor = Color.black;

    void Start()
    {
        // Vérifie que le powerController est bien assigné
        if (powerController == null)
        {
            Debug.LogError("PlayerPowerController non assigné dans PowerBarUI !");
            return;
        }

        // S'abonne à l'événement PowersUpdated
        powerController.PowersUpdated.AddListener(UpdateAllPowersUI);

        // Met à jour l'UI au démarrage
        UpdateAllPowersUI();
    }

    void OnDestroy()
    {
        // Se désabonne de l'événement pour éviter les fuites de mémoire
        if (powerController != null)
        {
            powerController.PowersUpdated.RemoveListener(UpdateAllPowersUI);
        }
    }

    private void UpdateAllPowersUI()
    {
        // Met à jour l'affichage pour chaque pouvoir
        UpdatePowerUI(powerController.WaterPower, waterIcon, waterChargesText);
        UpdatePowerUI(powerController.AirPower, airIcon, airChargesText);
        UpdatePowerUI(powerController.FirePower, fireIcon, fireChargesText);
        UpdatePowerUI(powerController.EarthPower, earthIcon, earthChargesText);
    }

    private void UpdatePowerUI(Power power, Image icon, TextMeshProUGUI chargesText)
    {
        if (power == null) return;

        // Met à jour le texte des charges
        chargesText.text = power.Charges.ToString();

        // Met à jour la couleur de l'icône (débloqué = couleur, bloqué = noir)
        if (power.Charges > 0)
        {
            icon.color = unlockedColor; // En couleur si débloqué
        }
        else
        {
            icon.color = lockedColor; // En noir si bloqué
        }
    }
}