// ***********************************************************************
// Assembly         : WD.XLC.Domain
// Author           : shahid_k
// Created          : 04-27-2017
//
// Last Modified By : shahid_k
// Last Modified On : 05-02-2017
// ***********************************************************************
// <copyright file="Resizer.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WD.DataAccess.Helpers;



// namespace: WD.XLC.Domain.Helpers
//
// summary:	.


namespace WD.XLC.Domain.Helpers
{
    // This class is used to dynamically resize and reposition all controls on a form. Container
    // controls are processed recursively so that all controls on the form are handled.
    // 
    // Usage:
    //  Resizing functionality requires only three lines of code on a form:
    // 
    //  1. Create a form-level reference to the Resize class:
    //     Dim myResizer as Resizer
    // 
    //  2. In the Form_Load event, call the  Resizer class FIndAllControls method:
    //     myResizer.FindAllControls(Me)
    // 
    //  3. In the Form_Resize event, call the  Resizer class ResizeAllControls method:
    //     myResizer.ResizeAllControls(Me) 
    //-------------------------------------------------------------------------------
    public class Resizer
    {

        //----------------------------------------------------------
        // ControlInfo Structure of original state of all processed controls. 
        //----------------------------------------------------------
        private class ControlInfo
        {
            public string name;
            public string parentName;
            public double leftOffsetPercent;
            public double topOffsetPercent;
            public double heightPercent;
            public int originalHeight;
            public int originalWidth;
            public double widthPercent;
            public float originalFontSize;
        }

        //-------------------------------------------------------------------------
        // ctrlDict Dictionary of (control name, control info) for all processed controls. 
        //-------------------------------------------------------------------------

        private Dictionary<string, ControlInfo> ctrlDict = new Dictionary<string, ControlInfo>();
        //----------------------------------------------------------------------------------------
        // FindAllControls Recursive function to process all controls contained in the initially passed
        // control container and store it in the Control dictionary. 
        //----------------------------------------------------------------------------------------

        public void FindAllControls(Control thisCtrl)
        {
            // -- If the current control has a parent, store all original relative position
            // -- and size information in the dictionary.
            // -- Recursively call FindAllControls for each control contained in the
            // -- current Control. 
            foreach (Control ctl in thisCtrl.Controls)
            {
                try
                {
                    if ((ctl.Parent != null))
                    {
                        int parentHeight = ctl.Parent.Height;
                        int parentWidth = ctl.Parent.Width;

                        ControlInfo c = new ControlInfo();
                        c.name = ctl.Name;
                        c.parentName = ctl.Parent.Name;
                        c.topOffsetPercent = (HelperUtility.ConvertTo<double>(ctl.Top, 1) / HelperUtility.ConvertTo<double>(parentHeight, 0.00));
                        c.leftOffsetPercent = HelperUtility.ConvertTo<double>(ctl.Left, 0.00) / HelperUtility.ConvertTo<double>(parentWidth, 0.00);
                        c.heightPercent = HelperUtility.ConvertTo<double>(ctl.Height, 0.00) / HelperUtility.ConvertTo<double>(parentHeight, 0.00);
                        c.widthPercent = HelperUtility.ConvertTo<double>(ctl.Width, 0.00) / HelperUtility.ConvertTo<double>(parentWidth, 0.00);
                        c.originalFontSize = ctl.Font.Size;
                        c.originalHeight = ctl.Height;
                        c.originalWidth = ctl.Width;
                        ctrlDict.Add(c.name, c);
                    }

                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message);
                }

                if (ctl.Controls.Count > 0)
                {
                    FindAllControls(ctl);
                }

            }
            //-- For Each

        }

        //----------------------------------------------------------------------------------------
        // ResizeAllControls Recursive function to resize and reposition all controls contained in the
        // Control dictionary. 
        //----------------------------------------------------------------------------------------

        public void ResizeAllControls(Control thisCtrl)
        {
            float fontRatioW = 0;
            float fontRatioH = 0;
            float fontRatio = 0;
            Font f = default(Font);

            //-- Resize and reposition all controls in the passed control
            foreach (Control ctl in thisCtrl.Controls)
            {
                try
                {
                    if ((ctl.Parent != null))
                    {
                        int parentHeight = ctl.Parent.Height;
                        int parentWidth = ctl.Parent.Width;

                        ControlInfo c = ctrlDict.Where(x => x.Key == ctl.Name).FirstOrDefault().Value;


                        try
                        {
                            //-- Get the current control's info from the control info dictionary
                            bool ret = c == null ? false : true;
                            //-- If found, adjust the current control based on control relative
                            //-- size and position information stored in the dictionary
                            if ((ret))
                            {
                                //-- Size
                                ctl.Width = Convert.ToInt32(parentWidth * c.widthPercent);
                                ctl.Height = Convert.ToInt32(parentHeight * c.heightPercent);

                                //-- Position
                                ctl.Top = Convert.ToInt32(parentHeight * c.topOffsetPercent);
                                ctl.Left = Convert.ToInt32(parentWidth * c.leftOffsetPercent);

                                //-- Font
                                f = ctl.Font;
                                fontRatioW = ctl.Width / c.originalWidth;
                                fontRatioH = ctl.Height / c.originalHeight;
                                fontRatio = (fontRatioW + fontRatioH) / 2;
                                //-- average change in control Height and Width
                                ctl.Font = new Font(f.FontFamily, c.originalFontSize * fontRatio, f.Style);

                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                //-- Recursive call for controls contained in the current control
                if (ctl.Controls.Count > 0)
                {
                    ResizeAllControls(ctl);
                }

            }
            //-- For Each
        }
    }
}
