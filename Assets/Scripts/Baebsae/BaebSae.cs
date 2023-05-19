using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaebSae : MonoBehaviour
{   

    private readonly string[] levelList = new string[] {
      "Egg",
      "OLv1",
      "OLv2",
      "OLv3",
      "OLv4",
      "OLv5",
      "OLv6",
      "OLv7",
      "OLv8",
      "OLv9",
      "OLv10",
      "OLv11",
      "OLv12",
      "FLv1",
      "FLv2",
      "FLv3",
      "FLv4",
      "FLv5",
      "FLv6",
      "FLv7",
      "FLv8",
      "FLv9",
      "FLv10",
      "FLv11",
      "FLv12",
      "CLv1",
      "CLv2",
      "CLv3",
      "CLv4",
      "CLv5",
      "CLv6",
      "CLv7",
      "CLv8",
      "CLv9",
      "CLv10",
      "CLv11",
      "CLv12",

    
     };

    public AudioSource audioSource;
    private void Start()
    {
         audioSource = GetComponent<AudioSource>();

    } 
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
    //등록되어있는 인덱스 
    private int index;//등록되어있는 타일 인덱스

    


   
  
    //레벨 1일때 알까기 
    public void hatch(){
      Ocean ocean = GameObject.FindGameObjectWithTag("Ocean").GetComponent<Ocean>();
      GameObject newEgg =Instantiate(Resources.Load("OLv1"), ocean.Oplaces[index].transform.position, ocean.Oplaces[index].transform.rotation) as GameObject;
      newEgg.transform.SetParent(ocean.Oplaces[index].transform,false);
      newEgg.GetComponent<BaebSae>().setIndex(index);
      ocean.Oplaces[index].setBaebsae("OLv1");
      
      Destroy(gameObject);

    }
  
  public int getIndex(){
      return index;
  }
  public void setIndex(int index){
  this.index = index;
  }
  public string getName(int level){
    return levelList[level];
  } 
  
    public int getLevel(){
      return level;
    }

 

    //입력 명령
    private void OnMouseDown()
   {
    touchRealeased =false;
    //offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x -transform.position.x;
    //offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y-transform.position.y;
    
    
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
      int thisBaebasaeLv =gameObject.GetComponent<BaebSae>().getLevel();//충돌한 뱁새 레벨 
      int collisionBaebsaeLv = collision.gameObject.GetComponent<BaebSae>().getLevel(); //충돌당한 뱁세 레벨
      int thisBaebasaeIndex = gameObject.GetComponent<BaebSae>().getIndex(); 
      int collisionBaebsaeIndex = collision.gameObject.GetComponent<BaebSae>().getIndex();
      
   


    if(touchRealeased && thisBaebasaeLv == collisionBaebsaeLv && thisBaebasaeLv < 36 && thisBaebasaeLv>0)
    {
      
      
      //1부터 4까지 
      if(thisBaebasaeLv <12){
      Ocean ocean = GameObject.FindGameObjectWithTag("Ocean").GetComponent<Ocean>();
      Transform respawnPoint =ocean.Oplaces[thisBaebasaeIndex].getPlace().transform;

      
      GameObject evolvedBaebabsae=Instantiate(Resources.Load(levelList[thisBaebasaeLv]),respawnPoint.position, Quaternion.identity) as GameObject;
      evolvedBaebabsae.transform.SetParent(ocean.Oplaces[thisBaebasaeIndex].getPlace().transform,false);
      //evolvedBaebabsae.transform.localScale =new Vector2(respawnPoint.localScale.x, respawnPoint.localScale.y);

      //Ocean.GetComponent<Ocean>().lostbirds(levelList[thisBaebasaeLv]);
      //충돌되서 없어진 자리 index추가 
      Destroy(collision.gameObject);
      Destroy(gameObject);
      audioSource.Play();
      evolvedBaebabsae.GetComponent<BaebSae>().setIndex(thisBaebasaeIndex);
      touchRealeased=false;
      

      ocean.EmptySet.Add(collisionBaebsaeIndex);
      ocean.Oplaces[thisBaebasaeIndex].setBaebsae(levelList[thisBaebasaeLv]);
      ocean.Oplaces[collisionBaebsaeIndex].setBaebsae("");

  
      }

      //실헐실 끝 생성시 다른 레벨로 넘어감 
      else if(thisBaebasaeLv ==12){
  
            
      
      Ocean ocean = GameObject.FindGameObjectWithTag("Ocean").GetComponent<Ocean>();
      Forest forest= GameObject.FindGameObjectWithTag("Forest").GetComponent<Forest>();
      forest.waitingbirds.Insert(0, new WaitingBirds(){baebSae = levelList[thisBaebasaeLv],index = forest.EmptySet[0]});
      forest.EmptySet.RemoveAt(0);
      

   

      Destroy(collision.gameObject);
      Destroy(gameObject);
      touchRealeased=false;
      audioSource.Play();

      ocean.EmptySet.Add(collisionBaebsaeIndex);
      ocean.EmptySet.Add(thisBaebasaeIndex);
      ocean.Oplaces[thisBaebasaeIndex].setBaebsae("");
      ocean.Oplaces[collisionBaebsaeIndex].setBaebsae("");




      
   
      }

      else if(thisBaebasaeLv >12 && thisBaebasaeLv<24){
      Forest forest = GameObject.FindGameObjectWithTag("Forest").GetComponent<Forest>();
      Transform respawnPoint =forest.Fplaces[thisBaebasaeIndex].getPlace().transform;

      
      GameObject evolvedBaebabsae=Instantiate(Resources.Load(levelList[thisBaebasaeLv]),respawnPoint.position, Quaternion.identity) as GameObject;
      evolvedBaebabsae.transform.SetParent(forest.Fplaces[thisBaebasaeIndex].getPlace().transform,false);
      //evolvedBaebabsae.transform.localScale =new Vector2(respawnPoint.localScale.x, respawnPoint.localScale.y);

      //Ocean.GetComponent<Ocean>().lostbirds(levelList[thisBaebasaeLv]);
      //충돌되서 없어진 자리 index추가 
      Destroy(collision.gameObject);
      Destroy(gameObject);
      evolvedBaebabsae.GetComponent<BaebSae>().setIndex(thisBaebasaeIndex);
      touchRealeased=false;
      

      forest.EmptySet.Add(collisionBaebsaeIndex);
      forest.Fplaces[thisBaebasaeIndex].setBaebsae(levelList[thisBaebasaeLv]);
      forest.Fplaces[collisionBaebsaeIndex].setBaebsae("Empty");
   
      }


      //Debug.Log(evolvedBaebabsae.transform.position.x);
      //Debug.Log("진화 완료");
    }
   }




}
