namespace CoLib
{

public static partial class Commands
{
	#region ChangeTo
	
	public static CommandDelegate ChangeTo(Ref<float> single, float endSingle, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");
		float startSingle = 0.0f;
		return Commands.Sequence(
			Commands.Do(delegate() {
				startSingle = single.Value;
			}),
			Commands.Duration( delegate(double t) {
				single.Value = (endSingle - startSingle) * (float) t + startSingle;
			}, duration, ease)
		);
	}
	
	public static CommandDelegate ChangeTo(Ref<double> single, double endSingle, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		double startSingle = 0.0;
		return Commands.Sequence(
			Commands.Do(delegate() {
				startSingle = single.Value;
			}),
			Commands.Duration( delegate(double t) {
				single.Value = (endSingle - startSingle) * t + startSingle;
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeTo(Ref<short> single, short endValue, double duration , CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		var reference = ToDoubleRef(single);
		return Commands.Sequence (
			Commands.Do( () => reference.Value = (double) single.Value),
			Commands.ChangeTo(reference, (double) endValue, duration, ease)
		);
	}

	public static CommandDelegate ChangeTo(Ref<int> single, int endValue, double duration , CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		var reference = ToDoubleRef (single);
		return Commands.Sequence (
			Commands.Do( () => reference.Value = (double) single.Value),
			Commands.ChangeTo(reference, (double) endValue, duration, ease)
		);
	}

	public static CommandDelegate ChangeTo(Ref<long> single, long endValue, double duration , CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		var reference = ToDoubleRef (single);
		return Commands.Sequence (
			Commands.Do( () => reference.Value = (double) single.Value),
			Commands.ChangeTo(reference, (double) endValue, duration, ease)
		);
	}

	public static CommandDelegate ChangeTo<T>(IInterpolatable<T> interpolatable, T endValue, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(interpolatable, "interpolatable");

		T startValue = interpolatable.GetValue();
		return Commands.Sequence(
			Commands.Do(delegate() {
				startValue = interpolatable.GetValue();
			}),
			Commands.Duration( delegate(double t) {
				interpolatable.Interpolate(startValue, endValue, t);
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeTo<T>(Ref<T> val, T endValue, IInterpolator<T> interpolator, double duration, CommandEase ease = null) where T : struct
	{
		CheckArgumentNonNull(val, "val");
		CheckArgumentNonNull(interpolator, "interpolator");

		T startValue = val.Value;
		return Commands.Sequence(
			Commands.Do(delegate() {
				startValue = val.Value;
			}),
			Commands.Duration( delegate(double t) {
				val.Value = interpolator.Interpolate(startValue, endValue, t);
			}, duration, ease)
		);
	}

	#endregion
	
	#region ChangeBy

	public static CommandDelegate ChangeBy(Ref<float> single, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");
		double lastT = 0.0;
		return Commands.Sequence(
			Commands.Do(delegate() {
				lastT = 0.0;	
			}),
			Commands.Duration( delegate(double t) {
				single.Value +=  offset * (float) (t - lastT);
				lastT = t;
			}, duration, ease)
		);
	}
	
	public static CommandDelegate ChangeBy(Ref<double> single, double offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		double lastT = 0.0;
		return Commands.Sequence(
			Commands.Do(delegate() {
				lastT = 0.0;	
			}),
			Commands.Duration( delegate(double t) {
				single.Value +=  offset * (t - lastT);
				lastT = t;
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeBy(Ref<short> single, short offset, double duration , CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		var reference = ToDoubleRef(single);
		return Commands.Sequence (
			Commands.Do( () => reference.Value = (double) single.Value),
			Commands.ChangeBy(reference, (double) offset, duration, ease)
		);
	}

	public static CommandDelegate ChangeBy(Ref<int> single, int offset, double duration , CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		var reference = ToDoubleRef(single);
		return Commands.Sequence (
			Commands.Do( () => reference.Value = (double) single.Value),
			Commands.ChangeBy(reference, (double) offset, duration, ease)
		);
	}

	public static CommandDelegate ChangeBy(Ref<long> single, long offset, double duration , CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		var reference = ToDoubleRef(single);
		return Commands.Sequence (
			Commands.Do( () => reference.Value = (double) single.Value),
			Commands.ChangeBy(reference, (double) offset, duration, ease)
		);
	}

	#endregion
	
	#region ChangeFrom
	
	public static CommandDelegate ChangeFrom(Ref<float> single, float startSingle, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		float endSingle = 0.0f;
		return Commands.Sequence(
			Commands.Do(delegate() {
				endSingle = single.Value;
			}),
			Commands.Duration( delegate(double t) {
				single.Value = (endSingle - startSingle) * (float) t + startSingle;	
			}, duration, ease)
		);
	}
	
	public static CommandDelegate ChangeFrom(Ref<double> single, double startSingle, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		double endSingle = 0.0;
		return Commands.Sequence(
			Commands.Do(delegate() {
				endSingle = single.Value;
			}),
			Commands.Duration( delegate(double t) {
				single.Value = (endSingle - startSingle) * t + startSingle;	
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeFrom(Ref<short> single, short startValue, double duration , CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		var reference = ToDoubleRef(single);
		return Commands.Sequence (
			Commands.Do( () => reference.Value = (double) single.Value),
			Commands.ChangeFrom(reference, (double) startValue, duration, ease)
		);
	}

	public static CommandDelegate ChangeFrom(Ref<int> single, int startValue, double duration , CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		var reference = ToDoubleRef(single);
		return Commands.Sequence (
			Commands.Do( () => reference.Value = (double) single.Value),
			Commands.ChangeFrom(reference, (double) startValue, duration, ease)
		);	}

	public static CommandDelegate ChangeFrom(Ref<long> single, long startValue, double duration , CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");

		var reference = ToDoubleRef(single);
		return Commands.Sequence (
			Commands.Do( () => reference.Value = (double) single.Value),
			Commands.ChangeFrom(reference, (double) startValue, duration, ease)
		);
	}

	public static CommandDelegate ChangeFrom<T>(IInterpolatable<T> interpolatable, T startValue, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(interpolatable, "interpolatable");

		T endValue = interpolatable.GetValue();
		return Commands.Sequence(
			Commands.Do(delegate() {
				endValue = interpolatable.GetValue();
			}),
			Commands.Duration( delegate(double t) {
				interpolatable.Interpolate(startValue, endValue, t);
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeFrom<T>(Ref<T> val, T startValue, IInterpolator<T> interpolator, double duration, CommandEase ease = null) where T : struct
	{
		CheckArgumentNonNull(val, "val");
		CheckArgumentNonNull(interpolator, "interpolator");

		T endValue = val.Value;
		return Commands.Sequence(
			Commands.Do(delegate() {
				endValue = val.Value;
			}),
			Commands.Duration( delegate(double t) {
				val.Value = interpolator.Interpolate(startValue, endValue, t);
			}, duration, ease)
		);
	}

	#endregion
	
	#region ChangeFromOffset
	
	public static CommandDelegate ChangeFromOffset(Ref<float> single, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");
		
		float endSingle = 0.0f;
		float startSingle = 0.0f;
		return Commands.Sequence(
			Commands.Do(delegate() {
				endSingle = single.Value;
				startSingle = endSingle + offset;
			}),
			Commands.Duration( delegate(double t) {
				single.Value = (endSingle - startSingle) * (float) t + startSingle;	
			}, duration, ease)
		);
	}
	
	public static CommandDelegate ChangeFromOffset(Ref<double> single, double offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");
		
		double endSingle = 0.0;
		double startSingle = 0.0;
		
		return Commands.Sequence(
			Commands.Do(delegate() {
				endSingle = single.Value;
				startSingle = endSingle + offset;
			}),
			Commands.Duration( delegate(double t) {
				single.Value = (endSingle - startSingle) * t + startSingle;	
			}, duration, ease)
		);
	}
		
	public static CommandDelegate ChangeFromOffset(Ref<short> single, short offset, double duration , CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");
		
		var reference = ToDoubleRef(single);
		return Commands.Sequence (
			Commands.Do( () => reference.Value = (double) single.Value),
			Commands.ChangeFromOffset(reference, (double) offset, duration, ease)
		);
	}
		
	public static CommandDelegate ChangeFromOffset(Ref<int> single, int offset, double duration , CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");
		
		var reference = ToDoubleRef(single);
		return Commands.Sequence (
			Commands.Do( () => reference.Value = (double) single.Value),
			Commands.ChangeFromOffset(reference, (double) offset, duration, ease)
		);
	}
		
	public static CommandDelegate ChangeFromOffset(Ref<long> single, long offset, double duration , CommandEase ease = null)
	{
		CheckArgumentNonNull(single, "single");
		
		var reference = ToDoubleRef(single);
		return Commands.Sequence (
			Commands.Do( () => reference.Value = (double) single.Value),
			Commands.ChangeFromOffset(reference, (double) offset, duration, ease)
		);
	}	
	
 	#endregion 
	
	#region ScaleBy
	
	public static CommandDelegate ScaleBy(Ref<float> scale, float scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(scale, "scale");

		float lastScaleFactor = 1.0f;
		return Commands.Sequence(
			Commands.Do(delegate(){
				lastScaleFactor = 1.0f;
			}),
			Commands.Duration( delegate(double t) {
				float newScaleFactor = (float)t * (scaleFactor - 1.0f) + 1.0f;
				scale.Value = scale.Value * newScaleFactor / lastScaleFactor;
				lastScaleFactor = newScaleFactor;
			}, duration, ease)
		);
	}
	
	public static CommandDelegate ScaleBy(Ref<double> scale, double scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(scale, "scale");

		double lastScaleFactor = 1.0;
		return Commands.Sequence(
			Commands.Do(delegate(){
				lastScaleFactor = 1.0;
			}),
			Commands.Duration( delegate(double t) {
				double newScaleFactor = t * (scaleFactor - 1.0) + 1.0;
				scale.Value = scale.Value * newScaleFactor / lastScaleFactor;
				lastScaleFactor = newScaleFactor;
			}, duration, ease)
		);
	}

	#endregion

	#region Static private methods

	private static Ref<double> ToDoubleRef(this Ref<short> reference)
	{
		double val = (double) reference.Value;

		Ref<double> newReference = new Ref<double> (
			() => val,
			(t) => {
				val = t;
				reference.Value = System.Convert.ToInt16(
					System.Math.Round(val, System.MidpointRounding.AwayFromZero)
				);
			}
		);
		return newReference;
	}

	private static Ref<double> ToDoubleRef(this Ref<int> reference)
	{
		double val = (double) reference.Value;
		Ref<double> newReference = new Ref<double> (
			() => val,
			(t) => {
				val = t;
				reference.Value = System.Convert.ToInt32(
					System.Math.Round(val, System.MidpointRounding.AwayFromZero)
				);
			}
		);
		return newReference;
	}

	private static Ref<double> ToDoubleRef(this Ref<long> reference)
	{
		double val = (double) reference.Value;
		Ref<double> newReference = new Ref<double> (
			() => val,
			(t) => {
				val = t;
				reference.Value = System.Convert.ToInt64(
					System.Math.Round(val, System.MidpointRounding.AwayFromZero)
				);
			}
		);
		return newReference;
	}

	#endregion
}
	
}
	

