using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {

    public GameObject gameController;
    public AudioClip ingredientSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
        gameController.GetComponent<GameController>().PlaySoundEffect(ingredientSound);
        gameController.GetComponent<GameController>().NextWord();
    }
}
