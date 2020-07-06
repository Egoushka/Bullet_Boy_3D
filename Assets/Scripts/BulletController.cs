using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BulletController : MonoBehaviour
{
	private int numberPoints = 50;
	private Vector3[] positions = new Vector3[50];
	public Transform  player, thirdDot;
	public Transform[] enemies;
    private int countEnemies=0, killEnemies = 0;
	private bool onClick = false, kill = false, bullet = false;
	public Text winText, countCoin;
	public LineRenderer lineRenderer;
	
	void Start()
	{
		//lineRenderer.SetVertexCount(numberPoints);
		lineRenderer.positionCount = numberPoints;
		for(int i = 0; i < enemies.Length; i++)
		{
			if(enemies[i].transform.tag == "NotMainEnemy")
				countEnemies++;
		}
        if(Money.count == 0)
            countCoin.text = "You have no coin\n";
        else
            countCoin.text = "Your coin: " + Money.count.ToString();
		StartCoroutine(Instructions("Try to kill this fucker"));
	}
	void Update()
	{
        if(onClick){
            Ray castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            {
                thirdDot.transform.position = hit.point;
            }
		Debug.Log(countEnemies.ToString());

			DrawLineTrajectory();
        }
        if(!bullet){
		    if (Input.GetMouseButtonDown(0)) {
				lineRenderer.enabled = true;
			    onClick = true;
		    }
		    if (Input.GetMouseButtonUp(0)) {
			    StartCoroutine(CalculeteTrajectoryOfBullet());
                bullet = true;
			    onClick = false;
				lineRenderer.enabled = false;
		    }
        }
//        Debug.Log(onClick.ToString());
	}
	public Vector3 CalculeteTrajectory (float t, Vector3 p0, Vector3 p1, Vector3 p2) {
		p0 = new Vector3(p0.x,0.5f,p0.z);
		p1 = new Vector3(p1.x,0.5f,p1.z);
		p2 = new Vector3(p2.x,0.5f,p2.z);
		float u = 1-t;
        float tt = t*t;
        float  uu = u*u;
        Vector3 p = uu*p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
	}
	private void DrawLineTrajectory () {
		float t;
		for (int i = 1; i < numberPoints + 1  ; i++) {
			t = i / (float)numberPoints;
			positions[i - 1] = CalculeteTrajectory(t, player.position, thirdDot.position,enemies[countEnemies].position );
		}
		lineRenderer.SetPositions(positions);
	}
	private IEnumerator CalculeteTrajectoryOfBullet () {
		float t;
		for (int i = 1; i < numberPoints + 1  ; i++) {
            if(kill)
                break;
			t = i / (float)numberPoints;
			positions[i - 1] = CalculeteTrajectory(t, player.position, thirdDot.position,enemies[countEnemies].position );
            gameObject.transform.position = positions[i - 1];
            yield return new WaitForSeconds(0.01f);
		}
        kill = false;
 
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Coin") {
			Money.count++;
			countCoin.text = "Your coin: " + Money.count.ToString();
		}
        if (other.transform.tag == "Enemy")
		{
			countEnemies++;
			killEnemies++;
            bullet = false;
			if(countEnemies == enemies.Length && killEnemies != enemies.Length)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		   		StartCoroutine(Instructions("Try again"));
            	return;
			}
            else if (countEnemies == enemies.Length && enemies.Length == killEnemies)
            {
                winText.text = "Good job";
			    NextScene();
            }
            Debug.Log(other.transform.position.ToString());
            
            Vector3 v3 = other.transform.position;
            Camera.main.transform.position = new Vector3(10, Camera.main.transform.position.y,Camera.main.transform.position.z );
            player.transform.position = v3;
            gameObject.transform.position = v3;
            kill = true;
		}
        if (other.transform.tag == "NotMainEnemy")
			killEnemies++;

        if (other.transform.tag == "Wall")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		    StartCoroutine(Instructions("Try again"));
            return;
        }
        Destroy(other.gameObject);
	}
	IEnumerator Instructions(string str) {
		winText.text = str;
		yield return new WaitForSecondsRealtime(5);
		winText.text = string.Empty;
	}
	private void NextScene() {
		
		for(int i = 1; i < SceneManager.sceneCountInBuildSettings + 1;i++){
				Debug.Log(i.ToString() + " level(1)");
			if(i == SceneManager.sceneCountInBuildSettings - 1)
			{
				SceneManager.LoadScene("Menu");
				break;
			}
			if(SceneManager.GetActiveScene().name == i.ToString() + " level")
			{
				i++;
				Debug.Log(i.ToString() + " level(2)");
				SceneManager.LoadScene(i.ToString() + " level");
				break;
			}
			
		}
        countCoin.text = "Your coin: " + Money.count.ToString();
	}
}