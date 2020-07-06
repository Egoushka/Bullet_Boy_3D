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
    private int countEnemies=0;
	private bool onClick = false, kill = false, bullet = false;
	public Text winText, countCoin;
	
	void Start()
	{
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
        }
        if(!bullet){
		    if (Input.GetMouseButtonDown(0)) {
			    onClick = true;
		    }
		    if (Input.GetMouseButtonUp(0)) {
			    StartCoroutine(CalculeteTrajectory());
                bullet = true;
			    onClick = false;
		    }
        }
        Debug.Log(onClick.ToString());
	}
	public Vector3 CalculeteTrajectory (float t, Vector3 p0, Vector3 p1, Vector3 p2) {
		float u = 1-t;
        float tt = t*t;
        float  uu = u*u;
        Vector3 p = uu*p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
	}
	private IEnumerator CalculeteTrajectory () {
		float t;
		for (int i = 1; i < numberPoints + 1  ; i++) {
            if(kill)
                break;
			t = i / (float)numberPoints;
			positions[i - 1] = CalculeteTrajectory(t, player.position, thirdDot.position,enemies[countEnemies].position );
            gameObject.transform.position = positions[i - 1];
            yield return new WaitForSeconds(0.1f);
		}
        kill = false;
 
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Coin") {
			Money.count++;
			countCoin.text = "Your coin: " + Money.count.ToString();
            Destroy(other.gameObject);
		}
        if (other.transform.tag == "Enemy")
		{
			countEnemies++;
            bullet = false;
            if((SceneManager.GetActiveScene().name == "Third level" && countEnemies == 2)||SceneManager.GetActiveScene().name != "Third level")
            {
                winText.text = "Good job";
			    NextScene();
            }
            Debug.Log(other.transform.position.ToString());
            
            Vector3 v3 = other.transform.position;
            Camera.main.transform.position = new Vector3(10, Camera.main.transform.position.y,Camera.main.transform.position.z );

            player.transform.position = v3;
            gameObject.transform.position = v3;
            //gameObject.transform.rotation = Quaternion.identity;
            kill = true;
            Destroy(other.gameObject);
		}
        else
        {
            if (other.transform.tag == "Wall")
		    {
			    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		        StartCoroutine(Instructions("Try again"));
            }
            return;
        }
		
	}
	void OnCollisionEnter(Collision other) {
		
     
	}
	IEnumerator Instructions(string str) {
		winText.text = str;
		yield return new WaitForSecondsRealtime(5);
		winText.text = string.Empty;
	}
	private void NextScene() {
		switch (SceneManager.GetActiveScene().name) {

		case "First level":
			SceneManager.LoadScene("Second level");
			break;
		case "Second level":
			SceneManager.LoadScene("Third level");
			break;
		case "Third level":
			SceneManager.LoadScene("Menu");
			break;
		default:
			break;
		}
        countCoin.text = "Your coin: " + Money.count.ToString();
	}
}