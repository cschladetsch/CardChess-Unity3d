using System;

namespace CoLib
{

public delegate double CommandEase(double t);

/// <summary>
/// This class contains helper methods for creating common easing functions.
/// An easing function takes an input value <c>t</c> where an uneased <c>t</c> 
/// ranges from 0 <= t <= 1 . Some easing functions, (such as <c>BackEase</c> returns
/// values outside the range 0 <= t <= 1). For a given valid easing function, f(t), 
/// f(0) = 0 and f(1) = 1.
/// </summary>
public static class Ease
{
	/// <summary>
	/// The default ease. It doesn't modify the value
	/// of t.
	/// </summary>
	public static CommandEase Linear() 
	{ 
		return t => t; 
	}
	
	/// <summary>
	/// Quantises t into numSteps + 1 levels, using round operation.
	/// </summary>
	/// <param name="numSteps"> Must be >= 1. </param> 
	/// <exception cref="System.ArgumentOutOfRangeException"></exception>
	public static CommandEase RoundStep(int numSteps = 1) 
	{
		CheckNumStepsGreaterThanZero(numSteps);
		return t => Math.Round(t * numSteps) / numSteps; 
	}
	
	/// <summary>
	/// Quantises t into numSteps + 1 levels, using ceil operation.
	/// This increases the average value of t over the duration
	/// of the ease.
	/// </summary>
	/// <param name="numSteps"> Must be >= 1. </param>
	/// <exception cref="System.ArgumentOutOfRangeException"></exception>
	public static CommandEase CeilStep(int numSteps = 1) 
	{ 
		CheckNumStepsGreaterThanZero(numSteps);
		return t => Math.Ceiling(t * numSteps) / numSteps; 
	}
	
	/// <summary>
	/// Quantises t into numSteps + 1 levels, using floor operation.
	/// This increases the average value of t over the duration
	/// of the ease.
	/// </summary>
	/// <param name="numSteps"> Must be >= 1. </param>
	/// <exception cref="System.ArgumentOutOfRangeException"> </exception>
	public static CommandEase FloorStep(int numSteps = 1) 
	{
		CheckNumStepsGreaterThanZero(numSteps);
		return t => Math.Floor(t * numSteps) / numSteps; 
	}
		
	/// <summary>
	/// Averages the output from several easing functions.
	/// </summary>
	public static CommandEase AverageComposite(params CommandEase[] eases)
	{
		return t => {
			double average = 0.0;
			for (int i = 0; i < eases.Length; ++i) {
				average += eases[i](t);
			}
			return average / eases.Length;
		};
	}
		
	/// <summary>
	/// Sequentially triggers easing functions. For instance, if we have
	/// 3 easing functions, 0 <= t < 0.33 is handled by first easing function
	/// 0.33 <= t < 0.66 by second, and 0.66 <= t <= 1.0 by third.
	/// </summary>
	public static CommandEase SequentialComposite(params CommandEase[] eases)
	{
		return t => {
			int index = (int)(t * eases.Length);
			if (index >= eases.Length) { return 1.0; } else if (index < 0) { return 0.0; }
			else {
				double sequenceLength = 1.0 / eases.Length;
				double sequenceT = (t - (index * sequenceLength)) / sequenceLength;
				return (eases[index](sequenceT) + index) * sequenceLength;
			}
		};
	}

	public static CommandEase WeightedComposite(
			double weightOne, CommandEase easeOne, 
			double weightTwo, CommandEase easeTwo, 
			double weightThree, CommandEase easeThree)
	{
			double totalWeight = weightOne + weightTwo + weightThree;
		return t => {
			double currentWeight = t * totalWeight;
			if (currentWeight < weightOne) { return easeOne(t * 3.0) / 3.0; }
			else if (currentWeight < weightOne + weightTwo) { return easeTwo(3 * (t - 1.0/3.0)) / 3.0 + 1.0/3.0; }
			else { return easeThree(3 * (t - 2.0/3.0)) / 3.0 + 2.0/3.0; }
		};
	}
		
