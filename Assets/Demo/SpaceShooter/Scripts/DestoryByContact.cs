using UnityEngine;

public class DestoryByContact : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject playerExplosion;

    [SerializeField] private int scoreValue;

    private BattleController _battleController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("BattleController");

        if (null != gameControllerObject)
        {
            _battleController = gameControllerObject.GetComponent<BattleController>();

            if (null == _battleController)
            {
                Debug.Log("Cannot find 'BattleController' script!");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy"))
        {
            return;
        }

        if (null != explosion)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (null != playerExplosion)
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
        }

        if (other.CompareTag("Player"))
        {
            _battleController.GameOver();
        }

        _battleController.AddScore(scoreValue);

        Destroy(other.gameObject);
        Destroy(this.gameObject);
    }
}