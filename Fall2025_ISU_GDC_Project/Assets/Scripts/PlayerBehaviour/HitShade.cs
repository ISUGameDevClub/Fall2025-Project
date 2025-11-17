using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class HitShade : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color _flashcolor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;
    public SpriteRenderer _SPrend;


    private Coroutine _damageFlashCoroutine;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        OnServerInitialized();

        _SPrend = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnServerInitialized()
    {
        _materials = new Material[_spriteRenderers.Length];

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }  
    }

    public void CallDamageFlash()
    {
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

   private IEnumerator DamageFlasher()
    {
        /*SetFlashColor();*/

        _SPrend.material.SetFloat("_NewFlash", 0.5f);
        float currentFlashamount = 0f;
        float elapsedTime = 0f;
       
        yield return new WaitForSeconds(0.25f);

        _SPrend.material.SetFloat("_NewFlash", 0f);
        Debug.Log("Finished Coroutine");
    }
}