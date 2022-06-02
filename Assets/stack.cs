using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
   

public class stack : MonoBehaviour
{
    int stack_uzunlugu;
    int caunt = 1;
    int skor=0;
    const float max_deger = 6f;
    const float hiz_degeri = 0.06f;
    const float buyukluk = 4f;
    Vector2 stack_boyut = new Vector2(buyukluk, buyukluk);
    float hız = hiz_degeri;
    float hata_payi=0.1f;
    GameObject[] go_stack;
    int stack_index;
    bool x_ekseninde_hareket;
    bool stack_alindi = false;
    Vector3 camera_pos;
    Vector3 eski_stack_pos;
    float hassasiyet;
    bool dead = false;
    int combo=0;
    Color32 renk;
    public Color32 renk1;
    public Color32 renk2;
    public Text textimiz;
    public GameObject g_panel;
    int high_score;
    public Text high_score_Text;
    int sayac = 0;
    Camera camera;


    void Start(){
        high_score = PlayerPrefs.GetInt("highscore");
        high_score_Text.text = high_score.ToString();
        textimiz.text = skor.ToString() ;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camera.backgroundColor = renk2;
        renk=renk1;
        stack_uzunlugu = transform.GetChildCount();
        go_stack = new GameObject[stack_uzunlugu];
        for(int i=0; i<stack_uzunlugu; i++){
            go_stack[i] = transform.GetChild(i).gameObject;
            go_stack[i].GetComponent<Renderer>().material.color = renk;
        }
        stack_index = stack_uzunlugu-1;
    }

    void ArtikParca0l(Vector3 konum, Vector3 scale,Color32 renkli){
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localScale = scale;
        go.transform.position = konum; 
        go.GetComponent<Renderer>().material.color = renkli;
        go.AddComponent<Rigidbody>();
    }
    
