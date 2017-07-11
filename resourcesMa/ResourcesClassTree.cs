using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CircularProgressControl.resourcesMa
{
    public partial class ResourcesClassTree : UserControl
    {
        public ResourcesClassTree()
        {
            InitializeComponent();
        }


        /*
        private void loadUnitsToManualCheckTree(string schoolName)
        {
            List<KQZUnits> unitsInfo = attendBaseData.GetAllUnits();
            if (advTree_ManualCheck == null || unitsInfo == null)
            {
                return;
            }
            advTree_ManualCheck.Nodes[0].Nodes.Clear();
            advTree_ManualCheck.Nodes[0].Text = schoolName;
            createUnitsNode(advTree_ManualCheck.Nodes[0], 0, unitsInfo, true);
            // advTree_ManualCheck.Nodes[0].ExpandAll();
        }

        private void createUnitsNode(DevComponents.AdvTree.Node node, int id, List<KQZUnits> unitsInfo, bool isImage = true, bool checkBoxVisible = false)
        {
            List<KQZUnits> units = unitsInfo.FindAll(x => x.ParentID == id);
            for (int i = 0; i < units.Count; i++)
            {
                DevComponents.AdvTree.Node childNode = new DevComponents.AdvTree.Node();
                childNode.Tag = units[i];
                childNode.Text = units[i].UnitName;
                childNode.Name = "Unit" + units[i].UnitID.ToString();
                if (isImage)
                { childNode.ImageIndex = 11; }
                childNode.CheckBoxVisible = checkBoxVisible;

                createUnitsNode(childNode, units[i].UnitID, unitsInfo, isImage, checkBoxVisible);

                node.Nodes.Add(childNode);
            }
        }
        */






    }
}
