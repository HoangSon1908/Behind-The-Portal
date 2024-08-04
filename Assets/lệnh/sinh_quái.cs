using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

//Lệnh này để xử lý việc gọi kẻ địch (Trong SummonEnemy)
public class sinh_quái : MonoBehaviour
{
    public int số_lượng_địch;//hàm lưu số kẻ địch hiện tại
    public static sinh_quái instance;

    private Wave wave;//Giá trị wave hiện tại

    public Wave[] waves;//Tạo ra danh sách wave để chỉnh sửa ở Inspector

    public Text Waves;//hiển thị cho người chơi wave hiện tại

    public float chờ = 10f;//thời gian chờ giữa mỗi wave
    private float đếm_ngược = 2f;//đếm ngược thời gian chờ mỗi wave

    private int currentWave;//số wave hiện tại

    public List<Transform> spawnPoints = new();//khai báo vị trí gọi quái trên mặt đất
    public List<Transform> flySpawnPoints = new();//khai báo vị trí gọi quái trên không

    private void Start()
    {
        instance = this;
        số_lượng_địch = 0;
    }
    private void Update()
    {
        if (số_lượng_địch > 0)//bao giờ không còn kẻ địch thì tiếp tục
        {
            return;
        }

        if (đếm_ngược<=0f)//nếu đếm ngược hết thì bắt đầu wave và đặt lại thời gian
        {
            StartCoroutine(bắt_đầu());
            
            đếm_ngược = chờ;
            return;
        }

        đếm_ngược -=Time.deltaTime;
    }

    IEnumerator bắt_đầu()
    {
        wave = waves[currentWave];//đặt giá trị cho wave hiện tại

        số_lượng_địch = wave.số_lượng+wave.số_lượng_2+wave.số_lượng_bay;//đếm số kẻ địch trên sân
        //kiểm tra nếu GameObject kẻ địch tồn tại thì gọi ra
        if (wave.kẻ_địch != null)
        {
            StartCoroutine(Gọi_quái());
        }
        if (wave.kẻ_địch_bay != null)
        {
            StartCoroutine(Gọi_quái_bay());
        }
        if (wave.kẻ_địch_2 != null)
        {
            StartCoroutine(Gọi_quái_2());
        }
        currentWave++;
        Waves.text = "Wave : " + currentWave.ToString() + "/10";
        yield return 0;
    }
    //tập lệnh xử lý việc gọi quái lặp lại sau mỗi 2s
    IEnumerator Gọi_quái()
    {
        for (int i = 0; i < wave.số_lượng; i++)
        {
            tạo_quái();
            yield return new WaitForSeconds(2f);
        }
    }
    IEnumerator Gọi_quái_2()
    {
        for (int i = 0; i < wave.số_lượng_2; i++)
        {
            tạo_quái_2();
            yield return new WaitForSeconds(3f);
        }
    }
    IEnumerator Gọi_quái_bay()
    {
        for (int i = 0; i < wave.số_lượng_bay; i++)
        {
            tạo_quái_bay();
            yield return new WaitForSeconds(2f);
        }
    }
    
    //Tập lệnh gọi quái ở ngẫu nhiên các SpawPoint 
    public void tạo_quái()
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);

        Vector3 spawnPosition = spawnPoints[randomIndex].position;
        Quaternion spawnRotation = spawnPoints[randomIndex].rotation;

        Instantiate(wave.kẻ_địch, spawnPosition, spawnRotation);
    }
    public void tạo_quái_2()
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);

        Vector3 spawnPosition = spawnPoints[randomIndex].position;
        Quaternion spawnRotation = spawnPoints[randomIndex].rotation;

        Instantiate(wave.kẻ_địch_2, spawnPosition, spawnRotation);
    }
    public void tạo_quái_bay()
    {
        int randomIndex = UnityEngine.Random.Range(0, flySpawnPoints.Count);

        Vector3 spawnPosition = flySpawnPoints[randomIndex].position;
        Quaternion spawnRotation = flySpawnPoints[randomIndex].rotation;

        Instantiate(wave.kẻ_địch_bay, spawnPosition, spawnRotation);
    }

    public int Wave()
    {
        return currentWave - 1;
    }
}
