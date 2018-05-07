using UnityEngine;

namespace CoLib
{

/// <summary>
/// Tweening for unity specific classes.
/// </summary>
public static partial class Commands
{
	#region ChangeTo

	public static CommandDelegate ChangeTo(Ref<Vector2> vector, Vector2 endVector, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(vector, "vector");
		Vector2 startVector = Vector2.zero;
		return Commands.Sequence(
			Commands.Do(delegate() {
				startVector = vector.Value;
			}),
			Commands.Duration( delegate(double t) {
				vector.Value = (endVector - startVector) * (float)t + startVector;
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeTo(Ref<Vector3> vector, Vector3 endVector, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(vector, "vector");

		Vector3 startVector = Vector3.zero;
		return Commands.Sequence(
			Commands.Do(delegate() {
				startVector = vector.Value;
			}),
			Commands.Duration( delegate(double t) {
				vector.Value = (endVector - startVector) * (float)t + startVector;
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeTo(Ref<Vector4> vector, Vector4 endVector, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(vector, "vector");

		Vector4 startVector = Vector4.zero;
		return Commands.Sequence(
			Commands.Do(delegate() {
				startVector = vector.Value;
			}),
			Commands.Duration( delegate(double t) {
				vector.Value = (endVector - startVector) * (float)t + startVector;
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeTo(Ref<Rect> rect, Rect endRect, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(rect, "rect");

		return ChangeTo(rect, endRect, duration, new Vector2(0.5f, 0.5f), ease);
	}

	public static CommandDelegate ChangeTo(Ref<Rect> rect, Rect endRect, double duration, Vector2 anchorPoint, CommandEase ease = null)
	{
		CheckArgumentNonNull(rect, "rect");

		Rect startRect = new Rect();
		Vector2 startAnchorPoint = Vector2.zero;
		Vector2 endAnchorPoint = Vector2.zero;
		return Commands.Sequence(
			Commands.Do(delegate() {
				startRect = rect.Value;
				startAnchorPoint = new Vector2(
					startRect.x + startRect.width * anchorPoint.x,
					startRect.y + startRect.height * anchorPoint.y
				);
				endAnchorPoint = new Vector2(
					endRect.x + endRect.width * anchorPoint.x,
					endRect.y + endRect.height * anchorPoint.y
				);
			}),
			Commands.Duration( delegate(double t) {
				Rect newRect = new Rect();
				newRect.width = (endRect.width - startRect.width) * (float)t + startRect.width;
				newRect.height = (endRect.height - startRect.height) * (float)t + startRect.height;
				Vector2 newAnchorPoint = Vector2.Lerp(startAnchorPoint, endAnchorPoint, (float)t);
				newRect.x = newAnchorPoint.x - anchorPoint.x * newRect.width;
				newRect.y = newAnchorPoint.y - anchorPoint.y * newRect.height;
				rect.Value = newRect;
			}, duration, ease)
		);
	}

	#endregion

	#region ChangeBy
	
	public static CommandDelegate ChangeBy(Ref<Vector2> vector, Vector2 offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(vector, "vector");

		double lastT = 0.0;
		return Commands.Sequence(
			Commands.Do(delegate() {
				lastT = 0.0;	
			}),
			Commands.Duration( delegate(double t) {
				vector.Value +=  offset * (float) (t - lastT);
				lastT = t;
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeBy(Ref<Vector3> vector, Vector3 offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(vector, "vector");

		double lastT = 0.0;
		return Commands.Sequence(
			Commands.Do(delegate() {
				lastT = 0.0;	
			}),
			Commands.Duration( delegate(double t) {
				vector.Value +=  offset * (float) (t - lastT);
				lastT = t;
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeBy(Ref<Vector4> vector, Vector4 offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(vector, "vector");

		double lastT = 0.0;
		return Commands.Sequence(
			Commands.Do(delegate() {
				lastT = 0.0;	
			}),
			Commands.Duration( delegate(double t) {
				vector.Value +=  offset * (float) (t - lastT);
				lastT = t;
			}, duration, ease)
		);
	}

	#endregion

	#region ChangeFrom

	public static CommandDelegate ChangeFrom(Ref<Vector2> vector, Vector2 startVector, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(vector, "vector");

		Vector2 endVector = Vector2.zero;
		return Commands.Sequence(
			Commands.Do(delegate() {
				endVector = vector.Value;
			}),
			Commands.Duration( delegate(double t) {
				vector.Value =  (endVector - startVector) * (float)t + startVector;
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeFrom(Ref<Vector3> vector, Vector3 startVector, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(vector, "vector");

		Vector3 endVector = Vector3.zero;
		return Commands.Sequence(
			Commands.Do(delegate() {
				endVector = vector.Value;
			}),
			Commands.Duration( delegate(double t) {
				vector.Value = (endVector - startVector) * (float)t + startVector;	
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeFrom(Ref<Vector4> vector, Vector4 startVector, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(vector, "vector");

		Vector4 endVector = Vector4.zero;
		return Commands.Sequence(
			Commands.Do(delegate() {
				endVector = vector.Value;
			}),
			Commands.Duration( delegate(double t) {
				vector.Value = (endVector - startVector) * (float)t + startVector;
			}, duration, ease)
		);
	}

	public static CommandDelegate ChangeFrom(Ref<Rect> rect, Rect startRect, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(rect, "rect");

		return ChangeFrom(rect, startRect, duration, new Vector2(0.5f, 0.5f), ease);
	}

	public static CommandDelegate ChangeFrom(Ref<Rect> rect, Rect startRect, double duration, Vector2 anchorPoint, CommandEase ease = null)
	{
		CheckArgumentNonNull(rect, "rect");

		Rect endRect = new Rect();
		Vector2 startAnchorPoint = Vector2.zero;
		Vector2 endAnchorPoint = Vector2.zero;
		return Commands.Sequence(
			Commands.Do(delegate() {
				endRect = rect.Value;
				startAnchorPoint = new Vector2(
					startRect.x + startRect.width * anchorPoint.x,
					startRect.y + startRect.height * anchorPoint.y
				);
				endAnchorPoint = new Vector2(
					endRect.x + endRect.width * anchorPoint.x,
					endRect.y + endRect.height * anchorPoint.y
				);
			}),
			Commands.Duration( delegate(double t) {
				Rect newRect = new Rect();
				newRect.width = (endRect.width - startRect.width) * (float)t + startRect.width;
				newRect.height = (endRect.height - startRect.height) * (float)t + startRect.height;
				Vector2 newAnchorPoint = Vector2.Lerp(startAnchorPoint, endAnchorPoint, (float)t);
				newRect.x = newAnchorPoint.x - anchorPoint.x * newRect.width;
				newRect.y = newAnchorPoint.y - anchorPoint.y * newRect.height;
				rect.Value = newRect;
			}, duration, ease)
		);
	}

	#endregion

	#region ChangeFromOffset

	public static CommandDelegate ChangeFromOffset(Ref<Vector2> vector, Vector2 offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(vector, "vector");

		return Commands.Defer( () => Commands.ChangeFrom(vector, vector.Value + offset, duration, ease));
	}

	public static CommandDelegate ChangeFromOffset(Ref<Vector3> vector, Vector3 offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(vector, "vector");

		return Commands.Defer( () => Commands.ChangeFrom(vector, vector.Value + offset, duration, ease));

	}

	public static CommandDelegate ChangeFromOffset(Ref<Vector4> vector, Vector4 offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(vector, "vector");

		return Commands.Defer( () => Commands.ChangeFrom(vector, vector.Value + offset, duration, ease));
	}

	#endregion

	#region Rotate

	public static CommandDelegate RotateTo(Ref<Quaternion> rotation, Quaternion endRotation, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(rotation, "rotation");

		Quaternion startRotation = Quaternion.identity;
		return Commands.Sequence(
			Commands.Do( delegate() {
				startRotation = rotation.Value;	
			}),
			Commands.Duration( delegate(double t) {
				rotation.Value = Quaternion.LerpUnclamped(startRotation, endRotation, (float) t);
			}, duration, ease)
		);
	}

	public static CommandDelegate RotateBy(Ref<Quaternion> rotation, Quaternion offsetRotation, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(rotation, "rotation");

		double lastT = 0.0;
		return Commands.Sequence(
			Commands.Do( delegate() {
				lastT = 0.0;
			}),
			Commands.Duration( delegate(double t) {
				rotation.Value *= Quaternion.LerpUnclamped(Quaternion.identity, offsetRotation, (float) t) * 
					Quaternion.Inverse(Quaternion.LerpUnclamped(Quaternion.identity, offsetRotation, (float) lastT));
				lastT = t;
			}, duration, ease)
		);
	}

	public static CommandDelegate RotateFrom(Ref<Quaternion> rotation, Quaternion startRotation, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(rotation, "rotation");

		Quaternion endRotation = Quaternion.identity;
		return Commands.Sequence(
			Commands.Do( delegate() {
				endRotation = rotation.Value;	
			}),
			Commands.Duration( delegate(double t) {
				rotation.Value = Quaternion.LerpUnclamped(startRotation, endRotation, (float)t);
			}, duration, ease)
		);
	}

	public static CommandDelegate RotateFromOffset(Ref<Quaternion> rotation, Quaternion offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(rotation, "rotation");

		return Commands.Defer(
			() => Commands.RotateFrom(rotation, rotation.Value * offset, duration, ease)
		);
	}

	#endregion

	#region ScaleBy

	public static CommandDelegate ScaleBy(Ref<Vector2> scale, Vector2 scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(scale, "scale");

		Vector2 lastScaleFactor = Vector2.one;
		return Commands.Sequence(
			Commands.Do(delegate(){
				lastScaleFactor = Vector2.one;
			}),
			Commands.Duration( delegate(double t) {
				Vector2 newScaleFactor = (float)t * (scaleFactor - Vector2.one) + Vector2.one;
				scale.Value = new Vector2(
					scale.Value.x * newScaleFactor.x / lastScaleFactor.x,
					scale.Value.y * newScaleFactor.y / lastScaleFactor.y
				);
				lastScaleFactor = newScaleFactor;
			}, duration, ease)
		);
	}

	public static CommandDelegate ScaleBy(Ref<Vector3> scale, Vector3 scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(scale, "scale");

		Vector3 lastScaleFactor = Vector3.one;
		return Commands.Sequence(
			Commands.Do(delegate(){
				lastScaleFactor = Vector3.one;
			}),
			Commands.Duration( delegate(double t) {
				Vector3 newScaleFactor = (float)t * (scaleFactor - Vector3.one) + Vector3.one;
				scale.Value = new Vector3(
					scale.Value.x * newScaleFactor.x / lastScaleFactor.x,
					scale.Value.y * newScaleFactor.y / lastScaleFactor.y,
					scale.Value.z * newScaleFactor.z / lastScaleFactor.z
				);
				lastScaleFactor = newScaleFactor;
			}, duration, ease)
		);
	}

	public static CommandDelegate ScaleBy(Ref<Vector4> scale, Vector4 scaleFactor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(scale, "scale");

		Vector4 lastScaleFactor = Vector4.one;
		return Commands.Sequence(
			Commands.Do(delegate(){
				lastScaleFactor = Vector4.one;
			}),
			Commands.Duration( delegate(double t) {
				Vector4 newScaleFactor = (float)t * (scaleFactor - Vector4.one) + Vector4.one;
				scale.Value = new Vector4(
					scale.Value.x * newScaleFactor.x / lastScaleFactor.x,
					scale.Value.y * newScaleFactor.y / lastScaleFactor.y,
					scale.Value.z * newScaleFactor.z / lastScaleFactor.z,
					scale.Value.w * newScaleFactor.w / lastScaleFactor.w
				);
				lastScaleFactor = newScaleFactor;
			}, duration, ease)
		);
	}

	#endregion

	#region Tint

	public static CommandDelegate TintTo(Ref<Color> color, Color endColor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		Color startColor = Color.white;
		return Commands.Sequence(
			Commands.Do (delegate() {
				startColor = color.Value;
			}),
			Commands.Duration(delegate(double t) {
				color.Value = Color.Lerp(startColor, endColor, (float)t);
			}, duration, ease)
		);
	}

	public static CommandDelegate TintTo(Ref<Color32> color, Color32 endColor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		Color32 startColor = new Color32();
		return Commands.Sequence(
			Commands.Do (delegate() {
				startColor = color.Value;
			}),
			Commands.Duration(delegate(double t) {
				color.Value = Color32.Lerp(startColor, endColor, (float)t);
			}, duration, ease)
		);
	}

	public static CommandDelegate TintBy(Ref<Color> color, Color offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		double lastT = 0.0;
		return Commands.Sequence(
			Commands.Do (delegate() {
				lastT = 0.0;
			}),
			Commands.Duration(delegate(double t) {
				color.Value += offset * (float)(t - lastT);
				lastT = t;
			}, duration, ease)
		);
	}

	public static CommandDelegate TintBy(Ref<Color32> color, Color32 offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		double lastT = 0.0;
		return Commands.Sequence(
			Commands.Do (delegate() {
				lastT = 0.0;
			}),
			Commands.Duration(delegate(double t) {
				color.Value = new Color32(
					(byte) (color.Value.r + offset.r * (t - lastT)),
					(byte) (color.Value.g + offset.g * (t - lastT)),
					(byte) (color.Value.b + offset.b * (t - lastT)),
					(byte) (color.Value.a + offset.a * (t - lastT))
				);
				lastT = t;
			}, duration, ease)
		);
	}

	public static CommandDelegate TintFrom(Ref<Color> color, Color startColor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		Color endColor = Color.white;
		return Commands.Sequence(
			Commands.Do (delegate() {
				endColor = color.Value;
			}),
			Commands.Duration(delegate(double t) {
				color.Value = Color.Lerp(startColor, endColor, (float)t);
			}, duration, ease)
		);
	}

	public static CommandDelegate TintFrom(Ref<Color32> color, Color32 startColor, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		Color32 endColor = new Color32();
		return Commands.Sequence(
			Commands.Do (delegate() {
				endColor = color.Value;
			}),
			Commands.Duration(delegate(double t) {
				color.Value = Color32.Lerp(startColor, endColor, (float)t);
			}, duration, ease)
		);
	}

	#endregion

	#region Alpha

	public static CommandDelegate AlphaTo(Ref<Color> color, float endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		Ref<float> alphaRef = new Ref<float>(
			() => color.Value.a,
			(t) => {
				Color tempColor = color.Value;
				tempColor.a = t;
				color.Value = tempColor; 
			}
		);
		return ChangeTo(alphaRef, endAlpha, duration, ease);
	}

	public static CommandDelegate AlphaTo(Ref<Color32> color, byte endAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		Ref<double> alphaRef = new Ref<double>(
			() => color.Value.a / 255.0,
			(t) => {
				Color32 tempColor = color.Value;
				tempColor.a = (byte) (t * 255);
				color.Value = tempColor; 
			}
		);
		return ChangeTo(alphaRef, endAlpha, duration, ease);
	}

	public static CommandDelegate AlphaBy(Ref<Color> color, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		Ref<float> alphaRef = new Ref<float>(
			() => color.Value.a,
			(t) => {
				Color tempColor = color.Value;
				tempColor.a = t;
				color.Value = tempColor; 
			}
		);

		return ChangeBy(alphaRef, offset, duration, ease);
	}

	public static CommandDelegate AlphaBy(Ref<Color32> color, byte offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		Ref<double> alphaRef = new Ref<double>(
			() => color.Value.a / 255.0,
			(t) => {
				Color32 tempColor = color.Value;
				tempColor.a = (byte) (t * 255);
				color.Value = tempColor; 
			}
		);
		return ChangeBy(alphaRef, offset, duration, ease);
	}

	public static CommandDelegate AlphaFrom(Ref<Color> color, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		Ref<float> alphaRef = new Ref<float>(
			() => color.Value.a,
			(t) => {
				Color tempColor = color.Value;
				tempColor.a = t;
				color.Value = tempColor; 
			}
		);

		return ChangeFrom(alphaRef, startAlpha, duration, ease);
	}

	public static CommandDelegate AlphaFrom(Ref<Color32> color, float startAlpha, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		Ref<double> alphaRef = new Ref<double>(
			() => color.Value.a / 255.0,
			(t) => {
				Color32 tempColor = color.Value;
				tempColor.a = (byte) (t * 255);
				color.Value = tempColor; 
			}
		);
		return ChangeFrom(alphaRef, startAlpha, duration, ease);
	}

	public static CommandDelegate AlphaFromOffset(Ref<Color> color, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		return Commands.Defer( () => Commands.AlphaFrom(color, color.Value.a + offset, duration, ease));
	}

	public static CommandDelegate AlphaFromOffset(Ref<Color32> color, float offset, double duration, CommandEase ease = null)
	{
		CheckArgumentNonNull(color, "color");

		return Commands.Defer( () => Commands.AlphaFrom(color, (color.Value.a / 255.0f) + offset, duration, ease));
	}

	#endregion
}

}

