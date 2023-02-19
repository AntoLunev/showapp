namespace showapp.services;

public class Calculator
{
    public Calculator()
    {
        // Do nothing. This will allow to run some methods
    }

    public int Add(int a, int b)
    {
        return a+b;
    }

    public double Add(double a, double b)
    {
        return a+b;
    }

    public double Add(double a, int b)
    {
        var x = (double) b;
        return this.Add(a, x);
    }

    public double Add(int a, double b)
    {
        return this.Add(b, a);
    }
}