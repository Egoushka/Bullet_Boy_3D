using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
   public void PlayPressed()
    {
        SceneManager.LoadScene("First level");
    }
    public void ExitPressed()
    {
        Application.Quit();
    }
}
/*void Update()
	{
        if(onClick){
            
     Vector3 mouse = new Vector3(0, 0, Input.mousePosition.z);
     Ray castPoint = Camera.main.ScreenPointToRay(mouse);
     Vector3 v3 = new Vector3(player.position.x -enemy.position.x, 0.5f,castPoint.origin.z);
     castPoint.origin = v3;
     RaycastHit hit;
     if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
     {
         thirdDot.transform.position = hit.point;
     }
        }
		if (Input.GetMouseButtonDown(0)) {
			onClick = true;
		}
		if (Input.GetMouseButtonUp(0)) {
			StartCoroutine(CalculeteQuadraticBezierPoint());
			onClick = false;
		}

	}*/