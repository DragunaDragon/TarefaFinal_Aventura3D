using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cloth;

public class HealthBase : MonoBehaviour, IDamageable
{
	public float startLife = 10f;
	public bool destroyOnKill = false;
	[SerializeField] private float _currentLife;



	public Action<HealthBase> OnDamage;
	public Action<HealthBase> OnKill;

	public List<UIFillUpdate> uIFillUpdate;

	public float damageMultiply = 1;


	public void Awake()
	{
		Init();
	}


	public void Init()
	{
		ResetLife();
	}

	public void ResetLife()
	{
		_currentLife = startLife;
		UpdateUI();
	}

	protected virtual void Kill()
	{
		if(destroyOnKill)

		Destroy(gameObject, 3f);

		OnKill?.Invoke(this);
	}


	[NaughtyAttributes.Button]
	public void Damage()
	{
		Damage(5);
	}


	public void Damage(float f)
	{
		_currentLife -= f * damageMultiply; 

		if (_currentLife <= 0)
		{
			Kill();
		}
		UpdateUI();
		OnDamage?.Invoke(this);
	
	}

    public void Damage(float damage, Vector3 dir)
    {
		Damage(damage);
    }


	private void UpdateUI()
    {
		if(uIFillUpdate !=null)
        {
			uIFillUpdate.ForEach(i => i.UpdateValue((float)_currentLife / startLife));
        }
    }

	public void ChangeDamageMultiply(float damage, float duration)
	{
		StartCoroutine(ChangeDamageCoroutine(damageMultiply, duration));
	}

	IEnumerator ChangeDamageCoroutine(float damageMultiply, float duration)
	{
		this.damageMultiply = damageMultiply; 
		yield return new WaitForSeconds(duration);
		this.damageMultiply = 1;
	}
}
