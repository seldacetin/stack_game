using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stack : MonoBehaviour
{
    int stack_uzunlugu;
    int caunt = 1;
    const float max_deger = 6f;
    const float hiz_degeri = 0.06f;
    const float buyukluk = 4f;
    Vector2 stack_boyut = new Vector2(buyukluk, buyukluk);
    float hız = hiz_degeri;
    GameObject[] go_stack;
    int stack_index;
    bool x_ekseninde_hareket;
    bool stack_alindi = false;
    Vector3 camera_pos;
    Vector3 eski_stack_pos;
    float hassasiyet;
    bool dead = false;

    void Start(){
        stack_uzunlugu = transform.childCount;
        go_stack = new GameObject[stack_uzunlugu];
        for(int i=0; i<stack_uzunlugu; i++){
            go_stack[i] = transform.GetChild(i).gameObject;
        }
        stack_index = stack_uzunlugu-1;
    }

    void ArtikParca0l(Vector3 konum, Vector3 scale){
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localScale = scale;
        go.transform.position = konum;
        go.AddComponent<Rigidbody>();
    }
    
    void Update(){
        if(!dead){
            if(Input.GetMouseButtonDown(0)){
                if(Stack_Kontrol()){
                    Stack_Al_Koy();
                    caunt++;
                }
                else{
                    Bitir();
                }
            
            }
        
            Hareketlendir();
            transform.position = Vector3.Lerp(transform.position,camera_pos,0.1f);
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
            ArtikParca0l(konum , boyut);

        }
        else{
            float fark = eski_stack_pos.z-go_stack[stack_index].transform.localPosition.z;
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
            ArtikParca0l(konum , boyut);
        }
        return true;
    }
    void Bitir(){
        //Debug.Log("Game Over");
        //Destroy(gameObject, 2f);
        dead = true;
        go_stack[stack_index].AddComponent<Rigidbody>();
    }
}
