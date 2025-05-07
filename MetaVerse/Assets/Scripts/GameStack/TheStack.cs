using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    private const float BoundSize = 12f;
    private const float MovingBoundSize = 2.5f;
    private const float StackMovingSpeed = 5.0f;
    private const float BlockMovingSpeed = 5.0f;
    private const float ErrorMargin = 0.5f;

    public GameObject originBlock = null;

    private Vector2 prevBlockPosition;
    private Vector2 desirePosition;
    private Vector2 stackBounds = new Vector2(BoundSize, 1);

    Transform lastBlock = null;
    float blockTransition = 0f;

    int stackCount = -1;
    public int Score { get { return stackCount; } }

    int comboCount = 0;
    public int Combo { get { return comboCount; } }

    private int maxCombo = 0;
    public int MaxCombo {  get => maxCombo; }

    int bestScore = 0;
    public int BestScore { get => bestScore; }
    int bestCombo = 0;
    public int BestCombo { get => bestCombo; }
    private const string BestScoreKey = "BestScore";
    private const string BestComboKey = "BestCombo";
    
    public Color prevColor;
    public Color nextColor;

    bool isMovingRight = true; //처음에 오른쪽으로 이동
    
    private bool isGameOver = true;

    void Start()
    {
        if (originBlock == null)
        {
            Debug.Log("OriginBlock is Null");
            return;
        }

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        bestCombo = PlayerPrefs.GetInt(BestComboKey, 0);

        prevBlockPosition = Vector2.down;

        Spawn_Block();

    }

    void Update()
    {
        if (isGameOver)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if(PlaceBlock())
            {
                Spawn_Block();
            }
            else
            {
                Debug.Log("GameOver");
                UpdateScore();
                isGameOver = true;
                GameOverEffect();
                UIManager.Instance.SetScoreUI();
            }
        }
        MoveBlock();
        transform.position = Vector2.Lerp(transform.position, desirePosition, StackMovingSpeed * Time.deltaTime);
    }

    bool Spawn_Block()
    {
        if (lastBlock != null)
            prevBlockPosition = lastBlock.localPosition;

        GameObject newBlock = null;
        Transform newTrans = null;

        newBlock = Instantiate(originBlock);

        if (newBlock == null)
        {
            Debug.Log("NewBlock Instantiate Failed");
            return false;
        }

        ColorChange(newBlock);

        newTrans = newBlock.transform;
        newTrans.parent = this.transform;
        newTrans.localPosition = prevBlockPosition + Vector2.up;
       
        newTrans.localScale = new Vector2(stackBounds.x, stackBounds.y);

        stackCount++;

        desirePosition = Vector2.down * stackCount;
        blockTransition = 0;
        lastBlock = newTrans;


        isMovingRight = !isMovingRight; //오른쪽으로 한번 왼쪽으로 한번 씩 나오게 됨
        UIManager.Instance.UpdateScore();
        return true;

    }
    Color GetRandomColor() //색깔
    {
        float r = Random.Range(100f, 250f) / 255f;
        float g = Random.Range(100f, 250f) / 255f;
        float b = Random.Range(100f, 250f) / 255f;

        return new Color(r, g, b);
    }
    void ColorChange(GameObject go)
    {
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);
        Renderer rn = go.GetComponent<Renderer>();
        if (rn == null)
        {
            Debug.Log("Renderer is NULL");
            return;
        }
        rn.material.color = applyColor;
        Camera.main.backgroundColor = new Color(1f, 1f, 1f) - applyColor; // 2D에서는 비슷하게 하니까 헷갈려서 1,1,1에서 stack색을 빼줘서 색반전느낌
        if (applyColor.Equals(nextColor) == true)
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }
    }

    void MoveBlock()
    {
        blockTransition += Time.deltaTime * BlockMovingSpeed;

        float movePosition = Mathf.PingPong(blockTransition, BoundSize)-BoundSize/ 2;

        if (isMovingRight)
        {
            lastBlock.localPosition = new Vector2(movePosition * MovingBoundSize, stackCount);
        }
        else
        {
            lastBlock.localPosition = new Vector2(-movePosition * MovingBoundSize, stackCount);
        }


    }

    bool PlaceBlock()
    {
        Vector2 lastPosition = lastBlock.localPosition; //위치 받아오기

        float deltaX = prevBlockPosition.x - lastPosition.x;
        bool isNegativeNum = (deltaX < 0) ? true : false;

        deltaX = Mathf.Abs(deltaX);
        if (deltaX > ErrorMargin)
        {
            stackBounds.x -= deltaX;
            if (stackBounds.x <= 0)
            {
                return false;
            }

            float middle = (prevBlockPosition.x + lastPosition.x) / 2;
            lastBlock.localScale = new Vector2(stackBounds.x, 1);

            Vector2 tempPosition = lastBlock.localPosition;
            tempPosition.x = middle;
            lastBlock.localPosition = lastPosition = tempPosition;

            float rubbleHalfScale = deltaX / 2f;
            CreateRubble(
                new Vector2(isNegativeNum
                ? lastPosition.x + stackBounds.x / 2 + rubbleHalfScale
                : lastPosition.x - stackBounds.x / 2 - rubbleHalfScale
                , lastPosition.y),
                new Vector2(deltaX, 1)
                );
            comboCount = 0;

        }
        else
        {
            ComboCheck();
            lastBlock.localPosition = prevBlockPosition + Vector2.up;
        }
        return true;
    }
    void CreateRubble(Vector2 pos, Vector2 scale)
    {
        GameObject go = Instantiate(lastBlock.gameObject);
        go.transform.parent = this.transform;

        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.transform.localRotation = Quaternion.identity;

        go.AddComponent<Rigidbody2D>();
        go.name = "Rubble";
    }

    void ComboCheck()
    {
        comboCount++;
        if (comboCount > maxCombo)
            maxCombo = comboCount;

        if ((comboCount % 5) == 0)
        {
            Debug.Log("5Combo Success!");
            
            stackBounds += new Vector2(0.5f, 0);
            stackBounds.x =
                (stackBounds.x > BoundSize) ? BoundSize : stackBounds.x;
            lastBlock.localScale = new Vector2(stackBounds.x, 1);
            // 콤보 달성시 새로 나오는 것만 커지면 맞추기 어려워서, 콤보를채운 마지막것도 크기 키우기
        }
    }
    void UpdateScore()
    {
        if (bestScore < stackCount)
        {
            Debug.Log("최고점수갱신");
            bestScore = stackCount;
            bestCombo = maxCombo;

            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.SetInt(BestComboKey, bestCombo);
        }
    }
    void GameOverEffect()
    {
        int childCount = this.transform.childCount;

        for (int i = 1; i <=childCount&& i <= 20; i++)
        {
            int idx = childCount - i;
            if (idx < 0)
                break;

            GameObject go =
                this.transform.GetChild(idx).gameObject;

            if (go.name.Equals("Rubble"))
                continue;

            Rigidbody2D rigid = go.AddComponent<Rigidbody2D>();
            rigid.AddForce(
                (Vector2.up * Random.Range(0, 10f)
                + Vector2.right * (Random.Range(0, 10f)-5f))
                * 100f
                );
        }
    }
    public void Restart()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        isGameOver = false;

        lastBlock = null;
        desirePosition = Vector2.zero;
        stackBounds = new Vector2(BoundSize, 1);

        stackCount = -1;
        isMovingRight = true;
        blockTransition = 0f;

        comboCount = 0;
        maxCombo = 0;

        prevBlockPosition = Vector3.down;

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        Spawn_Block();
        Spawn_Block();
    }
}
