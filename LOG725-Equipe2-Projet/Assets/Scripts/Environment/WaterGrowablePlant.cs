using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class WaterGrowablePlant : MonoBehaviour
{
    [SerializeField]
    private Sprite vineSegmentSprite; // Sprite utilisé pour chaque segment de la liane

    [SerializeField, Min(1)]
    private int numberOfSegments = 3; // Nombre de segments de la liane (défini dans l'Inspector)

    [SerializeField]
    private float segmentHeight = 1f; // Hauteur de chaque segment (pour l'espacement vertical)

    [SerializeField]
    private AudioClip growSound; // Son joué lors de la pousse de la liane

    private bool hasGrown = false; // Indique si la liane a déjà poussé

    private void Awake()
    {
        // Assure que le collider est en mode "Trigger" pour détecter les collisions
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si la liane n'a pas déjà poussé
        if (hasGrown) return;

        // Vérifie si l'objet qui entre en collision est un projectile
        PowerProjectile projectile = other.GetComponent<PowerProjectile>();
        if (projectile != null)
        {
            // Vérifie si le projectile est de type "Water"
            if (projectile.powerType == PowerType.Water)
            {
                // Marque la plante comme activée
                hasGrown = true;

                // Joue un son de pousse (si défini)
                if (growSound != null)
                {
                    AudioManager.GetSingleton().PlaySFX(growSound);
                }

                // Fait pousser la liane
                GrowVine();

                // Détruit le projectile
                Destroy(projectile.gameObject);
            }
        }
    }

    private void GrowVine()
    {
        // Crée l'objet parent de la liane
        GameObject vine = new GameObject("Vine");
        vine.transform.position = transform.position;
        vine.tag = "Ladder"; // Ajoute le tag "Ladder" pour que le joueur puisse grimper

        // Ajoute un BoxCollider2D pour la zone d'escalade
        BoxCollider2D collider = vine.AddComponent<BoxCollider2D>();
        collider.isTrigger = true; // Doit être un Trigger pour que le joueur puisse le détecter
        collider.size = new Vector2(0.5f, numberOfSegments * segmentHeight); // Ajuste la hauteur en fonction du nombre de segments
        collider.offset = new Vector2(0, (numberOfSegments * segmentHeight) / 2f - 0.5f); // Ajuste l'offset (voir Étape 2)

        // Crée les segments de la liane comme enfants
        for (int i = 0; i < numberOfSegments; i++)
        {
            GameObject segment = new GameObject($"VineSegment_{i}");
            segment.transform.parent = vine.transform;
            segment.transform.localPosition = new Vector3(0, i * segmentHeight, 0); // Positionne chaque segment au-dessus du précédent

            // Ajoute un SpriteRenderer pour le segment
            SpriteRenderer renderer = segment.AddComponent<SpriteRenderer>();
            renderer.sprite = vineSegmentSprite;
            renderer.sortingLayerName = "Interactable"; // Définit le Sorting Layer sur "Interactable"
        }
    }
}