    void Update(){
        if(!dead){
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer){
           
                if(Input.GetMouseButtonDown(0)){
                    Oyun();
                }
                Hareketlendir();
                transform.position = Vector3.Lerp(transform.position,camera_pos,0.1f);
            }
            else if(Application.platform == RuntimePlatform.Android){
                if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
                    Oyun();
                }
                Hareketlendir();
                transform.position = Vector3.Lerp(transform.position,camera_pos,0.1f);
            }
        }
    }

    public void Oyun(){
        if(Stack_Kontrol()){
            Stack_Al_Koy();
            caunt++;
            skor++;
            textimiz.text = skor.ToString(); 
            if(skor > high_score){
                high_score = skor;
            }
            renk = new Color32((byte)(renk.r - 5),(byte)(renk.g - 5),(byte)(renk.b - 5),renk.a);       
            renk2 = new Color32((byte)(renk2.r - 2),(byte)(renk2.g - 2),(byte)(renk2.b - 1),renk2.a);          
        }
        else{
            Bitir();
        }
    }
    void Stack_Al_Koy(){
        eski_stack_pos = go_stack[stack_index].transform.localPosition;
        if(stack_index <= 0){
            stack_index = stack_uzunlugu;
        }
        stack_alindi = false;
        stack_index--;
        x_ekseninde_hareket = !x_ekseninde_hareket;
        camera_pos = new Vector3(0, -caunt, 0);
        go_stack[stack_index].transform.localScale = new Vector3(stack_boyut.x,1,stack_boyut.y);
        go_stack[stack_index].GetComponent<Renderer>().material.color = Color32.Lerp(go_stack[stack_index].GetComponent<Renderer>().material.color,renk,0.5f);
        //Camera camera =GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camera.backgroundColor = Color32.Lerp(camera.backgroundColor,renk2,0.1f);        
    }
    void Hareketlendir(){
        if(x_ekseninde_hareket){
            if(!stack_alindi){
            go_stack[stack_index].transform.localPosition = new Vector3(-9,caunt,hassasiyet);
            stack_alindi = true; 
            }
            if(go_stack[stack_index].transform.localPosition.x > max_deger){
                hız = hiz_degeri * -1;
            }
            else if(go_stack[stack_index].transform.localPosition.x < -max_deger){
                hız = hiz_degeri;
            }
            go_stack[stack_index].transform.localPosition+= new Vector3(hız,0,0);
        }
        else{
            if(!stack_alindi){
            go_stack[stack_index].transform.localPosition = new Vector3(hassasiyet,caunt,-9);
            stack_alindi = true; 
            }
            if(go_stack[stack_index].transform.localPosition.z > max_deger){
                hız = hiz_degeri * -1;
            }
            else if(go_stack[stack_index].transform.localPosition.z < -max_deger){
                hız = hiz_degeri;
            }
            go_stack[stack_index].transform.localPosition+= new Vector3(0,0,hız);
        }

    }
    bool Stack_Kontrol() {
        if(x_ekseninde_hareket){
            float fark = eski_stack_pos.x-go_stack[stack_index].transform.localPosition.x;
            if(Mathf.Abs(fark)>hata_payi){
                combo=0;
                Vector3 konum;
                if(go_stack[stack_index].transform.localPosition.x > eski_stack_pos.x){
                    konum = new Vector3(go_stack[stack_index].transform.position.x + go_stack[stack_index].transform.position.x / 2 , go_stack[stack_index].transform.position.y , go_stack[stack_index].transform.position.z / 2 );
                }
                else{
                    konum = new Vector3(go_stack[stack_index].transform.position.x - go_stack[stack_index].transform.position.x / 2 , go_stack[stack_index].transform.position.y , go_stack[stack_index].transform.position.z / 2 );
                }
                Vector3 boyut = new Vector3(fark,1,stack_boyut.y);
                stack_boyut.x -= Mathf.Abs(fark);
                if(stack_boyut.x<0)
                {
                    return false;
                }
                go_stack[stack_index].transform.localScale = new Vector3(stack_boyut.x,1,stack_boyut.y);
                float mid = go_stack[stack_index].transform.localPosition.x / 2 + eski_stack_pos.x / 2;
                go_stack[stack_index].transform.localPosition = new Vector3(mid , caunt , eski_stack_pos.z);
                hassasiyet = go_stack[stack_index].transform.localPosition.x;
                ArtikParca0l(konum , boyut, go_stack[stack_index].GetComponent<Renderer>().material.color);
            }
            else{
                combo++;
                if(combo>3){
                    stack_boyut.x+=0.3f;
                    if(stack_boyut.x>buyukluk){

                        stack_boyut.x=buyukluk;
                        
                    }
                    go_stack[stack_index].transform.localScale = new Vector3(stack_boyut.x,1,stack_boyut.y);
                    go_stack[stack_index].transform.localPosition = new Vector3(eski_stack_pos.x , caunt , eski_stack_pos.z);
                }
                else{
                    go_stack[stack_index].transform.localPosition = new Vector3(eski_stack_pos.x , caunt , eski_stack_pos.z);
                }
                hassasiyet = go_stack[stack_index].transform.localPosition.x;
            }
        }
        else{
            float fark = eski_stack_pos.z-go_stack[stack_index].transform.localPosition.z;
            if(Mathf.Abs(fark)>hata_payi){
                combo=0;
                Vector3 konum;
                if(go_stack[stack_index].transform.localPosition.x > eski_stack_pos.z){
                    konum = new Vector3(go_stack[stack_index].transform.position.x , go_stack[stack_index].transform.position.y , go_stack[stack_index].transform.position.z / 2 + go_stack[stack_index].transform.localScale.z / 2 );
                }
                else{
                    konum = new Vector3(go_stack[stack_index].transform.position.x , go_stack[stack_index].transform.position.y , go_stack[stack_index].transform.position.z / 2 - go_stack[stack_index].transform.localScale.z / 2 );
                }
                Vector3 boyut = new Vector3(stack_boyut.x,1,fark);
                stack_boyut.y -= Mathf.Abs(fark);
                if(stack_boyut.y<0)
                {
                    return false;
                }
                go_stack[stack_index].transform.localScale = new Vector3(stack_boyut.x,1,stack_boyut.y);
                float mid = go_stack[stack_index].transform.localPosition.z / 2 + eski_stack_pos.z / 2;
                go_stack[stack_index].transform.localPosition = new Vector3(eski_stack_pos.x , caunt , mid);
                hassasiyet = go_stack[stack_index].transform.localPosition.z;
                ArtikParca0l(konum , boyut , go_stack[stack_index].GetComponent<Renderer>().material.color);
                combo++;
            }
            
            else{
                combo++;
                if(combo>3){
                    stack_boyut.y+=0.3f;
                    if(stack_boyut.y>buyukluk){
                       stack_boyut.y=buyukluk;                     
                    }
                    go_stack[stack_index].transform.localScale = new Vector3(stack_boyut.x,1,stack_boyut.y);
                    go_stack[stack_index].transform.localPosition = new Vector3(eski_stack_pos.x , caunt , eski_stack_pos.z);
                }
                else{
                    go_stack[stack_index].transform.localPosition = new Vector3(eski_stack_pos.x , caunt , eski_stack_pos.z);
                }
                hassasiyet = go_stack[stack_index].transform.localPosition.z;
            }
        }
        return true;
    }
    void Bitir(){
        dead = true;
        go_stack[stack_index].AddComponent<Rigidbody>();
        g_panel.SetActive(true);
        PlayerPrefs.SetInt("highscore", high_score);
        high_score_Text.text = high_score.ToString();
        textimiz.text = "";
    }
    public void Yeni_Oyun(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);       
    }
}
