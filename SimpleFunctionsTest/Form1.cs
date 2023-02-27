using SimpleFunctionsTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using vdControls;
using VectorDraw.Geometry;
using VectorDraw.Professional.Constants;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Actions;

namespace SimpleFunctionsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string DocPath;

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string fname;

            object ret = vdsc.BaseControl.ActiveDocument.GetOpenFileNameDlg(0, "", 0);
            if (ret == null) return;

            DocPath = ret as string;
            fname = (string)ret;

            bool success = vdsc.BaseControl.ActiveDocument.Open(fname);
            if (!success) return;

        }

        

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            
        }

        private void btnDrawRect_Click(object sender, EventArgs e)
        {
            DrawDest();
        }


        public void DrawDest()
        {
            gPoint userPoint;

            vdsc.BaseControl.ActiveDocument.Prompt("Select a Point");

            StatusCode ret =
            vdsc.BaseControl.ActiveDocument.ActionUtility.getUserPoint(out userPoint);
            vdsc.BaseControl.ActiveDocument.Prompt(null);


            vdsc.BaseControl.ActiveDocument.Prompt("Other corner:");

            object ret2 =
            vdsc.BaseControl.ActiveDocument.ActionUtility.getUserRect(userPoint);
            vdsc.BaseControl.ActiveDocument.Prompt(null);

            Vector v = ret2 as Vector;
            if (v != null)
            {
                double angle = v.x;
                double width = v.y;
                double height = v.z;

                gPoint userpoint2 = userPoint.Polar(0.0, width);

                userpoint2 = userpoint2.Polar(VectorDraw.Geometry.Globals.HALF_PI, height);
                vdsc.BaseControl.ActiveDocument.CommandAction.CmdRect(userPoint, userpoint2);
            }
        }

        public void LineToPLine()
        {
            foreach(vdFigure f in vdsc.BaseControl.ActiveDocument.Model.Entities)
            {
                if (f != null)
                {
                    try
                    {
                        if (f is vdLine)
                        {
                            vdLine vl = (vdLine)f;
                            vdLine vl2 = (vdLine)f;

                            vdPolyline vpl = new vdPolyline();

                            vpl.setDocumentDefaults();
                            gPoint startPoint1 = new gPoint(vl.StartPoint.x,vl.StartPoint.y+5);
                            Console.WriteLine(startPoint1.Point2d);
                            gPoint startPoint2 = new gPoint(vl.StartPoint.x, vl.StartPoint.y-5);
                            Console.WriteLine(startPoint2.Point2d);
                            gPoint endPoint1 = new gPoint(vl.EndPoint.x, vl.EndPoint.y+5);
                            Console.WriteLine(endPoint1.Point2d);
                            gPoint endPoint2 = new gPoint(vl.EndPoint.x, vl.EndPoint.y - 5);
                            Console.WriteLine(endPoint2.Point2d);

                            gPoints vplPoints = new gPoints();
                            vplPoints.Add(startPoint1);
                            vplPoints.Add(startPoint2);
                            vplPoints.Add(endPoint1);
                            vplPoints.Add(endPoint2);



                            vpl.VertexList.Add(startPoint1);
                            vpl.VertexList.Add(startPoint2);
                            vpl.VertexList.Add(endPoint2);
                            vpl.VertexList.Add(endPoint1);
                            vpl.Flag = VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE;
                            

                            vdPolyline vpl2 = new vdPolyline();
                            vpl2 = vl.AsPolyline();
                            vdPolyline poly = (vdPolyline)vpl2;
                            vpl2.GetBoundaryPolyFromPoint(startPoint1,10);
                            

                            vl2.StartPoint = startPoint1;
                            vl2.EndPoint = endPoint1;
                            vl2.PenColor.ColorIndex = 3;

                            //vdsc.BaseControl.ActiveDocument.ActiveLayOut.Entities.RemoveItem(vl);
                            vdsc.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(vpl);
                            vdsc.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(vpl2);


                            //이건 이미 있다고?
                            //vdsc.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(vl2);
                            vdsc.BaseControl.ActiveDocument.Redraw(true);

                            vpl2 = new VectorDraw.Professional.vdFigures.vdPolyline();

                            vpl2.BoundingBox.AddPoints(vplPoints);
                            Console.WriteLine(vpl2.Area());

                            vpl = new VectorDraw.Professional.vdFigures.vdPolyline();
                        }
                    }

                    finally
                    {

                    }
                }
                return;
            }
        }

        public void LineToPLine2()
        {
            foreach (vdFigure f in vdsc.BaseControl.ActiveDocument.Model.Entities)
            {
                if (f != null)
                {
                    try
                    {
                        if(f is vdLine)
                        {
                            vdLine vl = (vdLine)f;
                            gPoint endPoint1 = new gPoint(vl.EndPoint.x, vl.EndPoint.y + 1);
                            gPoint startPoint1 = new gPoint(vl.StartPoint.x, vl.StartPoint.y + 1);

                            vdPolyline vpl = new vdPolyline();
                            vpl.SetUnRegisterDocument(vdsc.BaseControl.ActiveDocument);
                            vpl.setDocumentDefaults();

                            vpl.VertexList.Add(vl.StartPoint);
                            vpl.VertexList.Add(vl.EndPoint);
                            vpl.VertexList.Add(endPoint1);
                            vpl.VertexList.Add(startPoint1);

                            Console.WriteLine(vpl.VertexList);
                            vpl.Flag = VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE;

                            Console.WriteLine(vpl.Area());

                            vdsc.BaseControl.ActiveDocument.Model.Entities.AddItem(vpl);
                            vdsc.BaseControl.ActiveDocument.Redraw(true);
                        }
                    }

                    catch
                    {

                    }
                }
            }
        }
        private void btnLineFilter_Click(object sender, EventArgs e)
        {
            LineToPLine2();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
