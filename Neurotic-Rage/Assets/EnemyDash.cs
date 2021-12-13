using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDash : MonoBehaviour
{
	public LayerMask layer;
	public NavMeshAgent agent;
	public GameObject player;
	private Vector3 target;
	public Animator anim;
	public float rotationSpeed;
	public float damage;
	public float dashMoveSpeed;
	public float dashCooldown;
	public float dashChargeTime;
	public float attackCooldown;
	public float dashDist;
	public float toClose;
	public float attackRange;
	private float distance;
	private bool isAttacking, moveTowards,canDash, isDashing;
	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		Invoke("LateStart", 4);
	}
	public void LateStart()
	{
		canDash = true;
	}
	private void Update()
	{
		distance = Vector3.Distance(transform.position, player.transform.position);
		if (distance <= attackRange)
		{
			if (!isAttacking)
			{
				StartCoroutine(Attack());
			}
		}
		else if (distance>= dashDist&& distance > attackRange)
		{
			if (!isAttacking)
			{
				if (canDash)
				{
					StartCoroutine(Dash());
				}
			}
		}
		if (distance <= toClose||isDashing)
		{
			agent.destination = transform.position;
		}
		else
		{
			agent.destination = player.transform.position;
			var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / 2 * Time.deltaTime);
		}

		if (moveTowards)
		{
			float targetDis = Vector3.Distance(target, transform.position);
			if (targetDis <= 1.1f)
			{
				moveTowards = false;
				isDashing = false;
				isAttacking = false;
				transform.LookAt(new Vector3(target.x,transform.position.y, target.z));
				agent.destination = player.transform.position;
			}
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, Time.deltaTime * dashMoveSpeed);
		}
	}
	public IEnumerator Attack()
	{
		anim.SetTrigger("Attack");
		isAttacking = true;
		yield return new WaitForSeconds(attackCooldown);
		isAttacking = false;
	}
	public IEnumerator Dash()
	{
		target = player.transform.position;
		if (Physics.Linecast(transform.position, target,layer))
		{
			canDash = false;
			yield return new WaitForSeconds(dashCooldown);
			canDash = true;
			yield break;
		}
		canDash = false;
		isAttacking = true;
		isDashing = true;
		anim.SetTrigger("Dash");
		yield return new WaitForSeconds(dashChargeTime);
		transform.LookAt(player.transform.position);
		moveTowards = true;
		yield return new WaitForSeconds(dashCooldown);
		isDashing = false;
		isAttacking = false;
		canDash = true;
	}
}