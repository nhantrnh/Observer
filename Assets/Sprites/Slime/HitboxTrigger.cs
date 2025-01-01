using UnityEngine;

public class HitboxTrigger : MonoBehaviour
{
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collider){

        PlayerTouchGround feetHitBox = collider.GetComponent<PlayerTouchGround>();
        if (feetHitBox != null) {
            Debug.Log("Hit");
            Transform feetTransform = feetHitBox.gameObject.GetComponent<Transform>();
            GameObject mainObj = feetTransform.parent.gameObject;

            if (mainObj != null) {
                mainObj.GetComponent<MainController>().SetIsGrounded(true);
                GameObject parent = gameObject.transform.parent.gameObject;

                if (!parent.GetComponent<SlimeControl>().GetIsSpinning()){
                    mainObj.GetComponent<MainController>().ChangeHealthPoint(-1);
                }
                else {
                    Destroy(parent);
                }
            }
        }
    }
}
