using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputObject 
{
    void Use(int playerViewID, Vector3 inputVector);
    //void Down(Vector3 inputVector3);
    //void Drag(Vector3 inputVector3);
    //void Up(Vector3 inputVector3);

    //InputHandler onUseByRPCEvent { get; set; }

}
