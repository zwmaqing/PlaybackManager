using System.Xml;
using System.Data;


namespace Helper.Xml
{
    /// <summary>
    /// Xml的操作公共类
    /// </summary>    
    public class XmlHelper
    {
        #region 字段定义
        /// <summary>
        /// XML文件的物理路径
        /// </summary>
        private string _filePath = string.Empty;
        /// <summary>
        /// Xml文档
        /// </summary>
        private XmlDocument _xml;
        /// <summary>
        /// XML的根节点
        /// </summary>
        private XmlElement _element;
        #endregion

        #region 构造方法
        /// <summary>
        /// 实例化XmlHelper对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        public XmlHelper(string xmlFilePath)
        {
            //获取XML文件的绝对路径
            _filePath = (xmlFilePath);
        }
        #endregion

        #region 创建XML的根节点
        /// <summary>
        /// 创建XML的根节点
        /// </summary>
        private void CreateXMLElement()
        {

            //创建一个XML对象
            _xml = new XmlDocument();

            if (System.IO.File.Exists(_filePath))
            {
                //加载XML文件
                _xml.Load(this._filePath);
            }

            //为XML的根节点赋值
            _element = _xml.DocumentElement;
        }
        #endregion

        #region 获取指定XPath表达式的节点对象
        /// <summary>
        /// 获取指定XPath表达式的节点对象
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public XmlNode GetNode(string xPath)
        {
            //创建XML的根节点
            CreateXMLElement();

            //返回XPath节点
            return _element.SelectSingleNode(xPath);
        }
        #endregion

        #region 获取指定XPath表达式节点的值
        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public string GetValue(string xPath)
        {
            //创建XML的根节点
            CreateXMLElement();

            //返回XPath节点的值
            return _element.SelectSingleNode(xPath).InnerText;
        }
        #endregion

        #region 获取指定XPath表达式节点的属性值
        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public string GetAttributeValue(string xPath, string attributeName)
        {
            //创建XML的根节点
            CreateXMLElement();

            //返回XPath节点的属性值
            return _element.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }
        #endregion

        #region 新增节点
        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点插入到当前Xml文件中。
        /// </summary>        
        /// <param name="xmlNode">要插入的Xml节点</param>
        public void AppendNode(XmlNode xmlNode)
        {
            //创建XML的根节点
            CreateXMLElement();

            //导入节点
            XmlNode node = _xml.ImportNode(xmlNode, true);

            //将节点插入到根节点下
            _element.AppendChild(node);
        }

        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将DataSet中的第一条记录插入Xml文件中。
        /// </summary>        
        /// <param name="ds">DataSet的实例，该DataSet中应该只有一条记录</param>
        public void AppendNode(DataSet ds)
        {
            //创建XmlDataDocument对象
            XmlDataDocument xmlDataDocument = new XmlDataDocument(ds);

            //导入节点
            XmlNode node = xmlDataDocument.DocumentElement.FirstChild;

            //将节点插入到根节点下
            AppendNode(node);
        }
        #endregion

        #region 删除节点
        /// <summary>
        /// 删除指定XPath表达式的节点
        /// </summary>        
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public void RemoveNode(string xPath)
        {
            //创建XML的根节点
            CreateXMLElement();

            //获取要删除的节点
            XmlNode node = _xml.SelectSingleNode(xPath);

            //删除节点
            _element.RemoveChild(node);
        }
        #endregion //删除节点

        #region 保存XML文件
        /// <summary>
        /// 保存XML文件
        /// </summary>        
        public void Save()
        {
            //创建XML的根节点
            CreateXMLElement();

            //保存XML文件
            _xml.Save(this._filePath);
        }
        #endregion //保存XML文件

        #region 静态方法

        #region 创建根节点对象
        /// <summary>
        /// 创建根节点对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>        
        private static XmlElement CreateRootElement(string xmlFilePath)
        {
            //定义变量，表示XML文件的绝对路径
            string filePath = "";

            //获取XML文件的绝对路径
            filePath = (xmlFilePath);

            //创建XmlDocument对象
            XmlDocument xmlDocument = new XmlDocument();
            //加载XML文件
            xmlDocument.Load(filePath);

            //返回根节点
            return xmlDocument.DocumentElement;
        }
        #endregion

