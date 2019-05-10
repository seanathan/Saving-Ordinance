using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

public class narrationReader : MonoBehaviour {
    public float displayTime = 4f;
    public  float dispcountdown ;
    public string outMessage = "";
    public string dialogueFile;
    public XmlReader dialogueRead;

    // Update is called once per frame
    void Update () {
        dialogueRead = XmlReader.Create(dialogueFile);

        if (dispcountdown <= 0f)
        {
            SimpleStreamAxis();
            outMessage = dialogueRead.GetAttribute("message");
        }
        else
        {
            DialogueBox.PrintToDBox(outMessage, gameObject);
            dispcountdown -= Time.deltaTime;

        }
    }

    IEnumerable SimpleStreamAxis()
    {

        dialogueRead.MoveToContent();
        while (dialogueRead.Read())
        {
            if (dialogueRead.NodeType == XmlNodeType.Element)
            {
                if (dialogueRead.Name == "message")
                {
                    XElement el = XNode.ReadFrom(dialogueRead) as XElement;
                    if (el != null)
                    {
                    //    outMessage = el.ToString();

                        dispcountdown = displayTime;
                        yield return el;
                    }
                }
            }
        }
    }


    public void Reading()
    {
        //XmlReader dialogueRead = XmlReader.Create(Application.persistentDataPath + "/outputxmltest.xml");


        while (dialogueRead.Read())
        {
     
            if (dialogueRead.NodeType == XmlNodeType.Element && dialogueRead.Name == "Dialogue")
                if (dialogueRead.HasAttributes)
                {
                    outMessage = dialogueRead.GetAttribute("message");
                    
                    dispcountdown = displayTime;
                    return;

                }
            //dispcountdown = dialogueRead.GetAttribute("displayTime");


        }
    }
}
