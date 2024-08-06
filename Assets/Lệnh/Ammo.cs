using UnityEngine;
//Lệnh này để xử lý va chạm của viên đạn (Bên trong mỗi viên đạn)
public class Ammo : MonoBehaviour
{
    public int damage;//sát thương đạn
    public GameObject hitEffectPrefab;

    public bool isPlayer; // Biến kiểm tra xem đạn có phải do người chơi bắn ra không
    public bool isEnemy; // Biến kiểm tra xem đạn có phải do kẻ địch bắn ra không

    public Vector3 enemyTransform;

    private void Awake()
    {
        enemyTransform= transform.position;
    }

    // Phương thức được gọi khi có va chạm xảy ra
    void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra xem đối tượng va chạm có tag "Player" và đạn do kẻ địch bắn ra không
        if (collision.gameObject.CompareTag("Player") && isEnemy)
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage,enemyTransform); // Gây sát thương cho người chơi
        }
        // Kiểm tra xem đối tượng va chạm có tag "Enemy" và đạn do người chơi bắn ra không
        else if (collision.gameObject.CompareTag("Enemy") && isPlayer)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage); // Gây sát thương cho kẻ địch
        }

        if(hitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(hitEffectPrefab, collision.contacts[0].point, Quaternion.identity);
            Destroy(hitEffect, 1f);
        }
        Destroy(gameObject); // Hủy đối tượng đạn sau khi xử lý va chạm
    }
}
