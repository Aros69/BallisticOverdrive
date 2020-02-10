using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AuthorityColor : NetworkBehaviour{

    public override void OnStartAuthority(){
        base.OnStartAuthority();
        transform.localScale = new Vector3(2f, 2f, 2f);
    }

}