        #region 指定XPath表达式节点的值 查增删改 操作

        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public static string GetValue(string xmlFilePath, string xPath)
        {
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            //返回XPath节点的值
            XmlNode nodeOne = rootElement.SelectSingleNode(xPath);
            if (nodeOne != null)
            {
                return nodeOne.Value;
            }
            else return "";

        }

        /// <summary>
        /// Gets the node value.
        /// 获取节点 Value 属性值
        /// </summary>
        /// <param name="xmlFilePath">The XML file path.</param>
        /// <param name="xmlPath">The XML path.</param>
        /// <param name="childNodeName">Name of the child node.</param>
        /// <returns>System.String.</returns>
        /// <remarks>SAP_SchoolSystem</remarks>
        public static string GetNodeValue(string xmlFilePath, string xmlPath, string childNodeName)
        {
            XmlDataDocument xmlDoc = new System.Xml.XmlDataDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNode node = xmlDoc.SelectSingleNode(xmlPath);
            XmlNodeList xnl = xmlDoc.SelectSingleNode(xmlPath).ChildNodes; 
            string Str="";
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.Name == childNodeName)
                {
                    Str = xe.GetAttribute("value"); ;//该节点的
                }
            }  
            return Str;
        }

        /// <summary>
        /// Adds the node value.
        /// 添加节点 及Value 属性值
        /// </summary>
        /// <param name="xmlFilePath">The XML file path.</param>
        /// <param name="xmlPath">The XML path.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="value">The value.</param>
        /// <remarks>SAP_SchoolSystem</remarks>
        public static void AddNodeValue(string xmlFilePath, string xmlPath, string nodeName, string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNode root = xmlDoc.SelectSingleNode(xmlPath);//查找
            XmlElement xe1 = xmlDoc.CreateElement(nodeName);//创建一个<>节点
            xe1.SetAttribute("key", nodeName);//设置该节点key属性
            xe1.SetAttribute("value", value);//设置该节点ISBN属性
            root.AppendChild(xe1);
            xmlDoc.Save(xmlFilePath);
        }

        /// <summary>
        /// Edits the node value.
        /// 编辑节点 Value 属性值
        /// </summary>
        /// <param name="xmlFilePath">The XML file path.</param>
        /// <param name="xmlPath">The XML path.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="value">The value.</param>
        /// <remarks>SAP_SchoolSystem</remarks>
        public static void EditNodeValue(string xmlFilePath, string xmlPath, string nodeName, string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNodeList xnl = xmlDoc.SelectSingleNode(xmlPath).ChildNodes;

            foreach (XmlNode xn1 in xnl)//遍历
            {
                XmlElement xe2 = (XmlElement)xn1;//转换类型
                if (xe2.Name == nodeName)//如果找到
                {
                    xe2.SetAttribute("value", value);//设置该节点value属性
                    break;//找到退出来就可以了
                }
            }
            xmlDoc.Save(xmlFilePath); 
        }

        /// <summary>
        /// Delete the node.
        /// 删除节点及属性值
        /// </summary>
        /// <param name="xmlFilePath">The XML file path.</param>
        /// <param name="xmlPath">The XML path.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <remarks>SAP_SchoolSystem</remarks>
        public static void DelNode(string xmlFilePath, string xmlPath, string nodeName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNodeList xnl = xmlDoc.SelectSingleNode(xmlPath).ChildNodes;

            foreach (XmlNode xn1 in xnl)//遍历
            {
                XmlElement xe2 = (XmlElement)xn1;//转换类型
                if (xe2.Name == nodeName)//如果找到
                {
                    xe2.ParentNode.RemoveChild(xe2);//删除该节点的全部内容
                    break;//找到退出来就可以了
                }
            }
            xmlDoc.Save(xmlFilePath); 
        }

        #endregion

        #region 获取指定XPath表达式节点的属性值
        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的相对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public static string GetAttributeValue(string xmlFilePath, string xPath, string attributeName)
        {
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            //返回XPath节点的属性值
            return rootElement.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }
        #endregion

        #endregion

        public static void SetValue(string xmlFilePath, string xPath, string newtext)
        {
            //string path = SysHelper.GetPath(xmlFilePath);
            //var queryXML = from xmlLog in xelem.Descendants("msg_log")
            //               //所有名字为Bin的记录
            //               where xmlLog.Element("user").Value == "Bin"
            //               select xmlLog;

            //foreach (XElement el in queryXML)
            //{
            //    el.Element("user").Value = "LiuBin";//开始修改
            //}
            //xelem.Save(path);
        }
    }
}
