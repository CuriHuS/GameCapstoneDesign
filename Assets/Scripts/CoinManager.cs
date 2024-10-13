using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int value;

    public void OnMouseDown()
	{
		GameManager.IncrementCoins(value);

		Destroy(this.gameObject);
	}
}
