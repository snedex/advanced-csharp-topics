using System.Numerics;

namespace CSharpTopics.Section6
{
    //Used to specify the return result of the alorithm 
    public enum WorkflowResult
    {
        Solved,
        Failure
    }

    public class QuadraticEquationSolver
    {
        //ax^2+bx+c == 0 
        public Tuple<Complex, Complex> Start(double a, double b, double c)
        {
            //calculate discriminant
            var disc = b * b - 4 * a * c;

            //real root or complex root?
            if (disc < 0)
                return SolveComplex(a, b, c, disc);
            else
                return SolveSimple(a, b, c, disc);
        }

        //Copy with a result for showing workflow results
        //Now moved the output into an out parameter
        public WorkflowResult StartWithResult(double a, double b, double c, out Tuple<Complex, Complex> result)
        {
            //calculate discriminant
            var disc = b * b - 4 * a * c;

            //we can't solve negative roots
            if (disc < 0)
            {
                result = null;
                return WorkflowResult.Failure;
            }
            else
                return SolveSimple(a, b, disc, out result);
        }

        //We can also remove c here as well
        private WorkflowResult SolveSimple(double a, double b, double disc, out Tuple<Complex, Complex> result)
        {
            var rootDisc = Math.Sqrt(disc);

            result = Tuple.Create (
                new Complex((-b + rootDisc) / ( 2 * a), 0),
                new Complex((-b - rootDisc) / ( 2 * a), 0)
            );

            return WorkflowResult.Solved;
        }

        private Tuple<Complex, Complex> SolveSimple(double a, double b, double c, double disc)
        {
            var rootDisc = Math.Sqrt(disc);

            return Tuple.Create (
                new Complex((-b + rootDisc) / ( 2 * a), 0),
                new Complex((-b - rootDisc) / ( 2 * a), 0)
            );
        }

        private Tuple<Complex, Complex> SolveComplex(double a, double b, double c, double disc)
        {
            var rootDisc = Complex.Sqrt(new Complex(disc, 0));

            return Tuple.Create (
                (-b + rootDisc) / ( 2 * a),
                (-b - rootDisc) / ( 2 * a)
            );
        }
    }
}