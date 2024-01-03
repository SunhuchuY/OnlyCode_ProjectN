using UnityEngine;

public class FriendToSearch : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Monster":
                if (collision != null)
                {
                    if(transform.parent.GetComponent<Friend>().searchObj  != null)
                        Debug.Log("Not null");


                    transform.parent.GetComponent<Friend>().searchObj = collision.GetComponent<Monster>();
                }   
                break;
        }
    }
}
