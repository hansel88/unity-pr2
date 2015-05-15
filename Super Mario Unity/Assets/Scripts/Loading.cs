using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {

    public void LoadLevel()
    {
        StartCoroutine("Wait");
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);

        Application.LoadLevel("Level_01");
    }
	
 
 
    
}
