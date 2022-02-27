using System.Collections.Generic;
using UnityEngine;
using ShugoshinHajiki.Objects.Enemy;
using ShugoshinHajiki.Effects;

/// <summary>
/// 湧きパターンを模索するために作ったスクリプト。
///
/// 湧きパターンの書き方>>>>
///     1. 真左が0、左上が1、真上が2...左下が7の時計回りで指定する。
///     2. 同時に沸かせる分はカッコでくくる。１体しか出さない場合も必須。
///     3. カッコと半角数字以外を入れたら事故る。スペースも厳禁。
///     
///     例：左上から順番に時計回りで出す ↓↓↓
///         (0)(1)(2)(3)(4)(5)(6)(7)
///
///     例：時計回りに向い合せで同時に出す ↓↓↓
///         (04)(15)(26)(37)
///         
///     例：8方向一気に出す ↓↓↓
///         (012345678)
///
///
/// Tips: Consoleでボタンを押してエラーと警告を非表示にすると見やすい。
/// </summary>
public class EnemySpawnTester : MonoBehaviour
{
    // スポナーを入れておく配列
    [SerializeField] GameObject[] spawners = null;

    // 敵のオブジェクト群
    [field:SerializeField, Header("沸かせる敵。自由に変えてOK")] GameObject enemy = null;

    // 1ウェーブの周期数
    [field:SerializeField, Header("同時に沸く1セットが、何回組み合わさって1ウェーブになるか")] int spawnAmount = 8;
    [field:SerializeField, Header("ランダム生成時に、同時に沸く敵の上限数。2なら2体同時湧きまであり得る")] int sameTimeSpawnLimit = 2;

    // 湧き時間関係の変数
    float _spawnTime = 0;
    [field:SerializeField, Header("次の敵が湧くまでの間隔")] float spawnDuration = 3.0f;

    // 湧きリスト関係
    List<int[]> _spawnList = new List<int[]>();
    int _listIndex = 0;

    // 固定湧きのとき使う変数
    [field:SerializeField, Header("チェックが入っているとランダムで湧きパターンが生成される。Consoleでパターンは見れる")] bool isRandom = true;
    [field:SerializeField, Header("上のチェックが入っていないとき、ここを読んでパターンを作る。書き方はこのスクリプトの9行目を見て")] string petternInput = "";
    

    private void Update()
    {
        _spawnTime -= Time.deltaTime;

        if(_spawnTime <= 0)
        {
            _spawnTime = spawnDuration;

            // リストの中身を再構築する
            if(_spawnList.Count == 0)
            {
                // ランダム生成にチェックが入っている場合
                if (isRandom)
                {
                    // 決まった回数分の湧きを生成する。
                    for (int i = 0; i < spawnAmount; i++)
                    {
                        // この1ループで1回分の湧きをランダム生成する。
                        int _sameFlag = Random.Range(1, sameTimeSpawnLimit + 1);
                        var _addArr = new List<int>();

                        for(int n = 0; n < _sameFlag; n++)
                        {
                            _addArr.Add(Random.Range(0, 8));
                        }

                        _spawnList.Add(_addArr.ToArray());
                    }
                }
                // 湧きを指定する場合
                else
                {
                    // 文字列を一文字ずつ取って読み取る
                    char[] _charList = petternInput.ToCharArray();
                    var _tempList = new List<int>();
                    spawnAmount = 0;

                    foreach (char c in _charList)
                    { 
                        switch (c)
                        {
                            case '(':
                                _tempList = new List<int>();
                                break;

                            case ')':
                                _spawnList.Add(_tempList.ToArray());
                                spawnAmount++;
                                break;

                            default:
                                _tempList.Add(int.Parse(c.ToString()));
                                break;
                        }
                    }
                }


                // 湧きパターンをデバッグ表示する部分
                string _debugOutput = "";
                foreach(int[] l in _spawnList)
                {
                    // 1回分の湧きをカッコでくくる。
                    _debugOutput += "(";
                    foreach(int n in l)
                    {
                        // 1回分の湧きの数字を詰め込む。
                        _debugOutput += n.ToString();
                    }
                    _debugOutput += ")";
                }

                Debug.Log($"湧きパターン： {_debugOutput}");
            }

            // 沸かせる部分
            foreach (int i in _spawnList[_listIndex])
            {
                Vector3 _spawnPoint = spawners[i].GetComponent<EnemySpawner>().Spawn();
                Instantiate(enemy, _spawnPoint, Quaternion.identity);
                GetComponent<EnemySignSpawner>().Spawn(_spawnPoint);
            }

            _listIndex++;

            // 一回りしたら初期化する
            if (_listIndex >= spawnAmount)
            {
                Debug.Log("一周回ったのでリセット！");
                _listIndex = 0;

                if (isRandom)
                {
                    _spawnList = new List<int[]>();
                }
            }
        }
    }
}
