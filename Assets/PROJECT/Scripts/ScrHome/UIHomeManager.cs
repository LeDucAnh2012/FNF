using FridayNightFunkin.Data;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FridayNightFunkin.UI.HomeUI
{
    public class UIHomeManager : MonoBehaviour
    {
        public static UIHomeManager instance;
        [TabGroup("1", "Panel")] [SerializeField] private PanelHomeUI panelHomeUI;
        [TabGroup("1", "Panel")] [SerializeField] private PanelSetting panelSetting;

        [TabGroup("1", "Popup")] [SerializeField] private PopupListSong popupListSong;
        [TabGroup("1", "Popup")] [SerializeField] private PopupNotEnough popupNotEnough;

        private float deltaTime;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            ShowPanelHomeUI();
            DataSong.SetTypePlay(-1);
        }
        private void Start()
        {
            if (VariableSystem.ShowListSong)
                ShowListSong();
        }
        public void ShowListSong()
        {
            VariableSystem.ShowListSong = false;
            popupListSong.Show();
        }
        public void ShowPanelHomeUI()
        {
            panelHomeUI.ShowPanel();
        }
        public void ShowPopupNotEnough(string _str = null)
        {
            if (_str == null)
                popupNotEnough.ShowPopup("Not Enough Coin");
            else
                popupNotEnough.ShowPopup(_str);
        }
        public void ShowPanelSetting()
        {
            panelSetting.Show();
        }
        public void Test(bool istest) { }
        // [Button("RUN")]
        //private void LoadText()
        //{
        //Dictionary<string, Dictionary<string, string>> _GetDiction;
        //    string _strKey = ((int)indexSong + 1).ToString();
        //    string _strName = nameSong.ToString();

        //    _GetDiction = new Dictionary<string, Dictionary<string, string>>();

        //    string _nameFile = nameSong.ToString();
        //    TextAsset xml = Resources.Load<TextAsset>(_nameFile);
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.Load(new StringReader(xml.text));
        //    XmlNodeList xmlNodeList = xmlDoc.DocumentElement.ChildNodes;

        //    XmlAttribute xmlAttribute = xmlDoc.DocumentElement.GetAttributeNode(_strKey);
        //    XmlElement xmlElement = xmlDoc.DocumentElement;

        //    Debug.Log("str key: " + _strKey);
        //    Debug.Log("xmlElement.GetAttribute: " + xmlElement.GetAttribute(_strKey));


        //    Dictionary<string, string> _GetText = new Dictionary<string, string>(); ;
        //    foreach (XmlNode node in xmlNodeList)
        //        _GetText.Add(node.Name, node.InnerText);

        //    _GetDiction.Add(xmlElement.GetAttribute(_strKey), _GetText);

        //    string result = "Not Found";

        //    if (_GetDiction.ContainsKey(_strKey))
        //    {
        //        Dictionary<string, string> _tmp = _GetDiction[_strKey];
        //        if (_tmp.ContainsKey(_strName))
        //            result = _tmp[_strName];
        //    }
        //    txtInfor.text = result;
        //}
    }

}