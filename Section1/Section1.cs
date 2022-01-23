using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace CSharpTopics.Section1
{
    public sealed class Section1 : SectionBase
    {
        public override void RunSection()
        {
            #region Numerics
            UnCheckedIntOperations();

            FPOperations();

            DecimalOperations();

            SIMDandVectorsIntrinsics();

            SIMDandVectorTypes();
            #endregion
        }

        public void SIMDandVectorTypes()
    {
        //Generalised vectors that can be SIMD accelarated classes
        //Framework handles all this for you
        Console.WriteLine("\n===SIMD and Vectors (Vector<T>)===\n\n");

        Console.WriteLine($"Int Vector Size: {Vector<int>.Count}");
        Console.WriteLine($"Register Size: {Vector<int>.Count * 32}\n");

        //Adding arrays
        byte[] array1 = Enumerable.Range(1, 128).Select(x => (byte)x).ToArray();
        byte[] array2 = Enumerable.Range(4, 128).Select(x => (byte)x).ToArray();

        var registerCapacity = Vector<byte>.Count;
        var result = new byte[128];

        //Increment by register capacity as we will skip this many with SIMD ops
        //if no SIMD this will move one at a time
        for(int i = 0; i < array1.Length; i += registerCapacity)
        {
            //using the Vector to view and traverse the array starting at i, till max
            var va = new Vector<byte>(array1, i);
            var vb = new Vector<byte>(array2, i);

            //Unlike intrinsics, has operator support
            var vresult = va + vb;

            //Vector will then copy to output starting at i till max
            vresult.CopyTo(result, i);
        }

        Console.WriteLine($"array1: {ByteArrayToString(array1)}");
        Console.WriteLine($"array2: {ByteArrayToString(array2)}");
        Console.WriteLine($"resultant: {ByteArrayToString(result)}");
    }



    public void SIMDandVectorsIntrinsics()
    {
        //Single Instruction Multiple Data
        //128, 256, 512 bit registers or not at all
        Console.WriteLine("\n===SIMD and Vectors (Intrinsics)===\n\n");

        //Intrinsics
        Console.WriteLine($"Avx support: {Avx.IsSupported}");
        Console.WriteLine($"Avx2 support: {Avx2.IsSupported}");
        Console.WriteLine($"Sse support: {Sse.IsSupported}");
        Console.WriteLine($"Sse2 support: {Sse2.IsSupported}");
        Console.WriteLine($"Sse3 support: {Sse3.IsSupported}");
        Console.WriteLine($"Ssse3 support: {Ssse3.IsSupported}");
        Console.WriteLine($"Sse41 support: {Sse41.IsSupported}");
        Console.WriteLine($"Sse42 support: {Sse42.IsSupported}\n");

        if(Sse.IsSupported)
        {
            var f = Sse.Add(Vector128.Create(1.0f), Vector128.Create(1.0f));
            Console.WriteLine($"SseAdd Sse.Add(Vector128.Create(1.0f), Vector128.Create(1.0f)):\n {f}\n");

            var v1 = Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f);
            var v2 = Vector128.Create(1.0f, 2.0f, 4.0f, 8.0f);
            var sum = Sse.Add(v1, v2);
            Console.WriteLine($"var v1 = Vector128.Create(1.0f, 2.0f, 3.0f, 4.0f);");
            Console.WriteLine($"var v2 = Vector128.Create(1.0f, 2.0f, 4.0f, 8.0f);");
            Console.WriteLine($"SseAdd Sse.Add(v1, v2):\n {sum}\n");
        }

        //Overflows and div 0
        //Division is only supported by float
        if(Sse2.IsSupported)
        {
            var x = Vector128.Create(1.0f);
            var y = Vector128.Create(0.0f);
            var div = Sse2.Divide(x, y);
            Console.WriteLine($"var x = Vector128.Create(1.0f);");
            Console.WriteLine($"var y = Vector128.Create(0.0f);");
            Console.WriteLine($"Div 0 Sse2.Divide(x, y):\n {div}\n");

            //Overflows happen the runtime or compiler wont save you here, you have to check or deal..
            //128 bit register, 16x 1 byte numbers
            var z = Vector128.Create((byte)255);
            var a = Vector128.Create((byte)1);
            var sum = Sse2.Add(z, a);

            Console.WriteLine($"var z = Vector128.Create((byte)255);");
            Console.WriteLine($"var a = Vector128.Create((byte)1);");
            Console.WriteLine($"Overflow Sse2.Add(z, a):\n {sum}\n");
        }
    }

    private static void DecimalOperations()
    {
        Console.WriteLine("\n===Decimals===\n\n");

        //Representation issues aren't the same as float
        //Slower but good for financial calculations
        Decimal d = 0.1m + 0.2m;
        Console.WriteLine($"Decimal addition 0.1m + 0.2m:\n {d}\n");
        Console.WriteLine($"Comparison: (0.1m + 0.2m) == 0.3m:\n {(0.1m + 0.2m) == 0.3m}\n");
    }

    private static void UnCheckedIntOperations()
    {
        Console.WriteLine("\n===Unchecked Ints===\n\n");

        //Overflowing integral types with a no check scope, compiler allows
        unchecked {
            Console.WriteLine($"unchecked compiler:\n {Int32.MinValue - 1}\n");
        }

        //No project wide checks enabled
        var minValue = Int32.MinValue;
        Console.WriteLine($"default pre minValue:\n {minValue}\n");
        minValue--;
        Console.WriteLine($"default post minValue:\n {minValue}\n");

        //Scoped checks
        try {
            checked {
                var checkedMinValue = Int32.MinValue;
                Console.WriteLine($"checked minValue pre:\n {checkedMinValue}\n");
                checkedMinValue--;
                Console.WriteLine($"checked minValue post:\n {checkedMinValue}\n");
            }
        }catch(Exception)
        {
            Console.WriteLine("checked runtime says no.");
        }
    }

    private static void FPOperations()
    {
        Console.WriteLine("\n===Floating Points===\n\n");

        //Look mum, no exceptions!
        float firstValue = 1;
        firstValue /= 0f;

        //Inifinty on it's side
        Console.WriteLine($"Div 1 / 0 is infinite?\n {float.IsInfinity(firstValue)}: {firstValue}\n");

        var secondValue = -1 / 0f;
        Console.WriteLine($"Div -1 / 0 is infinite?\n {float.IsInfinity(secondValue)}: {secondValue}\n");

        //NaN
        Console.WriteLine($"What are infinities divided?\n {firstValue / secondValue}\n");

        //It propogates on any subsequent operations
        Console.WriteLine($"NaN operation on a value:\n {100f + (firstValue / secondValue)}\n");

        //Any NaN comparison will result in false, use static comparison method
        Console.WriteLine($"Compare: (firstValue / secondValue) == float.NaN:\n {(firstValue / secondValue) == float.NaN}\n");
        Console.WriteLine($"Compare static: float.IsNaN(firstValue / secondValue):\n {float.IsNaN(firstValue / secondValue)}\n");

        //Representation issues 0.1 and 0.2 are approximations
        double d = 0.1 + 0.2;
        Console.WriteLine($"Float addition 0.1 + 0.2:\n {d}\n");
        
        //Comparisons has to have tolerances (Hard to find especially in loops)
        Console.WriteLine($"Comparison: (0.1 + 0.2) == 0.3:\n {(0.1 + 0.2) == 0.3}\n");
        Console.WriteLine($"Math.Abs(0.1 + 0.2) >= 0.3:\n {(Math.Abs(0.1 + 0.2) >= 0.3)}\n");
        
    }
    }
}