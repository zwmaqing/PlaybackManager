using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace CircularProgressControl
{
    public partial class Form1 : Office2007Form
    {
        public Form1()
        {
            InitializeComponent();

         

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleEndlessProgress_Executed(object sender, EventArgs e)
        {
           // bool running = !ToggleEndlessProgress.Checked;
            //ToggleEndlessProgress.Checked = !ToggleEndlessProgress.Checked;
            //if (running)
                //ToggleEndlessProgress.Text = "Stop";
            //else
                //ToggleEndlessProgress.Text = "Start";

        }


        private void ProgressValueVisible_Executed(object sender, EventArgs e)
        {

        }

        private void AppCommandTheme_Executed(object sender, EventArgs e)
        {
            ICommandSource source = sender as ICommandSource;
            if (source.CommandParameter is string)
            {
                eStyle style = (eStyle)Enum.Parse(typeof(eStyle), source.CommandParameter.ToString());
                // Using StyleManager change the style and color tinting
                if (StyleManager.IsMetro(style))
                {
                    // More customization is needed for Metro
                    // Capitalize App Button and tab

                        // Ribbon Control may contain items other than tabs so that needs to be taken in account

                    ribbonControl1.RibbonStripFont = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    if (style == eStyle.Metro)
                        StyleManager.MetroColorGeneratorParameters = DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters.DarkBlue;

                    // Adjust size of switch button to match Metro styling

                    // Adjust tab strip style


                    StyleManager.Style = style; // BOOM
                }
                else
                {
                    // If previous style was Metro we need to update other properties as well
                    StyleManager.ChangeStyle(style, Color.Empty);
                }
            }
            else if (source.CommandParameter is Color)
            {
                if (StyleManager.IsMetro(StyleManager.Style))
                    StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(Color.White, (Color)source.CommandParameter);
                else
                    StyleManager.ColorTint = (Color)source.CommandParameter;
            }
        }

        #region Automatic Color Scheme creation based on the selected color table

        private bool m_ColorSelected = false;

        private eStyle m_BaseStyle = eStyle.Office2010Silver;

        private void buttonStyleCustom_ExpandChange(object sender, System.EventArgs e)
        {
            if (buttonStyleCustom.Expanded)
            {
                // Remember the starting color scheme to apply if no color is selected during live-preview
                m_ColorSelected = false;
                m_BaseStyle = StyleManager.Style;
            }
            else
            {
                if (!m_ColorSelected)
                {
                    if (StyleManager.IsMetro(StyleManager.Style))
                        StyleManager.MetroColorGeneratorParameters = DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters.Default;
                    else
                        StyleManager.ChangeStyle(m_BaseStyle, Color.Empty);
                }
            }
        }

        private void buttonStyleCustom_ColorPreview(object sender, DevComponents.DotNetBar.ColorPreviewEventArgs e)
        {
            if (StyleManager.IsMetro(StyleManager.Style))
            {
                Color baseColor = e.Color;
                StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(Color.White, baseColor);
            }
            else
                StyleManager.ColorTint = e.Color;
        }

        private void buttonStyleCustom_SelectedColorChanged(object sender, System.EventArgs e)
        {
            m_ColorSelected = true; // Indicate that color was selected for buttonStyleCustom_ExpandChange method
            buttonStyleCustom.CommandParameter = buttonStyleCustom.SelectedColor;
        }




        #endregion

        private void btn_help_Click(object sender, EventArgs e)
        {

        }
    }
}