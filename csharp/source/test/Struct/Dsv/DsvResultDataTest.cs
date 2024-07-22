namespace Occhitta.Libraries.Struct.Dsv;

/// <summary>
/// <see cref="DsvResultData{TValue}" />検証クラスです。
/// </summary>
[TestFixture]
public class DsvResultDataTest {
	#region メンバー定数定義
	/// <summary>例外構文</summary>
	private const string FailureText = """
System.InvalidOperationException {
	TargetSite : (System.Reflection.RuntimeMethodInfo)TSource get_Data()
	Message : "成功情報ではありません。"
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2146233079
	StackTrace : ...
}
""";
	#endregion メンバー定数定義

	#region 内部メソッド定義:ChooseValue
	/// <summary>
	/// 成功情報を取得します。
	/// </summary>
	/// <typeparam name="TValue">項目種別</typeparam>
	/// <param name="sourceData">結果情報</param>
	/// <returns>項目情報</returns>
	private static TValue ChooseValue<TValue>(DsvResultData<TValue> sourceData) =>
		sourceData.Value;
	#endregion 内部メソッド定義:ChooseValue

	#region 検証メソッド定義:SuccessCode
	/// <summary>
	/// <see cref="DsvResultData{TValue}.Success(TValue)" />を検証します。
	/// </summary>
	[TestCase(""  )]
	[TestCase("A" )]
	[TestCase(null)]
	public void SuccessCode(string source) {
		var actual = DsvResultData<string>.Success(source);
		Assert.Multiple(() => {
			Assert.That(actual.State, Is.True);
			Assert.That(actual.Value, Is.SameAs(source));
			Assert.That(actual.Index, Is.Zero);
			Assert.That(actual.Count, Is.Zero);
		});
	}
	#endregion 検証メソッド定義:SuccessCode

	#region 検証メソッド定義:FailureCode
	/// <summary>
	/// <see cref="DsvResultData{TValue}.Failure(int, int)" />を検証します。
	/// </summary>
	/// <param name="index">開始位置</param>
	/// <param name="count">要素個数</param>
	[TestCase(-1, -1)]
	[TestCase(-1, +0)]
	[TestCase(-1, +1)]
	[TestCase(+0, -1)]
	[TestCase(+0, +0)]
	[TestCase(+0, +1)]
	[TestCase(+1, -1)]
	[TestCase(+1, +0)]
	[TestCase(+1, +1)]
	public void FailureCode(int index, int count) {
		var actual = DsvResultData<string>.Failure(index, count);
		var errors = Assert.Throws<InvalidOperationException>(() => ChooseValue(actual));
		Assert.Multiple(() => {
			Assert.That(actual.State, Is.False);
			Assert.That(actual.Index, Is.EqualTo(index));
			Assert.That(actual.Count, Is.EqualTo(count));
			Assert.That(ExceptionUtilities.ToString(errors, "\t"), Is.EqualTo(FailureText));
		});
	}
	#endregion 検証メソッド定義:FailureCode
}
