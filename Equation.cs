using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearSolver
{

    /*
     * This Class contains several parameters that an Equation or Inequality shall has,
     * provides getters and setters to modify these fields.
     * The Constructor needs one String as parameter which should contain a well orgnized 
     * function. The Constructor will process the string to split variables and coefficients,
     * store them correspondingly in two arrays and determines the type (Less than, Larger than, etc.)
     * of the function.
     */ 
    public class Equation
    {
        private int type{get;set;} //Indicates the type of this function, see util class for details.
        private double[] coefficient = new double[0]; //the coefficients corresponding to each varaible.
        private String[] variable = new String[0];  // Variables involved in this equation.
        private double rHS = 0;  //Right Hand Side

        /* Constructor
         * input a certain function as a string, the constructor will split the string, inditify and store variables, 
         * coefficients separately but correspondingly.
         * The input string should be well orgnized and checked before transfer into the construtor.
         * The string should like: x1+5x2+2x3=-3x4
         */
        public Equation(String equStr)
        {
            char[] equCharArray = equStr.ToCharArray(0,equStr.Length); // Transfer a string to a char array
            bool isPositive = true; // Indicates that if the current number is positive or not.
            bool isVariableIndex = false; //Indicates that if the current processed number is part of a variable index or not 
            bool afterFloat=false; // Indicates that if a float is detected or not
            bool isRHS = false; // Indicates that if a equality or inequality is detected so the following is the right hand side of the function
            String tmpVariable = ""; 
            double? tmpNumber = null;
            int numOfAfterFloat = 0;

            // Check the type of the function
            if (equStr.IndexOf(">=") != -1)
            {
                type = util.LARGERTHAN_EQUALTO;
            }
            else if (equStr.IndexOf("<=") != -1)
            {
                type = util.LESSTHAN_EQUALTO;
            }
            else if (equStr.IndexOf(">") != -1)
            {
                type = util.LARGERTHAN;
            }
            else if (equStr.IndexOf("<") != -1)
            {
                type = util.LESSTHAN;
            }
            else if (equStr.IndexOf("=") != -1)
            {
                type = util.EQUALTO;
            }



            // process every char value and determine what is coefficients which is the variable and its index.
            for (int i = 0; i < equCharArray.Length; i++)
            {
                switch (equCharArray[i])
                {
                    case '+':
                        if (tmpVariable == "" && tmpNumber != null)
                        {
                            if (!isPositive)
                            {
                                tmpNumber *= -1;
                            }
                            rHS += (double)tmpNumber;
                            tmpNumber = null;
                        }
                        else if (tmpVariable != "")
                        {
                            if (!isPositive)
                            {
                                tmpNumber *= -1;
                            }
                            variable = util.insert(variable, tmpVariable);
                            tmpVariable = "";
                            isVariableIndex = false;
                        }
                        if (isRHS)
                        {
                            isPositive = false;
                        }
                        else
                        {
                            isPositive = true;
                        }
                        break;
                    case '-':
                        if (tmpVariable == "" && tmpNumber!=null)
                        {
                            if (!isPositive)
                            {
                                tmpNumber *= -1;
                            }
                            rHS += (double)tmpNumber;
                            tmpNumber = null;
                        }
                        else if(tmpVariable!="")
                        {
                            if (!isPositive)
                            {
                                tmpNumber *= -1;
                            }
                            variable = util.insert(variable, tmpVariable);
                            tmpVariable = "";
                            isVariableIndex = false;
                        }
                        if (isRHS)
                        {
                            isPositive = true;
                        }
                        else
                        {
                            isPositive = false;
                        }
                        break;
                    case '=':
                    case '<':
                    case '>':
                        if (!isRHS)
                        {
                            if (tmpVariable == "")
                            {
                                if (!isPositive)
                                {
                                    tmpNumber *= -1;
                                }
                                rHS += (double)tmpNumber;
                                tmpNumber = null;
                                afterFloat = false;
                                numOfAfterFloat = 0;
                            }
                            else
                            {
                                variable = util.insert(variable, tmpVariable);
                                tmpVariable = "";
                                isVariableIndex = false;
                            }
                            isRHS = true;
                            isPositive = false;
                        }
                        break;
                    case 'x':
                    case 'X':
                        if (tmpNumber == null)
                        {
                            tmpNumber = 1;
                            if (!isPositive)
                            {
                                tmpNumber *= -1;
                            }
                            coefficient = util.insert(coefficient, (double)tmpNumber);
                            tmpNumber = null;
                            afterFloat = false;
                            numOfAfterFloat = 0;
                        }
                        else
                        {
                            if (!isPositive)
                            {
                                tmpNumber *= -1;
                            }
                            coefficient = util.insert(coefficient, (double)tmpNumber);
                            tmpNumber = null;
                            afterFloat = false;
                            numOfAfterFloat = 0;
                        }
                        isVariableIndex = true;
                        tmpVariable = "x";
                        break;
                    case '.':
                        afterFloat = true;
                        break;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        if (isVariableIndex)
                        {
                            tmpVariable += equCharArray[i].ToString();
                        }
                        else
                        {
                            if(tmpNumber==null)
                            {
                                tmpNumber = equCharArray[i] - 48;
                            }
                            else
                            {
                                if (!afterFloat)
                                {
                                    tmpNumber = tmpNumber * 10 + equCharArray[i] - 48;
                                }
                                else
                                {
                                    tmpNumber = tmpNumber + (((double)(equCharArray[i] - 48)) / Math.Pow(10,(1+(numOfAfterFloat++))));
                                }
                            }
                        }
                        break;
                }
            }

            // update the final value if there is some
            if (tmpNumber != null)
            {
                if (!isPositive)
                {
                    tmpNumber *= -1;
                }
                rHS += (double)tmpNumber;
                tmpNumber = null;
            }
            if (tmpVariable != "")
            {
                variable = util.insert(variable, tmpVariable);
                tmpVariable = "";
                isVariableIndex = false;
            }
            rHS = -rHS;
        }


        /*
         * Getters & Setters
         */ 
        public int getType()
        {
            return type;
        }

        public void setType(int type)
        {
            this.type = type;
        }

        public double[] getCoefficient()
        {
            return coefficient;
        }

        public void setCoefficient(double[] coefficient)
        {
            this.coefficient = coefficient;
        }

        public void setCoefficient(double newData, int index)
        {
            this.coefficient[index] = newData;
        }

        public String[] getVariable()
        {
            return variable;
        }

        public void setVariable(String[] variable)
        {
            this.variable = variable;
        }

        public double getRHS()
        {
            return rHS;
        }

        public void setRHS(double rHS)
        {
            this.rHS = rHS;
        }

    }
}
