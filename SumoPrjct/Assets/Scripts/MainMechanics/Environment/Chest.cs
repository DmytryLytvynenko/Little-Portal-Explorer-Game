using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : DestroyableEnvironment
{
    [SerializeField] private Animation _animation;
    protected override IEnumerator DropReward()
    {
        for (int i = 0; i < coinCount; i++)
        {
            yield return new WaitForSeconds(dropDelay);
            GameObject coin = coinPool.GetCoin();
            coin.transform.position = transform.position;
            coin.GetComponent<Rigidbody>().AddForce((Vector3.up * 2 + RandomVector.normalized) * dropFlyForce);
        }
        for (int i = 0; i < healCount; i++)
        {
            yield return new WaitForSeconds(dropDelay);
            GameObject heal = healPool.GetHeal();
            heal.transform.position = transform.position;
            heal.GetComponent<Rigidbody>().AddForce((Vector3.up * 2 + RandomVector.normalized) * dropFlyForce);
        }
    }
    protected override void OnEntityDied()
    {
        Destroy(healthControll);
        _animation.Play();
    }
    public void AnimationEventDropReward()
    {
        StartCoroutine(nameof(DropReward));
    }
}
