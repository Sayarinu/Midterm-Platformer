using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private Image[] images;
    [SerializeField]
    private Sprite full;
    [SerializeField]
    private Sprite empty;

    public int health=3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0;i<images.Length;i++){
            if(i<health){
                images[i].sprite = full;
            }else{
                images[i].sprite = empty;
            }
        }
    }
}
