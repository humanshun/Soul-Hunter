using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground_0 : MonoBehaviour
{
    [Header("Layer Setting")]
    // 各レイヤーの移動速度を格納する配列
    public float[] Layer_Speed = new float[7];
    // 各レイヤーのゲームオブジェクトを格納する配列
    public GameObject[] Layer_Objects = new GameObject[7];

    // カメラのTransform
    private Transform _camera;
    // プレイヤーのTransform
    public Transform player;
    // 各レイヤーの初期位置を格納する配列
    private float[] startPos = new float[7];
    // スプライトの幅
    private float boundSizeX;
    // オブジェクトのスケール
    private float sizeX;
    // プレイヤーとカメラのオフセット
    public float cameraOffsetX = 5.0f;

    void Start()
    {
        // メインカメラのTransformを取得
        _camera = Camera.main.transform;
        // 最初のレイヤーのスケールを取得
        sizeX = Layer_Objects[0].transform.localScale.x;
        // 最初のレイヤーのスプライトの幅を取得
        boundSizeX = Layer_Objects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        
        // 各レイヤーの初期位置をstartPos配列に保存
        for (int i = 0; i < Layer_Objects.Length; i++)
        {
            startPos[i] = Layer_Objects[i].transform.position.x;
        }
    }

    void Update()
    {
        // カメラをプレイヤーの位置に連動させる（オフセットを追加）
        Vector3 cameraPosition = _camera.position;
        cameraPosition.x = player.position.x + cameraOffsetX;
        _camera.position = cameraPosition;

        // 各レイヤーに対してパララックス効果を適用
        for (int i = 0; i < Layer_Objects.Length; i++)
        {
            // カメラの位置に基づくオフセットを計算
            float temp = (_camera.position.x * (1 - Layer_Speed[i]));
            // レイヤーの移動量を計算
            float distance = _camera.position.x * Layer_Speed[i];

            // レイヤーの新しい位置を設定
            Layer_Objects[i].transform.position = new Vector2(startPos[i] + distance, Layer_Objects[i].transform.position.y);

            // レイヤーがスプライトの幅を超えた場合、位置をループさせる
            if (temp > startPos[i] + boundSizeX * sizeX)
            {
                startPos[i] += boundSizeX * sizeX;
            }
            else if (temp < startPos[i] - boundSizeX * sizeX)
            {
                startPos[i] -= boundSizeX * sizeX;
            }
        }
    }
}
