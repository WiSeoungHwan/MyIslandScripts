using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private float maxHealthPoints = 100;

    [SerializeField]
    private float healthBarStepsLength = 10;

    [SerializeField]
    private float damagesDecreaseRate = 10;

    private float currentHealthPoints;

    private RectTransform imageRectTransform;

    private float damages;

    public float Health
    {
        get { return currentHealthPoints; }
        set
        {
            currentHealthPoints = Mathf.Clamp(value, 0, MaxHealthPoints);
            image.material.SetFloat("_Percent", currentHealthPoints / MaxHealthPoints);

            if (currentHealthPoints < Mathf.Epsilon)
                Damages = 0;
        }
    }

    public float Damages
    {
        get { return damages; }
        set
        {
            damages = Mathf.Clamp(value, 0, MaxHealthPoints);
            image.material.SetFloat("_DamagesPercent", damages / MaxHealthPoints);
        }
    }

    public float MaxHealthPoints
    {
        get { return maxHealthPoints; }
        set
        {
            maxHealthPoints = value;
            image.material.SetFloat("_Steps", MaxHealthPoints / healthBarStepsLength);
        }
    }

    protected void Awake()
    {
        imageRectTransform = image.GetComponent<RectTransform>();
        image.material = Instantiate(image.material); // Clone material
        image.material.SetVector("_ImageSize", new Vector4(imageRectTransform.rect.size.x, imageRectTransform.rect.size.y, 0, 0));

        MaxHealthPoints = MaxHealthPoints; // Force the call to the setter in order to update the material
        currentHealthPoints = MaxHealthPoints; // Force the call to the setter in order to update the material
        StartCoroutine("OneSecTimer");

    }

    protected void Update()
    {
        if (Damages < MaxHealthPoints)
        {
            Damages += damagesDecreaseRate * Time.deltaTime;
        }

        // Dummy test, you can remove this
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hurt(1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(1);
        }
    
    }
    IEnumerator OneSecTimer(){
        yield return new WaitForSeconds(1);
        if(Health < 5){
            Health += 1;
            Damages = 0;
        }
        StartCoroutine("OneSecTimer");
    }

    public void Hurt(float damagesPoints)
    {
        Damages = 0;
        Health -= damagesPoints;
    }
    public void Heal(float healPoints){
        Damages = healPoints;
        Health += Damages;
    }
}