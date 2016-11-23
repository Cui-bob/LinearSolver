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
    class ObjFunctionContainer : EquationContainer 
    {
        public ObjFunctionContainer(Equation equ, Canvas parent)
        {
            // Incoming parameters and initialize Objects
            this.equ = equ;
            int numOfVar = equ.getVariable().Length;
            this.position = new Point(0, 0);
            variableLb = new Label[numOfVar];
            varIndexLb = new Label[numOfVar];
            coeffecientLb = new Label[numOfVar];
            operatorLb = new Label[numOfVar];
            rightHandSideLB = new Label();
            equalityLb = new Label();
            container = new Canvas();


            // Set the size of the default container & add it to the parent content(now canvas)
            container.Width = 175;
            container.Height = labelHeight;
            parent.Children.Add(container);
            Canvas.SetLeft(container, 0);
            container.MouseMove += new MouseEventHandler(this.container_MouseOverHandler);
            container.MouseLeave += new MouseEventHandler(this.container_MouseLeaveHandler);


            updateEqu();
        }


        /*
        * updateEqu: Update the container with the associated Equation.
        */
        public new void updateEqu()
        {
            int numOfVar = equ.getVariable().Length;
            variableLb = new Label[numOfVar];
            varIndexLb = new Label[numOfVar];
            coeffecientLb = new Label[numOfVar];
            operatorLb = new Label[numOfVar];
            currentToLeft = 10;
            container.Children.Clear();

            //Special for Obj Function
            Label variableLbObj = new Label();
            Label varIndexLbObj = new Label();
            Label operatorLbObj = new Label();
            Label coeffecientLbObj = new Label();
            variableLbObj.Content = "z";
            varIndexLbObj.Content = "";
            operatorLbObj.Content = "";
            coeffecientLbObj.Content = "";


            variableLbObj.Height = labelHeight;
            variableLbObj.Width = widthPerLetter * variableLbObj.Content.ToString().Length;
            variableLbObj.Padding = new Thickness(0, variableLbObj.Padding.Top, 0, variableLbObj.Padding.Bottom);
            variableLbObj.Style = (Style)Application.Current.Resources["StrVar"];


            varIndexLbObj.Height = labelHeight;
            varIndexLbObj.Width = widthPerLetter * varIndexLbObj.Content.ToString().Length;
            varIndexLbObj.Padding = new Thickness(0, varIndexLbObj.Padding.Top, 0, varIndexLbObj.Padding.Bottom);
            varIndexLbObj.Style = (Style)Application.Current.Resources["StrVar"];


            operatorLbObj.Height = labelHeight;
            operatorLbObj.Width = widthPerLetter * operatorLbObj.Content.ToString().Length;
            operatorLbObj.Padding = new Thickness(0, operatorLbObj.Padding.Top, 0, operatorLbObj.Padding.Bottom);
            operatorLbObj.Style = (Style)Application.Current.Resources["StrVar"];


            coeffecientLbObj.Height = labelHeight;
            coeffecientLbObj.Width = widthPerLetter * coeffecientLbObj.Content.ToString().Length;
            coeffecientLbObj.Padding = new Thickness(0, coeffecientLbObj.Padding.Top, 0, coeffecientLbObj.Padding.Bottom);
            coeffecientLbObj.Style = (Style)Application.Current.Resources["StrVar"];


            container.Children.Add(operatorLbObj);
            Canvas.SetLeft(operatorLbObj, currentToLeft);
            currentToLeft += operatorLbObj.Width;


            container.Children.Add(coeffecientLbObj);
            Canvas.SetLeft(coeffecientLbObj, currentToLeft);
            currentToLeft += coeffecientLbObj.Width;


            container.Children.Add(variableLbObj);
            Canvas.SetLeft(variableLbObj, currentToLeft);
            currentToLeft += variableLbObj.Width;

            container.Children.Add(varIndexLbObj);
            Canvas.SetLeft(varIndexLbObj, currentToLeft);
            currentToLeft += varIndexLbObj.Width;




            for (int i = 0; i < numOfVar; i++)
            {
                variableLb[i] = new Label();
                varIndexLb[i] = new Label();
                coeffecientLb[i] = new Label();
                operatorLb[i] = new Label();
            }

            // Setup the right hand side Label & equality Label content and size
            rightHandSideLB.Content = equ.getRHS().ToString("0.##");
            rightHandSideLB.Height = labelHeight;
            rightHandSideLB.Width = widthPerLetter * rightHandSideLB.Content.ToString().Length;
            rightHandSideLB.Padding = new Thickness(0, rightHandSideLB.Padding.Top, 0, rightHandSideLB.Padding.Bottom);
            rightHandSideLB.Style = (Style)Application.Current.Resources["StrVar"];



            equalityLb.Content = "=";
            equalityLb.Height = labelHeight;
            equalityLb.Width = equalityLb.Content.ToString().Length * widthPerLetter;
            equalityLb.Padding = new Thickness(0, equalityLb.Padding.Top, 0, equalityLb.Padding.Bottom);
            equalityLb.Style = (Style)Application.Current.Resources["StrVar"];


            // Setup Label Content and size for each set of varibles and its coefficients
            for (int i = 0; i < variableLb.Length; i++)
            {

                // Set the font
                variableLb[i].Style = (Style)Application.Current.Resources["StrVar"];
                varIndexLb[i].Style = (Style)Application.Current.Resources["StrVar"];
                coeffecientLb[i].Style = (Style)Application.Current.Resources["StrVar"];
                operatorLb[i].Style = (Style)Application.Current.Resources["StrVar"];

                variableLb[i].Padding = new Thickness(0, 0, 0, 0);
                varIndexLb[i].Padding = new Thickness(0, 0, 0, 0);
                coeffecientLb[i].Padding = new Thickness(0, equalityLb.Padding.Top, 0, equalityLb.Padding.Bottom);
                operatorLb[i].Padding = new Thickness(0, equalityLb.Padding.Top, 0, equalityLb.Padding.Bottom);


                int varIndex = 0;
                for (int j = 0; j < equ.getVariable()[i].Length; j++)
                {
                    //int k = equ.getVariable()[i].ToCharArray()[j];
                    if (equ.getVariable()[i].ToCharArray()[j] <= 57)
                    {
                        varIndex = j;
                        break;
                    }
                }
                variableLb[i].Content = equ.getVariable()[i].Substring(0, varIndex).ToString();
                if (equ.getCoefficient()[i] < 0)
                {
                    if (equ.getCoefficient()[i] != -1)
                    {
                        coeffecientLb[i].Content = (-equ.getCoefficient()[i]).ToString("0.##");
                    }
                    else
                    {
                        coeffecientLb[i].Content = "";
                    }
                    operatorLb[i].Content = "-";
                }
                else // >0
                {
                    if (equ.getCoefficient()[i] != 1)
                    {
                        coeffecientLb[i].Content = equ.getCoefficient()[i].ToString("0.##");
                    }
                    else
                    {
                        coeffecientLb[i].Content = "";
                    }
                    if (i != 0)
                    {
                        operatorLb[i].Content = "+";
                    }
                    else
                    {
                        operatorLb[i].Content = "+";
                    }
                }

                varIndexLb[i].Content = equ.getVariable()[i].Substring(varIndex).ToString();

                variableLb[i].Height = labelHeight;
                variableLb[i].FontSize = 16;
                varIndexLb[i].Height = labelHeight;
                varIndexLb[i].FontSize = 8;
                coeffecientLb[i].Height = labelHeight;
                operatorLb[i].Height = labelHeight;

                variableLb[i].Width = variableLb[i].Content.ToString().Length * widthPerLetter;
                varIndexLb[i].Width = varIndexLb[i].Content.ToString().Length * widthPerLetter;
                coeffecientLb[i].Width = coeffecientLb[i].Content.ToString().Length * widthPerLetter;
                coeffecientLb[i].Foreground = Brushes.RoyalBlue;
                operatorLb[i].Width = operatorLb[i].Content.ToString().Length * widthPerLetter;

                if (LinearSolverUI.MainWindow.enteringVariable != null && equ.getVariable()[i].ToString() == LinearSolverUI.MainWindow.enteringVariable.ToString())
                {
                    variableLb[i].Foreground = Brushes.Red;
                    varIndexLb[i].Foreground = Brushes.Red;
                }

                container.Children.Add(operatorLb[i]);
                Canvas.SetLeft(operatorLb[i], currentToLeft);
                currentToLeft += operatorLb[i].Width + 3;

                container.Children.Add(coeffecientLb[i]);
                Canvas.SetLeft(coeffecientLb[i], currentToLeft);
                currentToLeft += coeffecientLb[i].Width;

                container.Children.Add(variableLb[i]);
                Canvas.SetLeft(variableLb[i], currentToLeft);
                currentToLeft += variableLb[i].Width;

                container.Children.Add(varIndexLb[i]);
                Canvas.SetLeft(varIndexLb[i], currentToLeft);
                Canvas.SetTop(varIndexLb[i], labelHeight / 2);
                currentToLeft += varIndexLb[i].Width;
            }

            container.Children.Add(equalityLb);
            Canvas.SetLeft(equalityLb, currentToLeft);
            currentToLeft += equalityLb.Width;

            container.Children.Add(rightHandSideLB);
            Canvas.SetLeft(rightHandSideLB, currentToLeft);
            currentToLeft += rightHandSideLB.Width;


            container.Width = currentToLeft;

        }



       /* container_MouseOverHandler: Detect mouse move on the container(Canvas) and change the 
        * Background property of it.
        */
        private void container_MouseOverHandler(object sender, EventArgs e)
        {
            container.Background = Brushes.Wheat;
        }


        /* container_MouseLeaveHandler: Detect mouse leave event on the container(Canvas) and 
         * recover the Background property of it.
         */
        private void container_MouseLeaveHandler(object sender, EventArgs e)
        {
            container.Background = new Canvas().Background;
        }

        /*
         * highLightRHS: To show RHS clearly when it is determined that the result is optimal.
         */ 
        public void highLightRHS()
        {
            this.rightHandSideLB.Foreground = Brushes.Red;
        }
    }
}
