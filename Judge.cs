using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Judge : MonoBehaviour
{
    public Slider angleSlider;              //UIのスライダー
    public GameObject USBPort;              //ゲームオブジェクト
    public Button shootButton;              //UIのボタン
    public GameManager gameManager;         //GameManagerスクリプト
    public TextMeshProUGUI resultText;      //UIのテキスト(TMP)
    public TextMeshProUGUI scoreText;       //UIのテキスト(TMP)
    public GameObject resultPanel;          //結果画面パネル
    public ParticleSystem successEffect;    //成功時のエフェクト
    public ParticleSystem failEffect;       //失敗時のエフェクト
    public AudioClip successAudio;          //成功時の効果音
    public AudioClip failAudio;             //失敗時の効果音
    
    private float USBangle = 0;             //USBの角度
    private float portAngle = 0;            //USBポートの角度
    private AudioSource audioSource;        //オーディオソースコンポーネントアタッチ用


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        //shootButtonにOnButtonClicked関数を割り当てる
        //audioSourceにAudioSourceコンポーネントを割り当てる(成功・失敗時の効果音用)
        shootButton.onClick.AddListener(OnButtonClicked);
        audioSource = GetComponent<AudioSource>();
        resultPanel.SetActive(false);
    }

    //shootButtonクリック時の関数
    void OnButtonClicked()
    {
        //USBの角度を、スライダーの数値から取得し格納する
        USBangle = angleSlider.value;

        //ポートの角度を、USBポートオブジェクトのZ軸の角度から取得し格納する
        portAngle = USBPort.transform.eulerAngles.z;

        //ポートの角度を-180°～180°内に収める
        if(portAngle > 180)
        {
            portAngle -= 360;
        }

        gameManager.OnShoot();
    }

    //アタッチされているコライダーに何かが触れたときの関数
    void OnTriggerEnter(Collider other)
    {   
        //触れたもオブジェクトのタグがUSBの場合、CheckResult関数を実行
        if(other.CompareTag("USB"))
        {
            CheckResult();        
        }
    }

    //最終的な判定を下す関数
    public void CheckResult()
    {
        //ポートの角度とUSBの角度の差を、最小の角度で計算し、その絶対値を格納する
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(portAngle, USBangle));

        //経過時間をgameManagerスクリプトから取得し格納する
        float elapsedTime = gameManager.timer.GetElapsedTime();


        //ここから点数計算。バカゲーなので点数は高ければ高いほど良い
        float baseScore = Mathf.Max(0, 10000000 - (elapsedTime * 1000000));
        float angleMultiplier = (angleDifference == 0) ? 10000f : (10000f / (angleDifference + 1));
        float finalScore = Mathf.Max(0, baseScore * angleMultiplier);

        //結果画面パネルをアクティブ化
        resultPanel.SetActive(true);

        if(angleDifference <= 19)
        {
            //成功時の処理
            resultText.text = $"ポートの角度 = {portAngle}\nUSBの角度 = {USBangle}\n角度の誤差 = {angleDifference}\n基礎スコア = {baseScore}\n×\n角度ボーナス = {angleMultiplier:F1}";
            scoreText.text = $"<size=450%>最終スコア\n {finalScore:F0}!!!\nCongratulations!\nCongratulations!</size>";
            successEffect.Play();
            audioSource.clip = successAudio;
            audioSource.Play();
        }
        else
        {
            //失敗時の処理
            resultText.text = $"ポートの角度 = {portAngle}\nUSBの角度 = {USBangle}\n角度の誤差 = {angleDifference}\n基礎スコア = {baseScore}\n×\n角度ボーナス = {angleMultiplier:F1}";
            scoreText.text = $"<size=450%>最終スコア\n 000000000!!!\nTOOOOOO BAAAAAAD!</size>";
            failEffect.Play();
            audioSource.clip = failAudio;
            audioSource.Play();
        }
    }
}
