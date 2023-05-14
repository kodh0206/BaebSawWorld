using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaebSae : MonoBehaviour
{   
 
    private readonly string[] levelList = new string[] {
      "Egg",
      "CellLv1",
      "CellLv2",
      "CellLv3",
      "BLv1",
      "BLv2",
      "BLv3",
      "BLv4",
      "BLv5"
     };

  
    [SerializeField]
    int level;//달결
    
    [SerializeField]
    float cashflow;
    //추후 효과 적용예정
      // 터치 입력
    private Vector2 touchPosition;
    private float offsetX, offsetY;
    private Transform currentPosition;
    public static bool touchRealeased;
    
    private Vector2
    
  
  public string GetName(int level){
    return levelList[level];
  } 

   
  
    //레벨 1일때 알까기 
    public void hatch(){
      Ocean ocean = GameObject.FindGameObjectWithTag("Ocean").GetComponent<Ocean>();
      GameObject newEgg =Instantiate(Resources.Load("CellLv1"), currentPosition.transform.position, transform.rotation) as GameObject;
      newEgg.transform.SetParent(ocean.Oplaces[ocean.getCurrentBirds()-1].transform,false);
      ocean.hatchEgg();
      Destroy(gameObject);
    }


    public int GetLevel(){
      return level;
    }

    //입력 명령
    private void OnMouseDown()
   {
    touchRealeased =false;
    offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x -transform.position.x;
    offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y-transform.position.y;
    
    
    if(level==1){
      hatch();
    }
   }


   private void OnMouseDrag()
   {
        touchPosition =Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(touchPosition.x -offsetX, touchPosition.y -offsetY);   
       
   }

   private void OnMouseUp(){
    touchRealeased =true;
   
   //레벨 1이면 알까기 
  
   }


  //뱁새병합
   private void OnTriggerStay2D(Collider2D collision)
   {
      int thisBaebasaeLv;//충돌한 뱁새 레벨 
      int collisionBaebsaeLv; //충돌당한 뱁세 레벨
      Transform respawnPoint =gameObject.GetComponent<Transform>();

      thisBaebasaeLv = gameObject.GetComponent<BaebSae>().GetLevel();
      collisionBaebsaeLv = collision.gameObject.GetComponent<BaebSae>().GetLevel();

    if(touchRealeased && thisBaebasaeLv == collisionBaebsaeLv && thisBaebasaeLv < 9 && thisBaebasaeLv>0)
    {
      
      
      //1부터 4까지 
      if(thisBaebasaeLv <4){
      Ocean ocean = GameObject.FindGameObjectWithTag("OLayingPlaces").GetComponent<Ocean>();
      GameObject evolvedBaebabsae=Instantiate(Resources.Load(levelList[thisBaebasaeLv]),respawnPoint.position, Quaternion.identity) as GameObject;
      evolvedBaebabsae.transform.SetParent(GameObject.FindGameObjectWithTag("OLayingPlaces").transform,true);
      evolvedBaebabsae.transform.localScale =new Vector2(respawnPoint.localScale.x, respawnPoint.localScale.y);


      Ocean o= GameObject.FindGameObjectWithTag("Ocean").GetComponent<Ocean>();
      o.GetComponent<Ocean>().updateBirds(levelList[thisBaebasaeLv]);
      o.GetComponent<Ocean>().deleteBirds(levelList[thisBaebasaeLv-1]);
      o.GetComponent<Ocean>().deleteBirds(levelList[thisBaebasaeLv-1]);
      //Ocean.GetComponent<Ocean>().lostbirds(levelList[thisBaebasaeLv]);
      touchRealeased=false;
      Destroy(collision.gameObject);
      Destroy(gameObject);
      //ocean
      }
      //실헐실 끝 생성시 다른 레벨로 넘어감 
      else if(thisBaebasaeLv ==4){
      GameObject evolvedBaebabsae=Instantiate(Resources.Load(levelList[thisBaebasaeLv]),respawnPoint.position, Quaternion.identity) as GameObject;
      GameObject forest= GameObject.FindGameObjectWithTag("Forest");
      forest.GetComponent<Forest>().updateBirds(levelList[thisBaebasaeLv]);
      Destroy(collision.gameObject);
      Destroy(gameObject);
      Destroy(evolvedBaebabsae);
      touchRealeased=false;
   
      }

         else if(thisBaebasaeLv >4){
          
      //진화한 뱁새
      GameObject evolvedBaebabsae=Instantiate(Resources.Load(levelList[thisBaebasaeLv]),respawnPoint.position, Quaternion.identity) as GameObject;
      evolvedBaebabsae.transform.SetParent(GameObject.FindGameObjectWithTag("FLayingPlaces").transform,true);
      evolvedBaebabsae.transform.localScale =new Vector2(respawnPoint.localScale.x, respawnPoint.localScale.y);

      GameObject forest= GameObject.FindGameObjectWithTag("Forest");
      forest.GetComponent<Forest>().updateBirds(levelList[thisBaebasaeLv]);
      forest.GetComponent<Forest>().deleteBirds(levelList[collisionBaebsaeLv]);
      forest.GetComponent<Forest>().deleteBirds(levelList[collisionBaebsaeLv]);
      touchRealeased=false;
      Destroy(collision.gameObject);
      Destroy(gameObject);
   
      }


      //Debug.Log(evolvedBaebabsae.transform.position.x);
      //Debug.Log("진화 완료");
    }
   }




}