	/// <summary>
	/// Eases a value, by pipelining it throguh several easing functions.
	/// The output of the first ease is used as input for the next.
	/// </summary>
	public static CommandEase ChainComposite(params CommandEase[] eases)
	{
		return t => {
			for (int i = 0; i < eases.Length; ++i) {
				t = eases[i](t);
			}
        	return t;
		};
	}
	
	/// <summary>
	/// Combines two easing functions. The inEase parameter maps to the range
	/// 0.0 <= t < 0.5, outEase maps to the range 0.5 <= t < 1.0 ,
	/// </summary>
	public static CommandEase InOutEase(CommandEase inEase, CommandEase outEase) 
	{	
		return t => { 
			if (t < 0.5) { 
				return 0.5 * inEase(t / 0.5); 
	    	}
	
        	return 0.5 * outEase( (t - 0.5) / 0.5) + 0.5;
		};
	}
		
	public static CommandEase OutEase(CommandEase inEase)
	{
		 return t => 1.0 - inEase(1.0 - t);
	}
	
	#region Polynomial Eases
	
	public static CommandEase InPolynomial(double power) 
	{ 
			return t => (double) Math.Pow((double) t, (double) power);
	} 
		
    public static CommandEase OutPolynomial(double power) { return OutEase(InPolynomial(power)); } 
    public static CommandEase InOutPolynomial(double power) 
    {
        return InOutEase(InPolynomial(power), OutPolynomial(power) ); 
    }
		
    public static CommandEase InQuad() { return InPolynomial(2.0); } 
    public static CommandEase OutQuad() { return OutPolynomial(2.0); } 
    public static CommandEase InOutQuad() { return InOutPolynomial(2.0); }

    public static CommandEase InCubic() { return InPolynomial(3.0); } 
    public static CommandEase OutCubic() { return OutPolynomial(3.0); } 
    public static CommandEase InOutCubic() { return InOutPolynomial(3.0); }

    public static CommandEase InQuart() { return InPolynomial(4.0); } 
    public static CommandEase OutQuart() { return OutPolynomial(4.0); } 
    public static CommandEase InOutQuart() { return InOutPolynomial(4.0); }

    public static CommandEase InQuint() { return InPolynomial(5.0); } 
    public static CommandEase OutQuint() { return OutPolynomial(5.0); } 
    public static CommandEase InOutQuint() { return InOutPolynomial(5.0); }
	
	#endregion
    
	/// <summary>
	/// Eases using a trigonometric functions.
	/// </summary>
    public static CommandEase InSin() 
	{
			return t => 1.0  - (double) Math.Cos((double) t * Math.PI / 2.0);
	} 
    public static CommandEase OutSin() { return OutEase(InSin()); } 
    public static CommandEase InOutSin() 
   	{
        return InOutEase(InSin(), OutSin());
    }
	
	public static CommandEase Elastic(double amplitude = 1.0, double period = 0.3)
	{
		return t => {
			double tempAmplitude = amplitude;
			double s = 0.0;
			
			if (t == 0) {
				return 0.0;
			} 
			else if (t == 1.0) { 
				return 1.0; 
			}
			
			if (tempAmplitude < 1.0) {
				tempAmplitude = 1.0; 
				s = period / 4.0;
			}
			else {
				s = period / 
					(2.0 * (double) Math.PI) * (double) Math.Asin ((double) 1.0 / tempAmplitude);
			}
			t -= 1.0;
			return -(tempAmplitude * (double) Math.Pow(2.0, 10.0 * t) * 
				(double) Math.Sin( (t - s) * (double) (2.0 * Math.PI) / period ));
    	};
	}
		
    public static CommandEase InElastic() 
	{
		return Elastic();
	} 
    public static CommandEase OutElastic() { return OutEase(InElastic()); } 
    public static CommandEase InOutElastic() 
    {
        return InOutEase(InElastic(), OutElastic());
    }

