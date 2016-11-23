using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LinearSolver;

namespace LinearSolverUI
{
    /*
     * The Object of this Class contains a Canvas with a specified width and height in order to 
     * contain several EquationsContainer to show these equations in a list.
     */ 
    class EquationContainerList
    {
        private EquationContainer[] equCtn;
        private ObjFunctionContainer objFunCtn;
        private Canvas canvas;
        private Point position;
        //private EquationContainer selectedEquCtn;
        private int width;
        private int height;
        Label objMissingNoticeLb = new Label();


        public EquationContainerList(ScrollViewer parent,Point position)
        {
            this.position = position;
            canvas = new Canvas();
            canvas.Width = 300;
            parent.Content = canvas;
            //parent.Children.Add(canvas);
            canvas.HorizontalAlignment = HorizontalAlignment.Left;
            canvas.VerticalAlignment = VerticalAlignment.Top;
            //canvas.Margin = new Thickness(position.X,position.Y,0,0);
            equCtn = new EquationContainer[0];

            
            objMissingNoticeLb.Content = "( No Objective Function )";
            objMissingNoticeLb.Foreground = Brushes.LightGray;
            canvas.Children.Add(objMissingNoticeLb);
            Canvas.SetTop(objMissingNoticeLb, 0);

            canvas.Height = 30;
        }

        /* newEquCtn: Create a new EquationContainer Object by an input Equation.
         * Input: 
         *      Equation equ: The Equation Object that will be used to create the EquationContainer.
         */
        public void newEquCtn(Equation equ)
        {
            equCtn = util.insert(equCtn,new EquationContainer(equ,0,0,canvas));
            canvas.Height += equCtn[0].getContainer().Height;
            redraw();
        }

        /* removeEquCtn: remove a specified EquationContainer from the list and redraw the window.
         * Input: 
         *      Canvas srcCanvas: the give Canvas Object which belongs to a EquationContainer.
         */
        public Equation removeEquCtn(Canvas srcCanvas)
        {
            EquationContainer srcContainer = findCtnByCanvas(srcCanvas);
            equCtn = util.removeFrom<EquationContainer>(equCtn,srcContainer);
            canvas.Children.Remove(srcContainer.getContainer());
            canvas.Height -= srcCanvas.Height;
            redraw();
            return srcContainer.getEqu();
        }

        /* setObjFunc: add the objective function to the list.
         * Input: 
         *      Equation objFunc: the Equation Object refer to the objective function.
         */
        public void setObjFunc(Equation objFunc)
        {
            if (objFunCtn == null)
            {
                objFunCtn = new ObjFunctionContainer(objFunc, canvas);
                canvas.Children.Remove(objMissingNoticeLb);
                redraw();
            }
        }

        /* 
         * redraw: Re-manage the windoe and show each EquationContainer in a proper loaction.
         */ 
        private void redraw()
        {
            if (objFunCtn != null)
            {
                Canvas.SetTop(objFunCtn.getContainer(), 0);
            }
            for (int i = 0; i < equCtn.Length; i++)
            {
                Canvas.SetTop(equCtn[i].getContainer(),equCtn[i].getContainer().Height * (i+1));
            }
        }


        /* findCtnByCanvas: use a canvas as an identifier to find a specified EquationContainer Object.
         * Input:
         *      Canvas canvas: The canvas of the wanted EquationContainer.
         * Return: If such a EquationContainer is found, it will be directly returned or, if not found or 
         * some of the parameters are null, return null.
         */ 
        private EquationContainer findCtnByCanvas(Canvas canvas)
        {
            if (equCtn == null || canvas == null)
            {
                return null;
            }
            for (int i = 0; i < equCtn.Length; i++)
            {
                if (equCtn[i].getContainer().Equals(canvas))
                {
                    return equCtn[i];
                }
            }
            return null;
        }

        /*
         * reset: reset all.
         */ 
        public void reset()
        {
            this.equCtn = new EquationContainer[0];
            this.canvas.Children.Clear();
            this.objFunCtn = null;
            this.canvas.Height = 30;
            this.canvas.Children.Add(objMissingNoticeLb);
        }

        /*
         * Getters & Setters
         */
        public void setHeight(int height)
        {
            this.height = height;
        }

        public int getHeight()
        {
            return this.height;
        }

        public void setWidth(int width)
        {
            this.width = width;
        }

        public int getWidth()
        {
            return this.width;
        }

        public Canvas getCanvas()
        {
            return this.canvas;
        }

        public Point getPosition()
        {
            return this.position;
        }

        public double getX()
        {
            return this.position.X;
        }

        public double getY()
        {
            return this.position.Y;
        }

        public void setPosition(Point position)
        {
            this.position = position;
        }

        public EquationContainer[] getEquCtn()
        {
            return equCtn;
        }

        public ObjFunctionContainer getObjFunCtn()
        {
            return this.objFunCtn;
        }
    }
}
