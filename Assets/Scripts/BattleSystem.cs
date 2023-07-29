using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerGO;
    public GameObject enemyGO;

    public TextMeshProUGUI dialogueText;

    public Vector3 playerLocation;
    public Vector3 enemyLocation;

    Unit playerUnit;
    Unit enemyUnit;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    public bool isPlayerTurn;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());

    }

    IEnumerator SetupBattle() 
    {
        playerUnit = playerGO.GetComponent<Unit>();
        playerLocation = playerGO.transform.position;

        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyLocation = enemyGO.transform.position;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        dialogueText.text = playerUnit.unitName + " vs. " + enemyUnit.unitName;
        
        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text = playerUnit.unitName + "'s turn!";
    }

    void EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + "'s turn!";
    }

    public void OnAttackButton()
    {
        if( state == BattleState.PLAYERTURN ) 
        StartCoroutine(PlayerAttack());
        else if( state == BattleState.ENEMYTURN )
        StartCoroutine(EnemyAttack());
    }

        public void OnEndButton()
    {
        if( state == BattleState.PLAYERTURN ) {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
        else if( state == BattleState.ENEMYTURN ) {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }  
    }

    IEnumerator PlayerAttack() 
    {
        // Damage the enemy
        if( playerUnit.currentEnergy > 0 )
        {
            bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

            // Update stats
            playerUnit.currentEnergy -= 1;
            playerHUD.SetEnergy(playerUnit.currentEnergy);
            enemyHUD.SetHP(enemyUnit.currentHP);

            yield return new WaitForSeconds(2f);

            // Check if enemy is dead
            if( isDead )
            {
                state = BattleState.WON;
                EndBattle();
            } 
        } else
        {
            dialogueText.text = "You are out of energy!";
        }
    }

    IEnumerator EnemyAttack()
    {
        // Damage the enemy
        if( enemyUnit.currentEnergy > 0 )
        {
            bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

            // Update stats
            enemyUnit.currentEnergy -= 1;
            enemyHUD.SetEnergy(enemyUnit.currentEnergy);
            playerHUD.SetHP(enemyUnit.currentHP);

            yield return new WaitForSeconds(2f);

            // Check if enemy is dead
            if( isDead )
            {
                state = BattleState.LOST;
                EndBattle();
            } 
        } else
        {
            dialogueText.text = "You are out of energy!";
        }
    }

    public void EndBattle()
    {
        if( state == BattleState.WON )
        {
            dialogueText.text = "You won!";
        } else if ( state == BattleState.LOST )
        {
            dialogueText.text = "You lost!";
        }
    }

    void Update()
    {
        if( state == BattleState.PLAYERTURN && playerUnit.currentEnergy > 0 ) {
            Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
            playerGO.transform.Translate(move * playerUnit.speed * Time.deltaTime);
        }


        if( state == BattleState.ENEMYTURN && enemyUnit.currentEnergy > 0 ) {
            Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
            enemyGO.transform.Translate(move * enemyUnit.speed * Time.deltaTime);
        }
     }
}
