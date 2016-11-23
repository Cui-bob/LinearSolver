using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearSolver
{
    /*
     * This class refers to a certain linear system with an Objective function and several constraints.
     * This class also offer methods to process/modify the (in)equlities in order to solve the system finally.
     */ 
    public class LinearSystem
    {
        private Equation objFunction;
        private Equation[] equ = new Equation[0]; // The Constraints
        private bool isProcessing = false; // Indicates that if the solving process is begin or not. If begins, new equation are not allowed.
        private int numOfSlackVars = 0; // Counts the number of slack variables used

        /* newEqu: Create a new equation with a given equation string and insert it into the equ array
         * Input: equStr - the string that contains a formated equation. Notice that the string's legality should be check before transfer to the Equation Constructor
         */
        public void newEqu(String equStr)
        {
            //TODO: Check legality for the equation string 
            if (isProcessing == false)
            {
                equ = util.insert(equ, new Equation(equStr));
                mergeVars(equ[equ.Length-1]);
            }
        }

        /* replaceWith: replace the source varible in source equation with the given variable(s) with certain coefficients.
         * Inputs: 
         *      srcEqu: The source Equation Object where the replacement will take place
         *      srcVar: The source variable which will be replaced
         *      rplVar: The variables (maybe more than one) that will replace the source variable
         *      rplCoefficient: the coefficients of the rplVar correspondingly by the array index. 
         */ 
        public void replaceWith(Equation srcEqu, String srcVar, String[] rplVar, double[] rplCoefficient, double RHS)
        {
            int indexOfSrcVar = 0;

            foreach (String var in srcEqu.getVariable())
            {
                if (var == srcVar)
                {
                    break;
                }
                else
                {
                    indexOfSrcVar++;
                }
            }
            //Check if there is a same variable in this equation
            if (indexOfSrcVar < srcEqu.getVariable().Length)
            {
                double factor = srcEqu.getCoefficient()[indexOfSrcVar];
                for (int i = 0; i < rplCoefficient.Length; i++)
                {
                    rplCoefficient[i] *= factor;
                }
                srcEqu.setVariable(util.removeFrom(srcEqu.getVariable(), indexOfSrcVar));
                srcEqu.setCoefficient(util.removeFrom(srcEqu.getCoefficient(), indexOfSrcVar));
                for (int i = 0; i < rplVar.Length; i++)
                {
                    srcEqu.setVariable(util.insert(srcEqu.getVariable(), rplVar[i]));
                    srcEqu.setCoefficient(util.insert(srcEqu.getCoefficient(), rplCoefficient[i]));
                }
                srcEqu.setRHS(srcEqu.getRHS() - RHS * factor);


                //recover the data in array
                for (int i = 0; i < rplCoefficient.Length; i++)
                {
                    rplCoefficient[i] /= factor;
                }
                //call merge
                mergeVars(srcEqu);
            }
        }

        /* reOrgnize: Re-orgnize the function that make the coefficient of the standard variable unit without effecting the result
         * of original function. 
         * Input:
         *      srcEqu: the source funcion that will be re-orgnized.
         *      stdVar: the standard variable whose coefficient should be unit as the result of this process.
         */ 
        public void reOrgnize(Equation srcEqu, String stdVar)
        {
            //locate the standard variable
            int indexOfStdVar = 0;
            foreach (String var in srcEqu.getVariable())
            {
                if (var == stdVar)
                {
                    break;
                }
                else
                {
                    indexOfStdVar++;
                }
            }

            double factor = srcEqu.getCoefficient()[indexOfStdVar];
            for (int i = 0; i < srcEqu.getCoefficient().Length; i++)
            {
                srcEqu.setCoefficient(srcEqu.getCoefficient()[i] / factor, i);
            }
            srcEqu.setCoefficient(1, indexOfStdVar);
            srcEqu.setRHS(srcEqu.getRHS()/factor);
        }


        /*
         * Getters & Setters
         */
        public Equation[] getEqu()
        {
            return equ;
        }

        public void setEqu(Equation[] equ)
        {
            this.equ = equ;
        }

        public void setObjFunction(String equStr)
        {
            objFunction = new Equation(equStr);
            for (int i = 0; i < objFunction.getCoefficient().Length; i++)
            {
                objFunction.setCoefficient(objFunction.getCoefficient()[i] * -1, i);
            }
        }

        public void setObjFunction()
        {
            objFunction = null;
        }

        public Equation getObjFunction()
        {
            return objFunction;
        }

        /* mergeVars: Check if there are same variables with different coefficients in the function. If so, merge them together.
         * Input:
         *      srcEqu: the source equation that will be checked and, if need, the merges happens in.
         */ 
        private void mergeVars(Equation srcEqu)
        {
            double[] newCoefficient = srcEqu.getCoefficient();
            if (srcEqu.getVariable().Length > 1)
            {
                for (int i = 0; i < srcEqu.getVariable().Length; i++)
                {
                    for (int j = i + 1; j < srcEqu.getVariable().Length; j++)
                    {
                        if (srcEqu.getVariable()[i] == srcEqu.getVariable()[j])
                        {
                            newCoefficient = srcEqu.getCoefficient();
                            srcEqu.setVariable(util.removeFrom(srcEqu.getVariable(), j));
                            newCoefficient[i] += newCoefficient[j];
                            newCoefficient = util.removeFrom(newCoefficient, j);
                            srcEqu.setCoefficient(newCoefficient);
                            j--;
                        }
                    }
                }
            }
        }

        /* chooseEtrVar: check the objective function and find the first variable with negative coeffient as the Entering Variable
         * Return: a string contains the Entering Variable(return null if not found, and this indicates that the objective function
         * is optimal).
         */
        public String chooseEtrVar()
        {
            for (int i = 0; i < objFunction.getCoefficient().Length; i++)
            {
                if(objFunction.getCoefficient()[i]<0)
                {
                    return objFunction.getVariable()[i];
                }
            }
            return null;
        }

        /* calRatio: calculate the ratio of RHS and Entering Variable coefficient.
         * Input: 
         *      srcEqu: The source equation who owns the ratio.
         *      EtrVar: The string contains the entering variable.
         * Return: The ratio of the RHS and EVC, null if EtrVar is not found in this equ.
         */ 
        public double? calRatio(Equation srcEqu, String EtrVar)
        {
            double EtrCoE = 0;
            for (int i = 0; i < srcEqu.getVariable().Length; i++)
            {
                if (EtrVar == srcEqu.getVariable()[i])
                {
                    EtrCoE = srcEqu.getCoefficient()[i];
                    break;
                }
            }
            if (EtrCoE != 0)
            {
                return (srcEqu.getRHS() / EtrCoE);
            }
            else
            {
                return null;
            }
        }

        /* formatEqu: format function to Equation which is absolutely equal.
         * Input:
         *      srcEqu: The source function to be formated.
         */
        public void formatEqu(Equation srcEqu)
        {
            switch (srcEqu.getType())
            {
                case util.LARGERTHAN:
                case util.LARGERTHAN_EQUALTO:
                    srcEqu.setVariable(util.insert(srcEqu.getVariable(),nextSlackVar()));
                    srcEqu.setCoefficient(util.insert(srcEqu.getCoefficient(),-1));
                    break;
                case util.EQUALTO:
                    break;
                case util.LESSTHAN:
                case util.LESSTHAN_EQUALTO:
                    srcEqu.setVariable(util.insert(srcEqu.getVariable(),nextSlackVar()));
                    srcEqu.setCoefficient(util.insert(srcEqu.getCoefficient(),1));
                    break;
            }
            srcEqu.setType(util.EQUALTO);
        }

        /* nextSlackVar: count the number of slack variables and generate it with name 's' + number.
         * Return: The next slack variable.
         */ 
        private String nextSlackVar()
        {
            String str = "s" + numOfSlackVars.ToString();
            numOfSlackVars++;
            return str;
        }

        /* choosePvtEmt: choose the Pivot Element by calculating the ratio.
         * Input:
         *      EtrVar: The string that contains the Entering Variable.
         * Return: an object array which contains two array objects, one for the variable string array,
         * another one for the coeffecients double array. Notice that should be cast into proper type 
         * before using. The Object at index 0 is String array and the object at index 1 is double array.
         */
        public Object[] choosePvtEmt(String EtrVar)
        {
            Object[] arraySet = new Object[4];
            int indexOfPvtEmt=0;
            double minOfRatio = double.MaxValue;
            //find the minimum of the ratio and locate the pivot element
            for (int i = 0; i < equ.Length; i++)
            {
                if (calRatio(equ[i], EtrVar) > 0 && calRatio(equ[i], EtrVar) <= minOfRatio)
                {
                    indexOfPvtEmt = i;
                    minOfRatio = (double)calRatio(equ[i], EtrVar);
                }
            }

            //reorgnize the pivot element
            reOrgnize(equ[indexOfPvtEmt],EtrVar);

            String[] rplVar = equ[indexOfPvtEmt].getVariable();
            double[] rplCoE = equ[indexOfPvtEmt].getCoefficient();
            for (int i = 0; i < rplVar.Length; i++)
            {
                if (rplVar[i] == EtrVar)
                {
                    rplVar = util.removeFrom(rplVar, i);
                    rplCoE = util.removeFrom(rplCoE, i);
                    i--;
                }
                else
                {
                    rplCoE[i] *= -1;
                }
            }
            arraySet[0] = rplVar;
            arraySet[1] = rplCoE;
            arraySet[2] = indexOfPvtEmt;
            arraySet[3] = equ[indexOfPvtEmt].getRHS();
            return arraySet;
        }

        /* solve: solve this certain linear system.
         * Return: A double number which indicates the optimum of the Object function.
         */ 
        public double solve()
        {
            String etrVar=chooseEtrVar();
            Object[] arraySet;
            String[] rplVar;
            double[] rplCoE;
            int indexOfPvtEmt;
            double rplRHS;
            //format equations
            for (int i = 0; i < equ.Length; i++)
            {
                formatEqu(equ[i]);
            }

            //solve
            while (etrVar != null)
            {
                arraySet = choosePvtEmt(etrVar);
                rplVar = (String[])arraySet[0];
                rplCoE = (double[])arraySet[1];
                indexOfPvtEmt = (int)arraySet[2];
                rplRHS = (double)arraySet[3];
                for (int i = 0; i < equ.Length; i++)
                {
                    if (indexOfPvtEmt != i)
                    {
                        replaceWith(equ[i], etrVar, rplVar, rplCoE, rplRHS);
                    }
                }
                replaceWith(objFunction, etrVar, rplVar, rplCoE, rplRHS);
                etrVar = chooseEtrVar();
            }
            return objFunction.getRHS();
        }

        /*
         * reset: reset all.
         */
        public void reset()
        {
            this.equ = new Equation[0];
            this.objFunction = null;
            this.numOfSlackVars = 0;
        }

        /* checkLegality: to check whether the input equation is well formated 
         * so that it is able to be recognized by the equation constructor.
         * Input: 
         *      String inputEqu: the string of the equation.
         *      bool isObj: true if the equation is an objective funtion.
         * Return: true if it is legal; if not, return false.
         */ 
        public static bool checkLegality(String inputEqu, bool isObj)
        {
            bool hasFloat = false;
            bool isVar = false;
            bool hasOperator = false;
            bool hasEquality = false;
            bool isNum = false;
            char[] inputEquArray = inputEqu.ToCharArray();
            for (int i = 0; i < inputEquArray.Length; i++)
            {
                switch (inputEquArray[i])
                {
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
                        if (!hasOperator && !isVar  && !isNum)
                        {
                            if (i == 0)
                            {
                                isNum = true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if(hasOperator && !isVar)
                        {
                            hasOperator = false;
                            hasFloat = false;
                            isNum = true;
                        }
                        else if (isVar)
                        {
                        }
                        break;
                    case 'x':
                    case 'X':
                        if (isVar)
                        {
                            return false;
                        }
                        else if (!hasOperator && !isNum && i!=1 && i!=0)
                        {
                            return false;
                        }
                        else if (i != 0 && inputEquArray[i - 1] == '.')
                        {
                            return false;
                        }
                        else
                        {
                            isVar = true;
                            isNum = false;
                            hasOperator = false;
                        }
                        break;
                    case '.':
                        if (hasFloat)
                        {
                            return false;
                        }
                        else if (isVar)
                        {
                            return false;
                        }
                        else if (i != 0 && (inputEquArray[i - 1] == '+' || inputEquArray[i - 1] == '-'))
                        {
                            return false;
                        }
                        else
                        {
                            hasFloat = true;
                        }
                        break;
                    case '=':
                    case '<':
                    case '>':
                        if (hasEquality)
                        {
                            return false;
                        }
                        else if (i != 0 && (inputEquArray[i - 1] == '.' || inputEquArray[i - 1] == '+' || inputEquArray[i - 1] == '-'))
                        {
                            return false;
                        }
                        else if ((inputEquArray[i] == '>' && i + 1 < inputEquArray.Length && inputEquArray[i + 1] == '=') || (inputEquArray[i] == '<' && i + 1 < inputEquArray.Length && inputEquArray[i + 1] == '='))
                        {
                            i++;
                            hasEquality = true;
                            isVar = false;
                            hasOperator = true;
                        }
                        else
                        {
                            hasEquality = true;
                            isVar = false;
                            hasOperator = true;
                        }
                        break;
                    case '+':
                    case '-':
                        if (isVar)
                        {
                            isVar = false;
                            hasOperator = true;
                        }
                        else if (hasOperator)
                        {
                            if (i != 0 && (inputEquArray[i - 1] == '<' || inputEquArray[i - 1] == '>' || inputEquArray[i - 1] == '='))
                            {
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            hasOperator = true;
                        }
                        break;
                    default:
                        return false;
                }
            }

            if (hasOperator == true)
            {
                return false;
            }
            if (hasEquality && isObj)
            {
                return false;
            }
            if (!hasEquality && !isObj)
            {
                return false;
            }
            return true;
        }
    } 
}
