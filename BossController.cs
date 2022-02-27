using UnityEngine;
using DG.Tweening;

namespace ShugoshinHajiki.Objects.Enemy
{
    /// <summary>
    /// TIPS. スロー処理でsequence.timeScaleが使えないため、DOTween全体のtimeScaleを変更しているので注意
    /// </summary>
    public class BossController : MonoBehaviour
    {
        [SerializeField] GameObject[] enemies = null;

        bool _canSpawn = false;
        bool _isSlow = false;
        bool _isArrive = false;

        // Bossの生存時間
        [field:SerializeField, Header("Bossの生存時間")]float arrivalTime = 30f;
        float _nowArriveTime = 0f;

        // Bossが敵を沸かせる間隔
        float _spawnDuration = 0.5f;
        [field: SerializeField, Header("Bossが敵を沸かせる間隔")] float spawnDurationDefault = 0.5f;
        [field: SerializeField] float spawnDurationOnSlow = 1f;
        float _passedTime = 0f;

        // Bossが一度に沸かせる敵の数
        [field: SerializeField, Header("Bossが一度に沸かせる敵の数")] int spawnCountOneTime = 5;
        int _nowSpawnCount = 0;

        // Bossにかかるスロー状態の有効時間
        [field: SerializeField, Header("Bossにかかるスロー状態の有効時間")] float slowEffectiveTime = 5f;
        float _nowSlowTime = 0f;

        Sequence sequence;


        private void Start()
        {
            DOTween.Init();
            sequence = DOTween.Sequence();
            Init();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.D))
            {
                this.transform.DOKill();
                Destroy(this.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                StartSpawn();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeSlow();
            }
        }

        private void FixedUpdate()
        {
            if (_isArrive)
            {
                // 生存時間の時間経過処理
                if (_nowArriveTime < arrivalTime)
                {
                    _nowArriveTime += Time.deltaTime;
                }
                else
                {
                    BackToBase();
                }

                // 敵を沸かせる時間経過処理
                if (_canSpawn)
                {
                    _passedTime += Time.deltaTime;

                    if (_passedTime >= _spawnDuration)
                    {
                        int enemyIndex = Random.Range(0, 3);
                        Instantiate(enemies[enemyIndex], transform.position, Quaternion.identity);

                        _passedTime = 0;
                        _nowSpawnCount += 1;

                        // 一回の沸き数を超えたら沸きをストップする
                        if (_nowSpawnCount >= spawnCountOneTime)
                        {
                            StopSpawn();
                        }
                    }
                }

                // スロー状態の時間経過処理
                if (_isSlow)
                {
                    _nowSlowTime += Time.deltaTime;
                    if (_nowSlowTime >= slowEffectiveTime)
                    {
                        RecoverSlow();
                        _nowSlowTime = 0;
                    }
                }
            }
        }


        public void Init()
        {
            Vector3[] path = DoTweenManager._Instance.GetPath(3);
            sequence.Append(transform.DOPath(path, 5, PathType.Linear)
                .OnComplete(StartArrive)
                ).OnComplete(AddSequence);
            sequence.AppendInterval(2f);
            sequence.Play();

            _spawnDuration = spawnDurationDefault;
        }


        void StartArrive()
        {
            _isArrive = true;
        }

        void StartSpawn()
        {
            _canSpawn = true;
        }


        void StopSpawn()
        {
            _canSpawn = false;
            _nowSpawnCount = 0;
        }


        public void TakeSlow()
        {
            DOTween.timeScale = 0.5f;
            _spawnDuration = spawnDurationOnSlow;
            _isSlow = true;
        }

        void RecoverSlow()
        {
            DOTween.timeScale = 1f;
            _spawnDuration = spawnDurationDefault;
            _isSlow = false;
        }

        void AddSequence()
        {
            if (_isArrive)
            {
                Vector3[] path = DoTweenManager._Instance.GetPath(3, 1, 11, 21, 23, 25, 15, 5);
                sequence.Append(transform.DOPath(path, 10, PathType.CatmullRom)
                    .SetEase(Ease.Linear)
                    .SetOptions(true)
                    .OnComplete(AddSequence)
                    );

                StartSpawn();
            }
        }

        void BackToBase()
        {
            RecoverSlow();
            StopSpawn();
            sequence.Kill();

            _isArrive = false;

            transform.DOMove(new Vector3(10f, 8f), 3);
        }
    }
}
