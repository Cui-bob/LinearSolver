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
using System.Timers;

namespace LinearSolverUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private LinearSystem ls = new LinearSystem();
        private EquationContainerList ecl;
        private int currentStep = 0;
        public static String enteringVariable;
        public static Equation pivotElement;
        private Object[] arraySet;

        public MainWindow()
        {
            InitializeComponent();
            ecl = new EquationContainerList(srcollViewer,new Point(320,125));
            srcollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            srcollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

        }

        /* newCstBt_Click: To handle the Mouse Click event on the 'New Constraints' Button.
         * Add a new constraints/objective function to the linear system according to the 
         * equBox input content and the check box 'Objective Function' to determine whether 
         * it is an objcetive function or just a normal constraints.
         */
        private void newCstBt_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep == 0)
            {
                if (equBox.Text != "" && equBox.Text != "Type Your Equation Here")
                {
                    if (isObj.IsChecked == true)
                    {
                        if (LinearSystem.checkLegality(equBox.Text, true))
                        {
                            ls.setObjFunction(equBox.Text);
                            ecl.setObjFunc(ls.getObjFunction());
                            //ecl.getEquCtn()[ecl.getEquCtn().Length - 1].getContainer().MouseRightButtonUp += new MouseButtonEventHandler(dltBt_Click);
                            equBox.Text = "Type Your Equation Here";
                            equBox.Foreground = Brushes.LightGray;
                            isObj.IsChecked = false;
                            isObj.IsEnabled = false;
                            noticeLb.Text = "";
                        }
                        else
                        {
                            noticeLb.Text = "Invalid Input!";
                        }
                    }
                    else
                    {
                        if (LinearSystem.checkLegality(equBox.Text, false))
                        {
                            ls.newEqu(equBox.Text);
                            ecl.newEquCtn(ls.getEqu()[ls.getEqu().Length - 1]);
                            ecl.getEquCtn()[ecl.getEquCtn().Length - 1].getContainer().MouseRightButtonUp += new MouseButtonEventHandler(dltBt_Click);
                            equBox.Text = "Type Your Equation Here";
                            equBox.Foreground = Brushes.LightGray;
                            noticeLb.Text = "";
                        }
                        else
                        {
                            noticeLb.Text = "Invalid Input!";
                        }
                    }
                }
                else
                {
                    noticeLb.Text = "Input shall not be EMPTY!";
                }
            }
        }

        /* slbBt_Click: To handle the mouse click event on the 'Solve' Button.
         * Solve the Linear System immediately and directly show up the result.
         */
        private void slvBt_Click(object sender, RoutedEventArgs e)
        {
            if (ls.getObjFunction() != null && ls.getEqu().Length > 0)
            {
                noticeLb.Text = "The Optimum of the Objective Function is: " + ls.solve().ToString();
            }
            else if (ls.getObjFunction() == null)
            {
                noticeLb.Text = "Objective Function shall not be NULL!";
            }
            else
            {
                noticeLb.Text = "There shall at least be one constraints!";
            }
        }

        /* dltBt_Click: To handle the Right click event on a EquationContainer.
         * This event means that user would like to remove this equation from the 
         * system as well as the UI.
         * currentStep should be checked before removing, since when the system is under
         * processing, none of the equation shall be removed.
         */ 
        private void dltBt_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep == 0)
            {
                Canvas canvasOnClick = (Canvas)sender;
                if (canvasOnClick != null)
                {
                    ls.setEqu(util.removeFrom<Equation>(ls.getEqu(), ecl.removeEquCtn(canvasOnClick)));
                }
            }
        }


        /* slvStpBt_Click: the handler for the 'Solve By Step' Button. Process the Linear System with 
         * proper methods corresponding to current step.
         */ 
        private void slvStpBt_Click(object sender, RoutedEventArgs e)
        {
            switch (currentStep)
            {
                case util.FORMAT_EQUATION:
                    equBox.Text = "System is processing currently...";
                    equBox.IsEnabled = false;
                    for (int i = 0; i < ls.getEqu().Length; i++)
                    {
                        ls.formatEqu(ls.getEqu()[i]);
                    }
                    noticeLb.Text = "Equation Formated; " + ls.getEqu().Length.ToString() + " Slack Variables added: ";
                    for (int i = 0; i < ls.getEqu().Length - 1; i++)
                    {
                        noticeLb.Text += "s" + i.ToString() + ", ";
                    }
                    noticeLb.Text += "s" + (ls.getEqu().Length - 1).ToString();
                    this.currentStep = util.FOUND_ENTERING_VARIABLE;
                    foreach (EquationContainer ec in ecl.getEquCtn())
                    {
                        ec.updateEqu();
                    }
                    break;
                case util.FOUND_ENTERING_VARIABLE:
                    enteringVariable = ls.chooseEtrVar();
                    if (enteringVariable != null)
                    {
                        foreach (EquationContainer ec in ecl.getEquCtn())
                        {
                            ec.updateEqu();
                        }
                        ecl.getObjFunCtn().updateEqu();
                        this.currentStep = 2;
                        noticeLb.Text = enteringVariable + " is choen as the Entering Variable";
                    }
                    else
                    {
                        //TODO: end process and show the optimum.
                        noticeLb.Text = "The Optimum of the Objective Function is: " + ecl.getObjFunCtn().getEqu().getRHS().ToString("0.###");
                        ecl.getObjFunCtn().highLightRHS();
                    }
                    break;
                case util.FOUND_PIVOT_ELEMENT:
                    arraySet = ls.choosePvtEmt(enteringVariable);
                    pivotElement = ls.getEqu()[(int)arraySet[2]];
                    foreach (EquationContainer ec in ecl.getEquCtn())
                    {
                        ec.updateEqu();
                    }
                    noticeLb.Text = "Equation[" + (int)arraySet[2] + "] is chosen as the Pivot Element since it has the minimum Ratio between RHS and Entering Variable's Coefficient";
                    currentStep = util.CALCULATING;
                    break;
                case util.CALCULATING:
                    ls.replaceWith(ls.getObjFunction(),enteringVariable,(String[])arraySet[0],(double[])arraySet[1],(double)arraySet[3]);
                    for (int i = 0; i < ls.getEqu().Length; i++)
                    {
                        if ((int)arraySet[2] != i)
                        {
                            ls.replaceWith(ls.getEqu()[i], enteringVariable, (String[])arraySet[0], (double[])arraySet[1], (double)arraySet[3]);
                        }
                    }
                    enteringVariable = null;
                    pivotElement = null;
                    foreach (EquationContainer ec in ecl.getEquCtn())
                    {
                        ec.updateEqu();
                    }
                    ecl.getObjFunCtn().updateEqu();
                    noticeLb.Text = "Replace " + enteringVariable + " in all the equations with " +  ((double[])arraySet[1])[0].ToString() +((String[])arraySet[0])[0];
                    for (int i = 1; i < ((String[])arraySet[0]).Length; i++)
                    {
                        noticeLb.Text +=  ", " + ((double[])arraySet[1])[i].ToString() + ((String[])arraySet[0])[i];
                    }
                    noticeLb.Text += " and " + ((double)arraySet[3]).ToString();
                    currentStep = util.FOUND_ENTERING_VARIABLE;
                    break;

            }
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        /* equBox_LostFocus: To handle the LostFocus event on equBox.
         * If equBox lost focus without any input in it, show up the 
         * promption and set the color to gray.
         */ 
        private void equBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (equBox.Text == "")
            {
                equBox.Text = "Type Your Equation Here";
                equBox.Foreground = Brushes.LightGray;
            }
            enterNoticeLb.Visibility = Visibility.Hidden;
        }


        /* equBox_KeyUp: To handle the Key Up event on the equBox.
         * Capture ENTER key.
         */ 
        private void equBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                newCstBt_Click((object)newCstBt,e);
                equBox.Text = "";
            }
        }


        /* equBox_GotFocus: To handle the event when 'equBox' got focused.
         * Just clear the promption.
         */ 
        private void equBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (equBox.Text == "Type Your Equation Here")
            {
                equBox.Text = "";
            }
        }

        /* Reset_Click: The handler for the 'Reset' Button.
         * Reset all the fields and UI in this system and ready for next use.
         */ 
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ls.reset();
            ecl.reset();
            noticeLb.Text = "";
            equBox.Text = "";
            this.currentStep = 0;
            enteringVariable = null;
            arraySet = null;
            pivotElement = null;
            equBox.IsEnabled = true;
            isObj.IsChecked = true;
            isObj.IsEnabled = true;
        }


        /* equBox_KeyDown: To handle the Key Down event on the equBox.
         * If the equBox is not empty, set the color of the font to 
         * black.
         */ 
        private void equBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (equBox.Text != "")
            {
                equBox.Foreground = Brushes.Black;
                enterNoticeLb.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                enterNoticeLb.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
