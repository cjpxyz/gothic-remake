using UnityEngine;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour {

    private EnemyAnimator enemy_Anim;
    private NavMeshAgent navMeshAgent;
    private EnemyController enemy_Controller;

    public float health = 100f;

    public bool is_Player, is_Boar, is_Cannibal;

    private bool is_Dead;

    private void Awake() {
        if (is_Boar || is_Cannibal) {
            enemy_Anim = GetComponent<EnemyAnimator>();
            enemy_Controller = GetComponent<EnemyController>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            // get enemy audio
        }

        if (is_Player) {

        }
    }

    public void ApplyDamage(float damage) {

        // if we died don't execute the rest of the code
        if (is_Dead) {
            return;
        }

        health -= damage;

        if (is_Player) {
            //show the stats(display the health UI value)

        }

        if (is_Boar || is_Cannibal) {
            // In case we attack out of the chase distance, they attcked monster will start to chase (higher chase_Distance)
            if (enemy_Controller.Enemy_State == EnemyState.PATROL) {
                enemy_Controller.chase_Distance = 50f;
            }
        }

        if (health <= 0f) {
            PlayerDied();

            is_Dead = true;
        }
    } // apply damage

    void PlayerDied() {
        if (is_Cannibal) {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 10f);

            enemy_Controller.enabled = false;
            navMeshAgent.enabled = false;
            enemy_Anim.enabled = false;

            // StartCoroutine

            // EnemyManager spawn more enemies
        }

        if (is_Boar) {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
            enemy_Controller.enabled = false;

            enemy_Anim.Dead();

            // StartCoroutine

            // EnemyMAnager spawn more enemies
        }

        if (is_Player) {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);
            for (int i = 0; i < enemies.Length; i++) {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            // call enemy manager to stop spawning enemies

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
        }

        if (tag == Tags.PLAYER_TAG) {
            Invoke("RestartGame", 3f);
        } else {
            Invoke("TurnOffGameObject", 3f);
        }

    } // player died

    void RestartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    void TurnOffGameObject() {
        gameObject.SetActive(false);
    }

} // class