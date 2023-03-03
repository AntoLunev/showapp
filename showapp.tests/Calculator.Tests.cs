using Xunit;
using showapp.services;

namespace showapp.tests;

public class UnitTest1
{
    [Fact]
    public void CalculatorAddTestsInts()
    {
        //  
        int a = 1, b = 2;
        var calc = new Calculator();
        
        var x = calc.Add(a,b);
        
        // assert
        Assert.IsType<int>(x);
    }

    [Fact]
    public void CalculatorAddTestsFloats()
    {
        //  
        double a = 1.1, b = 2.2;
        var calc = new Calculator();
        
        var x = calc.Add(a,b);
        
        // assert
        Assert.IsType<double>(x);
    }

    [Fact]
    public void CalculatorAddTestsFloats2()
    {
        //  
        double a = 1.1;
        int b = 2;
        var calc = new Calculator();
        
        var x = calc.Add(a,b);
        
        // assert
        Assert.IsType<double>(x);
    }

    [Fact]
    public void CalculatorAddTestsFloats3()
    {
        //  
        double b = 1.1;
        int a = 2;
        var calc = new Calculator();
        
        var x = calc.Add(a,b);
        
        // assert
        Assert.IsType<double>(x);
    }
    
    [Fact]
    public void CalculatorSubstractTestsInt()
    {
        //
        int a = 2, b = 3;
        var calc = new Calculator();
        
        var x = calc.Substract(b,a);
        
        // assert
        Assert.IsType<int>(x);
    }

}   
