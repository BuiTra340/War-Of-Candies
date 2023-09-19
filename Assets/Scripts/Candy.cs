using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public GameObject Explosion;
    private GameObject otherCandy;
    public int column;
    public int row;
    private int PreviousColumn;
    private int PreviousRow;
    Board board;
    public int targetX;
    public int targetY;

    private Vector2 firstPosTouch;
    private Vector2 finalPosTouch;
    private Vector2 tempPosition;
    private float swipeAngle;
    private float swipeResist = 1f;
    public bool isMatches;
    PlayerController playerController;
    FindMatches findMatches;
    AudioManager audioManager;
    private Vector3 PosPlayer = new Vector3(0, 9.3f, 0);

    ItemBuff item;
    public LayerMask layer;
    private void Start()
    {
        board = FindObjectOfType<Board>();
        playerController = FindObjectOfType<PlayerController>();
        findMatches = FindObjectOfType<FindMatches>();
        audioManager = FindObjectOfType<AudioManager>();
        item = FindObjectOfType<ItemBuff>();
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;
        //PreviousColumn = column;
        //PreviousRow = row;
    }
    private void FixedUpdate()
    {
        if(isMatches)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.red;
        }
        targetX = column;
        targetY = row;
        if(Mathf.Abs(targetX - transform.position.x ) > 0.1f)
        {
            // di chuyen den muc tieu da xac dinh
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.6f);
            if(board.allCandies[column, row] != this.gameObject)
            {
                board.allCandies[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }else
        {
            // cap nhat vi tri moi
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }

        if(Mathf.Abs(targetY - transform.position.y) >0.1f)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.6f);
            if (board.allCandies[column, row] != this.gameObject)
            {
                board.allCandies[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }

    }
    private void OnMouseDown()
    {
        if (board.state == GameState.move && playerController.TurnAttack > 0)
        {
            firstPosTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        if (board.state == GameState.move && playerController.TurnAttack > 0)
        {
            finalPosTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }
    void CalculateAngle()
    {
        if (Mathf.Abs(finalPosTouch.x - firstPosTouch.x) > swipeResist || Mathf.Abs(finalPosTouch.y - firstPosTouch.y) > swipeResist)
        {
            
                swipeAngle = Mathf.Atan2(finalPosTouch.y - firstPosTouch.y, finalPosTouch.x - firstPosTouch.x) * Mathf.Rad2Deg;
                if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)//Right Swipe
                {
                    MovePieces(1, 0);
                }
                else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)//Up Swipe
                {
                    MovePieces(0, 1);
                }
                else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)//Down Swipe
                {
                    MovePieces(0, -1);
                }
                else if (swipeAngle > 135 || swipeAngle <= -135 && column > 0)//Left Swipe
                {
                    MovePieces(-1, 0);
                }
                board.state = GameState.wait;
            
        }
        else board.state = GameState.move;
    }
    void MovePieces(int columnPiece, int rowPiece)
    {
        audioManager.PlaySFX(audioManager.moveCandy);
        playerController.TurnAttack--;
        playerController.ShieldStat = 0;
        playerController.EnemyBleed();
        item.IncreaseHpWhenMove();
        ItemBuff.Critical2Turns--;
        PreviousColumn = column;
        PreviousRow = row;
        otherCandy = board.allCandies[column + columnPiece, row + rowPiece];
        Destroy(Instantiate(Explosion, board.allCandies[column, row].transform.position, Quaternion.identity),.4f);
        Destroy(Instantiate(Explosion, otherCandy.transform.position, Quaternion.identity),.4f);
        otherCandy.GetComponent<Candy>().column -= columnPiece;
        otherCandy.GetComponent<Candy>().row -= rowPiece;
        column += columnPiece;
        row += rowPiece;
        StartCoroutine(CheckMoveCo());
    }

    IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(0.5f);
        if (otherCandy != null)
        {
            if (!isMatches && !otherCandy.GetComponent<Candy>().isMatches)
            {
                otherCandy.GetComponent<Candy>().row = row;
                otherCandy.GetComponent<Candy>().column = column;
                column = PreviousColumn;
                row = PreviousRow;
                yield return new WaitForSeconds(.5f);
                board.state = GameState.move;
            }
            else
            {
                board.DestroyCandy();
            }
            otherCandy = null;
        }
    }
}