    public static CommandEase InExpo() 
	{
		return t => t == 1.0 ? 1.0 : (double) Math.Pow(2.0, (double) (10.0 * (t - 1.0)) ); 
	} 
    public static CommandEase OutExpo() { return OutEase(InExpo()); } 
    public static CommandEase InOutExpo() 
    {
        return InOutEase(InExpo(), OutExpo());
    }

    public static CommandEase InCirc() 
	{ 
		return t => 1.0 - (double) Math.Sqrt((double) (1.0 - t * t) );
	} 
    public static CommandEase OutCirc() { return OutEase(InCirc()); } 
    public static CommandEase InOutCirc() 
    {
        return InOutEase(InCirc(), OutCirc());
    }

    public static CommandEase InBack(double amplitude = 0.2) 
	{
		return t => t * t * t - t * amplitude * (double) Math.Sin((double)t * Math.PI);
	} 
    public static CommandEase OutBack(double amplitude = 0.2) 
	{
		return OutEase(InBack(amplitude)); 
	} 
    public static CommandEase InOutBack(double amplitude = 0.2) 
    {
        return InOutEase(InBack(amplitude * 2.0), 
			OutBack(amplitude * 2.0));
    }
		
	public static CommandEase InBounce() 
	{
		return 	t => {
			t = 1.0 - t;
			if (t < 1.0 / 2.75) {
				return 1.0 - (7.5625 * t * t);
			} else if (t < 2.0 / 2.75) {
				t -= 1.5 / 2.75;
				return 1.0 - (7.5625 * t * t + 0.75);
			} else if (t < 2.5 / 2.75) {
				t -= 2.25 / 2.75;
				return 1.0 - (7.5625 * t * t + 0.9375);
			} else {
				t-= 2.625 / 2.75;
				return 1.0 - (7.5625 * t * t + 0.984375);
			}
		};
	} 
    public static CommandEase OutBounce() { return OutEase(InBounce()); } 
    public static CommandEase InOutBounce() 
    {
        return InOutEase(InBounce(), OutBounce());
    }
	
	/// <summary>
	/// A Hermite curve easing function. The Hermite curve is a cheap easing function, with
	/// adjustable tangents at it's endpoints.
	/// </summary>
	public static CommandEase InHermite(double startTangent = 0.0, 
		double endTangent = 1.0) 
	{
		return  t => { 
			// Hermite curve over normalised t interval:
        	//    p(t) = (-2t^2 - 3t^2 + 1) * p0 + (t^3 - 2t^2 + t) * m0 + (-2t^3+ 3t^2) *p1 + (t^3- t^2) * m1
        	// Where p0 = p at time 0, p1 = p at time 1, m0 = tangent at time 0, m1 = tangent at time 1.
       		// Note that in our case p0 = 0, and p1 = 1, while m0 = startTangent, and m1 = endTangent.
        	// This gives :
       		//    p(t) = (t^3 - 2t^2 + t) * m0 - 2t^3 + 3t^2 + (t^3 - t^2) * m1
        	double tSqr = t * t;
        	double tCbd =  t * t * t;
        	return
           		(tCbd - 2 * tSqr + t) * startTangent
           		- 2 * tCbd + 3 * tSqr
           		+ (tCbd - tSqr) * endTangent;
		};
	} 


    public static CommandEase OutHermite(double startTangent = 0.0, 
		double endTangent = 1.0) 
	{
			return OutEase(InHermite(startTangent,endTangent)); 
	}
	 
    public static CommandEase InOutHermite(double startTangent = 0.0, 
		double endTangent = 0.0) 
    {
        return InHermite(startTangent, endTangent);
    }
	
	public static CommandEase Smooth()
	{
		return InOutHermite(0.0, 0.0);
	}
		
	private static void CheckNumStepsGreaterThanZero(int numSteps)
	{
		if (numSteps <= 0) {
			throw new ArgumentOutOfRangeException("numSteps", numSteps, "numStep must be > 0");
		}
	}
}

}